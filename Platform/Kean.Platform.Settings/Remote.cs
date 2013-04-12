using System;
using Uri = Kean.Core.Uri;
using IO = Kean.IO;
using Kean.Core.Extension;
using Parallel = Kean.Core.Parallel;
using Collection = Kean.Core.Collection;

namespace Kean.Platform.Settings
{
	public class Remote :
		IDisposable
	{
		Collection.IDictionary<string, Action<string>> values = new Collection.Synchronized.Dictionary<string, Action<string>>();
		Collection.IDictionary<string, Action<string>> notifications = new Collection.Synchronized.Dictionary<string, Action<string>>();

		IO.ICharacterReader reader;
		IO.ICharacterWriter writer;
		Parallel.RepeatThread thread;

		Remote(IO.ICharacterDevice backend)
		{
			this.reader = IO.CharacterReader.Open(backend);

			IO.Text.Builder line = null;
			while (this.reader.Next() && this.reader.Last != '\n')
			{
				if (this.reader.Last == '>' && line == "# ")
					break;
				else if (!char.IsControl(this.reader.Last))
					line += this.reader.Last;
			}
			this.thread = Parallel.RepeatThread.Start("RemoteClient", this.Receive);
			this.writer = IO.CharacterWriter.Open(backend);
		}
		public bool Exists(string @object)
		{
			return true;
		}
		public void Call(string method, params object[] arguments)
		{
			this.Send(method, arguments);
		}
		public T Get<T>(string property)
		{
			return this.Receive<T>(property, () => this.Send(property));
		}
		public T Set<T>(string property, T value)
		{
			return this.Receive<T>(property, () => this.Send(property, value));
		}
		public void Listen<T>(string property, Action<T> callback)
		{
			this.Send(property + " notify");
			this.notifications[property] = value => callback(value.Parse<T>());
		}

		T Receive<T>(string property, Action send)
		{
			object @lock = new object();
			bool done = false;
			T result = default(T);
			Action<string> previous = this.values[property];
			this.values[property] = value =>
			{
				lock (@lock)
				{
					try { result = value.Parse<T>(); }
					finally
					{
						done = true;
						System.Threading.Monitor.PulseAll(@lock);
					}
				}
				this.values.Remove(property);
				previous.Call(value);
			};
			send();
			if (this.thread.IsCurrent)
				this.Receive();
			else
				lock (@lock)
					while (!done)
						System.Threading.Monitor.Wait(@lock, 500);
			return result;
		}

		void Receive()
		{
			IO.Text.Builder line = null;
			while (this.reader.Next() && this.reader.Last != '\n')
			{
				if (line.NotNull() || !char.IsWhiteSpace(this.reader.Last))
					if (this.reader.Last == '>' && line == "# ")
						line = null;
					else if (!char.IsControl(this.reader.Last))
						line += this.reader.Last;
			}
			Console.WriteLine("< " + line);
			string[] splitted = ((string)line).Split(new char[] { ' ' }, 3);
			if (splitted.Length > 1)
			{
				switch (splitted[0])
				{
					case "$": // value
						if (splitted.Length > 2)
							this.values[splitted[1]].Call(splitted[2]);
						break;
					case "%": // notification
						if (splitted.Length > 2)
							this.notifications[splitted[1]].Call(splitted[2]);
						break;
					default:
					case "!": // error
						System.Diagnostics.Debug.WriteLine(line);
						break;
				}
			}
		}

		void Send(string message, params object[] arguments)
		{
			IO.Text.Builder builder = new IO.Text.Builder(message);
			foreach (object argument in arguments)
				builder += " \"" + argument.AsString() + "\"";
			Console.WriteLine("> " + (string)builder);
			this.writer.WriteLine((string)builder);
		}

		public void Dispose()
		{
			if (this.writer.NotNull())
			{
				this.writer.Dispose();
				this.writer = null;
			}
			if (this.thread.NotNull())
			{
				this.thread.Dispose();
				this.thread = null;
			}
			if (this.reader.NotNull())
			{
				this.reader.Dispose();
				this.reader = null;
			}
		}


		public static Remote Open(Uri.Locator resource)
		{
			IO.ICharacterDevice result = null;
			switch (resource.Scheme)
			{
				case "tcp":
					result = IO.CharacterDevice.Open(IO.Net.Tcp.Connection.Connect(resource.Authority.Endpoint));
					break;
			}
			return result.NotNull() ? new Remote(result) : null;
		}
	}
}

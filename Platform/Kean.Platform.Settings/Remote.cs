using System;
using Uri = Kean.Core.Uri;
using IO = Kean.IO;
using Kean.Core.Extension;
using Parallel = Kean.Core.Parallel;
using Collection = Kean.Core.Collection;
using Error = Kean.Core.Error;
using Kean.Core;

namespace Kean.Platform.Settings
{
	public class Remote :
		Synchronized,
		IDisposable
	{
		public bool Debug { get; set; }
		Collection.IDictionary<string, Action<string>> values = new Collection.Synchronized.Dictionary<string, Action<string>>();
		Collection.IDictionary<string, Action<string>> notifications = new Collection.Synchronized.Dictionary<string, Action<string>>();
		object onResponseLock = new object();
		Action<bool> onResponse;
		event Action<bool> OnResponse
		{
			add { lock(this.onResponseLock) this.onResponse += value; }
			remove { lock (this.onResponseLock) this.onResponse -= value; }
		}
		IO.ICharacterReader reader;
		IO.ICharacterWriter writer;
		Parallel.RepeatThread thread;

		Remote(IO.ICharacterReader reader, IO.ICharacterWriter writer)
		{
			this.reader = reader;
			this.writer = writer;

			IO.Text.Builder line = null;
			while (this.reader.Next() && this.reader.Last != '\n')
			{
				if (this.reader.Last == '>' && line == "# ")
					break;
				else if (!char.IsControl(this.reader.Last))
					line += this.reader.Last;
			}
			this.thread = Parallel.RepeatThread.Start("RemoteClient", this.Receive);
		}
		public bool Exists(string @object)
		{
			return true;
		}
		public bool Call(string method, params object[] arguments)
		{
			object @lock = new object();
			bool done = false;
			bool result = false;
			Action<string> previous = this.values[method];
			Action<bool> onResponse = success =>
			{
				lock (@lock)
				{
					result = success;
					done = true;
					this.values[method] = previous;
					System.Threading.Monitor.PulseAll(@lock);
				}
			};
			this.values[method] = value =>
			{
				lock (@lock)
				{
					if (value == "failed")
					{
						result = false;
						this.OnResponse -= onResponse;
						done = true;
						System.Threading.Monitor.PulseAll(@lock);
					}
				}
				this.values.Remove(method);
				previous.Call(value);
			};
			this.OnResponse += onResponse;
			string sent = this.Send(method, arguments);
			if (this.thread.IsCurrent)
				this.Receive();
			else
				lock (@lock)
					for (int i = 0; i < 5 && !done; i++)
						System.Threading.Monitor.Wait(@lock, 1000);
			if (!done)
				Error.Log.Append(Error.Level.Recoverable, "Remote Method Call Timed Out.", "Timed out while waiting for response when calling \"{0}\" over \"{1}\".", sent, this.writer.Resource);
			return result;
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
		T Receive<T>(string property, Func<string> send)
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
			string sent = send();
			if (this.thread.IsCurrent)
				this.Receive();
			else
				lock (@lock)
					for (int i = 0; i < 5 && !done; i++)
						System.Threading.Monitor.Wait(@lock, 1000);
			if (!done)
				Error.Log.Append(Error.Level.Recoverable, "Remote Property Read Timed Out.", "Timed out while waiting for response when requesting property \"{0}\" over \"{1}\".", sent, this.writer.Resource);
			return result;
		}

		void Receive()
		{
			lock (this.Lock)
			{
				IO.Text.Builder line = null;
				while (this.reader.Next() && this.reader.Last != '\n')
				{
					if (line.NotNull() || !char.IsWhiteSpace(this.reader.Last))
						if (this.reader.Last == '>' && line == "# ")
						{
							line = null;
							this.OnResponseCall(true);
						}
						else if (!char.IsControl(this.reader.Last))
							line += this.reader.Last;
				}
				if (this.Debug)
					Console.WriteLine("< " + line);
				string[] splitted = ((string)line).Split(new char[] { ' ' }, 3);
				if (splitted.Length > 1)
				{
					switch (splitted[0])
					{
						case "$": // value
							if (splitted.Length > 2)
							{
								this.values[splitted[1]].Call(splitted[2]);
								this.OnResponseCall(true);
							}
							break;
						case "%": // notification
							if (splitted.Length > 2)
								this.notifications[splitted[1]].Call(splitted[2]);
							break;
						default:
						case "!": // error
							this.OnResponseCall(false);
							System.Diagnostics.Debug.WriteLine(line);
							break;
					}
				}
			}
		}

		void OnResponseCall(bool success)
		{
			Action<bool> onResponse;
			lock (this.onResponseLock)
			{
				onResponse = this.onResponse;
				this.onResponse = null;
			}
			onResponse.Call(success);
		}

		string Send(string message, params object[] arguments)
		{
			IO.Text.Builder builder = new IO.Text.Builder(message);
			foreach (object argument in arguments)
				builder += " \"" + argument.AsString() + "\"";
			string result = builder;
			if (this.Debug)
				Console.WriteLine("> " + result);
			this.writer.WriteLine(result);
			return result;
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
			return Remote.Open(result);
		}
		public static Remote Open(IO.ICharacterDevice characterDevice)
		{
			return Remote.Open(IO.CharacterReader.Open(characterDevice), IO.CharacterWriter.Open(characterDevice));
		}
		public static Remote Open(IO.ICharacterInDevice characterInDevice, IO.ICharacterOutDevice characterOutDevice)
		{
			return new Remote(IO.CharacterReader.Open(characterInDevice), IO.CharacterWriter.Open(characterOutDevice));
		}
		public static Remote Open(IO.ICharacterReader reader, IO.ICharacterWriter writer)
		{
			return reader.NotNull() && writer.NotNull() ? new Remote(reader, writer) : null;
		}
	}
}

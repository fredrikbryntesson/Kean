// 
//  Editor.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Kean.Core;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Uri = Kean.Core.Uri;
using Parallel = Kean.Core.Parallel;

namespace Kean.Platform.Settings
{
	public class Parser :
		IDisposable
	{
		internal Object Root { get; private set; }
		Object current;
		internal Object Current 
		{
			get { return this.current; }
			set
			{
				this.current = value;
				this.LineBuffer.Prompt = string.Format("# {0}> ", this.current);
			}
		}
		Cli.ITerminal terminal;
		Cli.LineBuffer.Abstract lineBuffer;
		Cli.LineBuffer.Abstract LineBuffer 
		{
			get { return this.lineBuffer; }
			set 
			{ 
				this.lineBuffer = value;
				this.LineBuffer.Execute = this.Execute;
				this.LineBuffer.Complete = this.Complete;
				this.LineBuffer.Help = this.Help;
				this.LineBuffer.RequestType = this.RequestType;
				this.LineBuffer.Error = line => "! " + line;
			}
		}

		[Property("asynchronous", "", "")]
		public Asynchronous Asynchronous { get; set; }

		public bool Interactive 
		{
			get { return !(this.LineBuffer is Cli.LineBuffer.Simple); }
			set
			{
				if (value != this.Interactive)
					this.LineBuffer = value ? (Cli.LineBuffer.Abstract)new Cli.LineBuffer.EditorWithHistory(this.terminal) : new Cli.LineBuffer.Simple(this.terminal);
			}
		}

		Parser(object root, bool interactive, Cli.ITerminal terminal)
		{
			this.terminal = terminal;
			this.LineBuffer = interactive ? (Cli.LineBuffer.Abstract)new Cli.LineBuffer.EditorWithHistory(terminal) : new Cli.LineBuffer.Simple(terminal);
			this.Current = this.Root = new Object(root) { Parser = this };
		}
		Tuple<string, Member, string[]> Parse(string line)
		{
			string prefix = "";
			Member member;
			if (line.StartsWith(".."))
			{
				member = this.Current.Parent;
				if (member.IsNull())
					member = this.Current;
				else
					prefix = "..";
				line = line.Substring(2);
			}
			else if (line.StartsWith("."))
			{
				member = this.Root;
				if (this.current != member)
					prefix = ".";
				line = line.Substring(1);
			}
			else
				member = this.current;

			string[] splitted = line.Split(new char[] {' '}, 2, StringSplitOptions.RemoveEmptyEntries);
			Collection.List<string> parameters = new Collection.List<string>();
			if (splitted.Length > 0)
			{
				foreach (string name in splitted[0].Split(new char[] { '.' }))
					if (member is Object)
					{
						Member next = (member as Object).Find(m => m.Name == name);
						if (next.IsNull())
						{
							parameters.Add(name);
							break;
						}
						else
							member = next;
					}
				if (splitted.Length > 1)
					parameters.Add(splitted[1].SplitAt());
			}
			return Tuple.Create(prefix, member, parameters.ToArray());
		}
		bool Execute(string line)
		{
			Tuple<string, Member, string[]> parsed = this.Parse(line);
			return parsed.Item2.Execute(this, parsed.Item3);
		}
		string Complete(string line)
		{
			Tuple<string, Member, string[]> parsed = this.Parse(line);
			return parsed.Item1 + parsed.Item2.NameRelative(this.Current) + parsed.Item2.Complete(parsed.Item3);
		}
		string Help(string line)
		{
			Tuple<string, Member, string[]> parsed = this.Parse(line);
			return parsed.Item2.Help(parsed.Item3);
		}
		bool RequestType(string line)
		{
			line = line.Trim();
			Tuple<string, Member, string[]> parsed = this.Parse(line);
			bool result = false;
			if (parsed.Item2.ToString() != line)
				this.TypeResponse(line, "");
			else
				result = parsed.Item2.RequestType(this);
			return result;
		}
		public void Read()
		{
			this.LineBuffer.Read();
		}
		public void Close()
		{
			if (this.LineBuffer.NotNull())
			{
				this.LineBuffer.Close();
				this.LineBuffer = null;
			}
		}
		internal void Answer(Member member, params string[] parameters)
		{
			this.LineBuffer.WriteLine("$ " + member + " " + string.Join(" ", parameters));
		}
		internal void Notify(Member member, params string[] parameters)
		{
			this.LineBuffer.WriteLine("% " + member + " " + string.Join(" ", parameters));
		}
		internal void Error(Member member, string message, params string[] parameters)
		{
			this.LineBuffer.WriteLine("! " + member + " " + message + " " + string.Join(" ", parameters));
		}
		internal void TypeResponse(string member, string message)
		{
			this.LineBuffer.WriteLine("? " + member + " " + message);
		}
		internal void TypeResponse(Member member, string message)
		{
			this.TypeResponse(member.ToString(), message);
		}
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		public static Parser Read(object root, IO.ICharacterInDevice device)
		{
			Parser result = null;
			if (device.NotNull())
			{
				result = new Parser(root, false, Cli.Terminal.Open(device, null));
				result.Read();
			}
			return result;
		}
		public static Parser Read(object root, Uri.Locator resource)
		{
			return Parser.Read(root, IO.CharacterDevice.Open(IO.ByteDevice.Open(resource, null)));
		}

		public static IDisposable Listen(object root, Uri.Locator resource)
		{
			IDisposable result = null;
			switch (resource.Scheme)
			{
				case "file":
					result = Parser.Read(root, resource);
					break;
				case "telnet":
					result = new IO.Net.Tcp.Server(connection => new Parser(root, true, new Cli.VT100(new IO.Net.Telnet.Server(connection))).Read(), resource.Authority.Endpoint);
					break;
				case "tcp":
					result = new IO.Net.Tcp.Server(connection =>
					{
						Cli.Terminal terminal = Cli.Terminal.Open(connection);
						if (terminal.NotNull())
						{
							terminal.NewLine = new char[] { '\r', '\n' };
							new Parser(root, false, terminal).Read();
						}
					}, resource.Authority.Endpoint);
					break;
				case "console":
					result = Parallel.Thread.Start("console", () => new Parser(root, true, new Cli.ConsoleTerminal()).Read());
					break;
				case "stdio":
					result = Parallel.Thread.Start("stdio", () => new Parser(root, false, Cli.Terminal.Open(IO.ByteDeviceSplitter.Open(Console.OpenStandardInput(), Console.OpenStandardOutput()))).Read());
					break;
			}
			return result;
		}
	}
}
 
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
	public class Editor :
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
				this.lineBuffer.Prompt = string.Format("# {0}> ", this.current);
			}
		}
		Cli.LineBuffer.Abstract lineBuffer;

		Editor(object root, Cli.ITerminal terminal)
		{
			//this.lineBuffer = new Cli.LineBuffer.Simple(terminal);
			//this.lineBuffer = new Cli.LineBuffer.Editor(terminal);
			this.lineBuffer = new Cli.LineBuffer.EditorWithHistory(terminal);
			this.lineBuffer.Execute = this.Execute;
			this.lineBuffer.Complete = this.Complete;
			this.lineBuffer.Help = this.Help;
			this.lineBuffer.RequestType = this.RequestType;
			this.lineBuffer.Error = line => "! " + line;

			this.Current = this.Root = new Object(root);
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
			this.lineBuffer.Read();
		}
		public void Close()
		{
			if (this.lineBuffer.NotNull())
			{
				this.lineBuffer.Close();
				this.lineBuffer = null;
			}
		}
		internal void Answer(Member member, params string[] parameters)
		{
			this.lineBuffer.WriteLine("$ " + member + " " + string.Join(" ", parameters));
		}
		internal void Notify(Member member, params string[] parameters)
		{
			this.lineBuffer.WriteLine("% " + member + " " + string.Join(" ", parameters));
		}
		internal void Error(Member member, string message, params string[] parameters)
		{
			this.lineBuffer.WriteLine("! " + member + " " + message + " " + string.Join(" ", parameters));
		}
		internal void TypeResponse(string member, string message)
		{
			this.lineBuffer.WriteLine("? " + member + " " + message);
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
		public static Editor Create(object root, Cli.ITerminal terminal)
		{
			return terminal.NotNull() ? new Editor(root, terminal) : null;
		}
		public static Editor Read(object root, Cli.ITerminal terminal)
		{
			Editor result = null;
			if (terminal.NotNull())
			{
				result = new Editor(root, terminal);
				result.Read();
			}
			return result;
		}

		public static IDisposable Listen(object root, Uri.Locator resource)
		{
			IDisposable result = null;
			switch (resource.Scheme)
			{
				case "file":
					result = Editor.Read(root, Cli.Terminal.Open(IO.ByteDevice.Open(resource, null)));
					break;
				case "telnet":
					result = new IO.Net.Tcp.Server(connection => new Editor(root, new Cli.VT100(new IO.Net.Telnet.Server(connection))).Read(), resource.Authority.Endpoint);
					break;
				case "tcp":
					result = new IO.Net.Tcp.Server(connection =>
					{
						Cli.Terminal terminal = Cli.Terminal.Open(connection);
						if (terminal.NotNull())
						{
							terminal.NewLine = new char[] { '\r', '\n' };
							new Editor(root, terminal).Read();
						}
					}, resource.Authority.Endpoint);
					break;
				case "console":
					result = Parallel.Thread.Start("console", () => new Editor(root, new Cli.ConsoleTerminal()).Read());
					break;
				case "stdio":
					result = Parallel.Thread.Start("stdio", () => new Editor(root, Cli.Terminal.Open(IO.ByteDeviceSplitter.Open(Console.OpenStandardInput(), Console.OpenStandardOutput()))).Read());
					break;
			}
			return result;
		}
	}
}
 
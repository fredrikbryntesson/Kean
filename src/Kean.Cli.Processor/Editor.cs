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

namespace Kean.Cli.Processor
{
	public class Editor
	{
		internal Object Root { get; private set; }
		Object current;
		internal Object Current 
		{
			get { return this.current; }
			set
			{
				this.current = value;
				this.lineBuffer.CommandPrompt = string.Format("# {0}> ", this.current);
			}
		}
		LineBuffer.Editor lineBuffer;

		public Editor(object root, IO.Stream reader, System.IO.TextWriter writer) :
			this(root, new IO.Reader(reader), writer)
		{ }
		public Editor(object root, IO.Reader reader, System.IO.TextWriter writer)
		{
			this.lineBuffer = new LineBuffer.Editor(reader, writer);
			this.lineBuffer.Execute = this.Execute;
			this.lineBuffer.Complete = this.Complete;
			this.lineBuffer.Help = this.Help;

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
				foreach (string name in splitted[0].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
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
				parameters.Add(splitted[1].Split(new char[] { ' ' },  StringSplitOptions.RemoveEmptyEntries));
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
		public void Read()
		{
			this.lineBuffer.Read();
		}
		internal void Answer(Member member, params string[] parameters)
		{
			this.lineBuffer.WriteLine("$ " + member + parameters.Fold((parameter, a) => a + " " + parameter, " "));
		}
		internal void Notify(Member member, params string[] parameters)
		{
			this.lineBuffer.WriteLine("% " + member + parameters.Fold((parameter, a) => a + " " + parameter, " "));
		}
	}
}
 
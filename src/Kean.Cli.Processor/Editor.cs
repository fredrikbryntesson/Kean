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
		Object current;
		LineBuffer.Editor lineBuffer;

		public Editor(object root, IO.Stream reader, System.IO.TextWriter writer) :
			this(root, new IO.Reader(reader), writer)
		{ }
		public Editor(object root, IO.Reader reader, System.IO.TextWriter writer)
		{
			this.current = new Object(root);
			this.lineBuffer = new LineBuffer.Editor(reader, writer);
			this.lineBuffer.Execute = this.Execute;
			this.lineBuffer.Complete = this.Complete;
			this.lineBuffer.Help = this.Help;
		}
		Tuple<Member, string, string> Parse(string line)
		{
			Member member = this.current;
			string[] splitted = line.Split(new char[] {' '}, 2, StringSplitOptions.RemoveEmptyEntries);
			string incomplete = "";
			if (splitted.Length > 0)
				foreach (string name in splitted[0].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
					if (member is Object)
					{
						Member next = (member as Object).Find(m => m.Name == name);
						if (next.IsNull())
						{
							incomplete = name;
							break;
						}
						else
							member = next;
					}
			return Tuple.Create(member, incomplete, splitted.Length > 1 ? splitted[1].Trim() : null);
		}
		bool Execute(string line)
		{
			bool result = false;
			Tuple<Member, string, string> parsed = this.Parse(line);
			if (result =(parsed.Item1 is Object && parsed.Item2.IsEmpty() && parsed.Item3.IsEmpty()))
			{
			}
			else if (result = (parsed.Item1 is Method && parsed.Item2.IsEmpty()))
			{
			}
			else if (result = (parsed.Item1 is Property && parsed.Item2.IsEmpty()))
			{
				if (parsed.Item3.NotEmpty())
					(parsed.Item1 as Property).Value = parsed.Item3;
				Console.WriteLine("# " + parsed.Item1.ToString() + " " + (parsed.Item1 as Property).Value);
			}
			return result;
		}
		string Complete(string line)
		{
			Tuple<Member, string, string> parsed = this.Parse(line);
			Collection.List<string> alternatives = new Collection.List<string>();
			string result = line;
			if (parsed.Item1 is Object && parsed.Item3.IsEmpty())
			{
				foreach (Member member in (parsed.Item1 as Object))
					if (member.Name.StartsWith(parsed.Item2))
						alternatives.Add(member.Name + (member is Object ? "." : member is Method ? "" : " "));
				result = parsed.Item1.Name.NotEmpty() ? parsed.Item1.ToString() + "." : "";
				if (alternatives.Count > 0)
					for (int i = 0; i < alternatives[0].Length && alternatives.All(s => s[i] == alternatives[0][i]); i++)
						result += alternatives[0][i];
			}
			return result;
		}
		string Help(string line)
		{
			string result = null;
			Tuple<Member, string, string> parsed = this.Parse(line);
			if (parsed.Item1 is Object)
			{
				Collection.List<Tuple<string, string>> results = new Collection.List<Tuple<string, string>>();
				foreach (Member member in (parsed.Item1 as Object))
					if (member.Name.StartsWith(parsed.Item2))
						results.Add(Tuple.Create(member.Name, member.Description));
				result = results.Fold((m, r) => r + m.Item1 + "\t" + m.Item2 + "\n", "");
			}
			else
				result = parsed.Item1.Usage + "\n";
			return result;
		}
		public void Read()
		{
			this.lineBuffer.Read();
		}
	}
}
 
// 
//  Program.cs
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
using IO = Kean.IO;
using Kean.Core.Extension;

namespace Kean.Cli.LineBuffer.Test
{
	class Program
	{
		static void Main(string[] args)
		{
            Editor editor = new Editor(new IO.ConsoleStream() { LocalEcho = false }, Console.Out) { Prompt = "::>" };
            Kean.Core.Tuple<string, string>[] completionDescription = new Kean.Core.Tuple<string, string>[] 
            {
                Kean.Core.Tuple.Create<string, string>("fish", "Fishes are a paraphyletic group of organisms."),
                Kean.Core.Tuple.Create<string, string>("firefox", "Mozilla Firefox is a free and open source web browser."),
                Kean.Core.Tuple.Create<string, string>("foot", "The foot is an anatomical structure found in many vertebrates."),
            };
           
            editor.Complete = text =>
            {
                string result = text;
                string[] parts = text.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                int hits = 0;
                
                if (parts.Length > 0)
                {
                    string last = parts[parts.Length - 1];
                    string found = null;
                    foreach (Kean.Core.Tuple<string, string> word in completionDescription)
                        if (word.Item1.StartsWith(last))
                        {
                            found = word.Item1;
                            hits++;
                        }
                    if(hits == 1)
                        result = text.Remove(text.Length - last.Length) + found;
                }
                return result;
            };
            Kean.Core.Collection.IDictionary<string, Action> commands = new Kean.Core.Collection.Dictionary<string, Action>();
            commands["play"] = () => Console.WriteLine("play");
            commands["beep"] = () => Console.Beep();
            commands["now"] = () => Console.WriteLine("now");
            editor.Execute = text => 
            {
                string[] parts = text.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                bool correct = true;
                foreach (string part in parts)
                    correct &= commands.Contains(part);
                if (correct)
                    foreach (string part in parts)
                        commands[part]();
            };
            editor.Help = text =>
            {
                System.Text.StringBuilder result = new System.Text.StringBuilder();
                string[] parts = text.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0)
                {
                    string last = parts[parts.Length - 1];
                    foreach (Kean.Core.Tuple<string, string> word in completionDescription)
                        if (word.Item1.StartsWith(last))
                            result.AppendLine(word.Item1 + " " + word.Item2);
                }
                return result.ToString();   
            };
            editor.Read();
		}
	}
}

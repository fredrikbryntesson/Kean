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
using IO = Kean.IO;

namespace Kean.Cli.LineBuffer
{
    public class Editor
    {
        object @lock = new object();
        bool executing;
		ITerminal terminal;
        History history;
        int oldmessage = 0;
        public Func<string, bool> Execute { get; set; }
        public Func<string, string> Complete { get; set; }
        public Func<string, string> Help { get; set; }
        public string Prompt { get; set; }
        public Editor(ITerminal terminal)
        {
			this.terminal = terminal;
            this.history = new History(c => this.terminal.Write(c));
        }
        public void Read()
        {
            bool triggerHelp = false;
            bool help = false;
            bool exit = false;
            this.terminal.Write(this.Prompt);
            while (this.terminal.Next() && !exit)
            {
                switch (this.terminal.Last)
                {
                    case (char)0:
                        break;
                    case (char)1: // Home
                        this.history.Current.MoveCursorHome();
                        break;
                    case (char)2: // Left Arrow
                        this.history.Current.MoveCursorLeft();
                        break;
                    case (char)3: // Copy to clipboard
                        break;
                    case (char)4: // Break
                        exit = true;
                        break;
                    case (char)5: // End
                        this.history.Current.MoveCursorEnd();
                        break;
                    case (char)6: // Right Arrow
                        this.history.Current.MoveCursorRight();
                        break;
                    // 7
                    case (char)8: // Backspace
                        this.history.Current.MoveCursorLeftAndDelete();
                        break;
                    case (char)9: // Tab
                        if ((help || this.Complete.IsNull()) && this.Help.NotNull())
                        {
                            this.terminal.WriteLine();
                            this.terminal.Write(this.Help(this.history.Current.ToString()));
                            this.terminal.Write(this.Prompt);
                            this.history.Current.Write();
                        }
                        else if (this.Complete.NotNull())
                        {
                            string old = this.history.Current.ToString();
                            string result = this.Complete(old);
                            if (result == old)
                                triggerHelp = true;
                            else
                            {
                                this.history.Current.Renew(result);
                             }
                        }
                        break;
                    case (char)10: // Newline
                        {

                            this.terminal.WriteLine();
                            this.history.Save();
                            lock (this.@lock)
                                this.executing = true;
						    if (this.Execute.IsNull() || this.Execute(this.history.Current.ToString()))
                            {
                                this.history.Add();
                                this.terminal.Write(this.Prompt);
                            }
                            else
                            {
                                this.terminal.Write(this.Prompt);
                                this.history.Add();
                            }
                            lock (this.@lock)
                                this.executing = false;
                        }
                        break;
                    // 11
                    // 12
                    case (char)13: // Down Arrow

                        if (!this.history.Empty)
                            this.history.Next();
                            break;
                    // 14
                    case (char)15: // Up Arrow
                        if (!this.history.Empty)
                            this.history.Previous();
                        break;
                    // 16
                    // 17
                    // 18
                    // 19
                    // 20
                    case (char)21: // Insert from Clipboard
                        break;
                    // 22
                    case (char)23: // Redo
                        break;
                    case (char)24: // Cut to Clipboard
                        break;
                    case (char)25: // Undo
                        break;
                    default:
                        this.history.Current.Insert(this.terminal.Last);
                        break;
                }
                help = triggerHelp;
                triggerHelp = false;
            }
        }
        public void WriteLine(string value)
        {
            lock (this.@lock)
            {
                if (this.executing)
                    this.terminal.WriteLine(value);
                else
                {
                    this.Remove(-this.oldmessage - this.Prompt.Length - this.history.Current.ToString().Length);
                    this.terminal.WriteLine(value);
                    this.oldmessage = value.Length;
                    this.terminal.Write(this.Prompt);
                    this.history.Current.Write();
                }
            }
        }
        void Remove(int steps)
        {
            if (steps < 0)
                while (steps++ != 0)
                    this.terminal.Write((char)8 + " " + (char)8);
            else
                while (steps-- != 0)
                    this.terminal.Write(" ");
        }
    }
}

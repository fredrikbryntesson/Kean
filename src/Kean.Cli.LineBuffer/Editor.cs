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
        IO.Reader reader;
        System.IO.TextWriter writer;
        Buffer line;
        int oldmessage = 0; 
        public Func<string, bool> Execute { get; set; }
        public Func<string, string> Complete { get; set; }
        public Func<string, string> Help { get; set; }
        public string CommandPrompt { get; set; }
        public string NotifyPrompt { get; set; }
        public Editor(IO.Stream reader, System.IO.TextWriter writer) :
            this(new IO.Reader(reader), writer)
        { }
        public Editor(IO.Reader reader, System.IO.TextWriter writer)
        {
            this.reader = reader;
            this.writer = writer;
            this.line = new Buffer(this.writer.Write);
        }
        public void Read()
        {
            bool triggerHelp = false;
            bool help = false;
            bool exit = false;
            this.writer.Write(this.CommandPrompt);
            while (this.reader.Next() && !exit)
            {
                switch (this.reader.Current)
                {
                    case (char)0:
                        break;
                    case (char)1: // Home
                        this.line.MoveCursorHome();
                        break;
                    case (char)2: // Left Arrow
                        this.line.MoveCursorLeft();
                        break;
                    case (char)3: // Copy to clipboard
                        break;
                    case (char)4: // Break
                        exit = true;
                        break;
                    case (char)5: // End
                        this.line.MoveCursorEnd();
                        break;
                    case (char)6: // Right Arrow
                        this.line.MoveCursorRight();
                        break;
                    // 7
                    case (char)8: // Backspace
                        this.line.MoveCursorLeftAndDelete();
                        break;
                    case (char)9: // Tab
                        if ((help || this.Complete.IsNull()) && this.Help.NotNull())
                        {
                            this.writer.WriteLine();
                            this.writer.Write(this.Help(this.line.ToString()));
                            this.writer.Write(this.CommandPrompt);
                            this.line.Write();
                        }
                        else if (this.Complete.NotNull())
                        {
                            string old = this.line.ToString();
                            string line = this.Complete(old);
                            if (line == old)
                                triggerHelp = true;
                            else
                            {
                                this.line.Renew(line);
                                this.line.Write();
                            }
                        }
                        break;
                    case (char)10: // Newline
                        {
                            this.writer.WriteLine();
                            if (this.Execute.IsNull() || this.Execute(this.line.ToString()))
                            {
                                this.line.Clear();
                                this.writer.Write(this.CommandPrompt);
                            }
                            else
                            {
                                this.writer.Write(this.CommandPrompt); 
                                this.line.Write();
                            }
                           
                        }
                        break;
                    // 11
                    // 12
                    case (char)13: // Down Arrow
                        break;
                    // 14
                    case (char)15: // Up Arrow
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
                        this.line.Insert(this.reader.Current);
                        break;
                }
                help = triggerHelp;
                triggerHelp = false;
            }
        }
        public void Write(string value)
        {
            lock (this.@lock)
            {
                this.Remove(-this.oldmessage - this.CommandPrompt.Length - this.line.ToString().Length);
                string message = this.NotifyPrompt + value;
                this.writer.Write(message);
                this.oldmessage = message.Length;
                this.writer.Write(this.CommandPrompt);
                this.line.Write();
            }
        }
        public void WriteLine(string value)
        {
            lock (this.@lock)
            {
                this.Remove(-this.oldmessage - this.CommandPrompt.Length - this.line.ToString().Length);
                string message = this.NotifyPrompt + value;
                this.writer.Write(message);
                this.oldmessage = message.Length;
                this.writer.Write(this.CommandPrompt);
                this.line.Write();
            }
        }
        void Remove(int steps)
        {
            if (steps < 0)
                while (steps++ != 0)
                    this.writer.Write((char)8 + " " + (char)8);
            else
                while (steps-- != 0)
                    this.writer.Write(" ");
        }
    }
}

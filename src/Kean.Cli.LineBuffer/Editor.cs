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
        IO.Reader reader;
        System.IO.TextWriter writer;
        int cursor;
        System.Text.StringBuilder line = new System.Text.StringBuilder();

        public Func<string, bool> Execute { get; set; }
        public Func<string, string> Complete { get; set; }
        public Func<string, string> Help { get; set; }
        public string Prompt { get; set; }
        public Editor(IO.Stream reader, System.IO.TextWriter writer) :
            this(new IO.Reader(reader), writer)
        { }
        public Editor(IO.Reader reader, System.IO.TextWriter writer)
        {
            this.reader = reader;
            this.writer = writer;
        }
        public void Read()
        {
            bool triggerHelp = false;
            bool help = false;
            bool exit = false;
            this.writer.Write(this.Prompt);
            while (this.reader.Next() && !exit)
            {
                switch (this.reader.Current)
                {
                    case (char)0:
                        break;
                    case (char)1: // Home
                        this.MoveCursor(-this.cursor - 1);
                        this.cursor = 0;
                        break;
                    case (char)2: // Left Arrow
                        if (this.cursor > 0)
                        {
                            this.MoveCursor(-1);
                            this.cursor--;
                        }
                        break;
                    case (char)3: // Copy to clipboard
                        break;
                    case (char)4: // Break
                        exit = true;
                        break;
                    case (char)5: // End
                        this.MoveCursor(this.cursor - this.line.Length);
                        this.cursor = this.line.Length - 1;
                        break;
                    case (char)6: // Right Arrow
                        if (this.cursor < this.line.Length - 1)
                        {
                            this.MoveCursor(1);
                            this.cursor++;
                        }
                        break;
                    // 7
                    case (char)8: // Backspace
                        if (this.cursor > 0)
                        {
                            this.line.Remove(this.line.Length - 1, 1);
                            this.cursor--;
                            this.writer.Write((char)8 + " " + (char)8);
                        }
                        break;
                    case (char)9: // Tab
                        if ((help || this.Complete.IsNull()) && this.Help.NotNull())
                        {
                            string line = this.line.ToString();
                            this.writer.WriteLine();
                            this.writer.Write(this.Help(line));
                            this.writer.Write(this.Prompt);
                            this.writer.Write(line);
                        }
                        else if (this.Complete.NotNull())
                        {
                            string old = this.line.ToString();
                            string line = this.Complete(old);
                            if (line == old)
                                triggerHelp = true;
                            else
                            {
                                this.MoveCursor(-old.Length);
                                this.cursor = line.Length;
                                this.line = new System.Text.StringBuilder(line);
                                this.writer.Write(line.ToString());
                            }
                        }
                        break;
                    case (char)10: // Newline
                        {
							this.writer.WriteLine();
                            this.writer.Write(this.Prompt);
                            if (this.Execute.IsNull() || this.Execute(this.line.ToString()))
							{
								this.line = new System.Text.StringBuilder();
								this.cursor = 0;
							}
							else
								Console.Write(this.line.ToString());
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
                        this.line.Insert(this.cursor, this.reader.Current);
                        this.writer.Write(this.line.ToString().Substring(this.cursor++));
                        this.MoveCursor(this.cursor - this.line.Length);
                        //Console.Error.WriteLine(this.reader.Current + ": " + ((int)this.reader.Current));
                        break;
                }
                help = triggerHelp;
                triggerHelp = false;
            }
        }
        void MoveCursor(int steps)
        {
            if (steps < 0)
            {
                char[] buffer = new char[-steps];
                while (steps < 0)
                    buffer[-++steps] = (char)8;
                this.writer.Write(buffer);
            }
            else if (steps > 0)
                this.writer.Write(this.line.ToString().Substring(this.cursor, steps));
        }
    }
}

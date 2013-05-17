// 
//  Edit.cs
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
using System.Text;

namespace Kean.Cli.LineBuffer.Buffer
{
    class Edit : 
		Abstract
    {
		Action<string> write;
        int cursor = 0;
        StringBuilder line = new StringBuilder();
		protected override string Value { get { return this.line.ToString(); } }
        public Edit(Action<string> write)
        {
            this.write = write;
        }
        public override void MoveCursorHome()
        {
            this.MoveCursor(-this.cursor);
            this.cursor = 0;
        }
		public override void MoveCursorLeft()
        {
            if (this.cursor > 0)
            {
                this.MoveCursor(-1);
                this.cursor--;
            }
        }
		public override void MoveCursorEnd()
        {
            this.MoveCursor(this.line.Length - this.cursor);
            this.cursor = this.line.Length;
        }
		public override void MoveCursorRight()
        {
            if (this.cursor < this.line.Length)
            {
                this.MoveCursor(1);
                this.cursor++;
            }
        }
		public override void DeletePreviousCharacter()
        {
            if (this.cursor > 0)
            {
                this.line.Remove(this.cursor - 1, 1);
                this.cursor--;
                this.write((char)8 + " " + (char)8);
                this.write(this.line.ToString().Substring(this.cursor, this.line.Length - this.cursor));
                this.write(" " + (char)8);
                this.MoveCursor(this.cursor - this.line.Length);
            }
        }
		public override void MoveCursorLeftAndNotDelete()
        {
            if (this.cursor > 0)
            {
                this.cursor--;
                this.write((char)8 + " " + (char)8);
            }
        }
		public override void DeleteCurrentCharacter()
		{
			if (this.cursor >= 0 && this.cursor < this.line.Length)
			{
				this.line.Remove(this.cursor, 1);
				this.write(" " + (char)8);
				this.write(this.line.ToString().Substring(this.cursor, this.line.Length - this.cursor));
				this.write(" " + (char)8);
				this.MoveCursor(this.cursor - this.line.Length);
			}
		}
		public override void Insert(char value)
        {
            this.line.Insert(this.cursor, value);
            this.write(this.line.ToString().Substring(this.cursor++));
            this.MoveCursor(this.cursor - this.line.Length);
        }
		public override void Insert(string value)
        {
            this.line.Insert(this.cursor, value);
            this.write(this.line.ToString().Substring(this.cursor));
            this.cursor += value.Length;
            this.MoveCursor(this.cursor - this.line.Length);
        }
		public override void Renew(string line)
        {
            this.RemoveAndDelete();
            this.line = new System.Text.StringBuilder(line);
            this.Write();
            this.cursor = line.Length;
        }
		public override void MoveCursor(int steps)
        {
            if (steps < 0)
            {
                char[] buffer = new char[-steps];
                while (steps < 0)
                    buffer[-++steps] = (char)8;
                this.write(new String(buffer));
            }
            else if (steps > 0)
                this.write(this.line.ToString().Substring(this.cursor, steps));
        }
		public override void Write()
        {
            this.write(this.line.ToString());
            this.cursor = this.line.Length;
        }
        void RemoveAndDelete()
        {
            this.MoveCursorEnd();
            while (this.line.Length > 0)
                this.DeletePreviousCharacter();
            this.cursor = 0;
        }
		public override void RemoveAndNotDelete()
        {
            this.MoveCursorEnd();
            int length = this.line.Length;
            while (length-- > 0)
                this.MoveCursorLeftAndNotDelete();
            this.cursor = 0;
        }
        public override Abstract Copy()
        {
            Edit result = new Edit(this.write);
            result.cursor = this.cursor;
            result.line = new StringBuilder(this.ToString());
            return result;
        }
    }
}

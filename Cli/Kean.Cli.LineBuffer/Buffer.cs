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
using System.Text;

namespace Kean.Cli.LineBuffer
{
    class Buffer
    {
		Action<string> write;
        int cursor = 0;
        StringBuilder line = new StringBuilder();
        public Buffer(Action<string> write)
        {
            this.write = write;
        }
        public void MoveCursorHome()
        {
            this.MoveCursor(-this.cursor);
            this.cursor = 0;
        }
        public void MoveCursorLeft()
        {
            if (this.cursor > 0)
            {
                this.MoveCursor(-1);
                this.cursor--;
            }
        }
        public void MoveCursorEnd()
        {
            this.MoveCursor(this.line.Length - this.cursor);
            this.cursor = this.line.Length;
        }
        public void MoveCursorRight()
        {
            if (this.cursor < this.line.Length - 1)
            {
                this.MoveCursor(1);
                this.cursor++;
            }
        }
        public void DeletePreviousCharacter()
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
        public void MoveCursorLeftAndNotDelete()
        {
            if (this.cursor > 0)
            {
                this.cursor--;
                this.write((char)8 + " " + (char)8);
            }
        }
		public void DeleteCurrentCharacter()
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
        public void Insert(char value)
        {
            this.line.Insert(this.cursor, value);
            this.write(this.line.ToString().Substring(this.cursor++));
            this.MoveCursor(this.cursor - this.line.Length);
        }
        public void Insert(string value)
        {
            this.line.Insert(this.cursor, value);
            this.write(this.line.ToString().Substring(this.cursor));
            this.cursor += value.Length;
            this.MoveCursor(this.cursor - this.line.Length);
        }
        public void Renew(string line)
        {
            this.RemoveAndDelete();
            this.line = new System.Text.StringBuilder(line);
            this.Write();
            this.cursor = line.Length;
        }
        public void MoveCursor(int steps)
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
        public void Write()
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
        public void RemoveAndNotDelete()
        {
            this.MoveCursorEnd();
            int length = this.line.Length;
            while (length-- > 0)
                this.MoveCursorLeftAndNotDelete();
            this.cursor = 0;
        }
        public Buffer Copy()
        {
            Buffer result = new Buffer(this.write);
            result.cursor = this.cursor;
            result.line = new StringBuilder(this.ToString());
            return result;
        }
        #region Object overides and IEquatable<Buffer>
        public override int GetHashCode()
        {
            return this.line.ToString().GetHashCode();
        }
        public override string ToString()
        {
            return this.line.ToString();
        }
        #endregion
        #region  IEquatable<Buffer>
        public override bool Equals(object other)
        {
            return (other is Buffer) && this.Equals((Buffer)other);
        }
        // other is not null here.
        public bool Equals(Buffer other)
        {
            return this.line.ToString() == other.line.ToString();
        }
        #endregion
        #region Comparison Functions and IComparable<Buffer>
        public static bool operator ==(Buffer left, Buffer right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Buffer left, Buffer right)
        {
            return !(left == right);
        }
        #endregion
    }
}

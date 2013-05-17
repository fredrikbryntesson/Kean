// 
//  Buffer.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2013 Simon Mika
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
using Text = Kean.IO.Text;

namespace Kean.Cli.LineBuffer.Buffer
{
    class Simple : 
		Abstract
    {
		Text.Builder line = new Text.Builder();
		protected override string Value { get { return this.line; } }
		public Simple()
        { }
		protected Simple(Simple original)
		{
			this.line = original.line.Copy();
		}
        public override void MoveCursorHome() { }
		public override void MoveCursorLeft() { }
		public override void MoveCursorEnd() { }
		public override void MoveCursorRight() { }
		public override void DeletePreviousCharacter() { }
		public override void MoveCursorLeftAndNotDelete() { }
		public override void DeleteCurrentCharacter() { }
		public override void Insert(char value)
        {
            this.line += value;
        }
		public override void Insert(string value)
        {
            this.line += value;
        }
		public override void Renew(string line)
        {
			this.line = line;
        }
		public override void MoveCursor(int steps) { }
		public override void Next()	{ }
		public override void Previous() { }
		public override void RemoveAndNotDelete() { }
		public override void Write() { }
        public override Abstract Copy()
        {
            return new Simple(this);
        }
		public override void AddNewCommand() { }
    }
}

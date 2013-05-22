// 
//  EditorWithHistory.cs
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
using Kean.Core;
using Kean.Core.Extension;
using Integer = Kean.Math.Integer;

namespace Kean.Cli.LineBuffer
{
	public class EditorWithHistory :
		Editor
	{
		int position = 0;
		int Position
		{
			get { return position; }
			set { this.position = Integer.Modulo(value, this.size); }
		}
		int last = 0;
		int size = 50;
		int first = 0;
		string[] history;

		public EditorWithHistory(ITerminal terminal) :
			base(terminal)
		{
			this.history = new string[this.size];
		}

		protected override void OnUp()
		{
			if (this.position != this.first)
			{
				if (this.position == this.last)
					this.history[this.position] = this.Current;
				this.Position--;
				this.ReplaceLine();
			}
		}

		protected override void OnDown()
		{
			if (this.position != this.last)
			{
				this.Position++;
				this.ReplaceLine();
			}
		}

		void ReplaceLine()
		{
			this.ClearCurrent();
			this.buffer.Set(this.history[this.position]);
			this.cursor = this.length = this.history[this.position].Length;
			this.terminal.ReplaceLine(this.Prompt + this.Current);
		}

		protected override void OnEnter()
		{
			if (this.Current.NotEmpty() && this.Current != this.history[Integer.Modulo(this.last - 1, this.size)])
			{
				this.history[this.last] = this.Current;
				this.last = (this.last + 1) % this.size;
				if (this.last == this.first)
					this.first = (this.first + 1) % this.size;
			}
			this.position = this.last;
			base.OnEnter();
		}
	}
}

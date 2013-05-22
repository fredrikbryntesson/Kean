// 
//  Editor.cs
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

namespace Kean.Cli.LineBuffer
{
	public class Editor :
		Abstract
	{
		internal Buffer buffer = new Buffer();
		protected int cursor;
		protected int length;
		protected bool help;

		public Editor(ITerminal terminal) :
			base(terminal)
		{ }

		protected override string Current { get { return this.buffer.ToString(); } }

		protected override int CurrentLength { get { return this.length; } }

		protected override void Insert(char c)
		{
			if (this.cursor == this.length)
			{
				this.buffer.Append(c);
				this.length++;
				this.terminal.Out.Write(c);
				this.cursor++;
			}
			else
			{
				this.buffer.Insert(this.cursor, c);
				this.length++;
				this.terminal.Out.Write(this.buffer.Substring(cursor));
				this.cursor++;
				this.MoveCursor(this.cursor - this.length);
			}
		}

		protected override void OnDelete()
		{
			if (this.cursor < this.length)
			{
				this.buffer.Delete(this.cursor);
				this.terminal.Out.Write(this.buffer.Substring(cursor) + " ");
				this.MoveCursor(this.cursor - this.length);
				this.length--;
			}
		}

		protected override void OnBackspace()
		{
			if (this.cursor > 0)
			{
				this.cursor--;
				this.buffer.Delete(this.cursor);
				this.MoveCursor(-1);
				this.terminal.Out.Write(this.buffer.Substring(cursor) + " ");
				this.MoveCursor(this.cursor - this.length);
				this.length--;
			}
		}

		protected override void OnLeft()
		{
			if (this.cursor > 0)
			{
				this.cursor--;
				this.MoveCursor(-1);
			}
		}

		protected override void OnRight()
		{
			if (this.cursor < this.length)
			{
				this.cursor++;
				this.MoveCursor(1);
			}
		}

		protected override void OnEnd()
		{
			this.MoveCursor(this.length - this.cursor);
			this.cursor = this.length;
		}

		protected override void OnHome()
		{
			this.MoveCursor(-this.cursor);
			this.cursor = 0;
		}

		protected override void ClearCurrent()
		{
			this.buffer.Clear();
			this.cursor = 0;
			this.length = 0;
		}

		protected override void OnTab()
		{
			if ((this.help || this.Complete.IsNull()) && this.Help.NotNull())
			{
				this.terminal.Out.WriteLine();
				this.terminal.Out.Write(this.Help(this.Current));
				this.terminal.Out.Write(this.Prompt);
				this.terminal.Out.Write(this.Current);
				this.help = false;
			}
			else if (this.Complete.NotNull())
			{
				string result = this.Complete(this.Current);
				if (result == this.Current)
					this.help = true;
				else
				{
					this.buffer.Set(result);
					this.cursor = result.Length;
					this.length = cursor;
				}
			}
		}
		protected virtual bool MoveCursor(int delta)
		{
			return delta == 0 || this.terminal.MoveCursor(delta) || this.terminal.Out.Write(delta > 0 ? this.buffer.Substring(this.cursor - 1, delta) : new string('\b', -delta));
		}
	}
}

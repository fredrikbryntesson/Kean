// 
//  EditWithHistory.cs
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
using Integer = Kean.Math.Integer;

namespace Kean.Cli.LineBuffer.Buffer
{
    class EditWithHistory : 
		Edit
    {
		int position = 0;
		int last = 0;
		int size = 5;
		int first = 0;
		string[] buffers;

		public EditWithHistory(Action<string> write) :
			base(write)
		{
			this.buffers = new string[this.size];
		}
		public override void Next() 
		{
			if (this.position != this.last)
			{
				this.position = Integer.Modulo(this.position + 1, this.size);
				this.Renew(this.buffers[this.position]);
			}
		}
		public override void Previous()
		{
			if (this.position != this.first)
			{
				if (this.position == this.last)
				{
					this.buffers[this.position] = this.Value;
				}
				this.position = Integer.Modulo(this.position - 1, this.size);
				this.Renew(this.buffers[this.position]);
				//this.write(this.line.ToString());
			}
		}
		public override void AddNewCommand()
		{
			if (this.line.ToString() != this.buffers[Integer.Modulo(this.last - 1, this.size)])
			{
				this.buffers[this.last] = this.Value;
				this.last = (this.last + 1) % this.size;
				if (this.last == this.first)
					this.first = (this.first + 1) % this.size;
			}
			this.position = this.last;
			this.write(this.Value);
		}
    }
}

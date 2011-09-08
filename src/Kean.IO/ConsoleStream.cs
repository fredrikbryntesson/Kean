// 
//  ConsoleStream.cs
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

namespace Kean.IO
{
	public class ConsoleStream :
		Stream
	{
		System.IO.Stream output;
		public bool LocalEcho { get; set; }
		public override bool Opened { get { return true; } }
		public override bool Ended { get { return false; } }
		public ConsoleStream()
		{
			this.LocalEcho = true;
			this.output = Console.OpenStandardOutput();
		}
		public override int Read()
		{
			ConsoleKeyInfo key = Console.ReadKey(!this.LocalEcho);
			char result = (char)0;
			switch (key.Key)
			{
				case ConsoleKey.Home: result = (char)1; break;
				case ConsoleKey.LeftArrow: result = (char)2; break;
					// 3 Copy to clipboard
				case ConsoleKey.Delete: result = (char)4; break;
				case ConsoleKey.End: result = (char)5; break;
				case ConsoleKey.RightArrow: result = (char)6; break;
					// 7
				//case ConsoleKey.Backspace: result = (char)8; break;
				//case ConsoleKey.Tab: result = (char)9; break;
                case ConsoleKey.Enter: result = (char)10; break;
					// 11
					// 12
				case ConsoleKey.DownArrow: result = (char)13; break;
					// 14
				case ConsoleKey.UpArrow: result = (char)15; break;
					// 16
					// 17
					// 18
					// 19
					// 20
					// 21 Insert from clipboard
					// 22
					// 23 Redo
					// 24 Cut to clipboard
					// 25 Undo
				default: result = key.KeyChar; break;
			}
			return result;
		}
		public override int Peek()
		{
			return -1;
		}
		public override void Write(byte value)
		{
			this.output.WriteByte(value);
		}
		public override void Close()
		{
			this.output.Close();
		}
	}
}

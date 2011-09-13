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
using Kean.Core;
using Kean.Core.Extension;

namespace Kean.Cli
{
	class ConsoleDevice :
		IO.ICharacterInDevice,
		IO.ICharacterOutDevice
	{
		public bool LocalEcho { get; set; }
		public bool Opened { get { return true; } }
		public bool Empty { get { return !Console.KeyAvailable; } }
		public ConsoleDevice()
		{
			this.LocalEcho = true;
		}
		public char? Read()
		{
			ConsoleKeyInfo key = Console.ReadKey(!this.LocalEcho);
			char? result = null;
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
		public char? Peek()
		{
			return null;
		}
		public bool Write(System.Collections.Generic.IEnumerable<char> buffer)
		{
			foreach (char c in buffer)
				Console.Write(c);
			return true;
		}
		public bool Close()
		{
			return true;
		}
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
	}
}

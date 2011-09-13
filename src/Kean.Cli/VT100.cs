// 
//  VT100.cs
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

namespace Kean.Cli
{
	public class VT100 :
		Terminal
	{
		#region Constructors
		public VT100(IO.ICharacterDevice device) :
			this(device, device)
		{ }
		public VT100(IO.ICharacterInDevice inDevice, IO.ICharacterOutDevice outDevice) :
			base(inDevice, outDevice)
		{
			this.NewLine = new char[] { '\r', '\n' };
			this.Write("initializing...");
			// Reset to inital state RIS   ESC c
			this.Write((char)0x1b, 'c');
			// Keyboard auto repeat mode off (local echo off)  ESC [ ? 8 h 
			this.Write((char)0x1b, '[', '1', '2', 'h');
			this.WriteLine("done");
		}
		#endregion
	}
}

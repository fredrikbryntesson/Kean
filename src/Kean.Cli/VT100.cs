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
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Geometry2D = Kean.Math.Geometry2D;

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
			this(new IO.Filter.CharacterInDevice(inDevice), outDevice)
		{ }
		VT100(IO.Filter.CharacterInDevice inDevice, IO.ICharacterOutDevice outDevice) :
			base(inDevice, outDevice)
		{
			inDevice.Filter = this.FilterInput;
			this.Out.NewLine = new char[] { '\r', '\n' };
		}
		#endregion
		public override bool Clear()
		{
			return this.Out.Write('\x1b', '[', '2', 'J');
		}
		char[] FilterInput(Func<char?> read)
		{
			Collection.IList<char> buffer = new Collection.List<char>();
			char? next;
			while (buffer.Count <= 0 && (next = read()).HasValue)
				switch (next.Value)
				{
					case '\x1b': // ESC
						if ((next = read()).HasValue)
							switch (next.Value)
							{
								case '[': // Arrow keys
									if ((next = read()).HasValue)
										switch (next.Value)
										{
											case 'A': this.OnCommand(EditCommand.UpArrow); break;
											case 'B': this.OnCommand(EditCommand.DownArrow); break;
										}
									break;
							}
						break;
					default:
						buffer.Add(next.Value);
						break;
				}
			return buffer.ToArray();
		}
	}
}

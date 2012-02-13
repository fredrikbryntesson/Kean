// 
//  Byte.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;

namespace Kean.Core.Serialize.Data
{
	public class Byte :
		Leaf<byte>
	{
		public override string Text { get { return this.Value.ToString(System.Globalization.NumberFormatInfo.InvariantInfo); } }
		public override byte[] Raw { get { return BitConverter.GetBytes(this.Value); } }
		public Byte(byte value) :
			base(value)
		{ }
		public Byte(object value, Reflect.Type type) :
			base(value, type)
		{ }
		public static Byte Create(string value)
		{
			byte result;
			return byte.TryParse(value, out result) ? new Byte(result) : null;
		}
		public static Byte Create(byte[] value)
		{
			return value.Length == 1 ? new Byte(value[0]) : null;
		}
	}
}

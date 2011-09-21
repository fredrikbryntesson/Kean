// 
//  Boolean.cs
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
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;

namespace Kean.Core.Serialize.Data
{
	public class Boolean :
		Leaf<bool>
	{
		public override string Text { get { return this.Value ? "true" : "false"; } }
		public override byte[] Binary { get { return new byte[] { Convert.ToByte(this.Value) }; } }
		public Boolean(bool value) :
			base(value)
		{ }
		public static Boolean Create(string value)
		{
			Boolean result;
			switch (value)
			{
				case "true":
					result = new Boolean(true);
					break;
				case "false":
					result = new Boolean(false);
					break;
				default:
					result = null;
					break;
			}
			return result;
		}
		public static Boolean Create(byte[] value)
		{
			return value.Length == 1 ? new Boolean(value[0] > 0) : null;
		}
	}
}

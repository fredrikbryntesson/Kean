// 
//  Enumeration.cs
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
	public class Enumeration :
		Leaf<object>
	{
		Reflect.Type type;
		public override string Text { get { return this.Value.ToString(); } }
		public override byte[] Binary 
		{ 
			get 
			{
				byte[] result;
				switch ((Reflect.Type)Enum.GetUnderlyingType(this.Type))
				{
					case "sbyte": result = new byte[] { unchecked((byte)(sbyte)this.Value) }; break;
					case "byte": result = new byte[] { (byte)this.Value }; break;
					case "short": result = BitConverter.GetBytes((short) this.Value); break;
					case "ushort": result = BitConverter.GetBytes((ushort)this.Value); break;
					case "int": result = BitConverter.GetBytes((int)this.Value); break;
					case "uint": result = BitConverter.GetBytes((uint)this.Value); break;
					case "long": result = BitConverter.GetBytes((long)this.Value); break;
					case "ulong": result = BitConverter.GetBytes((ulong)this.Value); break;
					default: result = new byte[0]; break;
				}
				return result;
			} 
		}
		public Enumeration(object value, Reflect.Type type) :
			base(value)
		{
			this.type = type;
		}
		public static Enumeration Create(string value, Reflect.Type type)
		{
			return new Enumeration((Enum)Enum.Parse(type, value), type);
		}
		public static Enumeration Create(byte[] value, Reflect.Type type)
		{
			Enumeration result = null;
			switch ((Reflect.Type)Enum.GetUnderlyingType(type))
			{
				case "sbyte": if (value.Length == 1) result = new Enumeration((Enum)Enum.ToObject(type, (sbyte)value[0]), type); break;
				case "byte": if (value.Length == 1) result = new Enumeration((Enum)Enum.ToObject(type, value[0]), type); break;
				case "short": if (value.Length == 2) result = new Enumeration((Enum)Enum.ToObject(type, BitConverter.ToInt32(value, 0)), type); break;
				case "ushort": if (value.Length == 2) result = new Enumeration((Enum)Enum.ToObject(type, BitConverter.ToUInt32(value, 0)), type); break;
				case "int": if (value.Length == 4) result = new Enumeration((Enum)Enum.ToObject(type, BitConverter.ToInt32(value, 0)), type); break;
				case "uint": if (value.Length == 4) result = new Enumeration((Enum)Enum.ToObject(type, BitConverter.ToUInt32(value, 0)), type); break;
				case "long": if (value.Length == 8) result = new Enumeration((Enum)Enum.ToObject(type, BitConverter.ToInt64(value, 0)), type); break;
				case "ulong": if (value.Length == 8) result = new Enumeration((Enum)Enum.ToObject(type, BitConverter.ToUInt64(value, 0)), type); break;
			}
			return result;
		}
	}
}

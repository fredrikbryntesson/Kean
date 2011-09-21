// 
//  Leaf.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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
using Reflect = Kean.Core.Reflect;
using Kean.Core.Reflect.Extension;

namespace Kean.Core.Serialize.Data
{
	public abstract class Leaf :
		Node
	{
		public abstract string Text { get; }
		public abstract byte[] Binary { get; }
		protected Leaf()
		{ }
		#region Creators
		public static Leaf Create(Reflect.Type type, string data)
		{
			Leaf result = null;
			switch (type.Category)
			{
				case Reflect.TypeCategory.Enumeration:
					result = Enumeration.Create(data, type);
					break;
				case Reflect.TypeCategory.Structure:
				case Reflect.TypeCategory.Class:
				case Reflect.TypeCategory.Primitive:
					switch (type)
					{
						case "bool": result = Boolean.Create(data); break;
						case "int": result = Integer.Create(data); break;
						case "byte": result = Byte.Create(data); break;
						case "char": result = Character.Create(data); break;
						case "long": result = Long.Create(data); break;
						case "short": result = Short.Create(data); break;
						case "uint": result = UnsignedInteger.Create(data); break;
						case "sbyte": result = SignedByte.Create(data); break;
						case "ulong": result = UnsignedLong.Create(data); break;
						case "ushort": result = UnsignedShort.Create(data); break;
						case "float": result = Single.Create(data); break;
						case "double": result = Double.Create(data); break;
						case "decimal": result = Decimal.Create(data); break;
						case "System.DateTime": result = DateTime.Create(data); break;
						case "System.TimeSpan": result = TimeSpan.Create(data); break;
						case "string": result = String.Create(data); break;
					}
					break;
			}
			return result;
		}
		public static Leaf Create(Reflect.Type type, byte[] data)
		{
			Leaf result = null;
			switch (type)
			{
				case "bool": result = Boolean.Create(data); break;
				case "string": result = String.Create(data); break;
			}
			return result;
		}
		#endregion
	}
	public abstract class Leaf<T> :
		Leaf
	{
		public T Value { get; private set; }
		protected Leaf(T value)
		{
			this.Value = value;
		}
	}
}

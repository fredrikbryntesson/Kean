// 
//  Method.cs
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
using Kean.Core.Basis.Extension;

namespace Kean.Core.Serialize.Serializer
{
	public class Method :
		Abstract
	{
		Collection.Dictionary<Reflect.TypeName, ISerializer> cache = new Collection.Dictionary<Reflect.TypeName, ISerializer>();
		public Method()
		{
		}
		public override bool Accepts (Type type)
		{
			return !type.IsPrimitive && this.GetSerializer(type).NotNull();
		}
		protected override T Deserialize<T> (Storage storage, Reflect.TypeName type, Data.Node data)
		{
			return this.GetSerializer(type).Deserialize<T>(storage, data);
		}
		protected override Data.Node Serialize<T> (Storage storage, Reflect.TypeName type, T data)
		{
			return this.GetSerializer(type).Serialize<T>(storage, data);
		}
		ISerializer GetSerializer(Reflect.TypeName type)
		{
			ISerializer result = null;
			if (cache.Contains(type))
				result = cache[type];
			else
			{
				object[] attributes = ((Type)type).GetCustomAttributes(typeof(MethodAttribute), true);
				if (attributes.Length == 1)
					result = (attributes[0] as MethodAttribute).Serializer;
				cache[type] = result;
			}
			return result;
		}
	}
}


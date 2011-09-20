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

namespace Kean.Core.Serialize.Serializer
{
	public class Enumeration :
		ISerializer
	{
		public Enumeration()
		{
		}
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type)
		{
			return type.Category == Reflect.TypeCategory.Enumeration ? this : null;
		}
		public Data.Node Serialize(Storage storage, Reflect.Type type, object data)
		{
			return new Data.Leaf<string>(Enum.GetName(type, data));
		}
		public object Deserialize(Storage storage, Kean.Core.Reflect.Type type, Kean.Core.Serialize.Data.Node data)
		{
			return data is Data.Leaf<string> ? Enum.Parse(data.Type ?? type, (data as Data.Leaf<string>).Value) : Enum.ToObject(type, 0);
		}
		#endregion
	}
}


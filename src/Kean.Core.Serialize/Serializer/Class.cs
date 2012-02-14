// 
//  Class.cs
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
using Kean.Core.Reflect.Extension;

namespace Kean.Core.Serialize.Serializer
{
	public class Class :
		ISerializer
	{
		public Class()
		{ }
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type)
		{
			return type.Category == Reflect.TypeCategory.Class ? this : null;
		}
		public Data.Node Serialize(Storage storage, Reflect.Type type, object data)
		{
			Data.Branch result = new Data.Branch(data, type);
			foreach (Reflect.Property property in data.GetProperties())
			{
				ParameterAttribute[] attributes = property.GetAttributes<ParameterAttribute>();
				if (attributes.Length == 1)
					result.Nodes.Add(storage.Serializer.Serialize(storage, property.Type, property.Data).UpdateName(property.Name).UpdateAttribute(attributes[0]));
			}
			return result;
		}
		public object Deserialize(Storage storage, Data.Node data)
		{
			object result = data.Type.Create();
			Reflect.Property[] properties = result.GetProperties();
			foreach (Data.Node node in (data as Data.Branch).Nodes)
			{
				Reflect.Property property = properties.Find(f => f.Name == node.Name);
				property.Data = storage.Serializer.Deserialize(storage, node.DefaultType(property.Type));
			}
			return result;
		}
		#endregion
	}
}


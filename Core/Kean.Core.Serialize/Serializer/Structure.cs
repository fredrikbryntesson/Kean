// 
//  Structure.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2013 Simon Mika
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
	public class Structure :
		ISerializer
	{
		public Structure()
		{
		}
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type)
		{
			return type.Category == Reflect.TypeCategory.Structure ? this : null;
		}
		public Data.Node Serialize(Storage storage, Reflect.Type type, object data, Uri.Locator locator)
		{
			Data.Branch result = new Data.Branch(data, type);
			foreach (Reflect.Field field in data.GetFields())
			{
				Uri.Locator l = locator.Copy();
				l.Fragment = (l.Fragment.NotEmpty() ? l.Fragment + "." : "") + field.Name;
				result.Nodes.Add(storage.Serialize(field.Type, field.Data, l).UpdateName(field.Name));
			}
			return result;
		}
		public object Deserialize(Storage storage, Data.Node data, object result)
		{
			result = data.Type.Create();
			Reflect.Field[] fields = result.GetFields();
			foreach (Data.Node node in (data as Data.Branch).Nodes)
			{
				Reflect.Field field = fields.Find(f => f.Name == node.Name);
				if (field.NotNull())
					storage.Deserialize(node, field.Type, d => field.Data = d);
				else
					new Exception.FieldMissing(data.Type, node.Name, node.Region).Throw();
			}
			return result;
		}
		#endregion
	}
}


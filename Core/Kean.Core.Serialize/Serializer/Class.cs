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
		public Data.Node Serialize(Storage storage, Reflect.Type type, object data, Uri.Locator locator)
		{
			Data.Node result;
			Uri.Locator l = storage.Resolver.Update(data, locator);
			if (l.NotNull())
				result = new Data.Link(l);
			else
			{
				result = new Data.Branch(data, type);
				foreach (Reflect.Property property in data.GetProperties())
				{
					ParameterAttribute[] attributes = property.GetAttributes<ParameterAttribute>();
					if (attributes.Length == 1 && property.Data.NotNull())
					{
						string name = attributes[0].Name ?? property.Name;
						l = locator.Copy();
						l.Fragment = (l.Fragment.NotEmpty() ? l.Fragment + "." : "") + name;
						(result as Data.Branch).Nodes.Add(storage.Serialize(property.Type, property.Data, l).UpdateName(name).UpdateAttribute(attributes[0]).UpdateLocator(locator));
					}
				}
			}
			return result;
		}
		public object Deserialize(Storage storage, Data.Node data, object result)
		{
            if (result.IsNull())
			    result = data.Type.Create();
			Reflect.Property[] properties = result.GetProperties();
			if (data is Data.Branch)
				foreach (Data.Node node in (data as Data.Branch).Nodes)
				{
					Reflect.Property property = properties.Find(p => {
						Core.Serialize.ParameterAttribute[] attributes;
						return p.Name == node.Name || (attributes = p.GetAttributes<Core.Serialize.ParameterAttribute>()).Length > 0 && attributes[0].Name == node.Name;
					});
					if (property.IsNull())
						new Exception.PropertyMissing(data.Type, node.Name, node.Region).Throw();
                    else if (!property.Writable)
                    {
                        if (property.Readable && (property.Type.Category == Reflect.TypeCategory.Class || property.Type.Category == Reflect.TypeCategory.Array || property.Type.Category == Reflect.TypeCategory.Interface))
                            storage.DeserializeContent(node.DefaultType(property.Type), property.Data);
                        else
                            new Exception.PropertyNotWriteable(data.Type, node.Name, node.Region).Throw();
                    }
                    else
						storage.Deserialize(node, property.Type, d => property.Data = d);
				}
			return result;
		}
		#endregion
	}
}


// 
//  Array.cs
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
	public class Array :
		ISerializer
	{
		public Array()
		{ }
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type)
		{
			return type.Category == Reflect.TypeCategory.Array ? this : null;
		}
		public Data.Node Serialize(Storage storage, Reflect.Type type, object data, Uri.Locator locator)
		{
			Data.Collection result = new Data.Collection(data, type);
			Reflect.Type elementType = ((System.Type)data.Type()).GetElementType();
			int c = 0;
			foreach (object child in data as System.Array)
				result.Nodes.Add(storage.Serialize(elementType, child, locator + "[" + c++ + "]"));
			return result;
		}
		public object Deserialize(Storage storage, Data.Node data)
		{
			System.Array result = null;
			Core.Collection.IList<Data.Node> nodes;
			Reflect.Type elementType;
			if (data is Data.Collection)
			{
				nodes = (data as Data.Collection).Nodes;
				elementType = ((System.Type)data.Type).GetElementType();
			}
			else 
			{ // only one element in array so it was impossible to know it was a collection
				nodes = new Kean.Core.Collection.List<Data.Node>(data);
				elementType = data.Type;
			}

			result = System.Array.CreateInstance(elementType, nodes.Count);
			int i = 0;
			foreach (Data.Node child in nodes)
			{
				int c = i++; // ensure c is unique in every closure of lambda function below
				storage.Deserialize(child.DefaultType(elementType), d => result.SetValue(d, c));
			}
			return result;
		}
		#endregion
	}
}


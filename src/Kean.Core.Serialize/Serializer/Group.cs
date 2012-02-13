// 
//  Group.cs
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
	public class Group :
		ISerializer
	{
		ISerializer[] serializers;
		public Group(params ISerializer[] serializers)
		{
			this.serializers = serializers;
		}
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type)
		{
			ISerializer result = null;
			foreach (ISerializer serializer in this.serializers)
				if ((result = serializer.Find(type)).NotNull())
					break;
			return result;
		}
		public Data.Node Serialize(Storage storage, Reflect.Type type, object data)
		{
			ISerializer serializer = this.Find(data.Type());
			return serializer.NotNull() ? serializer.Serialize(storage, type, data) : null;
		}
		public object Deserialize(Storage storage, Data.Node data)
		{
			ISerializer serializer = this.Find(data.Type);
			return serializer.NotNull() ? serializer.Deserialize(storage, data) : null;
		}
		#endregion
	}
}


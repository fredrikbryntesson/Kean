// 
//  Storage.cs
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
using Kean.Core.Extension;
using Kean.Core.Collection.Extension;

namespace Kean.Core.Serialize
{
	public abstract class Storage
	{
		Collection.Dictionary<Reflect.TypeName, ISerializer> cache = new Collection.Dictionary<Reflect.TypeName, ISerializer>();
		public Collection.IList<ISerializer> Serializers { get; private set; }

		protected Storage()
		{
			this.Serializers = new Collection.List<ISerializer>();
		}

		protected abstract Data.Node Load(string[] key);
		public T Load<T>(params string[] key)
		{
			Data.Node data = this.Load(key);
			return this.GetSerializer(data.Type ?? typeof(T)).Deserialize<T>(this, data);
		}

		protected abstract bool Store(Data.Node value, string key);
		public bool Store<T>(T value, params string[] key)
		{
			return this.Store(this.GetSerializer(typeof(T)).Serialize(this, value), key);
		}
		ISerializer GetSerializer(Reflect.TypeName typeName)
		{
			ISerializer result = null;
			object[] attributes;
			if (cache.Contains(typeName))
				result = cache[typeName];
			else
			{
				Type type = (Type) typeName;
				if (!type.IsPrimitive && (attributes = type.GetCustomAttributes(typeof(MethodAttribute), true)).Length == 1)
					result = (attributes[0] as MethodAttribute).Serializer;
				else
					result = this.Serializers.Find(serializer => serializer.Accepts(type));
				cache[typeName] = result;
			}
			return result;
		}
	}
}
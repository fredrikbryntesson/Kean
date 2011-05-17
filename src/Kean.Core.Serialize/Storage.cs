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
using Kean.Core.Basis.Extension;
using Kean.Core.Collection.Extension;

namespace Kean.Core.Serialize
{
	public abstract class Storage
	{
		public Collection.IList<ISerializer> Serializers { get; private set; }

		protected Storage()
		{
			this.Serializers = new Collection.List<ISerializer>();
		}

		protected abstract Data.Node Load(string[] key);
		public T Load<T>(params string[] key)
		{
			return this.Serializers.Find(serializer => serializer.Accepts(typeof(T))).Deserialize<T>(this, this.Load(key));
		}

		protected abstract bool Store(Data.Node value, string key);
		public bool Store<T>(T value, params string[] key)
		{
			return this.Store(this.Serializers.Find(serializer => serializer.Accepts(typeof(T))).Serialize(this, value), key);
		}
	}
}


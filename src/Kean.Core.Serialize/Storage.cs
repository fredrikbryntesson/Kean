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
		public ISerializer Serializer { get; private set; }

		protected Storage(params ISerializer[] serializers) :
			this(new Serializer.Group(serializers))
		{ }
		protected Storage(ISerializer serializer)
		{
			this.Serializer = serializer;
		}
		protected abstract Data.Node Load(params string[] key);
		public T Load<T>(params string[] key)
		{
			Data.Node data = this.Load(key);
			return this.Serializer.Find(data.Type ?? typeof(T)).Deserialize<T>(this, data);
		}

		protected abstract bool Store(Data.Node value, params string[] key);
		public bool Store<T>(T value, params string[] key)
		{
			return this.Store(this.Serializer.Find(typeof(T)).Serialize(this, value), key);
		}
	}
}
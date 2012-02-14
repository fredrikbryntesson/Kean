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
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Uri = Kean.Core.Uri;

namespace Kean.Core.Serialize
{
	public abstract class Storage :
		ISerializer
	{
		ISerializer serializer;
		public Resolver Resolver { get; private set; }

		protected Storage(params ISerializer[] serializers) :
			this(new Resolver(), serializers)
		{ }
		protected Storage(Resolver resolver, params ISerializer[] serializers) :
			this(resolver, new Serializer.Group(serializers))
		{ }
		protected Storage(Resolver resolver, ISerializer serializer)
		{
			this.Resolver = resolver;
			this.serializer = new Serializer.Cache(serializer);
		}
		protected abstract bool Store(Data.Node value, Uri.Locator locator);
		public bool Store<T>(T value, Uri.Locator locator)
		{
			return this.Store(this.Serialize(this, typeof(T), value), locator);
		}
		protected abstract Data.Node Load(Uri.Locator locator);
		public T Load<T>(Uri.Locator locator)
		{
			return (T)this.Deserialize(this, this.Load(locator).DefaultType(typeof(T)));
		}

		#region ISerializer Members
		public ISerializer Find(Kean.Core.Reflect.Type type)
		{
			return this.serializer.Find(type);
		}

		public Data.Node Serialize(Storage storage, Reflect.Type type, object data)
		{
			return this.serializer.Serialize(storage, type, data);
		}

		public object Deserialize(Storage storage, Serialize.Data.Node data)
		{
			object result = this.serializer.Deserialize(storage, data);
			if (data.NotNull() && data.Locator.NotNull())
				this.Resolver[data.Locator] = result;
			return result;
		}
		#endregion
	}
}
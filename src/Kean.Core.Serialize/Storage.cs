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
using Kean.Core.Reflect.Extension;

namespace Kean.Core.Serialize
{
	public abstract class Storage
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
			return this.Store(this.Serialize(typeof(T), value, locator), locator);
		}
		protected abstract Data.Node Load(Uri.Locator locator);
		public T Load<T>(Uri.Locator locator)
		{
			return (T)(this.Resolver[locator] ?? this.Deserialize(this.Load(locator).DefaultType(typeof(T)).UpdateLocators(locator)));
		}

		public Data.Node Serialize(Reflect.Type type, object data, Uri.Locator locator)
		{
			return this.serializer.Serialize(this, type, data, locator);
		}

		object Deserialize(Serialize.Data.Node node)
		{
			return node.NotNull() ? (this.Resolver[node.Locator] = this.serializer.Deserialize(this, node)) : null;
		}
		public void Deserialize(Serialize.Data.Node node, Action<object> set)
		{
			bool result = true;
			if (node is Data.Link)
				result = this.Resolver.Resolve((node as Data.Link).Target, d =>
				{
					this.Resolver[node.Locator] = d;
					set.Call(d);
				});
			else
				set.Call(this.Deserialize(node));
		}
	}
}
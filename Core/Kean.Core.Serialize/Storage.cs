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
		IRebuilder rebuilder;
		public Resolver Resolver { get; private set; }
		protected Storage(Resolver resolver, IRebuilder rebuilder, params ISerializer[] serializers)
		{
			this.Resolver = resolver ?? new Resolver();
			this.rebuilder = rebuilder ?? new Rebuilder.Identity();
			this.serializer = new Serializer.Cache(serializers.NotEmpty() ? new Serializer.Group(serializers) : new Serializer.Default());
		}
		protected abstract bool Store(Data.Node value, Uri.Locator locator);
		public bool Store<T>(T value, Uri.Locator locator)
		{
			return this.Store<T>(value, locator, null);
		}
		public bool Store<T>(T value, Uri.Locator locator, string name)
		{
			Data.Node data = this.Serialize(typeof(T), value, locator);
			if (name.NotEmpty() && data.NotNull())
				data.Name = name;
			return this.Store(this.rebuilder.Store(this, data), locator);
		}
		protected abstract Data.Node Load(Uri.Locator locator);
		public T Load<T>(Uri.Locator locator)
		{
			Data.Node node;
			return (T)(this.Resolver[locator] ?? ((node = this.Load(locator)).NotNull() ? this.Deserialize(null, this.rebuilder.Load(this, node.DefaultType(typeof(T)).UpdateLocators(locator))) : null));
		}
		public Data.Node Serialize(Reflect.Type type, object data, Uri.Locator locator)
		{
			return this.serializer.Serialize(this, type, data, locator);
		}
		object Deserialize(object result, Serialize.Data.Node node)
		{
			return node.NotNull() ? (this.Resolver[node.Locator] = this.serializer.Deserialize(this, node, result)) : null;
		}
		public bool DeserializeContent(Serialize.Data.Node node, object result)
		{
			return node.NotNull() && result.NotNull() && this.Deserialize(result, node).NotNull();
		}
		public void Deserialize(Serialize.Data.Node node, Reflect.Type type, Action<object> set)
		{
			node = node.DefaultType(type);
			if (node is Data.Link)
				this.Resolver.Resolve((node as Data.Link).Target, d =>
				{
					this.Resolver[node.Locator] = d;
					set.Call(d);
				});
			else
				set.Call(this.Deserialize(null, node));
		}
	}
}
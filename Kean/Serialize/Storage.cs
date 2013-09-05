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
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Kean.Reflect.Extension;
namespace Kean.Serialize
{
    public abstract class Storage :
        IStorage
    {
        ISerializer serializer;
        IRebuilder rebuilder;
        public Resolver Resolver { get; private set; }
        public Casing Casing { get; set; }
        protected Storage(Resolver resolver, IRebuilder rebuilder, params ISerializer[] serializers)
        {
            this.Resolver = resolver ?? new Resolver();
            this.rebuilder = rebuilder ?? new Rebuilder.Identity();
            this.serializer = new Serializer.Cache(serializers.NotEmpty() ? new Serializer.Group(serializers) : new Serializer.Default());
        }
        protected abstract bool Store(Data.Node value, IO.IByteOutDevice device);
        public bool Store<T>(T value, Uri.Locator resource, string name = null)
        {
            return this.Store(value, IO.ByteDevice.Create(resource), name);
        }
        public bool Store<T>(T value, IO.IByteOutDevice device, string name = null)
        {
            bool result = device.NotNull();
            if (result)
            {
                Data.Node data = this.Serialize(typeof(T), value, device.Resource);
                if (name.NotEmpty() && data.NotNull())
                    data.Name = name;
                result = this.Store(this.rebuilder.Store(this, data), device);
            }
            return result;
        }
        protected abstract Data.Node Load(IO.IByteInDevice device);
        public T Load<T>(Uri.Locator resource)
        {
            return this.Load<T>(IO.ByteDevice.Open(resource));
        }
        public T Load<T>(IO.IByteInDevice device)
        {
            Data.Node node;
            return device.IsNull() ? default(T) : (T)(this.Resolver[device.Resource] ?? ((node = this.Load(device)).NotNull() ? this.Deserialize(null, this.rebuilder.Load(this, node.DefaultType(typeof(T)).UpdateLocators(device.Resource))) : null));
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
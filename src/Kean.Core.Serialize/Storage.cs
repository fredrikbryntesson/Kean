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

namespace Kean.Core.Serialize
{
	public abstract class Storage
	{
		protected Storage()
		{
		}

		public void Add<T>(ISerializer<T> serializer)
		{
		}
		public void Add<T>(IDeserializer<T> deserializer)
		{
		}
		public void Add<T>(ISerializer<T> serializer, IDeserializer<T> deserializer)
		{
			this.Add(serializer);
			this.Add(deserializer);
		}
		public void Add<T, S>(S serializer) where S : ISerializer<T>, IDeserializer<T>
		{
			this.Add(serializer, serializer);
		}

		ISerializer<T> GetSerializer<T>()
		{
			return null;
		}
		protected abstract Data.Node Load(string[] path);
		public T Load<T>(params string[] path)
		{
			return this.GetDeserializer<T>().Deserialize(this, this.Load(path));
		}

		IDeserializer<T> GetDeserializer<T>()
		{
			return null;
		}
		protected abstract bool Store(Data.Node data, string path);
		public bool Store<T>(T data, params string[] path)
		{
			return this.Store(this.GetSerializer<T>().Serialize(this, data));
		}
	}
}


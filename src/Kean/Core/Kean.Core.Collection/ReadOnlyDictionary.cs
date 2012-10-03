// 
//  ReadOnlyDictionary.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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

namespace Kean.Core.Collection
{
	public class ReadOnlyDictionary<TKey, TValue> :
		IReadOnlyDictionary<TKey, TValue>
	{
		IDictionary<TKey, TValue> data;
		public ReadOnlyDictionary()
		{
			this.data = new Dictionary<TKey, TValue>();
		}
		public ReadOnlyDictionary(System.Collections.Generic.IEnumerable<KeyValue<TKey, TValue>> data) :
			this()
		{
			foreach (KeyValue<TKey, TValue> element in data)
				this.data[element.Key] = element.Value;
		}
		public ReadOnlyDictionary(params KeyValue<TKey, TValue>[] data) :
			this((System.Collections.Generic.IEnumerable<KeyValue<TKey, TValue>>)data)
		{ }
		#region IReadOnlyDictionary<TKey,TValue> Members
		public TValue this[TKey key] { get { return this.data[key]; } }
		public bool Contains(TKey key) { return this.data.Contains(key); }
		public IReadOnlyDictionary<TKey, TValue> Update(TKey key, TValue value)
		{
			ReadOnlyDictionary<TKey, TValue> result = new ReadOnlyDictionary<TKey, TValue>(this);
			result.data[key] = value;
			return result;
		}
		public IReadOnlyDictionary<TKey, TValue> Remove(TKey key)
		{
			ReadOnlyDictionary<TKey, TValue> result = new ReadOnlyDictionary<TKey, TValue>(this);
			if (!result.data.Remove(key))
				result = this;
			return result;
		}
		#endregion
		#region IEnumerable<KeyValue<TKey,TValue>> Members
		public System.Collections.Generic.IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
		{
			return this.data.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
		#region IEquatable<IDictionary<TKey,TValue>> Members
		public bool Equals(IDictionary<TKey, TValue> other)
		{
			return this.data.Equals(other);
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return (other is IDictionary<TKey, TValue>) && this.Equals(other as IDictionary<TKey, TValue>);
		}
		public bool Equals(ReadOnlyDictionary<TKey, TValue> other)
		{
			return this == other;
		}
		public override string ToString()
		{
			return this.data.ToString();
		}
		public override int GetHashCode()
		{
			return this.data.GetHashCode();
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(ReadOnlyDictionary<TKey, TValue> left, ReadOnlyDictionary<TKey, TValue> right)
		{
			return object.ReferenceEquals(left, right) ||
				!object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) &&
				left.data == right.data;
		}
		public static bool operator !=(ReadOnlyDictionary<TKey, TValue> left, ReadOnlyDictionary<TKey, TValue> right)
		{
			return !(left == right);
		}
		#endregion
	}
}
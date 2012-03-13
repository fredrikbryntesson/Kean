// 
//  Dictionary.cs
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
using Kean.Core.Extension;
using Kean.Core.Collection.Linked.Extension;

namespace Kean.Core.Collection.Linked
{
	public class Dictionary<TKey, TValue> :
		Dictionary<Link<KeyValue<TKey, TValue>>, TKey, TValue>
	{
		public Dictionary() { }
	}
	public class Dictionary<L, TKey, TValue> :
		IDictionary<TKey, TValue>
		where L : class, ILink<L, KeyValue<TKey, TValue>>, new()
	{
		L head;
		public Dictionary() { }

		#region IDictionary<TKey,TValue> Members
		public TValue this[TKey key]
		{
			get { return this.head.Find(item => item.Key.Equals(key)).Value; }
			set
			{
				this.Remove(key);
				this.head = new L() { Head = KeyValue.Create(key, value), Tail = this.head };
			}
		}
		public bool Contains(TKey key)
		{
			return this.head.Exists(item => item.Key.Equals(key));
		}
		public bool Remove(TKey key)
		{
			bool result = false;
			this.head = this.head.Remove(item => result = item.Key.Equals(key));
			return result;
		}
		#endregion

		#region IEnumerable<KeyValue<TKey,TValue>> Members
		public System.Collections.Generic.IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
		{
			return this.head.GetEnumerator();
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
			bool result = other.NotNull();
			int count = this.head.Count();
			if (result)
				foreach (KeyValue<TKey, TValue> pair in other)
					if (!(result = count-- == 0 || this[pair.Key].Equals(pair.Value)))
						break;
			return result && count == 0;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return (other is IDictionary<TKey, TValue>) && this.Equals(other as IDictionary<TKey, TValue>);
		}
		public override string ToString()
		{
			return this.head.ToString();
		}
		public override int GetHashCode()
		{
			return this.head.GetHashCode();
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(Dictionary<L, TKey, TValue> left, IDictionary<TKey, TValue> right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(Dictionary<L, TKey, TValue> left, IDictionary<TKey, TValue> right)
		{
			return !(left == right);
		}
		#endregion
	}
}
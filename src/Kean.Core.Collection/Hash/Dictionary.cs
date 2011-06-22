//
//  Dictionary.cs
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
using Kean.Core.Basis;
using Kean.Core.Basis.Extension;
using Kean.Core.Collection.Extension;

namespace Kean.Core.Collection.Hash
{
	public class Dictionary<TKey, TValue> :
		IDictionary<TKey, TValue>
		where TKey : System.IEquatable<TKey>
	{
		IList<Tuple<TKey, TValue>>[] data;
		public TValue this[TKey key]
		{
			get
			{
				TValue result = default(TValue);
				IList<Tuple<TKey, TValue>> list = this.data[this.Index(key)];
				if (list.NotNull() && list.Count > 0)
				{
					Tuple<TKey, TValue> entry = list.Find(e => e.Item1.Equals(key));
					if (entry.NotNull())
						result = entry.Item2;
				}
				return result;
			}
			set
			{
				IList<Tuple<TKey, TValue>> list = this.data[this.Index(key)];
				if (list.IsNull())
					this.data[this.Index(key)] = list = new Linked.List<Tuple<TKey, TValue>>();
				int index = list.Index(entry => entry.Item1.Equals(key));
				if (index >= 0)
					list[index] = Tuple.Create(key, value);
				else
					list.Add(Tuple.Create(key, value));
			}
		}
		public Dictionary() :
			this(20)
		{ }
		public Dictionary(int capacity)
		{
			this.data = new IList<Tuple<TKey, TValue>>[capacity];
		}
		int Index(TKey key)
		{
			int divided = key.GetHashCode();
			if (divided < 0)
				divided *= -1;
			return divided % this.data.Length;
		}
		public bool Contains(TKey key)
		{
			IList<Tuple<TKey, TValue>> list = this.data[this.Index(key)];
			return list.NotNull() && list.Count > 0 && list.Find(entry => entry.Item1.Equals(key)).NotNull();
		}
		public bool Remove(TKey key)
		{
			IList<Tuple<TKey, TValue>> list = this.data[this.Index(key)];
			return list.NotNull() && list.Remove(entry => entry.Item1.Equals(key));
		}
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is IDictionary<TKey, TValue> && this.Equals(other as IDictionary<TKey, TValue>);
		}
		public override int GetHashCode()
		{
			int result = 0;
			foreach (Tuple<TKey, TValue> pair in this)
				result ^= pair.Hash();
			return result;
		}
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
		#region IEnumerable<Tuple<TKey,TValue>> Members
		public System.Collections.Generic.IEnumerator<Tuple<TKey, TValue>> GetEnumerator()
		{
			foreach (IList<Tuple<TKey, TValue>> list in this.data)
				if (list.NotNull())
					foreach (Tuple<TKey, TValue> pair in list)
						yield return pair;
		}
		#endregion
		#region IEquatable<IDictionary<TKey,TValue>> Members
		public bool Equals(IDictionary<TKey, TValue> other)
		{
			bool result = other.NotNull();
            int count = this.data.Fold((list, c) => c + (list.NotNull() ? list.Count : 0), 0);
			if (result)
				foreach (Tuple<TKey, TValue> pair in other)
                    if (!(result = count-- == 0 || this[pair.Item1].Equals(pair.Item2)))
						break;

			return result && count == 0;
		}
		#endregion
	}
}



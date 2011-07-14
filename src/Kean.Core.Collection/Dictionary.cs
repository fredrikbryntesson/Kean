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

namespace Kean.Core.Collection
{
    public class Dictionary<TKey, TValue> :
        IDictionary<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        Hash.Dictionary<TKey, TValue> data;
        public Dictionary()
        {
            this.data = new Hash.Dictionary<TKey, TValue>();
        }
        public Dictionary(int capacity)
        {
            this.data = new Hash.Dictionary<TKey, TValue>(capacity);
        }

        #region IDictionary[TKey,TValue] implementation
        public bool Contains(TKey key)
        {
            return this.data.Contains(key);
        }

        public bool Remove(TKey key)
        {
            return this.data.Remove(key);
        }

        public TValue this[TKey key]
        {
            get { return this.data[key]; }
            set { this.data[key] = value; }
        }
        #endregion

        #region IEquatable[IDictionary[TKey,TValue]] implementation
        public bool Equals(IDictionary<TKey, TValue> other)
        {
            return this.data.Equals(other);
        }
        #endregion

        #region IEnumerable[Tuple[TKey,TValue]] implementation
        System.Collections.Generic.IEnumerator<Kean.Core.Basis.Tuple<TKey, TValue>> System.Collections.Generic.IEnumerable<Tuple<TKey, TValue>>.GetEnumerator()
        {
            return (this.data as System.Collections.Generic.IEnumerable<Tuple<TKey, TValue>>).GetEnumerator();
        }
        #endregion

        #region IEnumerable implementation
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (this.data as System.Collections.IEnumerable).GetEnumerator();
        }
        #endregion
        #region Object Overrides
        public override bool Equals(object other)
        {
            return (other is Dictionary<TKey, TValue>) && this.Equals(other as Dictionary<TKey, TValue>);
        }
        public bool Equals(Dictionary<TKey, TValue> other)
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
        public static bool operator ==(Dictionary<TKey, TValue> left, Dictionary<TKey, TValue> right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) &&
                left.data  == right.data;
        }
        public static bool operator !=(Dictionary<TKey, TValue> left, Dictionary<TKey, TValue> right)
        {
            return !(left == right);
        }
        #endregion
    }
}
// 
//  ImmutableVector.cs
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

namespace Kean.Core.Collection.Wrap
{
    public class ImmutableVector<T> :
        IImmutableVector<T>
    {
        object data;
        #region Constructor
        public ImmutableVector(IVector<T> data)
        {
            this.data = data;
        }
        public ImmutableVector(IImmutableVector<T> data)
        {
            this.data = data;
        }
        #endregion
        #region IImmutableVector<T>
        public int Count { get { return this.data is IVector<T> ? (this.data as IVector<T>).Count : (this.data as IImmutableVector<T>).Count; } }
        public T this[int index] { get { return this.data is IVector<T> ? (this.data as IVector<T>)[index] : (this.data as IImmutableVector<T>)[index]; } }
        #endregion
        #region IEnumerable<T> Members
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            return (this.data as System.Collections.Generic.IEnumerable<T>).GetEnumerator();
        }
        #endregion
        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
        #region IEquatable<IVector<T>> Members
        public bool Equals(IVector<T> other)
        {
            return (this.data as IEquatable<IVector<T>>).Equals(other);
        }
        #endregion
    }
}

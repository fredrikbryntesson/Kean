// 
//  CommonVector.cs
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
using Kean.Core.Basis.Extension;

namespace Kean.Core.Collection.Abstract
{
	public abstract class CommonVector<T> :
        System.Collections.Generic.IEnumerable<T>,
        System.IEquatable<IVector<T>>,
        System.IEquatable<IImmutableVector<T>>

	{
		public abstract int Count { get; }
		public CommonVector()
		{
		}
		protected abstract T Get(int index);
		#region IEnumerator<T>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as System.Collections.Generic.IEnumerable<T>).GetEnumerator();
		}
		System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
		{
			for (int i = 0; i < (this as IImmutableVector<T>).Count; i++)
				yield return (this as IImmutableVector<T>)[i];
		}
		#endregion
		#region Object override
		public override bool Equals(object other)
		{
			return other is IVector<T> && this.Equals(other as IVector<T>) ||
				other is IImmutableVector<T> && this.Equals(other as IImmutableVector<T>);
		}
		public override int GetHashCode ()
		{
			int result = 0;
			foreach (T item in (this as IVector<T>))
				result ^= item.GetHashCode();
			return result;
		}
		#endregion
		#region IEquatable<IVector<T>>
		public bool Equals(IVector<T> other)
		{
			bool result = other.NotNull() && (this as IImmutableVector<T>).Count == other.Count;
			for (int i = 0; result && i < (this as IVector<T>).Count; i++)
				result &= (this as IVector<T>)[i].Equals(other[i]);
			return result;
		}
		#endregion
		#region IEquatable<IImmutableVector<T>>
		public bool Equals(IImmutableVector<T> other)
		{
			bool result = !object.ReferenceEquals(other, null) && (this as IImmutableVector<T>).Count == other.Count;
			for (int i = 0; result && i < (this as IImmutableVector<T>).Count; i++)
				result &= (this as IImmutableVector<T>)[i].Equals(other[i]);
			return result;
		}
		#endregion
	}
}


// 
//  Array.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009 Simon Mika
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
namespace Kean.Core.Collection.Abstract
{
	public abstract class Array<T> :
		Interface.IArray<T>
	{
		public abstract int Count { get; }
		public abstract T this[int index] { get; set; }
		
		protected Array ()
		{ }
		#region IEnumerator<T>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as System.Collections.Generic.IEnumerable<T>).GetEnumerator();
		}
		System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
		{
			for (int i = 0; i < (this as Interface.IArray<T>).Count; i++)
				yield return (this as Interface.IArray<T>)[i];
		}
		#endregion
		#region Object override
		public override bool Equals(object other)
		{
			return other is Interface.IArray<T> && this.Equals(other as Interface.IArray<T>);
		}
		public override int GetHashCode ()
		{
			int result = 0;
			foreach (T item in (this as Interface.IArray<T>))
				result ^= item.GetHashCode();
			return result;
		}
		#endregion
		#region IEquatable<Interface.IArray<T>>
		public bool Equals(Interface.IArray<T> other)
		{
			bool result = !object.ReferenceEquals(other, null) && (this as Interface.IArray<T>).Count == other.Count;
			for (int i = 0; result && i < (this as Interface.IArray<T>).Count; i++)
				result &= (this as Interface.IArray<T>)[i].Equals(other[i]);
			return result;
		}
		#endregion
	}
}

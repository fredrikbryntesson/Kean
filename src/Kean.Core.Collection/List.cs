// 
//  List.cs
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

namespace Kean.Core.Collection
{
	public class List<T> :
		IList<T>
	{
		Array.List<T> items;
		public List()
		{
			this.items = new Array.List<T>();
		}
		#region IList<T>
		public int Count { get { return this.items.Count; } }
		public T this[int index]
		{
			get { return this.items[index]; }
			set { this.items[index] = value; }
		}
		public void Add(T item)
		{
			this.items.Add(item);
		}
		public T Remove()
		{
			return this.items.Remove();
		}
        public T Remove(int index)
		{
			return this.items.Remove(index);
		}
        public void Insert(int index, T item)
		{
			this.items.Insert(index, item);
		}
		#endregion
		#region System.Collections.IEnumerable
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this.items as System.Collections.IEnumerable).GetEnumerator();
		}
		#endregion
		#region System.Collections.Generic.IEnumerable<T>
		System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
		{
			return (this.items as System.Collections.Generic.IEnumerable<T>).GetEnumerator();
		}
		#endregion
		#region System.IEquatable<Interface.IVector<T>>
		public bool Equals(Interface.IVector<T> other)
		{
			return this.items.Equals(other);
		}
		#endregion
		#region System.Object
		public override bool Equals(object other)
		{
			return (this.items as object).Equals(other);
		}
		public override int GetHashCode()
		{
			return this.items.GetHashCode();
		}
		#endregion
	}
}

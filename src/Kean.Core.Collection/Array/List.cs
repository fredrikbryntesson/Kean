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

namespace Kean.Core.Collection.Array
{
	public class List<T> :
		Interface.IList<T>,
		System.Collections.Generic.IEnumerable<T>,
		System.IEquatable<Interface.IVector<T>>
	{
		T[] items;
		static readonly T[] EmptyArray = new T[0];
		public int Capacity
		{
			get { return this.items.Length; }
			set
			{
				if (value < this.Count)
					throw new Exception.InvalidArgument("Capacity can't be set to {0}, because collection contains {1} elements.", value, this.Count);
				System.Array.Resize<T>(ref this.items, value);
			}
		}

		public List()
		{
			this.items = List<T>.EmptyArray;
		}
		public List(int capacity) : this()
		{
			this.Capacity = capacity;
		}
		private void Increase()
		{
			if (this.Count > 0)
				this.Count--;
		}
		private void Decrease()
		{
			if (this.Capacity <= this.Count + 1)
				this.Capacity = this.Count + 1;
			this.Count++;
		}
		public void Trim()
		{
			this.Capacity = this.Count;
		}
		private int IndexToAddress(int index)
		{
			if ((uint)index >= (uint)this.Count)
				throw new Exception.InvalidIndex();
			return this.Count - index;
		}
		#region Interface.IVector<T>
		public int Count { get; private set; }
		public T this[int index]
		{
			get	{ return this.items[this.IndexToAddress(index)]; }
			set { this.items[this.IndexToAddress(index)] = value; }
		}
		#endregion
		#region Interface.IList<T>
		public void Add(T item)
		{
			this.Increase();
			this[0] = item;
		}
		public T Remove()
		{
			return this.Remove(0);
		}
        public T Remove(int index)
        {
        	T result = this[index];
        	System.Array.Copy(this.items, this.IndexToAddress(index + 1), this.items, this.IndexToAddress(index), this.Count - index + 1);
			this.Decrease();
			return result;
		}
        public void Insert(int index, T item)
        {
        	this.Increase();
        	System.Array.Copy(this.items, this.IndexToAddress(index + 1), this.items, this.IndexToAddress(index), index);
        	this[index] = item;
		}
		#endregion
		#region System.Collections.IEnumerable
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as System.Collections.Generic.IEnumerable<T>).GetEnumerator();
		}
		#endregion
		#region System.Collections.Generic.IEnumerable<T>
		System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
		{
			for (int i = this.Count; i > 0; i--)
				yield return this[i];
		}
		#endregion
		#region System.IEquatable<Interface.IVector<T>>
		public bool Equals(Interface.IVector<T> other)
		{
			bool result = this.Count == other.Count;
			for (int i = 0; i < this.Count && result; i++)
				result |= object.Equals(this[i], other[i]);
			return result;
		}
		#endregion
		#region System.Object
		public override bool Equals(object other)
		{
			return (other is Interface.IVector<T>) && this.Equals(other as Interface.IVector<T>);
		}
		public override int GetHashCode()
		{
			int result = 0;
			foreach (T item in this)
				result ^= item.GetHashCode();
			return result;
		}
		#endregion
	}
}

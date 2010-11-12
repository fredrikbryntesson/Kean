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
		Abstract.List<T>
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
		void Increase()
		{
			if (this.Count > 0)
				this.Count--;
		}
		void Decrease()
		{
			if (this.Capacity <= this.Count + 1)
				this.Capacity = this.Count + 1;
			this.Count++;
		}
		public void Trim()
		{
			this.Capacity = this.Count;
		}
		int IndexToAddress(int index)
		{
			if ((uint)index >= (uint)this.Count)
				throw new Exception.InvalidIndex();
			return this.Count - index;
		}
		#region IVector<T>
		public override int Count { get; private set; }
		public override T this[int index]
		{
			get	{ return this.items[this.IndexToAddress(index)]; }
			set { this.items[this.IndexToAddress(index)] = value; }
		}
		#endregion
		#region IList<T>
		public override void Add(T item)
		{
			this.Increase();
			this[0] = item;
		}
		public override T Remove()
		{
			return this.Remove(0);
		}
        public override T Remove(int index)
        {
        	T result = this[index];
        	System.Array.Copy(this.items, this.IndexToAddress(index + 1), this.items, this.IndexToAddress(index), this.Count - index + 1);
			this.Decrease();
			return result;
		}
        public override void Insert(int index, T item)
        {
        	this.Increase();
        	System.Array.Copy(this.items, this.IndexToAddress(index + 1), this.items, this.IndexToAddress(index), index);
        	this[index] = item;
		}
		#endregion
	}
}

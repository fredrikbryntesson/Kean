// 
//  Queue.cs
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
	public class Queue<T> :
		IQueue<T>
	{
		List<T> items;
		int head;
		int tail;
		int size;
		public Queue()
		{
			this.items = new List<T>();
		}
		public Queue(int capacity)
		{ 
			this.items = new List<T>(capacity);
		}
		#region IQueue<T>
		public bool Empty { get { return this.size == 0; } }
		public void Enqueue(T item)
		{
			if (this.size == this.items.Count)
			{
				this.items.Insert(this.tail, item);
				if (this.head > this.tail)
					this.head++;
			}
			else
				this.items[this.tail] = item;
			this.tail = (this.tail + 1) % this.items.Count;
			this.size++;
		}
		public T Peek()
		{
			if (this.size == 0)
				throw new Exception.Empty();
			return this.items[this.head];
		}
		public T Dequeue()
		{
			T result = this.Peek();
			// let garbage collector do its job
			this.items[this.head] = default(T);
			this.head = (this.head + 1) % this.items.Count;
			this.size--;
			return result;
		}
		#endregion
	}
}

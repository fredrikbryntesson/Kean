// 
//  Stack.cs
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
	public class Stack<T> :
		Interface.IStack<T>
	{
		List<T> items;
		public Stack()
		{
			this.items = new List<T>();
		}
		public Stack(int capacity)
		{
			this.items = new List<T>(capacity);
		}
		#region Interface.IStack<T>
		public bool Empty { get { return this.items.Count < 1; } }
		public void Push(T item)
		{
			this.items.Add(item);
		}
		public T Pop()
		{
			return this.items.Remove(0);
		}
		public T Peek()
		{
			return this.items[0];
		}
		#endregion
	}
}

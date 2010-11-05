// 
//  LinkedStack.cs
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
using Kean.Core.Collection.Extension;

namespace Kean.Core.Collection.Linked
{
	public class LinkedStack<T> :
		LinkedStack<Link<T>, T>
	{
		public LinkedStack () { }
	}
	public class LinkedStack<L, T> :
		Interface.IStack<T>
		where L : class, Interface.ILink<L, T>, new()
	{
		private L top;
		public bool Empty { get { return this.top == null; } }
		public LinkedStack() { }
		public void Push(T item)
		{
			this.top = this.top.Add(item);
		}
		public T Pop()
		{
			T result = this.top.Head;
			this.top = this.top.Tail;
			return result;
		}
		public T Peek()
		{
			return this.top.Head;
		}
	}
}

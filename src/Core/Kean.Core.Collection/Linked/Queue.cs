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
using Kean.Core.Collection.Extension;

namespace Kean.Core.Collection.Linked
{
	public class Queue<T> :
		Queue<Link<T>, T>
	{
		public Queue() { }
	}
	public class Queue<L, T> :
		Interface.IQueue<T>
		where L : class, Interface.ILink<L, T>, new()
	{
		private L first;
		private L last;
		public bool Empty { get { return this.first == null; } }
		public Queue() { }
		public void Enqueue(T item)
		{
			L link = new L() { Head = item };
			if (this.last == null)
				this.first = link;
			else
				this.last.Tail = link;
			this.last = link;
		}
		public T Dequeue()
		{
			if (this.first == null)
				throw new Exception.Empty();
			T result = this.first.Head;
			this.first = this.first.Tail;
			return result;
		}
	}
}
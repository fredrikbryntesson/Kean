// 
//  LinkedList.cs
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
	public class List<T> :
		List<Link<T>, T>
	{
		public List() { }
	}
	public class List<L, T> :
		IList<T>
		where L : class, ILink<L, T>, new()
	{
		private L first;
		public int Count { get { return this.first.Count<L, T>(); } }
		public T this[int index]
		{
			get { return this.first.Get<L, T>(index); }
			set { this.first.Set<L, T>(index, value); }
		}
		public List() { }
		public void Add(T element)
		{
			this.first = new L()
			{
				Head = element,
				Tail = this.first,
			};
		}
		public T Remove()
		{
			T result = default(T);
			if (this.first != null)
			{
				result = this.first.Head;
				this.first = this.first.Tail;
			}
			return result;
		}
        public T Remove(int index)
		{
			T result;
			this.first = this.first.Remove(index, out result);
			return result;
		}
        public void Insert(int index, T element)
		{
			this.first = this.first.Insert(index, element);
		}
		#region IEnumerator<T>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as System.Collections.Generic.IEnumerable<T>).GetEnumerator();
		}
		System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
		{
			L tail = this.first;
			while (tail != null)
			{
				yield return tail.Head;
				tail = tail.Tail;
			}
		}
		#endregion
		#region Object override
		public override bool Equals(object other)
		{
			return other is IVector<T> && this.Equals(other as IVector<T>);
		}
		public override int GetHashCode ()
		{
			return this.first.Fold((T element, int result) => result ^ element.GetHashCode());
		}
		#endregion
		#region IEquatable<Interface.IVector<T>>
		public bool Equals(IVector<T> other)
		{
			bool result = !object.ReferenceEquals(other, null) && (this as IVector<T>).Count == other.Count;
			for (int i = 0; result && i < (this as IVector<T>).Count; i++)
				result &= (this as IVector<T>)[i].Equals(other[i]);
			return result;
		}
		#endregion

	}
}

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

namespace Kean.Core.Collection.Synchronized
{
	public class Vector<T> :
		Interface.IVector<T>
	{
		private object guard;
		private Interface.IVector<T> data;
		#region Constructor
		public Vector(Interface.IVector<T> data) :
			this(data, new object())
		{ }
		public Vector(Interface.IVector<T> data, object guard)
		{
			this.data = data;
			this.guard = guard;
		}
		#endregion
		#region Interface.IVector<T>
		int Interface.IVector<T>.Count { get { lock (this.guard) return this.data.Count; } }
		T Interface.IVector<T>.this[int index] 
		{
			get { lock (this.guard) return this.data[index]; }
			set { lock (this.guard) this.data[index] = value; }
		}
		#endregion
		#region IEnumerator<T>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as System.Collections.Generic.IEnumerable<T>).GetEnumerator();
		}
		System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
		{
			lock (this.guard)
				foreach (T item in this.data)
					yield return item;
		}
		#endregion
		#region Object override
		public override bool Equals(object other)
		{
			lock (this.guard) return (this.data as object).Equals(other);
		}
		public override int GetHashCode ()
		{
			lock (this.guard) return this.data.GetHashCode();
		}
		#endregion
		#region IEquatable<Interface.IVector<T>>
		public bool Equals(Interface.IVector<T> other)
		{
			lock (this.guard) return this.data.Equals(other);
		}
		#endregion
	}

}

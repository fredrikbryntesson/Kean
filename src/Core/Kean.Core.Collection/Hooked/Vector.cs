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

namespace Kean.Core.Collection.Hooked
{
	public class Vector<T> : 
		Interface.IVector<T>
	{
		private Interface.IVector<T> data;
		public event Action<int, T, T> Replaced;
		public Func<int, T, T, T> OnReplace { private get; set; }
		public T this[int index]
		{
			get { return this.data[index]; }
			set { this.data[index] = this.OnReplace(index, value, this.data[index]); }
		}
		public int Count { get { return this.data.Count; } }
		public Vector(Interface.IVector<T> data)
		{
			this.data = data;
		}
		#region IEnumerator<T>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as System.Collections.Generic.IEnumerable<T>).GetEnumerator();
		}
		System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
		{
			return this.data.GetEnumerator();
		}
		#endregion
		#region Object override
		public override bool Equals(object other)
		{
			return other is Interface.IVector<T> && this.Equals(other as Interface.IVector<T>);
		}
		public override int GetHashCode()
		{
			return this.data.GetHashCode();
		}
		#endregion
		#region IEquatable<Interface.IVector<T>>
		public bool Equals(Interface.IVector<T> other)
		{
			return this.data.Equals(other);
		}
		#endregion
	}
}

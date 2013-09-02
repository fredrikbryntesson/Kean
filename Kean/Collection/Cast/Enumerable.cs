// 
//  Enumerable.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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

namespace Kean.Core.Collection.Cast
{
	public class Enumerable<T, S> :
		System.Collections.Generic.IEnumerable<S>
	{
		System.Collections.Generic.IEnumerable<T> data;
		Func<T, S> cast;

		public Enumerable(System.Collections.Generic.IEnumerable<T> data, Func<T, S> cast)
		{
			this.data = data;
			this.cast = cast;
		}
		#region IEnumerable<S> Members
		public System.Collections.Generic.IEnumerator<S> GetEnumerator()
		{
			return new Cast.Enumerator<T, S>(this.data.GetEnumerator(), cast);
		}
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
		#region Object Overrides
		public override string ToString()
		{
			return this.data.ToString();
		}
		public override int GetHashCode()
		{
			return this.data.GetHashCode();
		}
		public override bool Equals(object other)
		{
			return this.data.Equals(other);
		}
		#endregion
	}
}

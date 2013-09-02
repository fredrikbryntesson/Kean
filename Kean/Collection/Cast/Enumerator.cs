// 
//  Enumerator.cs
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
using Kean.Core;
using Kean.Core.Extension;

namespace Kean.Core.Collection.Cast
{
	public class Enumerator<T, S> :
		System.Collections.Generic.IEnumerator<S>
	{
		System.Collections.Generic.IEnumerator<T> enumerator;
		Func<T, S> cast;

		public Enumerator(System.Collections.Generic.IEnumerator<T> enumerator, Func<T, S> cast)
		{
			this.enumerator = enumerator;
			this.cast = cast;
		}
		~Enumerator()
		{
			Error.Log.Wrap((Action)this.Dispose)();
		}
		#region IEnumerator<S> Members
		public S Current { get { return this.cast(this.enumerator.Current); } }
		#endregion
		#region IEnumerator Members
		object System.Collections.IEnumerator.Current { get { return this.Current; } }
		public bool MoveNext() { return this.enumerator.MoveNext(); }
		public void Reset() { this.enumerator.Reset(); }
		#endregion
		#region Object Overrides
		public override string ToString()
		{
			return this.enumerator.ToString();
		}
		public override int GetHashCode()
		{
			return this.enumerator.GetHashCode();
		}
		public override bool Equals(object other)
		{
			return this.enumerator.Equals(other);
		}
		#endregion
		#region IDisposable Members
		public void Dispose()
		{
			if (this.enumerator.NotNull())
			{
				this.enumerator.Dispose();
				this.enumerator = null;
			}
		}
		#endregion
	}
}

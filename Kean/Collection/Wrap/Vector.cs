// 
//  Vector.cs
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

namespace Kean.Collection.Wrap
{
	public class Vector<T> :
		Abstract.Vector<T>
	{
		IVector<T> data;
		#region Constructor
		public Vector(IVector<T> data)
		{
			this.data = data;
		}
		#endregion
		#region IVector<T>
		public override int Count { get { return this.data.Count; } }
		public override T this[int index]
		{
			get { return this.data[index]; }
			set { this.data[index] = value; }
		}
		#endregion
	}
}

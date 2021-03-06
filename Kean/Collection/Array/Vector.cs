// 
//  Vector.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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
namespace Kean.Collection.Array
{
	public class Vector<T> :
		Abstract.Vector<T>
	{
		T[] data;
		public override int Count { get { return this.data.Length; } }
		public override T this[int index] {
			get { try { return this.data[index]; } catch (IndexOutOfRangeException e) { throw new Exception.InvalidIndex(e); } }
			set { try { this.data[index] = value; } catch (IndexOutOfRangeException e) { throw new Exception.InvalidIndex(e); } }
		}
		public Vector (int size) :
			this(new T[size])
		{
		}
		public Vector(params T[] data)
		{
			this.data = data;
		}
		#region Operators
		public static implicit operator Vector<T>(T[] data)
		{
			return new Vector<T>(data);
		}
		public static explicit operator T[](Vector<T> data)
		{
			return data.data;
		}
		#endregion
	}
}


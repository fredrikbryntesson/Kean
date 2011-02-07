// 
//  Abstract.cs
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
namespace Kean.Math
{
	public abstract class Abstract<R, V>
		where R : Abstract<R, V>, new()
		where V : struct
	{
		public V Value { get; private set; }
		#region Constants
		public abstract R Zero { get; }
		public abstract R One { get; }
		public abstract R Two { get; }
		#endregion
		#region Constructors
		protected Abstract (V value)
		{
			this.Value = value;
		}
		#endregion
		#region Functions
		#region Arithmetic Functions
		public abstract R Add(R value);
		public abstract R Substract(R value);
		public abstract R Multiply(R value);
		public abstract R Divide(R value);
		#endregion
		#region Trigometric Functions
		public abstract R Sinus();
		public abstract R Cosinus();
		public abstract R Tangens();
		#endregion
		#endregion
	}
}


// 
//  Single.cs
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
using Kean.Core.Basis.Extension;
namespace Kean.Math
{
	public class Single :
		Abstract<Single, float>
	{
		#region Constants
		public override Single Zero { get { return 0; } }
		public override Single One { get { return 1; } }
		public override Single Two { get { return 2; } }
		#endregion
		#region Constructors
		public Single() :
			base(0)
		{ }
		public Single(float value) :
			base(value)
		{ }
		#endregion
		#region Functions
		#region Arithmetic Functions
		public override Single Add(Single value)
		{
			return this + value;
		}
		public override Single Substract(Single value)
		{
			return this - value;
		}
		public override Single Multiply(Single value)
		{
			return this * value;
		}
		public override Single Divide(Single value)
		{
			return this / value;
		}
		#endregion
		#region Trigometric Functions
		public override Single Sinus()
		{
			return Single.Sinus(this.Value);
		}
		public override Single Cosinus()
		{
			return Single.Cosinus(this.Value);
		}
		public override Single Tangens()
		{
			return Single.Tangens(this.Value);
		}
		#endregion
		#endregion
		#region Cast Operators
		public static implicit operator float(Single value)
		{
			return value.IsNull() ? 0 : value.Value;
		}
		public static implicit operator Single(float value)
		{
			return new Single(value);
		}
		#endregion
		#region Static Functions
		#region Properties
		public static float MinimumValue { get { return float.MinValue; } }
		public static float MaximumValue { get { return float.MaxValue; } }
		public static float PI { get { return (float)System.Math.PI; } }
		#endregion
		#region Trigometric Functions
		public static float Sinus(float value)
		{
			return (float)System.Math.Sin(value);
		}
		public static float Cosinus(float value)
		{
			return (float)System.Math.Cos(value);
		}
		public static float Tangens(float value)
		{
			return (float)System.Math.Tan(value);
		}
		#endregion
		#endregion
	}
}


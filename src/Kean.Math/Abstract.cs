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
	public abstract class Abstract<R, V> :
        IEquatable<R>,
        IComparable<R>
        where R : Abstract<R, V>, new()
		where V : struct 
	{
		public V Value { get; private set; }
		#region Constants
		public abstract R Zero { get; }
		public abstract R One { get; }
		public abstract R Two { get; }
        public abstract R MinusInfinity { get; }
        public abstract R PlusInfinity { get; }
        #endregion
		#region Constructors
		protected Abstract (V value)
		{
			this.Value = value;
		}
		#endregion
        public abstract R SetValue(int value);
        public R Copy()
        {
            return new R() { Value = this.Value };
        }
        #region Artihmetic Operators
        public static R operator +(Kean.Math.Abstract<R, V> left, Kean.Math.Abstract<R, V> right)
        {
            return left.Add(right);
        }
        public static R operator -(Kean.Math.Abstract<R, V> left, Kean.Math.Abstract<R, V> right)
        {
            return left.Substract(right);
        }
        public static R operator -(Kean.Math.Abstract<R, V> value)
        {
            return value.Negate();
        }
        public static R operator *(Kean.Math.Abstract<R, V> left, Kean.Math.Abstract<R, V> right)
        {
            return left.Multiply(right);
        }
        public static R operator /(Kean.Math.Abstract<R, V> left, Kean.Math.Abstract<R, V> right)
        {
            return left.Divide(right);
        }
        public static bool operator ==(Kean.Math.Abstract<R, V> left, Kean.Math.Abstract<R, V> right)
        {
            return 
                object.ReferenceEquals(left, right) || (!object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null)) &&
                left.Equals(right);
        }
        public static bool operator !=(Kean.Math.Abstract<R, V> left, Kean.Math.Abstract<R, V> right)
        {
            return !(left == right);
        }
        #endregion
        #region Static Functions
        public static R Absolute(R value)
        {
            return value.LessThan(new R().Zero) ? -value : value;
        }
        public static R Maximum(params R[] values)
        {
            R result = new R().MinusInfinity;
            for(int i = 0; i < values.Length; i++)
                if (result.LessThan(values[i]))
                    result.Value = values[i].Value;
            return result;
        }
        public static R Minimum(params R[] values)
        {
            R result = new R().PlusInfinity;
            for (int i = 0; i < values.Length; i++)
                if (result.GreaterThan(values[i]))
                    result.Value = values[i].Value;
            return result;
        }
        #endregion
        #region Functions
        #region Arithmetic Functions
        public abstract R Add(V value);
		public abstract R Substract(V value);
		public abstract R Multiply(V value);
		public abstract R Divide(V value);
        public abstract R Negate();
        #endregion
		#region Trigometric Functions
		public abstract R Sinus();
		public abstract R Cosinus();
		public abstract R Tangens();
        #endregion
        #region Inverse Trigometric Functions
        public abstract R ArcusSinus();
        public abstract R ArcusCosinus();
        public abstract R ArcusTangens();
        #endregion
        #region Transcendental Functions
        public abstract R Exponential();
        public abstract R Logarithm();
        #endregion
        #region Power Function
        public abstract R Power(R value);
        #endregion
        #region Object overides and IEquatable<R>
        public override bool Equals(object other)
        {
            return (other is R) && this.Equals(other as R);
        }
        // other is not null here.
        public bool Equals(R other)
        {
            return this.Value.Equals(other.Value);
        }
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
        #endregion
        #region Comparison Functions and IComparable<R>
        public abstract bool LessThan(R other);
        public bool LessOrEqualThan(R other)
        {
            return this.LessThan(other) || this == other; 
        }
        public abstract bool GreaterThan(R other);
        public bool GreaterOrEqualThan(R other)
        {
            return this.GreaterThan(other) || this == other;
        }
        public int CompareTo(R other)
        {
            return this.LessThan(other) ? -1 : (this.GreaterThan(other) ? 1 : 0);
        }
        #endregion
        #endregion
        #region SetValue between class and struct.
        public static implicit operator V(Abstract<R, V> value)
        {
            return value.Value;
        }
        public static implicit operator Abstract<R, V>(V value)
        {
            return new R() { Value = value };
        }
        #endregion
    }
}


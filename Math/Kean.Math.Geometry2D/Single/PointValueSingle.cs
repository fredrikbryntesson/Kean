// 
//  PointValueSingle.cs
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
using Kean.Core.Extension;

namespace Kean.Math.Geometry2D.Single
{
    public struct PointValueSingle :
        Abstract.IPoint<float>, 
        Abstract.IVector<float>,
        IEquatable<PointValueSingle>
    {
        public float X;
        public float Y;
        #region IPoint<float> Members
        float Kean.Math.Geometry2D.Abstract.IPoint<float>.X { get { return this.X; } }
        float Kean.Math.Geometry2D.Abstract.IPoint<float>.Y { get { return this.Y; } }
        #endregion
        #region IVector<float> Members
        float Kean.Math.Geometry2D.Abstract.IVector<float>.X { get { return this.X; } }
        float Kean.Math.Geometry2D.Abstract.IVector<float>.Y { get { return this.Y; } }
        #endregion
        public float Norm { get { return Kean.Math.Single.SquareRoot(this.ScalarProduct(this)); } }
        public float Azimuth { get { return Kean.Math.Single.ArcusTangensExtended(this.Y, this.X); } }
        #region Static Constants
        public static PointValueSingle BasisX { get { return new PointValueSingle(1, 0); } }
        public static PointValueSingle BasisY { get { return new PointValueSingle(0, 1); } }
        #endregion
        public PointValueSingle(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        public float PNorm(float p)
        {
            float result;
            if (float.IsPositiveInfinity(p))
                result = Kean.Math.Single.Maximum(Kean.Math.Single.Absolute(this.X), Kean.Math.Single.Absolute(this.Y));
            else if (p == 1)
                result = Kean.Math.Single.Absolute(this.X) + Kean.Math.Single.Absolute(this.Y);
            else
                result = Kean.Math.Single.Power(Kean.Math.Single.Power(Kean.Math.Single.Absolute(this.X), p) + Kean.Math.Single.Power(Kean.Math.Single.Absolute(this.Y), p), 1 / p);
            return result;
        }
        /// <summary>
        /// Angle from current to other point vector.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Angle in radians.</returns>
        public float Angle(PointValueSingle other)
        {
            float result = 0;
            result = this.ScalarProduct(other) / (this.Norm * other.Norm);
            float sign = this.X * other.Y - this.Y * other.X;
            result = Kean.Math.Single.ArcusCosinus(Kean.Math.Single.Clamp(result, -1, 1));
            result *= sign < 0 ? -1 : 1;
            return result;
        }
        public float ScalarProduct(PointValueSingle other)
        {
            return this.X * other.X + this.Y * other.Y;
        }
        public float Distance(PointValueSingle other)
        {
            return (this - other).Norm;
        }
        #region Utility functions
        public PointValueSingle Swap()
        {
            return new PointValueSingle(this.Y, this.X);
        }
        public PointValueSingle Round()
        {
            return new PointValueSingle(Kean.Math.Single.Round(this.X), Kean.Math.Single.Round(this.Y));
        }
        public PointValueSingle Ceiling()
        {
            return new PointValueSingle(Kean.Math.Single.Ceiling(this.X), Kean.Math.Single.Ceiling(this.Y));
        }
        public PointValueSingle Floor()
        {
            return new PointValueSingle(Kean.Math.Single.Floor(this.X), Kean.Math.Single.Floor(this.Y));
        }
        public PointValueSingle Minimum(Abstract.IVector<float> floor)
        {
            return new PointValueSingle(Kean.Math.Single.Minimum(this.X, floor.X), Kean.Math.Single.Minimum(this.Y, floor.Y));
        }
        public PointValueSingle Maximum(Abstract.IVector<float> ceiling)
        {
            return new PointValueSingle(Kean.Math.Single.Maximum(this.X, ceiling.X), Kean.Math.Single.Maximum(this.Y, ceiling.Y));
        }
        public PointValueSingle Clamp(Abstract.IVector<float> floor, Abstract.IVector<float> ceiling)
        {
            return new PointValueSingle(Kean.Math.Single.Clamp(this.X, floor.X, ceiling.X), Kean.Math.Single.Clamp(this.Y, floor.Y, ceiling.Y));
        }
        #endregion
        #region Arithmetic Vector - Vector Operators
        public static PointValueSingle operator +(PointValueSingle left, PointValueSingle right)
        {
            return new PointValueSingle(left.X + right.X, left.Y + right.Y);
        }
        public static PointValueSingle operator +(PointValueSingle left, Abstract.IVector<float> right)
        {
            return new PointValueSingle(left.X + right.X, left.Y + right.Y);
        }
        public static PointValueSingle operator +(Abstract.IVector<float> left, PointValueSingle right)
        {
            return new PointValueSingle(left.X + right.X, left.Y + right.Y);
        }
        public static PointValueSingle operator -(PointValueSingle left, PointValueSingle right)
        {
            return new PointValueSingle(left.X - right.X, left.Y - right.Y);
        }
        public static PointValueSingle operator -(PointValueSingle left, Abstract.IVector<float> right)
        {
            return new PointValueSingle(left.X - right.X, left.Y - right.Y);
        }
        public static PointValueSingle operator -(Abstract.IVector<float> left, PointValueSingle right)
        {
            return new PointValueSingle(left.X - right.X, left.Y - right.Y);
        }
        public static PointValueSingle operator -(PointValueSingle vector)
        {
            return new PointValueSingle(-vector.X, -vector.Y);
        }
        public static PointValueSingle operator *(PointValueSingle left, PointValueSingle right)
        {
            return new PointValueSingle(left.X * right.X, left.Y * right.Y);
        }
        public static PointValueSingle operator *(PointValueSingle left, Abstract.IVector<float> right)
        {
            return new PointValueSingle(left.X * right.X, left.Y * right.Y);
        }
        public static PointValueSingle operator *(Abstract.IVector<float> left, PointValueSingle right)
        {
            return new PointValueSingle(left.X * right.X, left.Y * right.Y);
        }
        public static PointValueSingle operator /(PointValueSingle left, PointValueSingle right)
        {
            return new PointValueSingle(left.X / right.X, left.Y / right.Y);
        }
        public static PointValueSingle operator /(PointValueSingle left, Abstract.IVector<float> right)
        {
            return new PointValueSingle(left.X / right.X, left.Y / right.Y);
        }
        public static PointValueSingle operator /(Abstract.IVector<float> left, PointValueSingle right)
        {
            return new PointValueSingle(left.X / right.X, left.Y / right.Y);
        }
        #endregion
        #region Arithmetic Vector and Scalar
        public static PointValueSingle operator *(PointValueSingle left, float right)
        {
            return new PointValueSingle(left.X * right, left.Y * right);
        }
        public static PointValueSingle operator *(float left, PointValueSingle right)
        {
            return right * left;
        }
        public static PointValueSingle operator /(PointValueSingle left, float right)
        {
            return new PointValueSingle(left.X / right, left.Y / right);
        }
        #endregion
        #region Arithmetic Transform and Vector
        public static PointValueSingle operator *(TransformValue left, PointValueSingle right)
        {
            return new PointValueSingle(left.A * right.X + left.C * right.Y + left.E, left.B * right.X + left.D * right.Y + left.F);
        }
        #endregion
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>True if <paramref name="Left"/> equals <paramref name="Right"/> else false.</returns>
        public static bool operator ==(PointValueSingle left, PointValueSingle right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>False if <paramref name="Left"/> equals <paramref name="Right"/> else true.</returns>
        public static bool operator !=(PointValueSingle left, PointValueSingle right)
        {
            return !(left == right);
        }
        public static bool operator <(PointValueSingle left, PointValueSingle right)
        {
            return left.X < right.X && left.Y < right.Y;
        }
        public static bool operator >(PointValueSingle left, PointValueSingle right)
        {
            return left.X > right.X && left.Y > right.Y;
        }
        public static bool operator <=(PointValueSingle left, PointValueSingle right)
        {
            return left.X <= right.X && left.Y <= right.Y;
        }
        public static bool operator >=(PointValueSingle left, PointValueSingle right)
        {
            return left.X >= right.X && left.Y >= right.Y;
        }
        #endregion
        #region IEquatable<PointValueSingle> Members
        public bool Equals(PointValueSingle other)
        {
            return this == other;
        }
        #endregion
        #region Object Overrides
        public override bool Equals(object other)
        {
            return other is PointValueSingle && this.Equals((PointValueSingle)other);
        }
        public override int GetHashCode()
        {
            return 33 * this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
		public override string ToString()
        {
			return Kean.Math.Single.ToString(this.X) + ", " + Kean.Math.Single.ToString(this.Y);
		}
        #endregion
        #region Static Creators
        public static PointValueSingle Polar(float radius, float azimuth)
        {
            return new PointValueSingle(radius * Kean.Math.Single.Cosinus(azimuth), radius * Kean.Math.Single.Sinus(azimuth));
        }
        #endregion
        #region Casts
        public static explicit operator float[](PointValueSingle value)
        {
            return new float[] { value.X, value.Y };
        }
        public static explicit operator PointValueSingle(float[] value)
        {
            return new PointValueSingle(value[0], value[1]);
        }
        public static implicit operator string(PointValueSingle value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator PointValueSingle(string value)
        {
            PointValueSingle result = new PointValueSingle();
            if (value.NotEmpty())
            {
                try
                {
                    string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 2)
                        result = new PointValueSingle(Kean.Math.Single.Parse(values[0]), Kean.Math.Single.Parse(values[1]));
                }
                catch
                {
                }
            }
            return result;
        }
        public static explicit operator SizeValue(PointValueSingle value)
        {
            return new SizeValue(value.X, value.Y);
        }
        public static explicit operator PointValueSingle(SizeValue value)
        {
            return new PointValueSingle(value.Width, value.Height);
        }
        public static implicit operator PointValueSingle(Integer.PointValueSingle value)
        {
            return new PointValueSingle(value.X, value.Y);
        }
        public static explicit operator Integer.PointValueSingle(PointValueSingle value)
        {
            return new Integer.PointValueSingle(Kean.Math.Integer.Convert(value.X), Kean.Math.Integer.Convert(value.Y));
        }
        #endregion
        #region Static Operators
        public static PointValueSingle Floor(PointValueSingle other)
        {
            return new PointValueSingle(Kean.Math.Integer.Floor(other.X), Kean.Math.Integer.Floor(other.Y));
        }
        public static PointValueSingle Ceiling(PointValueSingle other)
        {
            return new PointValueSingle(Kean.Math.Integer.Ceiling(other.X), Kean.Math.Integer.Ceiling(other.Y));
        }
        public static PointValueSingle Maximum(PointValueSingle left, PointValueSingle right)
        {
            return new PointValueSingle(Kean.Math.Single.Maximum(left.X, right.X), Kean.Math.Single.Maximum(left.Y, right.Y));
        }
        public static PointValueSingle Minimum(PointValueSingle left, PointValueSingle right)
        {
            return new PointValueSingle(Kean.Math.Single.Minimum(left.X, right.X), Kean.Math.Single.Minimum(left.Y, right.Y));
        }
        #endregion
  }
}

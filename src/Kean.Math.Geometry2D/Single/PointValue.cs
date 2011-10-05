// 
//  PointValue.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System;
using Kean.Core.Extension;

namespace Kean.Math.Geometry2D.Single
{
    public struct PointValue :
        Abstract.IPoint<float>, Abstract.IVector<float>
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
        public float Length { get { return Kean.Math.Single.SquareRoot(this.ScalarProduct(this)); } }
        #endregion
        public PointValue(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        public void Clear()
        {
            this.X = this.Y = 0;
        }
        public void Set(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        public float Norm(float p)
        {
            float result;
            if (float.IsPositiveInfinity(p))
                result = Kean.Math.Single.Maximum(Kean.Math.Single.Absolute(this.X), Kean.Math.Single.Absolute(this.Y));
            else if (p == 1)
                result = Kean.Math.Single.Absolute(this.X) + Kean.Math.Single.Absolute(this.Y);
            else
                result = Kean.Math.Single.Power(Kean.Math.Single.Power(Kean.Math.Single.Absolute(this.X), p) + Kean.Math.Single.Power(Kean.Math.Single.Absolute(this.Y), p), 1f / p);
            return result;
        }
        public float ScalarProduct(PointValue other)
        {
            return this.X * other.X + this.Y * other.Y;
        }
        public float Distance(PointValue other)
        {
            return (this - other).Length;
        }
        /// <summary>
        /// Angle from current to other point vector.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Angle in radians.</returns>
        public float Angle(PointValue other)
        {
            float result = 0;
            result = this.ScalarProduct(other) / (this.Length * other.Length);
            float sign = this.X * other.Y - this.Y * other.X;
            result = Kean.Math.Single.ArcusCosinus(Kean.Math.Single.Clamp(result, -1, 1));
            result *= sign < 0 ? -1 : 1;
            return result;
        }
        #region Arithmetic Vector - Vector Operators
        public static void Add(ref PointValue left, ref PointValue right, ref PointValue result)
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
        }
        public static void Subtract(ref PointValue left, ref PointValue right, ref PointValue result)
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
        }
        public static void Multiply(ref PointValue left, ref PointValue right, ref PointValue result)
        {
            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
        }
        public static void Divide(ref PointValue left, ref PointValue right, ref PointValue result)
        {
            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
        }
        public static void Multiply(ref PointValue left, ref float right, ref PointValue result)
        {
            result.X = left.X * right;
            result.Y = left.Y * right;
        }
        public static void Divide(ref PointValue left, ref float right, ref PointValue result)
        {
            result.X = left.X / right;
            result.Y = left.Y / right;
        }
        public static PointValue operator +(PointValue left, PointValue right)
        {
            return new PointValue(left.X + right.X, left.Y + right.Y);
        }
        public static PointValue operator -(PointValue left, PointValue right)
        {
            return new PointValue(left.X - right.X, left.Y - right.Y);
        }
        public static PointValue operator -(PointValue vector)
        {
            return new PointValue(-vector.X, -vector.Y);
        }
        public static float operator *(PointValue left, PointValue right)
        {
            return left.X * right.X + left.Y * right.Y;
        }
        public void Add(float x, float y)
        {
            this.X += x;
            this.Y += y;
        }
        public void Add(PointValue other)
        {
            this.X += other.X;
            this.Y += other.Y;
        }
        #endregion
        #region Arithmetic Vector and Scalar
        public void Multiply(float scalar)
        {
            this.X *= scalar;
            this.Y *= scalar;
        }
        public static PointValue operator *(PointValue left, float right)
        {
            return new PointValue(left.X * right, left.Y * right);
        }
        public static PointValue operator *(float left, PointValue right)
        {
            return right * left;
        }
        #endregion
        #region Static Operators
        public static PointValue Floor(Geometry2D.Single.PointValue other)
        {
            return new PointValue(Kean.Math.Integer.Floor(other.X), Kean.Math.Integer.Floor(other.Y));
        }
        public static PointValue Ceiling(Geometry2D.Single.PointValue other)
        {
            return new PointValue(Kean.Math.Integer.Ceiling(other.X), Kean.Math.Integer.Ceiling(other.Y));
        }
        public static PointValue Floor(Geometry2D.Double.PointValue other)
        {
            return new PointValue(Kean.Math.Integer.Floor(other.X), Kean.Math.Integer.Floor(other.Y));
        }
        public static PointValue Ceiling(Geometry2D.Double.PointValue other)
        {
            return new PointValue(Kean.Math.Integer.Ceiling(other.X), Kean.Math.Integer.Ceiling(other.Y));
        }
        public static PointValue Maximum(PointValue left, PointValue right)
        {
            return new PointValue(Kean.Math.Single.Maximum(left.X, right.X), Kean.Math.Single.Maximum(left.Y, right.Y));
        }
        public static PointValue Minimum(PointValue left, PointValue right)
        {
            return new PointValue(Kean.Math.Single.Minimum(left.X, right.X), Kean.Math.Single.Minimum(left.Y, right.Y));
        }
        #endregion
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>True if <paramref name="Left"/> equals <paramref name="Right"/> else false.</returns>
        public static bool operator ==(PointValue left, PointValue right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>False if <paramref name="Left"/> equals <paramref name="Right"/> else true.</returns>
        public static bool operator !=(PointValue left, PointValue right)
        {
            return !(left == right);
        }
        public static bool operator <(PointValue left, PointValue right)
        {
            return left.X < right.X && left.Y < right.Y;
        }
        public static bool operator >(PointValue left, PointValue right)
        {
            return left.X > right.X && left.Y > right.Y;
        }
        public static bool operator <=(PointValue left, PointValue right)
        {
            return left.X <= right.X && left.Y <= right.Y;
        }
        public static bool operator >=(PointValue left, PointValue right)
        {
            return left.X >= right.X && left.Y >= right.Y;
        }
        #endregion
        #region Casts
        public static implicit operator PointValue(Integer.PointValue value)
        {
            return new PointValue(value.X, value.Y);
        }
        public static explicit operator Integer.PointValue(PointValue value)
        {
            return new Integer.PointValue((Kean.Math.Integer)(value.X), (Kean.Math.Integer)(value.Y));
        }
        public static implicit operator string(PointValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator PointValue(string value)
        {
            PointValue result = new PointValue();
            if (value.NotEmpty())
            {
                try
                {
					result = (PointValue)(Point)value;
				}
                catch
                {
                }
            }
            return result;
        }
        #endregion
        #region Object Overrides
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
		public override string ToString()
		{
			return this.ToString(true);
		}
		public string ToString(bool commaSeparated)
		{
			return ((Point)this).ToString(commaSeparated);
		}
        #endregion
    }
}

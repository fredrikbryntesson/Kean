// 
//  PointValueDouble.cs
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

namespace Kean.Math.Geometry2D.Double
{
    public struct PointValueDouble :
        Abstract.IPoint<double>, 
        Abstract.IVector<double>,
        IEquatable<PointValueDouble>
    {
        public double X;
        public double Y;
        #region IPoint<double> Members
        double Kean.Math.Geometry2D.Abstract.IPoint<double>.X { get { return this.X; } }
        double Kean.Math.Geometry2D.Abstract.IPoint<double>.Y { get { return this.Y; } }
        #endregion
        #region IVector<double> Members
        double Kean.Math.Geometry2D.Abstract.IVector<double>.X { get { return this.X; } }
        double Kean.Math.Geometry2D.Abstract.IVector<double>.Y { get { return this.Y; } }
        #endregion
        public double Norm { get { return Kean.Math.Double.SquareRoot(this.ScalarProduct(this)); } }
        public double Azimuth { get { return Kean.Math.Double.ArcusTangensExtended(this.Y, this.X); } }
        #region Static Constants
        public static PointValueDouble BasisX { get { return new PointValueDouble(1, 0); } }
        public static PointValueDouble BasisY { get { return new PointValueDouble(0, 1); } }
        #endregion
        public PointValueDouble(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public double PNorm(double p)
        {
            double result;
            if (double.IsPositiveInfinity(p))
                result = Kean.Math.Double.Maximum(Kean.Math.Double.Absolute(this.X), Kean.Math.Double.Absolute(this.Y));
            else if (p == 1)
                result = Kean.Math.Double.Absolute(this.X) + Kean.Math.Double.Absolute(this.Y);
            else
                result = Kean.Math.Double.Power(Kean.Math.Double.Power(Kean.Math.Double.Absolute(this.X), p) + Kean.Math.Double.Power(Kean.Math.Double.Absolute(this.Y), p), 1 / p);
            return result;
        }
        /// <summary>
        /// Angle from current to other point vector.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Angle in radians.</returns>
        public double Angle(PointValueDouble other)
        {
            double result = 0;
            result = this.ScalarProduct(other) / (this.Norm * other.Norm);
            double sign = this.X * other.Y - this.Y * other.X;
            result = Kean.Math.Double.ArcusCosinus(Kean.Math.Double.Clamp(result, -1, 1));
            result *= sign < 0 ? -1 : 1;
            return result;
        }
        public double ScalarProduct(PointValueDouble other)
        {
            return this.X * other.X + this.Y * other.Y;
        }
        public double Distance(PointValueDouble other)
        {
            return (this - other).Norm;
        }
        #region Utility functions
        public PointValueDouble Swap()
        {
            return new PointValueDouble(this.Y, this.X);
        }
        public PointValueDouble Round()
        {
            return new PointValueDouble(Kean.Math.Double.Round(this.X), Kean.Math.Double.Round(this.Y));
        }
        public PointValueDouble Ceiling()
        {
            return new PointValueDouble(Kean.Math.Double.Ceiling(this.X), Kean.Math.Double.Ceiling(this.Y));
        }
        public PointValueDouble Floor()
        {
            return new PointValueDouble(Kean.Math.Double.Floor(this.X), Kean.Math.Double.Floor(this.Y));
        }
        public PointValueDouble Minimum(Abstract.IVector<double> floor)
        {
            return new PointValueDouble(Kean.Math.Double.Minimum(this.X, floor.X), Kean.Math.Double.Minimum(this.Y, floor.Y));
        }
        public PointValueDouble Maximum(Abstract.IVector<double> ceiling)
        {
            return new PointValueDouble(Kean.Math.Double.Maximum(this.X, ceiling.X), Kean.Math.Double.Maximum(this.Y, ceiling.Y));
        }
        public PointValueDouble Clamp(Abstract.IVector<double> floor, Abstract.IVector<double> ceiling)
        {
            return new PointValueDouble(Kean.Math.Double.Clamp(this.X, floor.X, ceiling.X), Kean.Math.Double.Clamp(this.Y, floor.Y, ceiling.Y));
        }
        #endregion
        #region Arithmetic Vector - Vector Operators
        public static PointValueDouble operator +(PointValueDouble left, PointValueDouble right)
        {
            return new PointValueDouble(left.X + right.X, left.Y + right.Y);
        }
        public static PointValueDouble operator +(PointValueDouble left, Abstract.IVector<double> right)
        {
            return new PointValueDouble(left.X + right.X, left.Y + right.Y);
        }
        public static PointValueDouble operator +(Abstract.IVector<double> left, PointValueDouble right)
        {
            return new PointValueDouble(left.X + right.X, left.Y + right.Y);
        }
        public static PointValueDouble operator -(PointValueDouble left, PointValueDouble right)
        {
            return new PointValueDouble(left.X - right.X, left.Y - right.Y);
        }
        public static PointValueDouble operator -(PointValueDouble left, Abstract.IVector<double> right)
        {
            return new PointValueDouble(left.X - right.X, left.Y - right.Y);
        }
        public static PointValueDouble operator -(Abstract.IVector<double> left, PointValueDouble right)
        {
            return new PointValueDouble(left.X - right.X, left.Y - right.Y);
        }
        public static PointValueDouble operator -(PointValueDouble vector)
        {
            return new PointValueDouble(-vector.X, -vector.Y);
        }
        public static PointValueDouble operator *(PointValueDouble left, PointValueDouble right)
        {
            return new PointValueDouble(left.X * right.X, left.Y * right.Y);
        }
        public static PointValueDouble operator *(PointValueDouble left, Abstract.IVector<double> right)
        {
            return new PointValueDouble(left.X * right.X, left.Y * right.Y);
        }
        public static PointValueDouble operator *(Abstract.IVector<double> left, PointValueDouble right)
        {
            return new PointValueDouble(left.X * right.X, left.Y * right.Y);
        }
        public static PointValueDouble operator /(PointValueDouble left, PointValueDouble right)
        {
            return new PointValueDouble(left.X / right.X, left.Y / right.Y);
        }
        public static PointValueDouble operator /(PointValueDouble left, Abstract.IVector<double> right)
        {
            return new PointValueDouble(left.X / right.X, left.Y / right.Y);
        }
        public static PointValueDouble operator /(Abstract.IVector<double> left, PointValueDouble right)
        {
            return new PointValueDouble(left.X / right.X, left.Y / right.Y);
        }
        #endregion
        #region Arithmetic Vector and Scalar
        public static PointValueDouble operator *(PointValueDouble left, double right)
        {
            return new PointValueDouble(left.X * right, left.Y * right);
        }
        public static PointValueDouble operator *(double left, PointValueDouble right)
        {
            return right * left;
        }
        public static PointValueDouble operator /(PointValueDouble left, double right)
        {
            return new PointValueDouble(left.X / right, left.Y / right);
        }
        #endregion
        #region Arithmetic Transform and Vector
        public static PointValueDouble operator *(TransformValue left, PointValueDouble right)
        {
            return new PointValueDouble(left.A * right.X + left.C * right.Y + left.E, left.B * right.X + left.D * right.Y + left.F);
        }
        #endregion
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>True if <paramref name="Left"/> equals <paramref name="Right"/> else false.</returns>
        public static bool operator ==(PointValueDouble left, PointValueDouble right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>False if <paramref name="Left"/> equals <paramref name="Right"/> else true.</returns>
        public static bool operator !=(PointValueDouble left, PointValueDouble right)
        {
            return !(left == right);
        }
        public static bool operator <(PointValueDouble left, PointValueDouble right)
        {
            return left.X < right.X && left.Y < right.Y;
        }
        public static bool operator >(PointValueDouble left, PointValueDouble right)
        {
            return left.X > right.X && left.Y > right.Y;
        }
        public static bool operator <=(PointValueDouble left, PointValueDouble right)
        {
            return left.X <= right.X && left.Y <= right.Y;
        }
        public static bool operator >=(PointValueDouble left, PointValueDouble right)
        {
            return left.X >= right.X && left.Y >= right.Y;
        }
        #endregion
        #region IEquatable<PointValueDouble> Members
        public bool Equals(PointValueDouble other)
        {
            return this == other;
        }
        #endregion
        #region Object Overrides
        public override bool Equals(object other)
        {
            return other is PointValueDouble && this.Equals((PointValueDouble)other);
        }
        public override int GetHashCode()
        {
            return 33 * this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
		public override string ToString()
        {
			return Kean.Math.Double.ToString(this.X) + ", " + Kean.Math.Double.ToString(this.Y);
		}
        #endregion
        #region Static Creators
        public static PointValueDouble Polar(double radius, double azimuth)
        {
            return new PointValueDouble(radius * Kean.Math.Double.Cosinus(azimuth), radius * Kean.Math.Double.Sinus(azimuth));
        }
        #endregion
        #region Casts
        public static explicit operator double[](PointValueDouble value)
        {
            return new double[] { value.X, value.Y };
        }
        public static explicit operator PointValueDouble(double[] value)
        {
            return new PointValueDouble(value[0], value[1]);
        }
        public static implicit operator string(PointValueDouble value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator PointValueDouble(string value)
        {
            PointValueDouble result = new PointValueDouble();
            if (value.NotEmpty())
            {
                try
                {
                    string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 2)
                        result = new PointValueDouble(Kean.Math.Double.Parse(values[0]), Kean.Math.Double.Parse(values[1]));
                }
                catch
                {
                }
            }
            return result;
        }
        public static explicit operator SizeValue(PointValueDouble value)
        {
            return new SizeValue(value.X, value.Y);
        }
        public static explicit operator PointValueDouble(SizeValue value)
        {
            return new PointValueDouble(value.Width, value.Height);
        }
        public static implicit operator PointValueDouble(Single.PointValueDouble value)
        {
            return new PointValueDouble(value.X, value.Y);
        }
        public static implicit operator PointValueDouble(Integer.PointValueDouble value)
        {
            return new PointValueDouble(value.X, value.Y);
        }
        public static explicit operator Single.PointValueDouble(PointValueDouble value)
        {
            return new Single.PointValueDouble(Kean.Math.Single.Convert(value.X), Kean.Math.Single.Convert(value.Y));
        }
        public static explicit operator Integer.PointValueDouble(PointValueDouble value)
        {
            return new Integer.PointValueDouble(Kean.Math.Integer.Convert(value.X), Kean.Math.Integer.Convert(value.Y));
        }
        #endregion
        #region Static Operators
        public static PointValueDouble Ceiling(Geometry2D.Single.PointValueDouble other)
        {
            return new PointValueDouble(Kean.Math.Integer.Ceiling(other.X), Kean.Math.Integer.Ceiling(other.Y));
        }
        public static PointValueDouble Floor(Geometry2D.Single.PointValueDouble other)
        {
            return new PointValueDouble(Kean.Math.Integer.Floor(other.X), Kean.Math.Integer.Floor(other.Y));
        }
         public static PointValueDouble Floor(PointValueDouble other)
        {
            return new PointValueDouble(Kean.Math.Integer.Floor(other.X), Kean.Math.Integer.Floor(other.Y));
        }
        public static PointValueDouble Ceiling(PointValueDouble other)
        {
            return new PointValueDouble(Kean.Math.Integer.Ceiling(other.X), Kean.Math.Integer.Ceiling(other.Y));
        }
        public static PointValueDouble Maximum(PointValueDouble left, PointValueDouble right)
        {
            return new PointValueDouble(Kean.Math.Double.Maximum(left.X, right.X), Kean.Math.Double.Maximum(left.Y, right.Y));
        }
        public static PointValueDouble Minimum(PointValueDouble left, PointValueDouble right)
        {
            return new PointValueDouble(Kean.Math.Double.Minimum(left.X, right.X), Kean.Math.Double.Minimum(left.Y, right.Y));
        }
        #endregion
  }
}

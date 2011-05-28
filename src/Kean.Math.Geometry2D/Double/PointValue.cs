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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Kean.Core.Basis.Extension;

namespace Kean.Math.Geometry2D.Double
{
    public struct PointValue :
        Abstract.IPoint<double>, Abstract.IVector<double>
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
        public double Length { get { return Kean.Math.Double.SquareRoot(this.X * this.X + this.Y * this.Y); } }
        public double LengthSquared { get { return this.X * this.X + this.Y * this.Y; } }
        public double NormAbsolute { get { return Kean.Math.Double.Maximum(Kean.Math.Double.Absolute(this.X), Kean.Math.Double.Absolute(this.Y)); } }
        public double NormTaxicab { get { return Kean.Math.Double.Absolute(this.X) + Kean.Math.Double.Absolute(this.Y); } }
        public PointValue(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public void Clear()
        {
            this.X = this.Y = 0;    
        }
        public void Set(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public void Set(ref double x, ref double y)
        {
            this.X = x;
            this.Y = y;
        }
        #region Arithmetic Vector - Vector Operators
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
        public static double operator *(PointValue left, PointValue right)
        {
            return left.X * right.X + left.Y * right.Y;
        }
        public void Add(double x, double y)
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
        public void Multiply(double scalar)
        {
            this.X *= scalar;
            this.Y *= scalar;
        }
        public static PointValue operator *(PointValue left, double right)
        {
            return new PointValue(left.X * right, left.Y * right);
        }
        public static PointValue operator *(double left, PointValue right)
        {
            return right * left;
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
        #endregion
        #region Casts
        public static implicit operator PointValue(Single.PointValue value)
        {
            return new PointValue(value.X, value.Y);
        }
        public static implicit operator PointValue(Integer.PointValue value)
        {
            return new PointValue(value.X, value.Y);
        }
        public static explicit operator Single.PointValue(PointValue value)
        {
            return new Single.PointValue((Kean.Math.Single)(value.X), (Kean.Math.Single)(value.Y));
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
                    string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 2)
                        result = new PointValue(Kean.Math.Double.Parse(values[0]), Kean.Math.Double.Parse(values[1]));
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
            return Kean.Math.Double.ToString(this.X) + " " + Kean.Math.Double.ToString(this.Y);
        }
        #endregion
    }
}

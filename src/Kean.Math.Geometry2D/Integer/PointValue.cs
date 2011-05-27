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
using Kean.Core.Basis.Extension;

namespace Kean.Math.Geometry2D.Integer
{
	public struct PointValue :
		Abstract.IPoint<int>, Abstract.IVector<int>
	{
        public int X;
        public int Y;
        #region IPoint<int> Members
        int Kean.Math.Geometry2D.Abstract.IPoint<int>.X { get { return this.X; } }
        int Kean.Math.Geometry2D.Abstract.IPoint<int>.Y { get { return this.Y; } }
        #endregion
        #region IVector<int> Members
        int Kean.Math.Geometry2D.Abstract.IVector<int>.X { get { return this.X; } }
        int Kean.Math.Geometry2D.Abstract.IVector<int>.Y { get { return this.Y; } }
        #endregion
        public int Length { get { return Kean.Math.Integer.SquareRoot(this.X * this.X + this.Y * this.Y); } }
        public int LengthSquared { get { return this.X * this.X + this.Y * this.Y; } }
        public int NormAbsolute { get { return Kean.Math.Integer.Maximum(Kean.Math.Integer.Absolute(this.X), Kean.Math.Integer.Absolute(this.Y)); } }
        public int NormTaxicab { get { return Kean.Math.Integer.Absolute(this.X) + Kean.Math.Integer.Absolute(this.Y); } }
        public PointValue(int x, int y)
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
        public static int operator *(PointValue left, PointValue right)
        {
            return left.X * right.X + left.Y * right.Y;
        }
        public void Add(int x, int y)
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
        public void Multiply(int scalar)
        {
            this.X *= scalar;
            this.Y *= scalar;
        }
        public static PointValue operator *(PointValue left, int right)
        {
            return new PointValue(left.X * right, left.Y * right);
        }
        public static PointValue operator *(int left, PointValue right)
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
                        result = new PointValue(Kean.Math.Integer.Parse(values[0]), Kean.Math.Integer.Parse(values[1]));
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
            return Kean.Math.Integer.ToString(this.X) + " " + Kean.Math.Integer.ToString(this.Y);
        }
        #endregion
    }
}

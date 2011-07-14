// 
//  SizeValue.cs
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
	public struct SizeValue :
		Abstract.ISize<int>, Abstract.IVector<int>
	{
        public int Width;
        public int Height;
        #region ISize<int>
        int Abstract.ISize<int>.Width { get { return this.Width; } }
        int Abstract.ISize<int>.Height { get { return this.Height; } }
        #endregion
        #region IVector<int> Members
        int Abstract.IVector<int>.X { get { return this.Width; } }
        int Abstract.IVector<int>.Y { get { return this.Height; } }
        #endregion
        public int Area { get { return this.Width * this.Height; } }
        public bool IsEmpty { get { return this.Width == 0 || this.Height == 0; } }
        public SizeValue(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
        #region Static Operators
        public static SizeValue operator -(SizeValue left, SizeValue right)
        {
            return new SizeValue(left.Width - right.Width, left.Height - right.Height);
        }
        public static SizeValue operator +(SizeValue left, SizeValue right)
        {
            return new SizeValue(left.Width + right.Width, left.Height + right.Height);
        }
        public static SizeValue Floor(Geometry2D.Single.SizeValue other)
        {
            return new SizeValue(Kean.Math.Integer.Floor(other.Width), Kean.Math.Integer.Floor(other.Height));
        }
        public static SizeValue Ceiling(Geometry2D.Single.SizeValue other)
        {
            return new SizeValue(Kean.Math.Integer.Ceiling(other.Width), Kean.Math.Integer.Ceiling(other.Height));
        }
        public static SizeValue Floor(Geometry2D.Double.SizeValue other)
        {
            return new SizeValue(Kean.Math.Integer.Floor(other.Width), Kean.Math.Integer.Floor(other.Height));
        }
        public static SizeValue Ceiling(Geometry2D.Double.SizeValue other)
        {
            return new SizeValue(Kean.Math.Integer.Ceiling(other.Width), Kean.Math.Integer.Ceiling(other.Height));
        }
        public static SizeValue Maximum(SizeValue left, SizeValue right)
        {
            return new SizeValue(Kean.Math.Integer.Maximum(left.Width, right.Width), Kean.Math.Integer.Maximum(left.Height, right.Height));
        }
        public static SizeValue Minimum(SizeValue left, SizeValue right)
        {
            return new SizeValue(Kean.Math.Integer.Minimum(left.Width, right.Width), Kean.Math.Integer.Minimum(left.Height, right.Height));
        }
        #endregion
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>True if <paramref name="Left"/> equals <paramref name="Right"/> else false.</returns>
        public static bool operator ==(SizeValue left, SizeValue right)
        {
            return left.Width == right.Width && left.Height == right.Height;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>False if <paramref name="Left"/> equals <paramref name="Right"/> else true.</returns>
        public static bool operator !=(SizeValue left, SizeValue right)
        {
            return !(left == right);
        }
        #endregion
        #region Casts
        public static implicit operator string(SizeValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator SizeValue(string value)
        {
            SizeValue result = new SizeValue();
            if (value.NotEmpty())
            {
                try
                {
                    string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 2)
                        result = new SizeValue(Kean.Math.Integer.Parse(values[0]), Kean.Math.Integer.Parse(values[1]));
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
            return this.Width.GetHashCode() ^ this.Height.GetHashCode();
        }
        public override string ToString()
        {
            return Kean.Math.Integer.ToString(this.Width) + " " + Kean.Math.Integer.ToString(this.Height);
        }
        #endregion
   	}
}

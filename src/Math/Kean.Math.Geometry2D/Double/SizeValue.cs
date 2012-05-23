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
using Kean.Core.Extension;

namespace Kean.Math.Geometry2D.Double
{
	public struct SizeValue :
		Abstract.ISize<double>, Abstract.IVector<double>
	{
		public double Width;
		public double Height;
        #region ISize<double>
        double Abstract.ISize<double>.Width { get { return this.Width; } }
        double Abstract.ISize<double>.Height { get { return this.Height; } }
        #endregion
        #region IVector<double> Members
        double Abstract.IVector<double>.X { get { return this.Width; } }
        double Abstract.IVector<double>.Y { get { return this.Height; } }
        #endregion
        public double Area { get { return this.Width * this.Height; } }
        public bool IsEmpty { get { return this.Width == 0 || this.Height == 0; } }
        public double Norm { get { return Kean.Math.Double.SquareRoot(Kean.Math.Double.Squared(this.Width) + Kean.Math.Double.Squared(this.Height)); } }
        public double Azimuth { get { return Kean.Math.Double.ArcusTangensExtended(this.Height, this.Width); } }
        #region Static Constants
        public static SizeValue BasisX { get { return new SizeValue(1,0); } }
        public static SizeValue BasisY { get { return new SizeValue(0,1); } }
        #endregion
        #region Constructors
        public SizeValue(double width, double height)
		{
			this.Width = width;
			this.Height = height;
        }
        #endregion
        #region Methods
        #endregion
        #region Static Operators
        public static SizeValue operator -(SizeValue left, SizeValue right)
        {
            return new SizeValue(left.Width - right.Width, left.Height - right.Height);
        }
        public static SizeValue operator +(SizeValue left, SizeValue right)
        {
            return new SizeValue(left.Width + right.Width, left.Height + right.Height);
        }
        public static SizeValue Maximum(SizeValue left, SizeValue right)
        {
            return new SizeValue(Kean.Math.Double.Maximum(left.Width, right.Width), Kean.Math.Double.Maximum(left.Height, right.Height));
        }
        public static SizeValue Minimum(SizeValue left, SizeValue right)
        {
            return new SizeValue(Kean.Math.Double.Minimum(left.Width, right.Width), Kean.Math.Double.Minimum(left.Height, right.Height));
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
        public static bool operator <(SizeValue left, SizeValue right)
        {
            return left.Width < right.Width && left.Height < right.Height;
        }
        public static bool operator >(SizeValue left, SizeValue right)
        {
            return left.Width > right.Width && left.Height > right.Height;
        }
        public static bool operator <=(SizeValue left, SizeValue right)
        {
            return left.Width <= right.Width && left.Height <= right.Height;
        }
        public static bool operator >=(SizeValue left, SizeValue right)
        {
            return left.Width >= right.Width && left.Height >= right.Height;
        }
        #endregion
        #region Casts
        public static implicit operator SizeValue(Single.SizeValue value)
        {
            return new SizeValue(value.Width, value.Height);
        }
        public static implicit operator SizeValue(Integer.SizeValue value)
        {
            return new SizeValue(value.Width, value.Height);
        }
        public static explicit operator Single.SizeValue(SizeValue value)
        {
            return new Single.SizeValue((Kean.Math.Single)(value.Width), (Kean.Math.Single)(value.Height));
        }
        public static explicit operator Integer.SizeValue(SizeValue value)
        {
            return new Integer.SizeValue((Kean.Math.Integer)(value.Width), (Kean.Math.Integer)(value.Height));
        }
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
                        result = new SizeValue(Kean.Math.Double.Parse(values[0]), Kean.Math.Double.Parse(values[1]));
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
            return 33 * this.Width.GetHashCode() ^ this.Height.GetHashCode();
        }
        public override string ToString()
        {
            return Kean.Math.Double.ToString(this.Width) + ", " + Kean.Math.Double.ToString(this.Height);
        }
        #endregion
    }
}

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
using Kean.Core.Extension;

namespace Kean.Math.Geometry2D.Double
{
    public struct BoxValue :
        Abstract.IBox<PointValue, SizeValue, double>
    {
        public PointValue LeftTop;
        public SizeValue Size;
        #region IBox<PointValue,SizeValue,double> Members
        PointValue Kean.Math.Geometry2D.Abstract.IBox<PointValue, SizeValue, double>.LeftTop {get { return this.LeftTop; }}
        SizeValue Kean.Math.Geometry2D.Abstract.IBox<PointValue, SizeValue, double>.Size { get { return this.Size; } }
        #endregion
        public double Width { get { return this.Size.Width; } }
        public double Height { get { return this.Size.Height; } }

        public double Left { get { return this.LeftTop.X; } }
        public double Top { get { return this.LeftTop.Y; } }
        public double Right { get { return this.LeftTop.X + this.Size.Width; } }
        public double Bottom { get { return this.LeftTop.Y + this.Size.Height; } }
        public BoxValue(double left, double top, double width, double height)
        {
            this.LeftTop = new PointValue(left, top);
            this.Size = new SizeValue(width, height);
        }
        public BoxValue(PointValue leftTop, SizeValue size)
        {
            this.LeftTop = leftTop;
            this.Size = size;
        }
        public bool Contains(Double.PointValue point)
        {
            return this.Left <= point.X && point.X < this.Right && this.Top <= point.Y && point.Y < this.Bottom;
        }
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>True if <paramref name="Left"/> equals <paramref name="Right"/> else false.</returns>
        public static bool operator ==(BoxValue left, BoxValue right)
        {
            return left.Left == right.Left && left.Top == right.Top && left.Width == right.Width && left.Height == right.Height;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>False if <paramref name="Left"/> equals <paramref name="Right"/> else true.</returns>
        public static bool operator !=(BoxValue left, BoxValue right)
        {
            return !(left == right);
        }
        #endregion
        #region Static Operators
        public static BoxValue operator -(BoxValue left, ShellValue right)
        {
            return new BoxValue(left.LeftTop + right.LeftTop, left.Size - right.Size);
        }
        public static BoxValue operator +(BoxValue left, ShellValue right)
        {
            return new BoxValue(left.LeftTop - right.LeftTop, left.Size + right.Size);
        }
        #endregion
        #region Casts
        public static implicit operator BoxValue(Single.BoxValue value)
        {
            return new BoxValue(value.LeftTop, value.Size);
        }
        public static implicit operator BoxValue(Integer.BoxValue value)
        {
            return new BoxValue(value.LeftTop, value.Size);
        }
        public static explicit operator Single.BoxValue(BoxValue value)
        {
            return new Single.BoxValue((Single.PointValue)(value.LeftTop), (Single.SizeValue)(value.Size));
        }
        public static explicit operator Integer.BoxValue(BoxValue value)
        {
            return new Integer.BoxValue((Integer.PointValue)(value.LeftTop), (Integer.SizeValue)(value.Size));
        }
        public static implicit operator string(BoxValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator BoxValue(string value)
        {
            BoxValue result = new BoxValue();
            if (value.NotEmpty())
            {

                try
                {
					result = (BoxValue)(Box)value;
                }
                catch
                {
                }
            }
            return result;
        }
        #endregion
        #region Object Overrides
        public override string ToString()
        {
            return this.ToString(true);
        }
		public string ToString(bool commaSeparated)
		{
			return ((Box)this).ToString(commaSeparated);
		}
		public override int GetHashCode()
        {
            return this.LeftTop.GetHashCode() ^ this.Size.GetHashCode();
        }
        #endregion
    }
}

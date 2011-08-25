// 
//  ShellValue.cs
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
    public struct ShellValue :
        Abstract.IShell<float>
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;
        #region IShell<float> Members
        float Kean.Math.Geometry2D.Abstract.IShell<float>.Left { get { return this.Left; } }
        float Kean.Math.Geometry2D.Abstract.IShell<float>.Right { get { return this.Right; } }
        float Kean.Math.Geometry2D.Abstract.IShell<float>.Top { get { return this.Top; } }
        float Kean.Math.Geometry2D.Abstract.IShell<float>.Bottom { get { return this.Bottom; } }
        #endregion
        public PointValue LeftTop { get { return new PointValue(this.Left, this.Top); } }
        public SizeValue Size { get { return new SizeValue(this.Left + this.Right, this.Top + this.Bottom); } }
        public PointValue Balance { get { return new PointValue(this.Right - this.Left, this.Bottom - this.Top); } }
        public ShellValue(float left, float right, float top, float bottom)
        {
            this.Left = left;
            this.Right = right;
            this.Top = top;
            this.Bottom = bottom;
        }
        #region Static Operators
        public static SizeValue operator -(SizeValue left, ShellValue right)
        {
            return new SizeValue(left.Width - right.Left - right.Right, left.Height - right.Top - right.Bottom);
        }
        public static SizeValue operator +(SizeValue left, ShellValue right)
        {
            return new SizeValue(left.Width + right.Left + right.Right, left.Height + right.Top + right.Bottom);
        }
        public static ShellValue operator +(ShellValue left, ShellValue right)
        {
            return new ShellValue(left.Left + right.Left, left.Right + right.Right, left.Top + right.Top, left.Bottom + right.Bottom);
        }
        public static ShellValue operator -(ShellValue left, ShellValue right)
        {
            return new ShellValue(left.Left - right.Left, left.Right - right.Right, left.Top - right.Top, left.Bottom - right.Bottom);
        }
        public static ShellValue Maximum(ShellValue left, ShellValue right)
        {
            return new ShellValue(Kean.Math.Single.Maximum(left.Left, right.Left), Kean.Math.Single.Maximum(left.Right, right.Right), Kean.Math.Single.Maximum(left.Top, right.Top), Kean.Math.Single.Maximum(left.Bottom, right.Bottom));
        }
        public static ShellValue Minimum(ShellValue left, ShellValue right)
        {
            return new ShellValue(Kean.Math.Single.Minimum(left.Left, right.Left), Kean.Math.Single.Minimum(left.Right, right.Right), Kean.Math.Single.Minimum(left.Top, right.Top), Kean.Math.Single.Minimum(left.Bottom, right.Bottom));
        }
        #endregion
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>True if <paramref name="Left"/> equals <paramref name="Right"/> else false.</returns>
        public static bool operator ==(ShellValue left, ShellValue right)
        {
            return left.Left == right.Left && left.Right == right.Right && left.Top == right.Top && left.Bottom == right.Bottom;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>False if <paramref name="Left"/> equals <paramref name="Right"/> else true.</returns>
        public static bool operator !=(ShellValue left, ShellValue right)
        {
            return !(left == right);
        }
        #endregion
        #region Casts
        public static implicit operator ShellValue(Integer.ShellValue value)
        {
            return new ShellValue(value.Left, value.Right, value.Top, value.Bottom);
        }
        public static explicit operator Integer.ShellValue(ShellValue value)
        {
            return new Integer.ShellValue((Kean.Math.Integer)(value.Left), (Kean.Math.Integer)(value.Right), (Kean.Math.Integer)(value.Top), (Kean.Math.Integer)(value.Bottom));
        }
        public static implicit operator string(ShellValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator ShellValue(string value)
        {
            ShellValue result = new ShellValue();
            if (value.NotEmpty())
            {
                try
                {
                    string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 2)
                        result = new ShellValue(Kean.Math.Single.Parse(values[0]), Kean.Math.Single.Parse(values[1]), Kean.Math.Single.Parse(values[2]), Kean.Math.Single.Parse(values[3]));
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
            return this.Left.GetHashCode() ^ this.Right.GetHashCode() ^ this.Top.GetHashCode() ^ this.Bottom.GetHashCode();
        }
        public override string ToString()
        {
            return Kean.Math.Single.ToString(this.Left) + " " + Kean.Math.Single.ToString(this.Right) + " " + Kean.Math.Single.ToString(this.Top) + " " + Kean.Math.Single.ToString(this.Bottom);
        }
        #endregion
    }
}

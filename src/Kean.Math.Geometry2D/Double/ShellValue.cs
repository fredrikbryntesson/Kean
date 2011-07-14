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
using Kean.Core.Basis.Extension;

namespace Kean.Math.Geometry2D.Double
{
    public struct ShellValue :
        Abstract.IShell<double>
    {
        public double Left;
        public double Right;
        public double Top;
        public double Bottom;
        #region IShell<double> Members
        double Kean.Math.Geometry2D.Abstract.IShell<double>.Left { get { return this.Left; } }
        double Kean.Math.Geometry2D.Abstract.IShell<double>.Right { get { return this.Right; } }
        double Kean.Math.Geometry2D.Abstract.IShell<double>.Top { get { return this.Top; } }
        double Kean.Math.Geometry2D.Abstract.IShell<double>.Bottom { get { return this.Bottom; } }
        #endregion
        public ShellValue(double left, double right, double top, double bottom)
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
            return new ShellValue(Kean.Math.Double.Maximum(left.Left, right.Left), Kean.Math.Double.Maximum(left.Right, right.Right), Kean.Math.Double.Maximum(left.Top, right.Top), Kean.Math.Double.Maximum(left.Bottom, right.Bottom));
        }
        public static ShellValue Minimum(ShellValue left, ShellValue right)
        {
            return new ShellValue(Kean.Math.Double.Minimum(left.Left, right.Left), Kean.Math.Double.Minimum(left.Right, right.Right), Kean.Math.Double.Minimum(left.Top, right.Top), Kean.Math.Double.Minimum(left.Bottom, right.Bottom));
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
        public static implicit operator ShellValue(Single.ShellValue value)
        {
            return new ShellValue(value.Left, value.Right, value.Top, value.Bottom);
        }
        public static implicit operator ShellValue(Integer.ShellValue value)
        {
            return new ShellValue(value.Left, value.Right, value.Top, value.Bottom);
        }
        public static explicit operator Single.ShellValue(ShellValue value)
        {
            return new Single.ShellValue((Kean.Math.Single)(value.Left), (Kean.Math.Single)(value.Right), (Kean.Math.Single)(value.Top), (Kean.Math.Single)(value.Bottom));
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
                    if (values.Length == 4)
                        result = new ShellValue(Kean.Math.Double.Parse(values[0]), Kean.Math.Double.Parse(values[1]), Kean.Math.Double.Parse(values[2]), Kean.Math.Double.Parse(values[3]));
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
            return Kean.Math.Double.ToString(this.Left) + " " + Kean.Math.Double.ToString(this.Right) + " " + Kean.Math.Double.ToString(this.Top) + " " + Kean.Math.Double.ToString(this.Bottom);
        }
        #endregion
    }
}

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

namespace Kean.Math.Geometry2D.Integer
{
    public struct ShellValue :
        Abstract.IShell<int>
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
        #region IShell<int> Members
        int Kean.Math.Geometry2D.Abstract.IShell<int>.Left { get { return this.Left; } }
        int Kean.Math.Geometry2D.Abstract.IShell<int>.Right { get { return this.Right; } }
        int Kean.Math.Geometry2D.Abstract.IShell<int>.Top { get { return this.Top; } }
        int Kean.Math.Geometry2D.Abstract.IShell<int>.Bottom { get { return this.Bottom; } }
        #endregion
        public ShellValue(int left, int right, int top, int bottom)
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
            return new ShellValue(Kean.Math.Integer.Maximum(left.Left, right.Left), Kean.Math.Integer.Maximum(left.Right, right.Right), Kean.Math.Integer.Maximum(left.Top, right.Top), Kean.Math.Integer.Maximum(left.Bottom, right.Bottom));
        }
        public static ShellValue Minimum(ShellValue left, ShellValue right)
        {
            return new ShellValue(Kean.Math.Integer.Minimum(left.Left, right.Left), Kean.Math.Integer.Minimum(left.Right, right.Right), Kean.Math.Integer.Minimum(left.Top, right.Top), Kean.Math.Integer.Minimum(left.Bottom, right.Bottom));
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
                        result = new ShellValue(Kean.Math.Integer.Parse(values[0]), Kean.Math.Integer.Parse(values[1]), Kean.Math.Integer.Parse(values[2]), Kean.Math.Integer.Parse(values[3]));
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
            return Kean.Math.Integer.ToString(this.Left) + " " + Kean.Math.Integer.ToString(this.Right) + " " + Kean.Math.Integer.ToString(this.Top) + " " + Kean.Math.Integer.ToString(this.Bottom);
        }
        #endregion
    }
}

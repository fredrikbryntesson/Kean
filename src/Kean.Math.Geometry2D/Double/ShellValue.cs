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
        double left;
        double right;
        double top;
        double bottom;
        public double Left { get { return this.left; } }
        public double Right { get { return this.right; } }
        public double Top { get { return this.top; } }
        public double Bottom { get { return this.bottom; } }

        public ShellValue(double left, double right, double top, double bottom)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
        }
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
            ShellValue result = null;
            try
            {
                string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 4)
                    result = new ShellValue(Kean.Math.Double.Parse(values[0]), Kean.Math.Double.Parse(values[1]), Kean.Math.Double.Parse(values[2]), Kean.Math.Double.Parse(values[3]));
            }
            catch
            {
                result = null;
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

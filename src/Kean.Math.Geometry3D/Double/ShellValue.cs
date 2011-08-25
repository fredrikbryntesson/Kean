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

namespace Kean.Math.Geometry3D.Double
{
    public struct ShellValue :
        Abstract.IShell<double>
    {
        double left;
        double right;
        double top;
        double bottom;
        double front;
        double back;
        public double Left { get { return this.left; } }
        public double Right { get { return this.right; } }
        public double Top { get { return this.top; } }
        public double Bottom { get { return this.bottom; } }
        public double Front { get { return this.front; } }
        public double Back { get { return this.back; } }

        public ShellValue(double left, double right, double top, double bottom, double front, double back)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
            this.front = front;
            this.back = back;
        }
        #region Casts
        public static implicit operator ShellValue(Single.ShellValue value)
        {
            return new ShellValue(value.Left, value.Right, value.Top, value.Bottom, value.Front, value.Back);
        }
        public static implicit operator ShellValue(Integer.ShellValue value)
        {
            return new ShellValue(value.Left, value.Right, value.Top, value.Bottom, value.Front, value.Back);
        }
        public static explicit operator Single.ShellValue(ShellValue value)
        {
            return new Single.ShellValue((Kean.Math.Single)(value.Left), (Kean.Math.Single)(value.Right), (Kean.Math.Single)(value.Top), (Kean.Math.Single)(value.Bottom), (Kean.Math.Single)(value.Front), (Kean.Math.Single)(value.Back));
        }
        public static explicit operator Integer.ShellValue(ShellValue value)
        {
            return new Integer.ShellValue((Kean.Math.Integer)(value.Left), (Kean.Math.Integer)(value.Right), (Kean.Math.Integer)(value.Top), (Kean.Math.Integer)(value.Bottom), (Kean.Math.Integer)(value.Front), (Kean.Math.Integer)(value.Back));
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
                    if (values.Length == 6)
                        result = new ShellValue(Kean.Math.Double.Parse(values[0]), Kean.Math.Double.Parse(values[1]), Kean.Math.Double.Parse(values[2]), Kean.Math.Double.Parse(values[3]), Kean.Math.Double.Parse(values[4]), Kean.Math.Double.Parse(values[5]));
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
            return this.Left.GetHashCode() ^ this.Right.GetHashCode() ^ this.Top.GetHashCode() ^ this.Bottom.GetHashCode() ^ this.Front.GetHashCode() ^ this.Back.GetHashCode();
        }
        public override string ToString()
        {
            return Kean.Math.Double.ToString(this.Left) + " " + Kean.Math.Double.ToString(this.Right) + " " + Kean.Math.Double.ToString(this.Top) + " " + Kean.Math.Double.ToString(this.Bottom) + " " + Kean.Math.Double.ToString(this.Front) + " " + Kean.Math.Double.ToString(this.Back);
        }
        #endregion
    }
}

// 
//  Shell.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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

namespace Kean.Math.Geometry3D.Double
{
    public class Shell : Abstract.Shell<Transform, TransformValue, Shell, ShellValue, Box, BoxValue, Point, PointValue, Size, SizeValue, Kean.Math.Double, double>
    {
        public override ShellValue Value { get { return (ShellValue)this; } }
        public Shell() { }
        public Shell(double left, double right, double top, double bottom, double front, double back) : base(left, right, top, bottom, front, back) { }
        public Box Decrease(Size size)
          {
              return new Box(this.Left, this.Top, this.Front, size.Width - this.Left - this.Right, size.Height - this.Top - this.Bottom, size.Depth - this.Front - this.Back);
          }
          public Box Increase(Size size)
          {
              return new Box(-this.Left, -this.Right, -this.Front, size.Width + this.Left + this.Right, size.Height + this.Top + this.Bottom, size.Depth + this.Front + this.Back);
          }
        #region Casts
        public static explicit operator ShellValue(Shell value)
        {
            return new ShellValue(value.Left, value.Right, value.Top, value.Bottom, value.Front, value.Back);
        }
        public static implicit operator string(Shell value)
        {
            return value.ToString();
        }
        public static implicit operator Shell(string value)
        {
            Shell result = null;
            try
            {
                string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 4)
                    result = new Shell(Kean.Math.Double.Parse(values[0]), Kean.Math.Double.Parse(values[1]), Kean.Math.Double.Parse(values[2]), Kean.Math.Double.Parse(values[3]), Kean.Math.Double.Parse(values[4]), Kean.Math.Double.Parse(values[5]));
            }
            catch
            {
                result = null;
            }
            return result;
        }
        #endregion
    }
}

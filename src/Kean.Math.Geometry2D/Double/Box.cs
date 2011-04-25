// 
//  Point.cs
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

namespace Kean.Math.Geometry2D.Double
{
    public class Box : Abstract.Box<Transform, TransformValue, Box, BoxValue, Point, PointValue, Size, SizeValue, Kean.Math.Double, double>
    {
        public override BoxValue Value { get { return (BoxValue)this; } }
        public Box() { }
        public Box(double left, double top, double width, double height) : this(new Point(left, top), new Size(width, height)) { }
        public Box(PointValue leftTop, SizeValue size) : base((Point)leftTop, (Size)size) { }
        public Box(Point leftTop, Size size) : base(leftTop, size) { }
        public override Box Pad(double left, double right, double top, double bottom)
        {
            return new Box(new PointValue(this.Left - left, this.Top - top), new SizeValue(this.Size.Width + left + right, this.Size.Height + top + bottom));
        }
        public override Box Intersection(Box other)
        {
            double left = this.Left > other.Left ? this.Left : other.Left;
            double top = this.Top > other.Top ? this.Top : other.Top;
            double width = Kean.Math.Double.Maximum((this.Right < other.Right ? this.Right : other.Right) - left, 0);
            double height = Kean.Math.Double.Maximum((this.Bottom < other.Bottom ? this.Bottom : other.Bottom) - top, 0);
            return new Box(left, top, width, height);
        }
        #region Casts
        public static implicit operator Box(Kean.Math.Geometry2D.Single.Box value)
        {
            return new Box(value.LeftTop, value.Size);
        }
        public static implicit operator Box(Kean.Math.Geometry2D.Integer.Box value)
        {
            return new Box(value.LeftTop, value.Size);
        }
        public static explicit operator Kean.Math.Geometry2D.Single.Box(Box value)
        {
            return new Kean.Math.Geometry2D.Single.Box((Kean.Math.Geometry2D.Single.Point)(value.LeftTop), (Kean.Math.Geometry2D.Single.Size)(value.Size));
        }
        public static implicit operator Kean.Math.Geometry2D.Integer.Box(Box value)
        {
            return new Kean.Math.Geometry2D.Integer.Box((Kean.Math.Geometry2D.Integer.Point)(value.LeftTop), (Kean.Math.Geometry2D.Integer.Size)(value.Size));
        }
        public static implicit operator string(Box value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator Box(string value)
        {
            Box result = null;
            try
            {
                string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 4)
                    result = new Box((Point)(values[0] + " " + value[1]), (Size)(values[2] + " " + value[3]));
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

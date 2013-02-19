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
using Kean.Core.Extension;

namespace Kean.Math.Geometry3D.Integer
{
    public class Box : Abstract.Box<Transform, TransformValue, Box, BoxValue, Point, PointValue, Size, SizeValue, Kean.Math.Integer, int>
    {
        public override BoxValue Value { get { return (BoxValue)this; } }
        public Box() { }
        public Box(int left, int top, int front, int width, int height, int depth) : this(new Point(left, top, front), new Size(width, height, depth)) { }
        public Box(PointValue leftTop, SizeValue size) : base((Point)leftTop, (Size)size) { }
        public Box(Point leftTop, Size size) : base(leftTop, size) { }
        public override Box Pad(int left, int right, int top, int bottom, int front, int back)
        {
            return new Box(new PointValue(this.Left - left, this.Top - top, this.Front - front), new SizeValue(this.Size.Width + left + right, this.Size.Height + top + bottom, this.Size.Depth + front + back));
        }
        public override Box Intersection(Box other)
        {
            int left = this.Left > other.Left ? this.Left : other.Left;
            int top = this.Top > other.Top ? this.Top : other.Top;
            int front = this.Front > other.Front ? this.Front: other.Front;
            int width = Kean.Math.Integer.Maximum((this.Right < other.Right ? this.Right : other.Right) - left, 0);
            int height = Kean.Math.Integer.Maximum((this.Bottom < other.Bottom ? this.Bottom : other.Bottom) - top, 0);
            int depth = Kean.Math.Integer.Maximum((this.Back < other.Back ? this.Back : other.Back) - front, 0);
            return new Box(left, top, width, height, front, depth);
        }
        #region Casts
        public static explicit operator BoxValue(Box value)
        {
            return new BoxValue(value.LeftTopFront.Value, value.Size.Value);
        }
        public static implicit operator Box(BoxValue value)
        {
            return new Box(value.LeftTopFront, value.Size);
        }
        public static implicit operator string(Box value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator Box(string value)
        {
            Box result = null;
            if (value.NotEmpty())
            {

                try
                {
                    string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 6)
                        result = new Box((Point)(values[0] + " " + values[1] + " " + values[2]), (Size)(values[3] + " " + values[4] + " " + values[5]));
                }
                catch
                {
                    result = null;
                }
            }
            return result;
        }
        #endregion
    }
}

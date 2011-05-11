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

namespace Kean.Math.Geometry2D.Single
{
    public class Box : Abstract.Box<Transform, TransformValue, Box, BoxValue, Point, PointValue, Size, SizeValue, Kean.Math.Single, float>
    {
        public override BoxValue Value { get { return (BoxValue)this; } }
        public Box() { }
        public Box(float left, float top, float width, float height) : this(new Point(left, top), new Size(width, height)) { }
        public Box(PointValue leftTop, SizeValue size) : base((Point)leftTop, (Size)size) { }
        public Box(Point leftTop, Size size) : base(leftTop, size) { }
        public override Box Pad(float left, float right, float top, float bottom)
        {
            return new Box(new PointValue(this.Left - left, this.Top - top), new SizeValue(this.Size.Width + left + right, this.Size.Height + top + bottom));
        }
        public override Box Intersection(Box other)
        {
            float left = this.Left > other.Left ? this.Left : other.Left;
            float top = this.Top > other.Top ? this.Top : other.Top;
            float width = Kean.Math.Single.Maximum((this.Right < other.Right ? this.Right : other.Right) - left, 0);
            float height = Kean.Math.Single.Maximum((this.Bottom < other.Bottom ? this.Bottom : other.Bottom) - top, 0);
            return new Box(left, top, width, height);
        }
        #region Casts
        public static implicit operator Box(Integer.Box value)
        {
            return new Box(value.LeftTop, value.Size);
        }
        public static explicit operator Integer.Box(Box value)
        {
            return new Integer.Box((Integer.Point)(value.LeftTop), (Integer.Size)(value.Size));
        }
        public static explicit operator BoxValue(Box value)
        {
            return new BoxValue(value.LeftTop.Value, value.Size.Value);
        }
        public static implicit operator Box(BoxValue value)
        {
            return new Box(value.LeftTop, value.Size);
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
                    if (values.Length == 4)
                        result = new Box((Point)(values[0] + " " + value[1]), (Size)(values[2] + " " + value[3]));
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

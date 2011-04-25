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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
namespace Kean.Math.Geometry2D.Double
{
    public struct BoxValue :
        Abstract.IBox<PointValue, SizeValue, double>
    {
        PointValue leftTop;
        SizeValue size;
        public PointValue LeftTop
        {
            get { return this.leftTop; }
            set { this.leftTop = value; }
        }
        public SizeValue Size
        {
            get { return this.size; }
            set { this.size = value; }
        }
        public double Left { get { return this.leftTop.X; } }
        public double Top { get { return this.leftTop.Y; } }
        public double Right { get { return this.leftTop.X + this.size.Width; } }
        public double Bottom { get { return this.leftTop.Y + this.size.Height; } }
        public BoxValue(double left, double top, double width, double height)
        {
            this.leftTop = new PointValue(left, top);
            this.size = new SizeValue(width, height);
        }
        public BoxValue(PointValue leftTop, SizeValue size)
        {
            this.leftTop = leftTop;
            this.size = size;
        }
        #region Casts
        public static explicit operator BoxValue(Box value)
        {
            return new BoxValue(value.LeftTop.Value, value.Size.Value);
        }
        public static implicit operator Box(BoxValue value)
        {
            return new Box(value.LeftTop, value.Size);
        }
        #endregion
    }
}

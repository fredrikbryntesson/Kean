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
namespace Kean.Math.Geometry3D.Integer
{
    public struct BoxValue :
        Abstract.IBox<PointValue, SizeValue, int>
    {
        PointValue leftTopFront;
        SizeValue size;
        public PointValue LeftTopFront
        {
            get { return this.leftTopFront; }
            set { this.leftTopFront = value; }
        }
        public SizeValue Size
        {
            get { return this.size; }
            set { this.size = value; }
        }
        public BoxValue(int left, int top, int front, int width, int height, int depth)
        {
            this.leftTopFront = new PointValue(left, top, front);
            this.size = new SizeValue(width, height, depth);
        }
        public BoxValue(PointValue leftTopFront, SizeValue size)
        {
            this.leftTopFront = leftTopFront;
            this.size = size;
        }
        #region Casts
        public static implicit operator Box(BoxValue value)
        {
            return new Box(value.LeftTopFront, value.Size);
        }
        public static explicit operator BoxValue(Box value)
        {
            return new BoxValue(value.LeftTopFront.Value, value.Size.Value);
        }
        #endregion
    }
}

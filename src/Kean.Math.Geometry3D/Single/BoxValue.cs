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
namespace Kean.Math.Geometry3D.Single
{
    public struct BoxValue :
        Abstract.IBox<PointValue, SizeValue, float>
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
        public BoxValue(float left, float top, float front, float width, float height, float depth)
        {
            this.leftTop = new PointValue(left, top, front);
            this.size = new SizeValue(width, height, depth);
        }
        public BoxValue(PointValue leftTop, SizeValue size)
        {
            this.leftTop = leftTop;
            this.size = size;
        }
    }
}

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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
namespace Kean.Math.Geometry3D.Single
{
    public struct ShellValue :
        Abstract.IShell<float>
    {
        float left;
        float right;
        float top;
        float bottom;
        float front;
        float back;
        public float Left { get { return this.left; } }
        public float Right { get { return this.right; } }
        public float Top { get { return this.top; } }
        public float Bottom { get { return this.bottom; } }
        public float Front { get { return this.front; } }
        public float Back { get { return this.back; } }

        public ShellValue(float left, float right, float top, float bottom, float front, float back)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
            this.front = front;
            this.back = back;
        }
    }
}

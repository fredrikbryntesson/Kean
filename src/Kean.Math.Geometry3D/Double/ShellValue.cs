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
    }
}

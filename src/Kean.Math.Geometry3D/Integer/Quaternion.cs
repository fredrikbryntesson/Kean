// 
//  Quaternion.cs
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

namespace Kean.Math.Geometry3D.Integer
{
    public class Quaternion :
        Abstract.Quaternion<Quaternion, Point, Kean.Math.Integer, int>
    {
        public Quaternion() { }
        public Quaternion(Kean.Math.Integer x, Point y) :
            base(x, y) { }
        public Quaternion(Kean.Math.Integer x, Kean.Math.Integer y, Kean.Math.Integer z, Kean.Math.Integer w) :
            base(x, new Point(y, z, w)) { }
    }
}

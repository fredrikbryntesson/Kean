// 
//  Double.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;

namespace Kean.Math.Matrix
{
    public class Double :
        Abstract<Double, Kean.Math.Double, double>
    {
        public Double() { }
        public Double(Kean.Math.Integer order) :
            base(order) { }
        public Double(Kean.Math.Integer width, Kean.Math.Integer height) :
            base(width, height) { }
        public Double(Geometry2D.Integer.Size size) :
            base(size) { }
        public Double(Geometry2D.Integer.Size size, double[] elements) :
            base(size, elements) { }
        public Double(Kean.Math.Integer width, Kean.Math.Integer height, double[] elements) :
            base(new Geometry2D.Integer.Size(width, height), elements) { }
    }
}

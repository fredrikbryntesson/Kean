// 
//  Transform.cs
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

namespace Kean.Math.Geometry3D.Double
{
    public class Transform : 
        Abstract.Transform<Transform, TransformValue, Size, SizeValue, Kean.Math.Double, double>
    {
        public Transform() { }
        public Transform(Kean.Math.Double a, Kean.Math.Double b, Kean.Math.Double c, Kean.Math.Double d, Kean.Math.Double e, Kean.Math.Double f, Kean.Math.Double g, Kean.Math.Double h, Kean.Math.Double i, Kean.Math.Double j, Kean.Math.Double k, Kean.Math.Double l) : base(a, b, c, d, e, f, g, h, i, j, k, l) { }
        public override TransformValue Value { get { return (TransformValue)this; }}
        #region Casts
        public static explicit operator TransformValue(Transform value)
        {
            return new TransformValue(value.A, value.B, value.C, value.D, value.E, value.F, value.G, value.H, value.I, value.J, value.K, value.L);
        }
        #endregion
    }
}

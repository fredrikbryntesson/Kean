// 
//  InfinityPoint.cs
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

namespace Kean.Math.Geometry3D.Single
{
    public class InfinityPoint : Abstract.InfinityPoint<Transform, TransformValue, InfinityPoint, PointValue, Size, SizeValue, Kean.Math.Single, float>
    {
        public override PointValue Value { get { return (PointValue)this; } }
        public InfinityPoint() { }
        public InfinityPoint(Kean.Math.Single x, Kean.Math.Single y, Kean.Math.Single z) : base(x, y, z) { }
        #region Casts
        public static explicit operator PointValue(InfinityPoint value)
        {
            return new PointValue() { X = value.X, Y = value.Y, Z = value.Z };
        }
        #endregion
    }
}
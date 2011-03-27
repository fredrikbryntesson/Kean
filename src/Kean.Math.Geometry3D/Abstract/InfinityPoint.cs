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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
namespace Kean.Math.Geometry3D.Abstract
{
    public abstract class InfinityPoint<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue,R, V> :
		Vector<TransformType, TransformValue, PointType, PointValue,SizeType, SizeValue, R, V>,
		IPoint<V>
        where PointType : InfinityPoint<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointValue : struct, IPoint<V>, IVector<V>
        where TransformType : Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, ITransform<V>, new()
        where TransformValue : struct, ITransform<V>
        where SizeType : Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, IVector<V>, new()
        where SizeValue : struct, ISize<V>, IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
	{
        public new R X { get { return base.X; } }
		public new R Y { get { return base.Y; } }
        public new R Z { get { return base.Z; } }
        #region IPoint<V> Members
		V IPoint<V>.X { get { return this.X; } }
		V IPoint<V>.Y { get { return this.Y; } }
        V IPoint<V>.Z { get { return this.Z; } }
		#endregion
		#region Constructors
        protected InfinityPoint() { }
        protected InfinityPoint(R x, R y, R z) :
			base(x, y, z)
		{ }
        #endregion
        #region Arithmetic Operators
        public static PointType operator *(TransformType left, InfinityPoint<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return new PointType().Create(
                left.A * right.X + left.D * right.Y + left.G * right.Z, 
                left.B * right.X + left.E * right.Y + left.H * right.Z, 
                left.C * right.X + left.F * right.Y + left.I * right.Z);
        }
        #endregion
    }
}


// 
//  Size.cs
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
    public abstract class Size<TransformType, TransformValue, SizeType, SizeValue, R, V> :
        Vector<TransformType, TransformValue, SizeType, SizeValue, SizeType, SizeValue, R, V>,
		ISize<V>
        where TransformType : Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, ITransform<V>, new()
        where TransformValue : struct, ITransform<V>
        where SizeType : Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, IVector<V>, new()
        where SizeValue : struct, ISize<V>, IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
	{
		public V Width { get { return base.X; } }
		public V Height { get { return base.Y; } }
        public V Depth { get { return base.Z; } }
		#region ISize<V> Members
		V ISize<V>.Width { get { return this.Width; } }
		V ISize<V>.Height { get { return this.Height; } }
        V ISize<V>.Depth { get { return this.Depth; } }
		#endregion
		public V Volume { get { return base.X.Multiply(base.Y).Multiply(base.Z); } }
        protected Size() { }
	    protected Size(R width, R height, R depth) :
			base(width, height, depth)
		{ }
        #region Arithmetic Operators
        public static SizeType operator *(TransformType left, Size<TransformType, TransformValue, SizeType, SizeValue, R, V> right)
        {
            return Size<TransformType, TransformValue, SizeType, SizeValue, R, V>.Create(left.A * right.Width + left.D * right.Height + left.G * right.Depth, left.B * right.Width + left.E * right.Height + left.H * right.Depth, left.C * right.Width + left.F * right.Height + left.I * right.Depth);
        }
        #endregion
        public static SizeType Create(V width, V height, V depth)
        {
            return Vector<TransformType, TransformValue, SizeType, SizeValue, SizeType, SizeValue, R, V>.Create(width, height, depth);
        }
    }
}


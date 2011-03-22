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
	public abstract class Size<SizeType, R, V> :
		Vector<SizeType, R, V>,
		ISize<V>
        where SizeType : Size<SizeType, R, V>, new()
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
  	}
}


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
	public abstract class Point<PointType, R, V> :
		Vector<PointType, R, V>,
		IPoint<V>
        where PointType : Point<PointType, R, V>, new()
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
        protected Point() { }
        protected Point(R x, R y, R z) :
			base(x, y, z)
		{ }
        #endregion
    }
}


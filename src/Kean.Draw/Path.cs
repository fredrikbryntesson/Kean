// 
//  Path.cs
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
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw
{
	public class Path
	{
		public void MoveTo(Geometry2D.Single.Point end)
		{
		}
		public void LineTo(Geometry2D.Single.Point end)
		{
		}
		public void CurveTo(Geometry2D.Single.Point first, Geometry2D.Single.Point second, Geometry2D.Single.Point end)
		{
		}
		public void EllipticalArcTo(Geometry2D.Single.Point radius, float angle, bool largeArc, bool sweep, Geometry2D.Single.Point end)
		{
		}
 		public void Close()
		{
		}
	}
}

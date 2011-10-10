﻿// 
//  Close.cs
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

namespace Kean.Draw.PathSegment
{
	public class Close :
		LineTo
	{
		public override Geometry2D.Single.Point End
		{
			get { return this.SubpathStart; }
			set { this.SubpathStart = value; }
		}
		public Close() :
			base(new Geometry2D.Single.Point())
		{ }
		protected override Geometry2D.Single.Box SegmentBounds(Geometry2D.Single.Transform transform)
		{
			Geometry2D.Single.Point startPoint = transform * this.Start;
			Geometry2D.Single.Point endPoint = transform * this.End;
			Geometry2D.Single.Point leftTop = new Geometry2D.Single.Point(Kean.Math.Single.Minimum(startPoint.X, endPoint.X), Kean.Math.Single.Minimum(startPoint.Y, endPoint.Y));
			Geometry2D.Single.Size size = new Geometry2D.Single.Size(Kean.Math.Single.Absolute(startPoint.X - endPoint.X), Kean.Math.Single.Absolute(startPoint.Y - endPoint.Y));
			return new Geometry2D.Single.Box(leftTop, size);
		}
	}
}

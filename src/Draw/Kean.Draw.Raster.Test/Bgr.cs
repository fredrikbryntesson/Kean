﻿// 
//  Bgr.cs
//  
//  Author:
//      Simon Mika <smika@hx.se
//  
//  Copyright (c) 2012 Simon Mika
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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Raster.Test
{
	public class Bgr :
		Image<Bgr>
	{
		public Bgr() :
			base("Bgr", 0.01f)
		{ }
		protected override Draw.Image CreateImage(Geometry2D.Integer.Size size)
		{
			return new Raster.Bgr(size);
		}
	}
}

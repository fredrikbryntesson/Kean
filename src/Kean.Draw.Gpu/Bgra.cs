﻿// 
//  Bgra.cs
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
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Gpu
{
	public class Bgra :
		Packed
	{
		#region Constructors
		public Bgra(Raster.Bgra image) :
			base(Gpu.Backend.Factory.CreateImage(image))
		{ }
		public Bgra(Gpu.Image image) :
			this(image.Size, image.CoordinateSystem)
		{
			// TODO: color space conversion goes here (use Backend.IImage or Backend.IFactory)
		}
		public Bgra(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default)
		{ }
		public Bgra(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(Gpu.Backend.Factory.CreateImage(Gpu.Backend.ImageType.Bgra, size, coordinateSystem))
		{ }
		protected Bgra(Draw.Gpu.Backend.IImage image) : 
			base(image)
		{ }
		#endregion
		#region Image Overrides
		public override T Convert<T>()
		{
			T result = null;
			Raster.Image backend = this.Backend.Read();
			if (typeof(T) == typeof(Raster.Bgra))
				result = backend as T;
			else if (typeof(T) == typeof(Raster.Bgr) || typeof(T) == typeof(Raster.Monochrome))
				result = backend.Convert<T>() as T;
			return result;
		}
		public override Kean.Draw.Image ResizeTo(Geometry2D.Integer.Size size)
		{
            Bgra result = new Bgra(size);
            result.Canvas.Draw(this, new Geometry2D.Single.Box(0, 0, this.Size.Width, this.Size.Height), new Geometry2D.Single.Box(0, 0, size.Width, size.Height));
            return result.Canvas.Image;
		}
		public override Draw.Image Copy()
		{
			return new Bgra(this.Backend.Copy());
		}
		#endregion
	}
}

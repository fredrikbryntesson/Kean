// 
//  Monochrome.cs
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
	public class Monochrome :
		Packed
	{
		#region Constructors
		public Monochrome(Raster.Monochrome image) :
			base(Gpu.Backend.Factory.CreateImage(image))
		{ }
		public Monochrome(Gpu.Image image) :
			this(image.Size, image.CoordinateSystem)
		{
			// TODO: color space conversion goes here (use Backend.IImage or Backend.IFactory)
		}		
		public Monochrome(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default)
		{ }
		public Monochrome(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(Gpu.Backend.Factory.CreateImage(Gpu.Backend.ImageType.Monochrome, size, coordinateSystem))
		{ }
		protected internal Monochrome(Draw.Gpu.Backend.IImage image) : 
			base(image)
		{ }
		#endregion
		#region Image Overrides
		public override T Convert<T>()
		{
			T result = null;
			if (typeof(T) == typeof(Raster.Monochrome))
				result = (this.Canvas as Canvas).Read() as T;
			else if (typeof(T) == typeof(Raster.Bgra))
				result = (this.Canvas as Canvas).Read().Convert<Raster.Bgra>() as T;
			else if (typeof(T) == typeof(Raster.Bgr))
				result = (this.Canvas as Canvas).Read().Convert<Raster.Bgr>() as T;
			return result;
		}
		// TODO:  Resize Monochrome using GPU
		public override Kean.Draw.Image ResizeTo(Kean.Math.Geometry2D.Integer.Size size)
		{
            Monochrome result = new Monochrome(size);
            result.Canvas.Draw(this, new Geometry2D.Single.Box(0, 0, this.Size.Width, this.Size.Height), new Geometry2D.Single.Box(0, 0, size.Width, size.Height));
            return result.Canvas.Image;
        }
		public override Draw.Image Copy()
		{
			return new Monochrome(this.Backend.Copy());
		}		
		#endregion
	}
}

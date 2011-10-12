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
using Kean.Core.Reflect.Extension;
namespace Kean.Draw.Gpu
{
	public class Monochrome :
		Packed
	{
		#region Constructors
		public Monochrome(Raster.Monochrome image) :
			base(Gpu.Backend.Factory.CreateImage(image))
		{ }
		public Monochrome(Gpu.Yuv420 image) :
			this(image.Y.Backend)
		{ }
		public Monochrome(Gpu.Bgr image) :
			this(image.Size, image.CoordinateSystem)
		{
			this.Canvas.Draw(Map.BgrToMonochrome, image);
		}
		public Monochrome(Gpu.Bgra image) :
			this(image.Size, image.CoordinateSystem)
		{
			this.Canvas.Draw(Map.BgrToMonochrome, image);
		}
		public Monochrome(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default)
		{ }
		public Monochrome(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(Gpu.Backend.Factory.CreateImage(Gpu.Backend.TextureType.Monochrome, size, coordinateSystem))
		{ }
		protected internal Monochrome(Draw.Gpu.Backend.ITexture image) : 
			base(image)
		{ }
		#endregion
		#region Image Overrides
		public override T Convert<T>()
		{
			T result = null;
			if (typeof(T).IsSubclassOf(typeof(Raster.Image)))
				result = this.Backend.Read().Convert<T>() as T; 
			else
			{
				if (typeof(T) == typeof(Gpu.Monochrome))
					result = this.Copy() as T;
				else if (typeof(T) == typeof(Gpu.Bgr))
					result = new Bgr(this) as T;
				else if (typeof(T) == typeof(Gpu.Bgra))
					result = new Bgra(this) as T;
				else if (typeof(T) == typeof(Gpu.Yuv420))
					result = new Yuv420(this) as T;
			}
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
			return new Monochrome(this.Convert<Raster.Monochrome>());
		}		
		#endregion
	}
}

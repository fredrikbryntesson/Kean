// 
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
using Kean.Core.Reflect.Extension;
using Reflect = Kean.Core.Reflect;
using Kean.Core.Extension;

namespace Kean.Draw.Gpu
{
	public class Bgra :
		Packed
	{
		#region Constructors
		public Bgra(Raster.Bgra image) :
			base(Gpu.Backend.Factory.CreateImage(image))
		{ }
		public Bgra(Gpu.Yuv420 image) :
			this(image.Size, image.CoordinateSystem)
		{
			this.Canvas.Draw(Map.Yuv420ToBgr, image);
		}
		public Bgra(Gpu.Monochrome image) :
			this(image.Size, image.CoordinateSystem)
		{
            this.Canvas.Draw(Map.MonochromeToBgr, image);
		}
		public Bgra(Gpu.Bgr image) :
			this(image.Size, image.CoordinateSystem)
		{
			this.Canvas.Draw(image);
		}
		public Bgra(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default)
		{ }
		public Bgra(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(Gpu.Backend.Factory.CreateImage(Gpu.Backend.TextureType.Bgra, size, coordinateSystem))
		{ }
		protected Bgra(Draw.Gpu.Backend.ITexture image) : 
			base(image)
		{ }
		#endregion
		#region Image Overrides
		public override T Convert<T>()
		{
			T result = null;
			Reflect.Type type = typeof(T);
			if (type.Inherits<Raster.Image>())
			{
				Raster.Image backend = this.Backend.Read();
				if (backend.Type().Inherits<T>())
					result = backend as T;
				else if (type == typeof(Raster.Bgr) || type == typeof(Raster.Monochrome))
					result = backend.Convert<T>() as T;
			}
			else
			{
				if (type == typeof(Gpu.Bgra))
					result = this.Copy() as T;
				else if (type == typeof(Gpu.Bgr))
					result = new Bgr(this) as T;
				else if (type == typeof(Gpu.Monochrome))
					result = new Monochrome(this) as T;
				else if (type == typeof(Gpu.Yuv420))
					result = new Yuv420(this) as T;
			}
			return result;
		}
		public override Draw.Image ResizeTo(Geometry2D.Integer.Size size)
		{
            Bgra result = new Bgra(size);
            result.Canvas.Draw(this, new Geometry2D.Single.Box(0, 0, this.Size.Width, this.Size.Height), new Geometry2D.Single.Box(0, 0, size.Width, size.Height));
            return result.Canvas.Image;
		}
		public override Draw.Image Copy()
		{
			return new Bgra(this.Convert<Raster.Bgra>());
		}
		#endregion
	}
}

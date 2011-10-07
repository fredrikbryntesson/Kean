// 
//  Yuv420.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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

namespace Kean.Draw.Gpu
{
	public class Yuv420 : 
		Planar
	{
		public Monochrome Y { get; private set; }
		public Monochrome U { get; private set; }
		public Monochrome V { get; private set; }

		public Yuv420(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default)
		{ }
		public Yuv420(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			this(
			new Monochrome(Gpu.Backend.Factory.CreateImage(Gpu.Backend.ImageType.Monochrome, size, coordinateSystem)),
			new Monochrome(Gpu.Backend.Factory.CreateImage(Gpu.Backend.ImageType.Monochrome, size / 2, coordinateSystem)),
			new Monochrome(Gpu.Backend.Factory.CreateImage(Gpu.Backend.ImageType.Monochrome, size / 2, coordinateSystem)),
			size, coordinateSystem)
		{ }
		public Yuv420(Raster.Yuv420 image) :
			this(
			new Monochrome(image.Y),
			new Monochrome(image.U),
			new Monochrome(image.V),
			image.Size, image.CoordinateSystem)
		{ }
		public Yuv420(Gpu.Image image) :
			this(image.Size, image.CoordinateSystem)
		{
			// TODO: color space conversion goes here (use Backend.IImage or Backend.IFactory)
		}
		Yuv420(Monochrome y, Monochrome u, Monochrome v, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(size, coordinateSystem)
		{
			this.Y = y;
			this.U = u;
			this.V = v;
		}
		protected Yuv420(Yuv420 original) :
			base(original)
		{
			this.Y = original.Y.Copy() as Monochrome;
			this.U = original.U.Copy() as Monochrome;
			this.V = original.V.Copy() as Monochrome;
		}
		#region Draw.Image Overrides
		public override Draw.Canvas Canvas
		{
			get { return null; }
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return null;
		}
		public override Draw.Image Copy()
		{
			return new Yuv420(this);
		}
		public override Draw.Image Copy(Geometry2D.Integer.Size size, Geometry2D.Single.Transform transform)
		{
			return null;
		}
		public override Draw.Image ResizeTo(Geometry2D.Integer.Size size)
		{
			return null;
		}
		public override void Shift(Geometry2D.Integer.Size offset)
		{
		}
		public override float Distance(Draw.Image other)
		{
			return float.NaN;
		}
		public override void Dispose()
		{
			if (this.Y.NotNull())
			{
				this.Y.Dispose();
				this.Y = null;
			}
			if (this.U.NotNull())
			{
				this.U.Dispose();
				this.U = null;
			}
			if (this.V.NotNull())
			{
				this.V.Dispose();
				this.V = null;
			}
		}
		#endregion
	}
}

// 
//  Canvas.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
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
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.OpenGL
{
	public class Canvas :
		Draw.Canvas
	{
		protected internal Backend.Composition Backend { get; private set; }

		internal Canvas(Packed image) :
			base(image)
		{
			this.Backend = image.Backend.Composition;
		}
		internal Canvas(Planar image, params Packed[] channels) :
			base(image)
		{
			this.Backend = channels[0].Backend.Composition;
		}

		public Raster.Image Read()
		{
			return this.Read(new Geometry2D.Integer.Box(new Geometry2D.Integer.Point(), this.Size));
		}
		public Raster.Image Read(Geometry2D.Integer.Box region)
		{
			Raster.Packed result;
			switch (this.Backend.Type)
			{
				case OpenGL.Backend.TextureType.Rgb:
					result = new Raster.Bgr(this.Size, this.Image.CoordinateSystem);
					break;
				case OpenGL.Backend.TextureType.Argb:
					result = new Raster.Bgra(this.Size, this.Image.CoordinateSystem);
					break;
				case OpenGL.Backend.TextureType.Monochrome:
					result = new Raster.Monochrome(this.Size, this.Image.CoordinateSystem);
					break;
				default:
					result = null;
					break;
			}
			if (result.NotNull())
				this.Backend.Read(result.Pointer, region);
			return result;
		}
		#region Draw.Canvas Overrides
		#region Clip, Transform, Push & Pop
		protected override Geometry2D.Single.Box OnClipChange(Geometry2D.Single.Box clip)
		{
			this.Backend.SetClip(clip);
			return clip;
		}
		protected override Geometry2D.Single.Transform OnTransformChange(Geometry2D.Single.Transform transform)
		{
			//this.Backend.Transform = transform;
			return transform;
		}
		#endregion
		#region Create
		public override Draw.Canvas CreateSubcanvas(Geometry2D.Single.Box bounds)
		{
			return null;
		}
		#endregion
		#region Draw, Blend, Clear
		#region Draw Image
		void Draw(Map map, Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			this.Backend.Setup();
			image.Render(source, destination);
			this.Backend.Teardown();
		}
		public override void Draw(Draw.Map map, Draw.Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			if (!(image is Image))
				using (image = OpenGL.Image.Create(image))
					this.Draw(map as Map, image as Image, source, destination);
			else
				this.Draw(map as Map, image as Image, source, destination);
		}
		#endregion
		#region Draw Box
		public override void Draw(IColor color)
		{
			this.Draw(color, new Geometry2D.Single.Box(this.Size));
		}
		public override void Draw(IColor color, Geometry2D.Single.Box region)
		{
			this.Backend.Draw(color.Convert<Color.Bgra>(), region);
		}
		#endregion
		#region Draw Path
		public override void Draw(IPaint fill, Stroke stroke, Path path)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Draw Text
		public override void Draw(IPaint fill, Stroke stroke, Text text, Geometry2D.Single.Point position)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Blend
		public override void Blend(float factor)
		{
			this.Backend.Blend(factor);
		}
		#endregion
		#region Clear
		public override void Clear()
		{
			this.Draw(new Color.Bgra());
		}
		public override void Clear(Geometry2D.Single.Box region)
		{
			this.Backend.Draw(new Color.Bgra(), region);
		}
		#endregion
		#endregion
		#endregion
		public override void Dispose()
		{
			if (this.Backend.NotNull())
			{
				this.Backend.Dispose();
				this.Backend = null;
			}
			base.Dispose();
		}
	}
}

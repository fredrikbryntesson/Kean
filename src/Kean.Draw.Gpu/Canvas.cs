﻿using System;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Gpu
{
	public class Canvas :
		Draw.Canvas
	{
		protected internal Backend.ICanvas Backend { get; private set; }

		internal Canvas(Packed image) :
			base(image)
		{
			this.Backend = image.Backend.Canvas;
		}

		public Raster.Image Read()
		{
			return this.Read(new Geometry2D.Integer.Box(new Geometry2D.Integer.Point(), this.Size));
		}
		public Raster.Image Read(Geometry2D.Integer.Box region)
		{
			return this.Backend.Read(region);
		}
		#region Draw.Canvas Overrides
		#region Clip, Transform, Push & Pop
		protected override Geometry2D.Single.Box OnClipChange(Geometry2D.Single.Box clip)
		{
			this.Backend.Clip = clip;
			return clip;
		}
		protected override Geometry2D.Single.Transform OnTransformChange(Geometry2D.Single.Transform transform)
		{
			this.Backend.Transform = transform;
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
		public override void Draw(Draw.Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			if (!(image is Gpu.Image))
				using (image = Gpu.Image.Create(image))
					this.Backend.Draw((image as Gpu.Packed).Backend, source, destination);
			else
				this.Backend.Draw((image as Gpu.Packed).Backend, source, destination);
		}
		#endregion
		#region Draw Rectangle
		public override void Draw(IColor color)
		{
			this.Backend.Draw(color);
		}
		public override void Draw(IColor color, Geometry2D.Single.Box region)
		{
            this.Backend.Draw(color, region);
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
	}
}

using System;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Gpu
{
	public class Canvas :
		Draw.Canvas
	{
		protected internal Backend.ICanvas Backend { get; private set; }

		internal Canvas(Image image) :
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
			return clip;
		}
		protected override Geometry2D.Single.Transform OnTransformChange(Geometry2D.Single.Transform transform)
		{
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
		}
		#endregion
		#region Draw Rectangle
		public override void Draw(IColor color)
		{
		}
		public override void Draw(IColor color, Geometry2D.Single.Box region)
		{
		}
		#endregion
		#region Blend
		public override void Blend(float factor)
		{
		}
		#endregion
		#region Clear
		public override void Clear()
		{
		}
		public override void Clear(Geometry2D.Single.Box area)
		{
		}
		#endregion
		#endregion
		#endregion
	}
}

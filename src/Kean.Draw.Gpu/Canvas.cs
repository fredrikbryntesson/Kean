using System;

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
		public override void Clear(Kean.Math.Geometry2D.Single.Box area)
		{
			throw new NotImplementedException();
		}
		public override Kean.Draw.Canvas Create(Kean.Math.Geometry2D.Single.Size size)
		{
			throw new NotImplementedException();
		}
		public override void Draw(Kean.Draw.Image image)
		{
			throw new NotImplementedException();
		}
		public override void Draw(Kean.Draw.Image image, Kean.Math.Geometry2D.Single.Box source, Kean.Math.Geometry2D.Single.Box destination)
		{
			throw new NotImplementedException();
		}
		public override Kean.Draw.Canvas Subcanvas(Kean.Math.Geometry2D.Single.Box bounds)
		{
			throw new NotImplementedException();
		}
		public override bool TextAntiAlias
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}

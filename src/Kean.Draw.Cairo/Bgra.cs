using System;
using Buffer = Kean.Core.Buffer;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Cairo
{
	public class Bgra :
		Raster
	{
		public Bgra(Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<Color.Bgra>(size.Area), size)
		{ }
		public Bgra(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			base(buffer, new global::Cairo.ImageSurface(buffer, global::Cairo.Format.Argb32, size.Width, size.Height, size.Width), size)
		{ }
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Bgra(size);
		}
		public override float Distance(Draw.Image other)
		{
			Bgra o = other.Convert<Bgra>();
			return Buffer.Distance(o.Buffer);
		}
	}
}

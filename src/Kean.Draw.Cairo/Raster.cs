using System;
using Buffer = Kean.Core.Buffer;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Cairo
{
	public abstract class Raster :
		Image
	{
		public Buffer.Sized Buffer { get; private set; }
		protected Raster(Buffer.Sized buffer, global::Cairo.Surface backend, Geometry2D.Integer.Size size) :
			base(backend, size)
		{
			this.Buffer = buffer;
		}
	}
}

using System;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Gpu.Backend
{
	public interface ICanvas
	{
		IImage Image { get; }
		IFactory Factory { get; }
		Raster.Image Read(Geometry2D.Integer.Box region);
		void Draw(Draw.IColor color);
        void Draw(Draw.IColor color, Geometry2D.Single.Box area);
        void Draw(Draw.Gpu.Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination);
    }
}

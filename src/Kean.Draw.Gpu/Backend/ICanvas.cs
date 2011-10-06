using System;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Gpu.Backend
{
	public interface ICanvas
	{
		IImage Image { get; }
		IFactory Factory { get; }
		Raster.Image Read(Geometry2D.Integer.Box region);
		Raster.Image Read(Geometry2D.Integer.Box region, Gpu.Backend.ImageType type);
		void Draw(Draw.IColor color);
        void Draw(Draw.IColor color, Geometry2D.Single.Box region);
        void Draw(IImage image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination);
		void Blend(float factor);
		Geometry2D.Single.Box Clip { get; set; }
		Geometry2D.Single.Transform Transform { get; set; }
    }
}

using System;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Core.Collection;

namespace Kean.Draw.Gpu.Backend
{
	public interface IFrameBuffer :
		IDisposable
	{
		Collection.IReadOnlyVector<ITexture> Textures { get; }
		IFactory Factory { get; }

		Geometry2D.Integer.Size Size { get; }
		CoordinateSystem CoordinateSystem { get; set; }

		Geometry2D.Single.Box Clip { get; set; }
		Geometry2D.Single.Transform Transform { get; set; }

		void Use();
		void Unuse();

		Raster.Image Read(Geometry2D.Integer.Box region);
		void Draw(Draw.IColor color);
        void Draw(Draw.IColor color, Geometry2D.Single.Box region);
		void Blend(float factor);
    }
}

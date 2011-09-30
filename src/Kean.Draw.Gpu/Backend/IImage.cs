using System;
using Geometry2D = Kean.Math.Geometry2D;
namespace Kean.Draw.Gpu.Backend
{
	public interface IImage :
		IDisposable
	{
		Geometry2D.Integer.Size Resolution { get; }
		ImageType Type { get; }
		void Load(IntPtr data, Geometry2D.Integer.Size resolution, Geometry2D.Integer.Point offset, ImageType type);
		void Read(IntPtr data);
	}
}

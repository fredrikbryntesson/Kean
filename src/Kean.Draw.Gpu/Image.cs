using System;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Gpu
{
	public abstract class Image :
		Draw.Image,
		IDisposable
	{
		#region Constructors
		protected Image(Geometry2D.Integer.Size size, Draw.CoordinateSystem coordinateSystem) :
			base(size, coordinateSystem)
		{ }
		#endregion
		public override T Convert<T>()
		{
			throw new NotImplementedException();
		}
		public override Draw.Image Copy()
		{
			throw new NotImplementedException();
		}
		public override Draw.Image Copy(Geometry2D.Single.Size size, Geometry2D.Single.Transform transform)
		{
			throw new NotImplementedException();
		}
		public override Draw.Image Resize(Geometry2D.Single.Size restriction)
		{
			throw new NotImplementedException();
		}
		public override float Distance(Draw.Image other)
		{
			throw new NotImplementedException();
		}

		#region IDisposable Members

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

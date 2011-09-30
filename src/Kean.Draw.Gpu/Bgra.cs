using System;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Gpu
{
	public class Bgra :
		Image
	{
		#region Constructors
		public Bgra(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default)
		{ }
		public Bgra(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(size, coordinateSystem)
		{
		}
		#endregion
	}
}

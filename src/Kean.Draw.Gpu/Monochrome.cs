using System;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Gpu
{
	public class Monochrome :
		Image
	{
		#region Constructors
		public Monochrome(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default)
		{ }
		public Monochrome(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(size, coordinateSystem)
		{
		}
		#endregion
	}
}

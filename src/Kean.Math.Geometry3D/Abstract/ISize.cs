using System;

namespace Kean.Math.Geometry3D.Abstract
{
	public interface ISize<V>
		where V : struct
	{
		V Width { get; }
		V Height { get; }
		V Depth { get; }
	}
}

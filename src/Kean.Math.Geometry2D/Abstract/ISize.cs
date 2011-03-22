using System;

namespace Kean.Math.Geometry2D.Abstract
{
	public interface ISize<V>
		where V : struct
	{
		V Width { get; }
		V Height { get; }
	}
}

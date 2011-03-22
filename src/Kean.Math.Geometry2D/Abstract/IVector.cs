using System;

namespace Kean.Math.Geometry2D.Abstract
{
	public interface IVector<V>
		where V : struct
	{
		V X { get; }
		V Y { get; }
	}
}

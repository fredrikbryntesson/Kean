using System;

namespace Kean.Math.Geometry2D.Abstract
{
    public interface IBox<PointValueType, SizeValueType, V>
        where PointValueType : struct, IPoint<V>
        where SizeValueType : struct, ISize<V>
        where V : struct
    {
        PointValueType LeftTop { get; }
        SizeValueType Size { get; }
    }
}

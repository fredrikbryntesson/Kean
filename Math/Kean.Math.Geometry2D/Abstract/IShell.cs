using System;

namespace Kean.Math.Geometry2D.Abstract
{
    public interface IShell<V>
        where V : struct
    {
        V Left { get; }
        V Right { get; }
        V Top { get; }
        V Bottom { get; }
    }
}

using System;

namespace Kean.Math.Geometry3D.Abstract
{
    public interface IShell<V>
        where V : struct
    {
        V Left { get; }
        V Right { get; }
        V Top { get; }
        V Bottom { get; }
        V Front { get; }
        V Back { get; }
    }
}

using System;

namespace Kean.Math.Geometry2D.Single
{
    public class Point : Abstract.Point<Point, Kean.Math.Single, float>
    {
        public Point() { }
        public Point(Kean.Math.Single x, Kean.Math.Single y) : base(x, y) { }
    }
}

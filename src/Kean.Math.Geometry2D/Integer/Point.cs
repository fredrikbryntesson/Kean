using System;

namespace Kean.Math.Geometry2D.Integer
{
    public class Point : Abstract.Point<Point, Kean.Math.Integer, int>
    {
        public Point() { }
        public Point(Kean.Math.Integer x, Kean.Math.Integer y) : base(x, y) { }
    }
}

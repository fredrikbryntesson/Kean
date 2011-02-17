using System;

namespace Kean.Math.Matrix
{
    public class Single : Abstract<Kean.Math.Single, float>
    {
        public Single() { }
        public Single(Kean.Math.Integer order) : base(order) { }
        public Single(Kean.Math.Integer width, Kean.Math.Integer height) : base(width, height) { }
        public Single(Kean.Math.Geometry2D.Integer.Size size) : base(size) { }
    }
}

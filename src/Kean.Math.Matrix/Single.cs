using System;

namespace Kean.Math.Matrix
{
    public class Single : Kean.Math.Matrix.Abstract<Single, Kean.Math.Single, float>
    {
        public Single() { }
        public Single(Kean.Math.Integer order) : base(order) { }
        public Single(Kean.Math.Integer width, Kean.Math.Integer height) : base(width, height) { }
        public Single(Kean.Math.Geometry2D.Integer.Size size) : base(size) { }
        public Single(Kean.Math.Geometry2D.Integer.Size size, float[] elements) : base(size, elements) { }
        public Single(Kean.Math.Integer width, Kean.Math.Integer height, float[] elements) : base(new Kean.Math.Geometry2D.Integer.Size(width, height), elements) { }
    }
}

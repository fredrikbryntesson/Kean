using System;

namespace Kean.Math.Geometry2D.Test
{
    public static class All
    {
        public static void Test()
        {
            Double.PointValue.Test();
            Double.SizeValue.Test();
            Double.BoxValue.Test();
            //Double.TransformValue.Test();

            Single.PointValue.Test();
            Single.SizeValue.Test();
            Single.BoxValue.Test();
            //Single.TransformValue.Test();

            Integer.PointValue.Test();
            Integer.SizeValue.Test();
            Integer.BoxValue.Test();
            //Integer.TransformValue.Test();
    
            Integer.Point.Test();
            Single.Point.Test();
            Double.Point.Test();

            Integer.Size.Test();
            Single.Size.Test();
            Double.Size.Test();

            Integer.Box.Test();
            Single.Box.Test();
            Double.Box.Test();

            Integer.Transform.Test();
            Single.Transform.Test();
            Double.Transform.Test();
        }
    }
}

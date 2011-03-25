using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry2D.Double
{
    [TestFixture]
    public class Transform :
        Kean.Test.Math.Geometry2D.Abstract.Transform<Kean.Math.Geometry2D.Double.Transform, Kean.Math.Geometry2D.Double.TransformValue, Kean.Math.Geometry2D.Double.Point, Kean.Math.Geometry2D.Double.PointValue, Kean.Math.Geometry2D.Double.Size, Kean.Math.Geometry2D.Double.SizeValue,
        Kean.Math.Double, double>
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Transform0 = new Kean.Math.Geometry2D.Double.Transform(1, 4, 2, 5, 3, 6);
            this.Transform1 = new Kean.Math.Geometry2D.Double.Transform(7, 4, 2, 5, 7, 6);
            this.Transform2 = new Kean.Math.Geometry2D.Double.Transform(15, 48, 12, 33, 22, 64);
            this.Transform3 = new Kean.Math.Geometry2D.Double.Transform(-5 / 3.0f, 4 / 3.0f, 2 / 3.0f, -1 / 3.0f, 3 / 3.0f, -6 / 3.0f);
         
            this.Point0 = new Kean.Math.Geometry2D.Double.Point(-7, 3);
            this.Point1 = new Kean.Math.Geometry2D.Double.Point(2, -7);
            this.Size0 = new Kean.Math.Geometry2D.Double.Size(10, 10);

        }
        protected override double Cast(double value)
        {
            return (double)value;
        }
        public static void Test()
        {
            Transform fixture = new Transform();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

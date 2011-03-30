using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry3D.Double
{
    [TestFixture]
    public class Point :
        Kean.Test.Math.Geometry3D.Abstract.Point<Kean.Math.Geometry3D.Double.Transform, Kean.Math.Geometry3D.Double.TransformValue, Kean.Math.Geometry3D.Double.Point, Kean.Math.Geometry3D.Double.PointValue, Kean.Math.Geometry3D.Double.Size, Kean.Math.Geometry3D.Double.SizeValue, Kean.Math.Double, double>
    {
        protected override Kean.Math.Geometry3D.Double.Point CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Kean.Math.Geometry3D.Double.Point value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry3D.Double.Point(22.221f, -3.1f, 10);
            this.Vector1 = new Kean.Math.Geometry3D.Double.Point(12.221f, 13.1f, 20);
            this.Vector2 = new Kean.Math.Geometry3D.Double.Point(34.442f, 10.0f, 30);
        }
        protected override double Cast(double value)
        {
            return (double)value;
        }
        public static void Test()
        {
            Point fixture = new Point();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

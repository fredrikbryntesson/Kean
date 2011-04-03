using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry3D.Double
{
    [TestFixture]
    public class Size :
        Kean.Test.Math.Geometry3D.Abstract.Size<Kean.Math.Geometry3D.Double.Transform, Kean.Math.Geometry3D.Double.TransformValue, Kean.Math.Geometry3D.Double.Size, Kean.Math.Geometry3D.Double.SizeValue,
        Kean.Math.Double, double>
    {
        protected override Kean.Math.Geometry3D.Double.Size CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Kean.Math.Geometry3D.Double.Size value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry3D.Double.Size(22, -3, 10);
            this.Vector1 = new Kean.Math.Geometry3D.Double.Size(12, 13, 20);
            this.Vector2 = new Kean.Math.Geometry3D.Double.Size(34, 10, 30);
        }
        protected override double Cast(double value)
        {
            return (double)value;
        }
        public static void Test()
        {
            Size fixture = new Size();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

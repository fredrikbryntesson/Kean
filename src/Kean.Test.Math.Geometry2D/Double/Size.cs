using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry2D.Double
{
    [TestFixture]
    public class Size :
        Kean.Test.Math.Geometry2D.Abstract.Size<Kean.Math.Geometry2D.Double.Transform, Kean.Math.Geometry2D.Double.TransformValue, Kean.Math.Geometry2D.Double.Size, Kean.Math.Geometry2D.Double.SizeValue,
        Kean.Math.Double, double>
    {
        protected override Kean.Math.Geometry2D.Double.Size CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Kean.Math.Geometry2D.Double.Size value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry2D.Double.Size(22.221f, -3.1f);
            this.Vector1 = new Kean.Math.Geometry2D.Double.Size(12.221f, 13.1f);
            this.Vector2 = new Kean.Math.Geometry2D.Double.Size(34.442f, 10.0f);
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

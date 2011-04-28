using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Test.Math.Geometry2D.Double
{
    [TestFixture]
    public class Box :
        Kean.Test.Math.Geometry2D.Abstract.Box<Kean.Math.Geometry2D.Double.Transform, Kean.Math.Geometry2D.Double.TransformValue, Kean.Math.Geometry2D.Double.Box, Kean.Math.Geometry2D.Double.BoxValue, Kean.Math.Geometry2D.Double.Point, Kean.Math.Geometry2D.Double.PointValue, Kean.Math.Geometry2D.Double.Size, Kean.Math.Geometry2D.Double.SizeValue,
        Kean.Math.Double, double>
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Box0 = new Kean.Math.Geometry2D.Double.Box(1, 2, 3, 4);
            this.Box1 = new Kean.Math.Geometry2D.Double.Box(4, 3, 2, 1);
            this.Box2 = new Kean.Math.Geometry2D.Double.Box(2, 1, 4, 3);
        }
        protected override double Cast(double value)
        {
            return (double)value;
        }

        public void Run()
        {
            base.Run();
        }
        public static void Test()
        {
            Box fixture = new Box();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

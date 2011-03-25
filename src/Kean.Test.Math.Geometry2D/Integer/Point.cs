using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry2D.Integer
{
    [TestFixture]
    public class Point :
        Kean.Test.Math.Geometry2D.Abstract.Point< Kean.Math.Geometry2D.Integer.Transform,  Kean.Math.Geometry2D.Integer.TransformValue, Kean.Math.Geometry2D.Integer.Point, Kean.Math.Geometry2D.Integer.PointValue,  Kean.Math.Geometry2D.Integer.Size,  Kean.Math.Geometry2D.Integer.SizeValue, 
        Kean.Math.Integer, int>
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry2D.Integer.Point(22, -3);
            this.Vector1 = new Kean.Math.Geometry2D.Integer.Point(12, 13);
            this.Vector2 = new Kean.Math.Geometry2D.Integer.Point(34, 10);
        }
        protected override int Cast(double value)
        {
            return (int)value;
        }
        public static void Test()
        {
            Point fixture = new Point();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

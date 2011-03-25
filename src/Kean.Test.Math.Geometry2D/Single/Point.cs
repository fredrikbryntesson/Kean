using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry2D.Single
{
    [TestFixture]
    public class Point :
        Kean.Test.Math.Geometry2D.Abstract.Point< Kean.Math.Geometry2D.Single.Transform,  Kean.Math.Geometry2D.Single.TransformValue, Kean.Math.Geometry2D.Single.Point, Kean.Math.Geometry2D.Single.PointValue,  Kean.Math.Geometry2D.Single.Size,  Kean.Math.Geometry2D.Single.SizeValue, 
        Kean.Math.Single, float>
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry2D.Single.Point(22.221f, -3.1f);
            this.Vector1 = new Kean.Math.Geometry2D.Single.Point(12.221f, 13.1f);
            this.Vector2 = new Kean.Math.Geometry2D.Single.Point(34.442f, 10.0f);
        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        public static void Test()
        {
            Point fixture = new Point();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

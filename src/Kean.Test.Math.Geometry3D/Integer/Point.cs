using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry3D.Integer
{
    [TestFixture]
    public class Point :
        Kean.Test.Math.Geometry3D.Abstract.Point<Kean.Math.Geometry3D.Integer.Transform, Kean.Math.Geometry3D.Integer.TransformValue, Kean.Math.Geometry3D.Integer.Point, Kean.Math.Geometry3D.Integer.PointValue, Kean.Math.Geometry3D.Integer.Size, Kean.Math.Geometry3D.Integer.SizeValue, Kean.Math.Integer, int>
    {
        protected override Kean.Math.Geometry3D.Integer.Point CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Kean.Math.Geometry3D.Integer.Point value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry3D.Integer.Point(22, -3, 10);
            this.Vector1 = new Kean.Math.Geometry3D.Integer.Point(12, 13, 20);
            this.Vector2 = new Kean.Math.Geometry3D.Integer.Point(34, 10, 30);
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

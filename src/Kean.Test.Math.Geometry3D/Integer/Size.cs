using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry3D.Integer
{
    [TestFixture]
    public class Size :
        Kean.Test.Math.Geometry3D.Abstract.Size<Kean.Math.Geometry3D.Integer.Transform, Kean.Math.Geometry3D.Integer.TransformValue, Kean.Math.Geometry3D.Integer.Size, Kean.Math.Geometry3D.Integer.SizeValue,
        Kean.Math.Integer, int>
    {
        protected override Kean.Math.Geometry3D.Integer.Size CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Kean.Math.Geometry3D.Integer.Size value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry3D.Integer.Size(22, -3, 10);
            this.Vector1 = new Kean.Math.Geometry3D.Integer.Size(12, 13, 20);
            this.Vector2 = new Kean.Math.Geometry3D.Integer.Size(34, 10, 30);
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

using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry3D.Single
{
    [TestFixture]
    public class Size :
        Kean.Test.Math.Geometry3D.Abstract.Size<Kean.Math.Geometry3D.Single.Transform, Kean.Math.Geometry3D.Single.TransformValue, Kean.Math.Geometry3D.Single.Size, Kean.Math.Geometry3D.Single.SizeValue,
        Kean.Math.Single, float>
    {
        protected override Kean.Math.Geometry3D.Single.Size CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Kean.Math.Geometry3D.Single.Size value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry3D.Single.Size(22.221f, -3.1f, 10);
            this.Vector1 = new Kean.Math.Geometry3D.Single.Size(12.221f, 13.1f, 20);
            this.Vector2 = new Kean.Math.Geometry3D.Single.Size(34.442f, 10.0f, 30);
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

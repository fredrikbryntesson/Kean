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
            this.Vector0 = new Kean.Math.Geometry3D.Single.Size(22, -3, 10);
            this.Vector1 = new Kean.Math.Geometry3D.Single.Size(12, 13, 20);
            this.Vector2 = new Kean.Math.Geometry3D.Single.Size(34, 10, 30);
        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        public static void Test()
        {
            Size fixture = new Size();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry2D.Single
{
    [TestFixture]
    public class Size :
        Kean.Test.Math.Geometry2D.Abstract.Size<Kean.Math.Geometry2D.Single.Transform, Kean.Math.Geometry2D.Single.TransformValue, Kean.Math.Geometry2D.Single.Size, Kean.Math.Geometry2D.Single.SizeValue,
        Kean.Math.Single, float>
    {
        protected override Kean.Math.Geometry2D.Single.Size CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Kean.Math.Geometry2D.Single.Size value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry2D.Single.Size(22.221f, -3.1f);
            this.Vector1 = new Kean.Math.Geometry2D.Single.Size(12.221f, 13.1f);
            this.Vector2 = new Kean.Math.Geometry2D.Single.Size(34.442f, 10.0f);
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

using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry2D.Integer
{
    [TestFixture]
    public class Size :
        Kean.Test.Math.Geometry2D.Abstract.Size<Kean.Math.Geometry2D.Integer.Transform, Kean.Math.Geometry2D.Integer.TransformValue, Kean.Math.Geometry2D.Integer.Size, Kean.Math.Geometry2D.Integer.SizeValue,
        Kean.Math.Integer, int>
    {
        protected override Kean.Math.Geometry2D.Integer.Size CastFromString(string value)
        {
            return (Kean.Math.Geometry2D.Integer.Size)value;
        }
        protected override string CastToString(Kean.Math.Geometry2D.Integer.Size value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry2D.Integer.Size(22, -3);
            this.Vector1 = new Kean.Math.Geometry2D.Integer.Size(12, 13);
            this.Vector2 = new Kean.Math.Geometry2D.Integer.Size(34, 10);
        }
        protected override int Cast(double value)
        {
            return (int)value;
        }
        public static void Test()
        {
            Size fixture = new Size();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

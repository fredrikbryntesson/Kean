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
            this.Vector0 = new Kean.Math.Geometry2D.Integer.Size(5, 7);
            this.Vector1 = new Kean.Math.Geometry2D.Integer.Size(3, 6);
            this.Vector2 = new Kean.Math.Geometry2D.Integer.Size(8, 13);
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

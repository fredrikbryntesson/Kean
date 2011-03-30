using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry2D.Single
{
    [TestFixture]
    public class Transform :
        Kean.Test.Math.Geometry2D.Abstract.Transform<Kean.Math.Geometry2D.Single.Transform, Kean.Math.Geometry2D.Single.TransformValue, Kean.Math.Geometry2D.Single.Point, Kean.Math.Geometry2D.Single.PointValue, Kean.Math.Geometry2D.Single.Size, Kean.Math.Geometry2D.Single.SizeValue,
        Kean.Math.Single, float>
    {
        protected override Kean.Math.Geometry2D.Single.Transform CastFromString(string value)
        {
            return (Kean.Math.Geometry2D.Single.Transform)value;
        }
        protected override string CastToString(Kean.Math.Geometry2D.Single.Transform value)
        {
            return (string)value;
        } 
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Transform0 = new Kean.Math.Geometry2D.Single.Transform(1, 4, 2, 5, 3, 6);
            this.Transform1 = new Kean.Math.Geometry2D.Single.Transform(7, 4, 2, 5, 7, 6);
            this.Transform2 = new Kean.Math.Geometry2D.Single.Transform(15, 48, 12, 33, 22, 64);
            this.Transform3 = new Kean.Math.Geometry2D.Single.Transform(-5 / 3.0f, 4 / 3.0f, 2 / 3.0f, -1 / 3.0f, 3 / 3.0f, -6 / 3.0f);
         
            this.Point0 = new Kean.Math.Geometry2D.Single.Point(-7, 3);
            this.Point1 = new Kean.Math.Geometry2D.Single.Point(2, -7);
            this.Size0 = new Kean.Math.Geometry2D.Single.Size(10, 10);

        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        public static void Test()
        {
            Transform fixture = new Transform();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

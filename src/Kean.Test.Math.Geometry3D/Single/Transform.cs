using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry3D.Single
{
    [TestFixture]
    public class Transform :
        Kean.Test.Math.Geometry3D.Abstract.Transform<Kean.Math.Geometry3D.Single.Transform, Kean.Math.Geometry3D.Single.TransformValue, Kean.Math.Geometry3D.Single.Point, Kean.Math.Geometry3D.Single.PointValue, Kean.Math.Geometry3D.Single.Size, Kean.Math.Geometry3D.Single.SizeValue, Kean.Math.Single, float>
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Transform0 = new Kean.Math.Geometry3D.Single.Transform(-1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            this.Transform1 = new Kean.Math.Geometry3D.Single.Transform(-1, 2, 3, 4, 5, 6, 7, 8, -5, 10, 11, 12);
            this.Transform2 = new Kean.Math.Geometry3D.Single.Transform(30, 32, 36, 58, 81, 96, -10, 14, 24, 128, 182, 216);
            this.Transform3 = new Kean.Math.Geometry3D.Single.Transform(-0.5f, 1, -0.5f, 1, -5, 3, -0.5f, 3.66666666666666f, -2.16666666666667f, 0, 1, -2);
            this.Point0 = new Kean.Math.Geometry3D.Single.Point(34, 10, 30);
            this.Point1 = new Kean.Math.Geometry3D.Single.Point(226, 369, 444);
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

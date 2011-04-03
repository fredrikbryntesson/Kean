using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Test.Math.Geometry3D.Single
{
    [TestFixture]
    public class Point :
        Kean.Test.Math.Geometry3D.Abstract.Point<Kean.Math.Geometry3D.Single.Transform, Kean.Math.Geometry3D.Single.TransformValue, Kean.Math.Geometry3D.Single.Point, Kean.Math.Geometry3D.Single.PointValue, Kean.Math.Geometry3D.Single.Size, Kean.Math.Geometry3D.Single.SizeValue, Kean.Math.Single, float>
    {
        protected override Kean.Math.Geometry3D.Single.Point CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Kean.Math.Geometry3D.Single.Point value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry3D.Single.Point(22, -3, 10);
            this.Vector1 = new Kean.Math.Geometry3D.Single.Point(12, 13, 20);
            this.Vector2 = new Kean.Math.Geometry3D.Single.Point(34, 10, 30);
        }
        [Test]
        public void Norm()
        {
            Assert.That(this.Vector0.Norm.Squared().Value, Is.EqualTo(this.Cast(593)).Within(this.Precision));
            Assert.That(this.Vector0.Norm.Squared().Value, Is.EqualTo(this.Vector0.ScalarProduct(this.Vector0).Value).Within(this.Precision));
        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        public void Run()
        {
            this.Run(
                this.Norm
                );
        }
        public static void Test()
        {
            Point fixture = new Point();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

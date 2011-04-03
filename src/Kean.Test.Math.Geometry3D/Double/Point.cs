using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Test.Math.Geometry3D.Double
{
    [TestFixture]
    public class Point :
        Kean.Test.Math.Geometry3D.Abstract.Point<Kean.Math.Geometry3D.Double.Transform, Kean.Math.Geometry3D.Double.TransformValue, Kean.Math.Geometry3D.Double.Point, Kean.Math.Geometry3D.Double.PointValue, Kean.Math.Geometry3D.Double.Size, Kean.Math.Geometry3D.Double.SizeValue, Kean.Math.Double, double>
    {
        protected override Kean.Math.Geometry3D.Double.Point CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Kean.Math.Geometry3D.Double.Point value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry3D.Double.Point(22, -3, 10);
            this.Vector1 = new Kean.Math.Geometry3D.Double.Point(12, 13, 20);
            this.Vector2 = new Kean.Math.Geometry3D.Double.Point(34, 10, 30);
        }
        [Test]
        public void Norm()
        {
            Assert.That(this.Vector0.Norm.Squared().Value, Is.EqualTo(this.Cast(593)).Within(this.Precision));
            Assert.That(this.Vector0.Norm.Squared().Value, Is.EqualTo(this.Vector0.ScalarProduct(this.Vector0).Value).Within(this.Precision));
        }
        #region Spherical Coordinates Representation
        [Test]
        public void Sphere0()
        {
            Kean.Math.Geometry3D.Double.Point point = new Kean.Math.Geometry3D.Double.Point();
            Assert.That(point.Norm.Value, Is.EqualTo(0));
            Assert.That(point.Azimuth.Value, Is.EqualTo(0));
            Assert.That(point.Elevation.Value, Is.EqualTo(0));
        }
        [Test]
        public void Sphere1()
        {
            Kean.Math.Geometry3D.Double.Point point = new Kean.Math.Geometry3D.Double.Point(1, 0, 0);
            Assert.That(point.Norm.Value, Is.EqualTo(1));
            Assert.That(point.Azimuth.Value, Is.EqualTo(0));
            Assert.That(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Sphere2()
        {
            Kean.Math.Geometry3D.Double.Point point = new Kean.Math.Geometry3D.Double.Point(0, 5, 0);
            Assert.That(point.Norm.Value, Is.EqualTo(5));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
            Assert.That(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Sphere3()
        {
            Kean.Math.Geometry3D.Double.Point point = new Kean.Math.Geometry3D.Double.Point(0, -5, 0);
            Assert.That(point.Norm.Value, Is.EqualTo(5));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(-90)));
            Assert.That(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Sphere4()
        {
            Kean.Math.Geometry3D.Double.Point point = new Kean.Math.Geometry3D.Double.Point(0, 0, 1);
            Assert.That(point.Norm.Value, Is.EqualTo(1));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(0)));
            Assert.That(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(180)));
        }
        #endregion
        protected override double Cast(double value)
        {
            return (double)value;
        }
        public void Run()
        {
            this.Run(
                this.Sphere0,
                this.Sphere1,
                this.Sphere2,
                this.Sphere3,
                this.Sphere4,
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

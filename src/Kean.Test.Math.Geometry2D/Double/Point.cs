using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Test.Math.Geometry2D.Double
{
    [TestFixture]
    public class Point :
        Kean.Test.Math.Geometry2D.Abstract.Point< Kean.Math.Geometry2D.Double.Transform,  Kean.Math.Geometry2D.Double.TransformValue, Kean.Math.Geometry2D.Double.Point, Kean.Math.Geometry2D.Double.PointValue,  Kean.Math.Geometry2D.Double.Size,  Kean.Math.Geometry2D.Double.SizeValue, 
        Kean.Math.Double, double>
    {
        protected override Kean.Math.Geometry2D.Double.Point CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Kean.Math.Geometry2D.Double.Point value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry2D.Double.Point(22.221f, -3.1f);
            this.Vector1 = new Kean.Math.Geometry2D.Double.Point(12.221f, 13.1f);
            this.Vector2 = new Kean.Math.Geometry2D.Double.Point(34.442f, 10.0f);
        }
        protected override double Cast(double value)
        {
            return (double)value;
        }
        #region Polar Representation
        [Test]
        public void Polar0()
        {
            Kean.Math.Geometry2D.Double.Point point = new Kean.Math.Geometry2D.Double.Point();
            Assert.That(point.Norm.Value, Is.EqualTo(0));
            Assert.That(point.Azimuth.Value, Is.EqualTo(0));
        }
        [Test]
        public void Polar1()
        {
            Kean.Math.Geometry2D.Double.Point point = new Kean.Math.Geometry2D.Double.Point(1,0);
            Assert.That(point.Norm.Value, Is.EqualTo(1));
            Assert.That(point.Azimuth.Value, Is.EqualTo(0));
        }
        [Test]
        public void Polar2()
        {
            Kean.Math.Geometry2D.Double.Point point = new Kean.Math.Geometry2D.Double.Point(0, 1);
            Assert.That(point.Norm.Value, Is.EqualTo(1));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Polar3()
        {
            Kean.Math.Geometry2D.Double.Point point = new Kean.Math.Geometry2D.Double.Point(0, -5);
            Assert.That(point.Norm.Value, Is.EqualTo(5));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(-90)));
        }
        [Test]
        public void Polar4()
        {
            Kean.Math.Geometry2D.Double.Point point = new Kean.Math.Geometry2D.Double.Point(-1, 0);
            Assert.That(point.Norm.Value, Is.EqualTo(1));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(180)));
        }
        [Test]
        public void Polar5()
        {
            Kean.Math.Geometry2D.Double.Point point = new Kean.Math.Geometry2D.Double.Point(33, -7);
            double radius = point.Norm.Value;
            double azimuth = point.Azimuth.Value;
            Kean.Math.Geometry2D.Double.Point point2 = Kean.Math.Geometry2D.Double.Point.Polar(radius, azimuth);
            Assert.That(point.Distance(point2).Value, Is.EqualTo(0).Within(this.Precision));
        }
        #endregion
        public void Run()
        {
            this.Run(
                base.Run,
                this.Polar0,
                this.Polar1,
                this.Polar2,
                this.Polar3,
                this.Polar4,
                this.Polar5
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

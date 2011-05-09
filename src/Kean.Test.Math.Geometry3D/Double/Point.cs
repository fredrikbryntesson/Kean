using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry3D;

namespace Kean.Test.Math.Geometry3D.Double
{
    [TestFixture]
    public class Point :
        Kean.Test.Math.Geometry3D.Abstract.Point<Target.Double.Transform, Target.Double.TransformValue, Target.Double.Point, Target.Double.PointValue, Target.Double.Size, Target.Double.SizeValue, Kean.Math.Double, double>
    {
        protected override Target.Double.Point CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Target.Double.Point value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Target.Double.Point(22, -3, 10);
            this.Vector1 = new Target.Double.Point(12, 13, 20);
            this.Vector2 = new Target.Double.Point(34, 10, 30);
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
            Target.Double.Point point = new Target.Double.Point();
            Assert.That(point.Norm.Value, Is.EqualTo(0));
            Assert.That(point.Azimuth.Value, Is.EqualTo(0));
            Assert.That(point.Elevation.Value, Is.EqualTo(0));
        }
        [Test]
        public void Sphere1()
        {
            Target.Double.Point point = new Target.Double.Point(1, 0, 0);
            Assert.That(point.Norm.Value, Is.EqualTo(1));
            Assert.That(point.Azimuth.Value, Is.EqualTo(0));
            Assert.That(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Sphere2()
        {
            Target.Double.Point point = new Target.Double.Point(0, 5, 0);
            Assert.That(point.Norm.Value, Is.EqualTo(5));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
            Assert.That(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Sphere3()
        {
            Target.Double.Point point = new Target.Double.Point(0, -5, 0);
            Assert.That(point.Norm.Value, Is.EqualTo(5));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(-90)));
            Assert.That(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Sphere4()
        {
            Target.Double.Point point = new Target.Double.Point(-5, 0, 0);
            Assert.That(point.Norm.Value, Is.EqualTo(5));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(180)));
            Assert.That(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Sphere5()
        {
            Target.Double.Point point = new Target.Double.Point(0, 0, 1);
            Assert.That(point.Norm.Value, Is.EqualTo(1));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(0)));
            Assert.That(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(0)));
        }
        [Test]
        public void Sphere6()
        {
            Target.Double.Point point = new Target.Double.Point(33, -7, 12);
            double radius = point.Norm.Value;
            double azimuth = point.Azimuth.Value;
            double elevation = point.Elevation.Value;
            Target.Double.Point point2 = Target.Double.Point.Spherical(radius, azimuth, elevation);
            Assert.That(point.Distance(point2).Value, Is.EqualTo(0).Within(this.Precision));
        }
        #endregion
        protected override double Cast(double value)
        {
            return (double)value;
        }
        [Test]
        public void Casts()
        {
            // integer - double
            {
                Target.Integer.Point integer = new Target.Integer.Point(10, 20, 30);
                Target.Double.Point @double = integer;
                Assert.That(@double.X, Is.EqualTo(10));
                Assert.That(@double.Y, Is.EqualTo(20));
                Assert.That(@double.Z, Is.EqualTo(30));
                Assert.That((Target.Integer.Point)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.Point single = new Target.Single.Point(10, 20, 30);
                Target.Double.Point @double = single;
                Assert.That(@double.X, Is.EqualTo(10));
                Assert.That(@double.Y, Is.EqualTo(20));
                Assert.That(@double.Z, Is.EqualTo(30));
                Assert.That((Target.Single.Point)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueCasts()
        {
            // integer - double
            {
                Target.Integer.PointValue integer = new Target.Integer.PointValue(10, 20, 30);
                Target.Double.PointValue @double = integer;
                Assert.That(@double.X, Is.EqualTo(10));
                Assert.That(@double.Y, Is.EqualTo(20));
                Assert.That(@double.Z, Is.EqualTo(30));
                Assert.That((Target.Integer.PointValue)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.PointValue single = new Target.Single.PointValue(10, 20, 30);
                Target.Double.PointValue @double = single;
                Assert.That(@double.X, Is.EqualTo(10));
                Assert.That(@double.Y, Is.EqualTo(20));
                Assert.That(@double.Z, Is.EqualTo(30));
                Assert.That((Target.Single.PointValue)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.PointValue(10, 20, 30);
            Assert.That(textFromValue, Is.EqualTo("10 20 30"));
            Target.Single.PointValue @integerFromText = "10 20 30";
            Assert.That(@integerFromText.X, Is.EqualTo(10));
            Assert.That(@integerFromText.Y, Is.EqualTo(20));
            Assert.That(@integerFromText.Z, Is.EqualTo(30));
        }
        public void Run()
        {
            this.Run(
                this.Sphere0,
                this.Sphere1,
                this.Sphere2,
                this.Sphere3,
                this.Sphere4,
                this.Sphere5,
                this.Sphere6,
                this.Norm,
                this.Casts,
                this.ValueCasts,
                this.ValueStringCasts
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

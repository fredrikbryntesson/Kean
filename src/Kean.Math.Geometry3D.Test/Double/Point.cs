using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry3D;

namespace Kean.Math.Geometry3D.Test.Double
{
    [TestFixture]
    public class Point :
        Kean.Math.Geometry3D.Test.Abstract.Point<Point, Target.Double.Transform, Target.Double.TransformValue, Target.Double.Point, Target.Double.PointValue, Target.Double.Size, Target.Double.SizeValue, Kean.Math.Double, double>
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
        public override void Setup()
        {
            this.Vector0 = new Target.Double.Point(22, -3, 10);
            this.Vector1 = new Target.Double.Point(12, 13, 20);
            this.Vector2 = new Target.Double.Point(34, 10, 30);
        }
        protected override void Run()
        {
            base.Run();
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
        [Test]
        public void Norm()
        {
            Expect(this.Vector0.Norm.Squared().Value, Is.EqualTo(this.Cast(593)).Within(this.Precision));
            Expect(this.Vector0.Norm.Squared().Value, Is.EqualTo(this.Vector0.ScalarProduct(this.Vector0).Value).Within(this.Precision));
        }
        #region Spherical Coordinates Representation
        [Test]
        public void Sphere0()
        {
            Target.Double.Point point = new Target.Double.Point();
            Expect(point.Norm.Value, Is.EqualTo(0));
            Expect(point.Azimuth.Value, Is.EqualTo(0));
            Expect(point.Elevation.Value, Is.EqualTo(0));
        }
        [Test]
        public void Sphere1()
        {
            Target.Double.Point point = new Target.Double.Point(1, 0, 0);
            Expect(point.Norm.Value, Is.EqualTo(1));
            Expect(point.Azimuth.Value, Is.EqualTo(0));
            Expect(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Sphere2()
        {
            Target.Double.Point point = new Target.Double.Point(0, 5, 0);
            Expect(point.Norm.Value, Is.EqualTo(5));
            Expect(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
            Expect(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Sphere3()
        {
            Target.Double.Point point = new Target.Double.Point(0, -5, 0);
            Expect(point.Norm.Value, Is.EqualTo(5));
            Expect(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(-90)));
            Expect(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Sphere4()
        {
            Target.Double.Point point = new Target.Double.Point(-5, 0, 0);
            Expect(point.Norm.Value, Is.EqualTo(5));
            Expect(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(180)));
            Expect(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Sphere5()
        {
            Target.Double.Point point = new Target.Double.Point(0, 0, 1);
            Expect(point.Norm.Value, Is.EqualTo(1));
            Expect(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(0)));
            Expect(point.Elevation.Value, Is.EqualTo(Kean.Math.Double.ToRadians(0)));
        }
        [Test]
        public void Sphere6()
        {
            Target.Double.Point point = new Target.Double.Point(33, -7, 12);
            double radius = point.Norm.Value;
            double azimuth = point.Azimuth.Value;
            double elevation = point.Elevation.Value;
            Target.Double.Point point2 = Target.Double.Point.Spherical(radius, azimuth, elevation);
            Expect(point.Distance(point2).Value, Is.EqualTo(0).Within(this.Precision));
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
                Expect(@double.X, Is.EqualTo(10));
                Expect(@double.Y, Is.EqualTo(20));
                Expect(@double.Z, Is.EqualTo(30));
                Expect((Target.Integer.Point)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.Point single = new Target.Single.Point(10, 20, 30);
                Target.Double.Point @double = single;
                Expect(@double.X, Is.EqualTo(10));
                Expect(@double.Y, Is.EqualTo(20));
                Expect(@double.Z, Is.EqualTo(30));
                Expect((Target.Single.Point)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueCasts()
        {
            // integer - double
            {
                Target.Integer.PointValue integer = new Target.Integer.PointValue(10, 20, 30);
                Target.Double.PointValue @double = integer;
                Expect(@double.X, Is.EqualTo(10));
                Expect(@double.Y, Is.EqualTo(20));
                Expect(@double.Z, Is.EqualTo(30));
                Expect((Target.Integer.PointValue)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.PointValue single = new Target.Single.PointValue(10, 20, 30);
                Target.Double.PointValue @double = single;
                Expect(@double.X, Is.EqualTo(10));
                Expect(@double.Y, Is.EqualTo(20));
                Expect(@double.Z, Is.EqualTo(30));
                Expect((Target.Single.PointValue)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.PointValue(10, 20, 30);
            Expect(textFromValue, Is.EqualTo("10 20 30"));
            Target.Single.PointValue @integerFromText = "10 20 30";
            Expect(@integerFromText.X, Is.EqualTo(10));
            Expect(@integerFromText.Y, Is.EqualTo(20));
            Expect(@integerFromText.Z, Is.EqualTo(30));
        }
    }
}

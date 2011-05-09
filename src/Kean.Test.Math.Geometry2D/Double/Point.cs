using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry2D;

namespace Kean.Test.Math.Geometry2D.Double
{
    [TestFixture]
    public class Point :
        Kean.Test.Math.Geometry2D.Abstract.Point< Target.Double.Transform,  Target.Double.TransformValue, Target.Double.Point, Target.Double.PointValue,  Target.Double.Size,  Target.Double.SizeValue, 
        Kean.Math.Double, double>
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
            this.Vector0 = new Target.Double.Point(22.221f, -3.1f);
            this.Vector1 = new Target.Double.Point(12.221f, 13.1f);
            this.Vector2 = new Target.Double.Point(34.442f, 10.0f);
        }
        protected override double Cast(double value)
        {
            return (double)value;
        }
        #region Polar Representation
        [Test]
        public void Polar0()
        {
            Target.Double.Point point = new Target.Double.Point();
            Assert.That(point.Norm.Value, Is.EqualTo(0));
            Assert.That(point.Azimuth.Value, Is.EqualTo(0));
        }
        [Test]
        public void Polar1()
        {
            Target.Double.Point point = new Target.Double.Point(1,0);
            Assert.That(point.Norm.Value, Is.EqualTo(1));
            Assert.That(point.Azimuth.Value, Is.EqualTo(0));
        }
        [Test]
        public void Polar2()
        {
            Target.Double.Point point = new Target.Double.Point(0, 1);
            Assert.That(point.Norm.Value, Is.EqualTo(1));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(90)));
        }
        [Test]
        public void Polar3()
        {
            Target.Double.Point point = new Target.Double.Point(0, -5);
            Assert.That(point.Norm.Value, Is.EqualTo(5));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(-90)));
        }
        [Test]
        public void Polar4()
        {
            Target.Double.Point point = new Target.Double.Point(-1, 0);
            Assert.That(point.Norm.Value, Is.EqualTo(1));
            Assert.That(point.Azimuth.Value, Is.EqualTo(Kean.Math.Double.ToRadians(180)));
        }
        [Test]
        public void Polar5()
        {
            Target.Double.Point point = new Target.Double.Point(33, -7);
            double radius = point.Norm.Value;
            double azimuth = point.Azimuth.Value;
            Target.Double.Point point2 = Target.Double.Point.Polar(radius, azimuth);
            Assert.That(point.Distance(point2).Value, Is.EqualTo(0).Within(this.Precision));
        }
        #endregion
        [Test]
        public void Angles()
        {
           Assert.That(Target.Single.Point.BasisX.Angle(Target.Single.Point.BasisX).Value, Is.EqualTo(0).Within(this.Precision));
           Assert.That(Target.Single.Point.BasisX.Angle(Target.Single.Point.BasisY).Value, Is.EqualTo(Kean.Math.Single.Pi / 2).Within(this.Precision));
           Assert.That(Target.Single.Point.BasisX.Angle(-Target.Single.Point.BasisY).Value, Is.EqualTo(-Kean.Math.Single.Pi / 2).Within(this.Precision));
           Assert.That(Target.Single.Point.BasisX.Angle(-Target.Single.Point.BasisX).Value, Is.EqualTo(Kean.Math.Single.Pi).Within(this.Precision));
        }
        [Test]
        public void Casts()
        {
            // integer - double
            {
                Target.Integer.Point integer = new Target.Integer.Point(10, 20);
                Target.Double.Point @double = integer;
                Assert.That(@double.X, Is.EqualTo(10));
                Assert.That(@double.Y, Is.EqualTo(20));
                Assert.That((Target.Integer.Point)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.Point single = new Target.Single.Point(10, 20);
                Target.Double.Point @double = single;
                Assert.That(@double.X, Is.EqualTo(10));
                Assert.That(@double.Y, Is.EqualTo(20));
                Assert.That((Target.Single.Point)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueCasts()
        {
            // integer - double
            {
                Target.Integer.PointValue integer = new Target.Integer.PointValue(10, 20);
                Target.Double.PointValue @double = integer;
                Assert.That(@double.X, Is.EqualTo(10));
                Assert.That(@double.Y, Is.EqualTo(20));
                Assert.That((Target.Integer.PointValue)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.PointValue single = new Target.Single.PointValue(10, 20);
                Target.Double.PointValue @double = single;
                Assert.That(@double.X, Is.EqualTo(10));
                Assert.That(@double.Y, Is.EqualTo(20));
                Assert.That((Target.Single.PointValue)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.PointValue(10, 20);
            Assert.That(textFromValue, Is.EqualTo("10 20"));
            Target.Single.PointValue @integerFromText = "10 20";
            Assert.That(@integerFromText.X, Is.EqualTo(10));
            Assert.That(@integerFromText.Y, Is.EqualTo(20));
        }
        public void Run()
        {
            this.Run(
                base.Run,
                this.Polar0,
                this.Polar1,
                this.Polar2,
                this.Polar3,
                this.Polar4,
                this.Polar5,
                this.Angles,
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

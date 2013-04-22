using System;
using NUnit.Framework;

using Target = Kean.Math.Geometry3D;
namespace Kean.Math.Geometry3D.Test.Single
{
    [TestFixture]
    public class Point :
        Kean.Math.Geometry3D.Test.Abstract.Point<Point, Target.Single.Transform, Target.Single.TransformValue, Target.Single.Point, Target.Single.PointValue, Target.Single.Size, Target.Single.SizeValue, Kean.Math.Single, float>
    {
        protected override Target.Single.Point CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Target.Single.Point value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public override void Setup()
        {
            this.Vector0 = new Target.Single.Point(22, -3, 10);
            this.Vector1 = new Target.Single.Point(12, 13, 20);
            this.Vector2 = new Target.Single.Point(34, 10, 30);
        }
        protected override void Run()
        {
            base.Run();
            this.Run(
                this.Casts,
                this.ValueCasts,
                this.ValueStringCasts,
                this.Norm
                );
        }
        [Test]
        public void Norm()
        {
            Verify(this.Vector0.Norm.Squared().Value, Is.EqualTo(this.Cast(593)).Within(this.Precision));
            Verify(this.Vector0.Norm.Squared().Value, Is.EqualTo(this.Vector0.ScalarProduct(this.Vector0).Value).Within(this.Precision));
        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        [Test]
        public void Casts()
        {
            // integer - single
            Target.Integer.Point integer = new Target.Integer.Point(10, 20, 30);
            Target.Single.Point single = integer;
            Verify(single.X, Is.EqualTo(10));
            Verify(single.Y, Is.EqualTo(20));
            Verify(single.Z, Is.EqualTo(30));
            Verify((Target.Integer.Point)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueCasts()
        {
            // integer - single
            Target.Integer.PointValue integer = new Target.Integer.PointValue(10, 20, 30);
            Target.Single.PointValue single = integer;
            Verify(single.X, Is.EqualTo(10));
            Verify(single.Y, Is.EqualTo(20));
            Verify(single.Z, Is.EqualTo(30));
            Verify((Target.Integer.PointValue)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.PointValue(10, 20, 30);
            Verify(textFromValue, Is.EqualTo("10, 20, 30"));
            Target.Single.PointValue @integerFromText = "10 20 30";
            Verify(@integerFromText.X, Is.EqualTo(10));
            Verify(@integerFromText.Y, Is.EqualTo(20));
            Verify(@integerFromText.Z, Is.EqualTo(30));
        }
    }
}

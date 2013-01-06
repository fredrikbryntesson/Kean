using System;
using NUnit.Framework;
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
            Expect(this.Vector0.Norm.Squared().Value, Is.EqualTo(this.Cast(593)).Within(this.Precision));
            Expect(this.Vector0.Norm.Squared().Value, Is.EqualTo(this.Vector0.ScalarProduct(this.Vector0).Value).Within(this.Precision));
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
            Expect(single.X, Is.EqualTo(10));
            Expect(single.Y, Is.EqualTo(20));
            Expect(single.Z, Is.EqualTo(30));
            Expect((Target.Integer.Point)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueCasts()
        {
            // integer - single
            Target.Integer.PointValue integer = new Target.Integer.PointValue(10, 20, 30);
            Target.Single.PointValue single = integer;
            Expect(single.X, Is.EqualTo(10));
            Expect(single.Y, Is.EqualTo(20));
            Expect(single.Z, Is.EqualTo(30));
            Expect((Target.Integer.PointValue)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.PointValue(10, 20, 30);
            Expect(textFromValue, Is.EqualTo("10, 20, 30"));
            Target.Single.PointValue @integerFromText = "10 20 30";
            Expect(@integerFromText.X, Is.EqualTo(10));
            Expect(@integerFromText.Y, Is.EqualTo(20));
            Expect(@integerFromText.Z, Is.EqualTo(30));
        }
    }
}

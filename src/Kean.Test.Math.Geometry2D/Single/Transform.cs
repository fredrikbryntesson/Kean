using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry2D;

namespace Kean.Test.Math.Geometry2D.Single
{
    [TestFixture]
    public class Transform :
        Kean.Test.Math.Geometry2D.Abstract.Transform<Target.Single.Transform, Target.Single.TransformValue, Target.Single.Point, Target.Single.PointValue, Target.Single.Size, Target.Single.SizeValue,
        Kean.Math.Single, float>
    {
        protected override Target.Single.Transform CastFromString(string value)
        {
            return (Target.Single.Transform)value;
        }
        protected override string CastToString(Target.Single.Transform value)
        {
            return (string)value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Transform0 = new Target.Single.Transform(1, 4, 2, 5, 3, 6);
            this.Transform1 = new Target.Single.Transform(7, 4, 2, 5, 7, 6);
            this.Transform2 = new Target.Single.Transform(15, 48, 12, 33, 22, 64);
            this.Transform3 = new Target.Single.Transform(-5 / 3.0f, 4 / 3.0f, 2 / 3.0f, -1 / 3.0f, 3 / 3.0f, -6 / 3.0f);

            this.Point0 = new Target.Single.Point(-7, 3);
            this.Point1 = new Target.Single.Point(2, -7);
            this.Size0 = new Target.Single.Size(10, 10);

        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        [Test]
        public void Casts()
        {
            // integer - single
            Target.Integer.Transform integer = new Target.Integer.Transform(10, 20, 30, 40, 50, 60);
            Target.Single.Transform single = integer;
            Assert.That(single.A, Is.EqualTo(10));
            Assert.That(single.B, Is.EqualTo(20));
            Assert.That(single.C, Is.EqualTo(30));
            Assert.That(single.D, Is.EqualTo(40));
            Assert.That(single.E, Is.EqualTo(50));
            Assert.That(single.F, Is.EqualTo(60));
            Assert.That((Target.Integer.Transform)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueCasts()
        {
            // integer - single
            Target.Integer.TransformValue integer = new Target.Integer.TransformValue(10, 20, 30, 40, 50, 60);
            Target.Single.TransformValue single = integer;
            Assert.That(single.A, Is.EqualTo(10));
            Assert.That(single.B, Is.EqualTo(20));
            Assert.That(single.C, Is.EqualTo(30));
            Assert.That(single.D, Is.EqualTo(40));
            Assert.That(single.E, Is.EqualTo(50));
            Assert.That(single.F, Is.EqualTo(60));
            Assert.That((Target.Integer.TransformValue)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.TransformValue(10, 20, 30, 40, 50, 60);
            Assert.That(textFromValue, Is.EqualTo("10, 30, 50; 20, 40, 60; 0, 0, 1"));
            Target.Single.TransformValue @integerFromText = "10, 30, 50; 20, 40, 60; 0, 0, 1";
            Assert.That(@integerFromText.A, Is.EqualTo(10));
            Assert.That(@integerFromText.B, Is.EqualTo(20));
            Assert.That(@integerFromText.C, Is.EqualTo(30));
            Assert.That(@integerFromText.D, Is.EqualTo(40));
            Assert.That(@integerFromText.E, Is.EqualTo(50));
            Assert.That(@integerFromText.F, Is.EqualTo(60));
        }
        public void Run()
        {
            this.Run(
                this.Casts,
                this.ValueCasts,
                this.ValueStringCasts,
                base.Run
                );
        }
        public static void Test()
        {
            Transform fixture = new Transform();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

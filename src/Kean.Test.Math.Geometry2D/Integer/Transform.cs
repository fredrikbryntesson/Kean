using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry2D;

namespace Kean.Test.Math.Geometry2D.Integer
{
    [TestFixture]
    public class Transform :
        Kean.Test.Math.Geometry2D.Abstract.Transform<Target.Integer.Transform, Target.Integer.TransformValue, Target.Integer.Point, Target.Integer.PointValue, Target.Integer.Size, Target.Integer.SizeValue,
        Kean.Math.Integer, int>
    {
        protected override Target.Integer.Transform CastFromString(string value)
        {
            return (Target.Integer.Transform)value;
        }
        protected override string CastToString(Target.Integer.Transform value)
        {
            return (string)value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Transform0 = new Target.Integer.Transform(1, 4, 2, 5, 3, 6);
            this.Transform1 = new Target.Integer.Transform(7, 4, 2, 5, 7, 6);
            this.Transform2 = new Target.Integer.Transform(15, 48, 12, 33, 22, 64);
            this.Transform3 = new Target.Integer.Transform(-5 / 3, 4 / 3, 2 / 3, -1 / 3, 3 / 3, -6 / 3);

            this.Point0 = new Target.Integer.Point(-7, 3);
            this.Point1 = new Target.Integer.Point(2, -7);
            this.Size0 = new Target.Integer.Size(10, 10);

        }
        protected override int Cast(double value)
        {
            return (int)value;
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Integer.TransformValue(10, 20, 30, 40, 50, 60);
            Assert.That(textFromValue, Is.EqualTo("10, 30, 50; 20, 40, 60; 0, 0, 1"));
            Target.Integer.TransformValue @integerFromText = "10, 30, 50; 20, 40, 60; 0, 0, 1";
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
                this.Casting,
                this.CastingNull,
                this.CastToArray,
                this.CreateIdentity,
                this.CreateTranslation,
                this.CreateZeroTransform,
                this.Equality,
                this.GetTranslation,
                this.GetValueValues,
                this.MultiplicationTransformPoint,
                this.MultiplicationTransformTransform,
                this.ValueStringCasts
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

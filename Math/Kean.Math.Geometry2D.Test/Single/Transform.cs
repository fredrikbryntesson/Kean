using System;
using NUnit.Framework;

using Target = Kean.Math.Geometry2D;

namespace Kean.Math.Geometry2D.Test.Single
{
    [TestFixture]
    public class Transform :
        Kean.Math.Geometry2D.Test.Abstract.Transform<Transform, Target.Single.Transform, Target.Single.TransformValue, Target.Single.Shell, Target.Single.ShellValue, Target.Single.Box, Target.Single.BoxValue, Target.Single.Point, Target.Single.PointValue, Target.Single.Size, Target.Single.SizeValue, Kean.Math.Single, float>
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
        public override void Setup()
        {
            this.Transform0 = new Target.Single.Transform(1, 4, 2, 5, 3, 6);
            this.Transform1 = new Target.Single.Transform(7, 4, 2, 5, 7, 6);
            this.Transform2 = new Target.Single.Transform(15, 48, 12, 33, 22, 64);
            this.Transform3 = new Target.Single.Transform(-5 / 3.0f, 4 / 3.0f, 2 / 3.0f, -1 / 3.0f, 3 / 3.0f, -6 / 3.0f);

            this.Point0 = new Target.Single.Point(-7, 3);
            this.Point1 = new Target.Single.Point(2, -7);
            this.Size0 = new Target.Single.Size(10, 10);
        }
        protected override void Run()
        {
            base.Run();
            this.Run(
                this.Casts,
                this.ValueCasts,
                this.ValueStringCasts,
                this.ClassCasts,
                this.ClassStringCasts
                );
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
            Expect(single.A, Is.EqualTo(10));
            Expect(single.B, Is.EqualTo(20));
            Expect(single.C, Is.EqualTo(30));
            Expect(single.D, Is.EqualTo(40));
            Expect(single.E, Is.EqualTo(50));
            Expect(single.F, Is.EqualTo(60));
            Expect((Target.Integer.Transform)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueCasts()
        {
            // integer - single
            Target.Integer.TransformValue integer = new Target.Integer.TransformValue(10, 20, 30, 40, 50, 60);
            Target.Single.TransformValue single = integer;
            Expect(single.A, Is.EqualTo(10));
            Expect(single.B, Is.EqualTo(20));
            Expect(single.C, Is.EqualTo(30));
            Expect(single.D, Is.EqualTo(40));
            Expect(single.E, Is.EqualTo(50));
            Expect(single.F, Is.EqualTo(60));
            Expect((Target.Integer.TransformValue)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.TransformValue(10, 20, 30, 40, 50, 60);
			Expect(textFromValue, Is.EqualTo("10, 20, 30, 40, 50, 60"));
			Target.Single.TransformValue @integerFromText = "10 20 30 40 50 60";
            Expect(@integerFromText.A, Is.EqualTo(10));
            Expect(@integerFromText.B, Is.EqualTo(20));
            Expect(@integerFromText.C, Is.EqualTo(30));
            Expect(@integerFromText.D, Is.EqualTo(40));
            Expect(@integerFromText.E, Is.EqualTo(50));
            Expect(@integerFromText.F, Is.EqualTo(60));
        }
        [Test]
        public void ClassCasts()
        {
            // integer - single
            Target.Integer.Transform integer = new Target.Integer.Transform(10, 20, 30, 40, 50, 60);
            Target.Single.Transform single = integer;
            Expect(single.A, Is.EqualTo(10));
            Expect(single.B, Is.EqualTo(20));
            Expect(single.C, Is.EqualTo(30));
            Expect(single.D, Is.EqualTo(40));
            Expect(single.E, Is.EqualTo(50));
            Expect(single.F, Is.EqualTo(60));
            Expect((Target.Integer.Transform)single, Is.EqualTo(integer));
        }
        [Test]
        public void ClassStringCasts()
        {
            string textFromValue = new Target.Single.Transform(10, 20, 30, 40, 50, 60);
			Expect(textFromValue, Is.EqualTo("10, 20, 30, 40, 50, 60"));
			Target.Single.Transform @integerFromText = "10 20 30 40 50 60";
            Expect(@integerFromText.A, Is.EqualTo(10));
            Expect(@integerFromText.B, Is.EqualTo(20));
            Expect(@integerFromText.C, Is.EqualTo(30));
            Expect(@integerFromText.D, Is.EqualTo(40));
            Expect(@integerFromText.E, Is.EqualTo(50));
            Expect(@integerFromText.F, Is.EqualTo(60));
        }
    }
}

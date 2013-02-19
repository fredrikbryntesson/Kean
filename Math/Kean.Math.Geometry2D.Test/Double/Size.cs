using System;
using NUnit.Framework;

using Target = Kean.Math.Geometry2D;

namespace Kean.Math.Geometry2D.Test.Double
{
    [TestFixture]
    public class Size :
        Kean.Math.Geometry2D.Test.Abstract.Size<Size, Target.Double.Transform, Target.Double.TransformValue, Target.Double.Shell, Target.Double.ShellValue, Target.Double.Box, Target.Double.BoxValue, Target.Double.Point, Target.Double.PointValue, Target.Double.Size, Target.Double.SizeValue, Kean.Math.Double, double>
    {
        protected override Target.Double.Size CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Target.Double.Size value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public override void Setup()
        {
            base.Setup();
            this.Vector0 = new Target.Double.Size(22.221f, -3.1f);
            this.Vector1 = new Target.Double.Size(12.221f, 13.1f);
            this.Vector2 = new Target.Double.Size(34.442f, 10.0f);
        }
        protected override void Run()
        {
            base.Run();
            this.Run(
                this.Casts,
                this.ValueCasts,
                this.ValueStringCasts
                );
        }
        protected override double Cast(double value)
        {
            return (double)value;
        }
        [Test]
        public void Casts()
        {
            // integer - double
            {
                Target.Integer.Size integer = new Target.Integer.Size(10, 20);
                Target.Double.Size @double = integer;
                Expect(@double.Width, Is.EqualTo(10));
                Expect(@double.Height, Is.EqualTo(20));
                Expect((Target.Integer.Size)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.Size single = new Target.Single.Size(10, 20);
                Target.Double.Size @double = single;
                Expect(@double.Width, Is.EqualTo(10));
                Expect(@double.Height, Is.EqualTo(20));
                Expect((Target.Single.Size)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueCasts()
        {
            // integer - double
            {
                Target.Integer.SizeValue integer = new Target.Integer.SizeValue(10, 20);
                Target.Double.SizeValue @double = integer;
                Expect(@double.Width, Is.EqualTo(10));
                Expect(@double.Height, Is.EqualTo(20));
                Expect((Target.Integer.SizeValue)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.SizeValue single = new Target.Single.SizeValue(10, 20);
                Target.Double.SizeValue @double = single;
                Expect(@double.Width, Is.EqualTo(10));
                Expect(@double.Height, Is.EqualTo(20));
                Expect((Target.Single.SizeValue)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.SizeValue(10, 20);
            Expect(textFromValue, Is.EqualTo("10, 20"));
            Target.Single.SizeValue @integerFromText = "10, 20";
            Expect(@integerFromText.Width, Is.EqualTo(10));
            Expect(@integerFromText.Height, Is.EqualTo(20));
        }
    }
}

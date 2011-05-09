using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry3D;

namespace Kean.Test.Math.Geometry3D.Double
{
    [TestFixture]
    public class Size :
        Kean.Test.Math.Geometry3D.Abstract.Size<Target.Double.Transform, Target.Double.TransformValue, Target.Double.Size, Target.Double.SizeValue,
        Kean.Math.Double, double>
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
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Target.Double.Size(22, -3, 10);
            this.Vector1 = new Target.Double.Size(12, 13, 20);
            this.Vector2 = new Target.Double.Size(34, 10, 30);
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
                Target.Integer.Size integer = new Target.Integer.Size(10, 20, 30);
                Target.Double.Size @double = integer;
                Assert.That(@double.Width, Is.EqualTo(10));
                Assert.That(@double.Height, Is.EqualTo(20));
                Assert.That(@double.Depth, Is.EqualTo(30));
                Assert.That((Target.Integer.Size)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.Size single = new Target.Single.Size(10, 20, 30);
                Target.Double.Size @double = single;
                Assert.That(@double.Width, Is.EqualTo(10));
                Assert.That(@double.Height, Is.EqualTo(20));
                Assert.That(@double.Depth, Is.EqualTo(30));
                Assert.That((Target.Single.Size)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueCasts()
        {
            // integer - double
            {
                Target.Integer.SizeValue integer = new Target.Integer.SizeValue(10, 20, 30);
                Target.Double.SizeValue @double = integer;
                Assert.That(@double.Width, Is.EqualTo(10));
                Assert.That(@double.Height, Is.EqualTo(20));
                Assert.That(@double.Depth, Is.EqualTo(30));
                Assert.That((Target.Integer.SizeValue)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.SizeValue single = new Target.Single.SizeValue(10, 20,30);
                Target.Double.SizeValue @double = single;
                Assert.That(@double.Width, Is.EqualTo(10));
                Assert.That(@double.Height, Is.EqualTo(20));
                Assert.That(@double.Depth, Is.EqualTo(30));
                Assert.That((Target.Single.SizeValue)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.SizeValue(10, 20, 30);
            Assert.That(textFromValue, Is.EqualTo("10 20 30"));
            Target.Single.SizeValue @integerFromText = "10 20 30";
            Assert.That(@integerFromText.Width, Is.EqualTo(10));
            Assert.That(@integerFromText.Height, Is.EqualTo(20));
            Assert.That(@integerFromText.Depth, Is.EqualTo(30));
        }
        public void Run()
        {
            this.Run(
                base.Run,
                this.Casts,
                this.ValueCasts,
                this.ValueStringCasts
                );
        }
        public static void Test()
        {
            Size fixture = new Size();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

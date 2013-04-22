using System;
using NUnit.Framework;

using Target = Kean.Math.Geometry3D;

namespace Kean.Math.Geometry3D.Test.Double
{
    [TestFixture]
    public class Size :
        Kean.Math.Geometry3D.Test.Abstract.Size<Size, Target.Double.Transform, Target.Double.TransformValue, Target.Double.Size, Target.Double.SizeValue,
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
        public override void Setup()
        {
            this.Vector0 = new Target.Double.Size(22, -3, 10);
            this.Vector1 = new Target.Double.Size(12, 13, 20);
            this.Vector2 = new Target.Double.Size(34, 10, 30);
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
                Target.Integer.Size integer = new Target.Integer.Size(10, 20, 30);
                Target.Double.Size @double = integer;
                Verify(@double.Width, Is.EqualTo(10));
                Verify(@double.Height, Is.EqualTo(20));
                Verify(@double.Depth, Is.EqualTo(30));
                Verify((Target.Integer.Size)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.Size single = new Target.Single.Size(10, 20, 30);
                Target.Double.Size @double = single;
                Verify(@double.Width, Is.EqualTo(10));
                Verify(@double.Height, Is.EqualTo(20));
                Verify(@double.Depth, Is.EqualTo(30));
                Verify((Target.Single.Size)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueCasts()
        {
            // integer - double
            {
                Target.Integer.SizeValue integer = new Target.Integer.SizeValue(10, 20, 30);
                Target.Double.SizeValue @double = integer;
                Verify(@double.Width, Is.EqualTo(10));
                Verify(@double.Height, Is.EqualTo(20));
                Verify(@double.Depth, Is.EqualTo(30));
                Verify((Target.Integer.SizeValue)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.SizeValue single = new Target.Single.SizeValue(10, 20,30);
                Target.Double.SizeValue @double = single;
                Verify(@double.Width, Is.EqualTo(10));
                Verify(@double.Height, Is.EqualTo(20));
                Verify(@double.Depth, Is.EqualTo(30));
                Verify((Target.Single.SizeValue)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.SizeValue(10, 20, 30);
            Verify(textFromValue, Is.EqualTo("10, 20, 30"));
            Target.Single.SizeValue @integerFromText = "10 20 30";
            Verify(@integerFromText.Width, Is.EqualTo(10));
            Verify(@integerFromText.Height, Is.EqualTo(20));
            Verify(@integerFromText.Depth, Is.EqualTo(30));
        }
    }
}

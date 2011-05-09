using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry3D;

namespace Kean.Test.Math.Geometry3D.Double
{
    [TestFixture]
    public class Box :
        Kean.Test.Math.Geometry3D.Abstract.Box<Target.Double.Transform, Target.Double.TransformValue, Target.Double.Box, Target.Double.BoxValue, Target.Double.Point, Target.Double.PointValue, Target.Double.Size, Target.Double.SizeValue,
        Kean.Math.Double, double>
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Box0 = new Target.Double.Box(1, 2, 3, 4, 5, 6);
            this.Box1 = new Target.Double.Box(4, 3, 2, 1, 5, 6);
            this.Box2 = new Target.Double.Box(2, 1, 4, 3, 5, 6);
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
                Target.Integer.Box integer = new Target.Integer.Box(10, 20, 30, 40, 50, 60);
                Target.Double.Box @double = integer;
                Assert.That(@double.Left, Is.EqualTo(10));
                Assert.That(@double.Top, Is.EqualTo(20));
                Assert.That(@double.Front, Is.EqualTo(30));
                Assert.That(@double.Width, Is.EqualTo(40));
                Assert.That(@double.Height, Is.EqualTo(50));
                Assert.That(@double.Depth, Is.EqualTo(60));
                Assert.That((Target.Integer.Box)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.Box single = new Target.Single.Box(10, 20, 30, 40, 50, 60);
                Target.Double.Box @double = single;
                Assert.That(@double.Left, Is.EqualTo(10));
                Assert.That(@double.Top, Is.EqualTo(20));
                Assert.That(@double.Front, Is.EqualTo(30));
                Assert.That(@double.Width, Is.EqualTo(40));
                Assert.That(@double.Height, Is.EqualTo(50));
                Assert.That(@double.Depth, Is.EqualTo(60));
                Assert.That((Target.Single.Box)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueCasts()
        {
            // integer - double
            {
                Target.Integer.BoxValue integer = new Target.Integer.BoxValue(10, 20, 30, 40, 50, 60);
                Target.Double.BoxValue @double = integer;
                Assert.That(@double.Left, Is.EqualTo(10));
                Assert.That(@double.Top, Is.EqualTo(20));
                Assert.That(@double.Front, Is.EqualTo(30));
                Assert.That(@double.Width, Is.EqualTo(40));
                Assert.That(@double.Height, Is.EqualTo(50));
                Assert.That(@double.Depth, Is.EqualTo(60));
                Assert.That((Target.Integer.BoxValue)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.BoxValue single = new Target.Single.BoxValue(10, 20, 30, 40, 50, 60);
                Target.Double.BoxValue @double = single;
                Assert.That(@double.Left, Is.EqualTo(10));
                Assert.That(@double.Top, Is.EqualTo(20));
                Assert.That(@double.Front, Is.EqualTo(30));
                Assert.That(@double.Width, Is.EqualTo(40));
                Assert.That(@double.Height, Is.EqualTo(50));
                Assert.That(@double.Depth, Is.EqualTo(60));
                Assert.That((Target.Single.BoxValue)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Double.BoxValue(10, 20, 30, 40, 50, 60);
            Assert.That(textFromValue, Is.EqualTo("10 20 30 40 50 60"));
            Target.Double.BoxValue @doubleFromText = "10 20 30 40 50 60";
            Assert.That(@doubleFromText.Left, Is.EqualTo(10));
            Assert.That(@doubleFromText.Top, Is.EqualTo(20));
            Assert.That(@doubleFromText.Front, Is.EqualTo(30));
            Assert.That(@doubleFromText.Width, Is.EqualTo(40));
            Assert.That(@doubleFromText.Height, Is.EqualTo(50));
            Assert.That(@doubleFromText.Depth, Is.EqualTo(60));
     
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
            Box fixture = new Box();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry3D;

namespace Kean.Test.Math.Geometry3D.Integer
{
    [TestFixture]
    public class Box :
        Kean.Test.Math.Geometry3D.Abstract.Box<Target.Integer.Transform, Target.Integer.TransformValue, Target.Integer.Box, Target.Integer.BoxValue, Target.Integer.Point, Target.Integer.PointValue, Target.Integer.Size, Target.Integer.SizeValue,
        Kean.Math.Integer, int>
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Box0 = new Target.Integer.Box(1, 2, 3, 4, 5, 6);
            this.Box1 = new Target.Integer.Box(4, 3, 2, 1, 5, 6);
            this.Box2 = new Target.Integer.Box(2, 1, 4, 3, 5, 6);
        }
        protected override int Cast(double value)
        {
            return (int)value;
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Integer.BoxValue(10, 20, 30, 40, 50, 60);
            Assert.That(textFromValue, Is.EqualTo("10 20 30 40 50 60"));
            Target.Integer.BoxValue @integerFromText = "10 20 30 40 50 60";
            Assert.That(@integerFromText.Left, Is.EqualTo(10));
            Assert.That(@integerFromText.Top, Is.EqualTo(20));
            Assert.That(@integerFromText.Front, Is.EqualTo(30));
            Assert.That(@integerFromText.Width, Is.EqualTo(40));
            Assert.That(@integerFromText.Height, Is.EqualTo(50));
            Assert.That(@integerFromText.Depth, Is.EqualTo(60));
        }
        public void Run()
        {
            this.Run(
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

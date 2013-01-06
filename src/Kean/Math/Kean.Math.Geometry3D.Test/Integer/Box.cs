using System;
using NUnit.Framework;
using NUnit.Framework;
using Target = Kean.Math.Geometry3D;

namespace Kean.Math.Geometry3D.Test.Integer
{
    [TestFixture]
    public class Box :
        Kean.Math.Geometry3D.Test.Abstract.Box<Box, Target.Integer.Transform, Target.Integer.TransformValue, Target.Integer.Box, Target.Integer.BoxValue, Target.Integer.Point, Target.Integer.PointValue, Target.Integer.Size, Target.Integer.SizeValue,
        Kean.Math.Integer, int>
    {
        [TestFixtureSetUp]
        public override void Setup()
        {
            this.Box0 = new Target.Integer.Box(1, 2, 3, 4, 5, 6);
            this.Box1 = new Target.Integer.Box(4, 3, 2, 1, 5, 6);
            this.Box2 = new Target.Integer.Box(2, 1, 4, 3, 5, 6);
        }
        protected override void Run()
        {
            base.Run();
            this.Run(
                this.ValueStringCasts
                );
        }
        protected override int Cast(double value)
        {
            return (int)value;
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Integer.BoxValue(10, 20, 30, 40, 50, 60);
            Expect(textFromValue, Is.EqualTo("10, 20, 30, 40, 50, 60"));
            Target.Integer.BoxValue @integerFromText = "10 20 30 40 50 60";
            Expect(@integerFromText.Left, Is.EqualTo(10));
            Expect(@integerFromText.Top, Is.EqualTo(20));
            Expect(@integerFromText.Front, Is.EqualTo(30));
            Expect(@integerFromText.Width, Is.EqualTo(40));
            Expect(@integerFromText.Height, Is.EqualTo(50));
            Expect(@integerFromText.Depth, Is.EqualTo(60));
        }
    }
}

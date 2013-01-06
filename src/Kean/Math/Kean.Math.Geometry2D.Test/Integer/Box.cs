using System;
using NUnit.Framework;
using NUnit.Framework;
using Target = Kean.Math.Geometry2D;

namespace Kean.Math.Geometry2D.Test.Integer
{
    [TestFixture]
    public class Box :
		Kean.Math.Geometry2D.Test.Abstract.Box<Box, Target.Integer.Transform, Target.Integer.TransformValue, Target.Integer.Shell, Target.Integer.ShellValue, Target.Integer.Box, Target.Integer.BoxValue, Target.Integer.Point, Target.Integer.PointValue, Target.Integer.Size, Target.Integer.SizeValue, Kean.Math.Integer, int>
    {
        [TestFixtureSetUp]
        public override void Setup()
        {
            base.Setup(); 
            this.Box0 = new Target.Integer.Box(1, 2, 3, 4);
            this.Box1 = new Target.Integer.Box(4, 3, 2, 1);
            this.Box2 = new Target.Integer.Box(2, 1, 4, 3);
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
            string textFromValue = new Target.Integer.BoxValue(10, 20, 30, 40);
            Expect(textFromValue, Is.EqualTo("10, 20, 30, 40"));
            Target.Integer.BoxValue @integerFromText = "10 20 30 40";
            Expect(@integerFromText.Left, Is.EqualTo(10));
            Expect(@integerFromText.Top, Is.EqualTo(20));
            Expect(@integerFromText.Width, Is.EqualTo(30));
            Expect(@integerFromText.Height, Is.EqualTo(40));
        }
    }
}

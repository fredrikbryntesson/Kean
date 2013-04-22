using System;
using NUnit.Framework;

using Target = Kean.Math.Geometry3D;

namespace Kean.Math.Geometry3D.Test.Integer
{
    [TestFixture]
    public class Size :
        Kean.Math.Geometry3D.Test.Abstract.Size<Size, Target.Integer.Transform, Target.Integer.TransformValue, Target.Integer.Size, Target.Integer.SizeValue,
        Kean.Math.Integer, int>
    {
        protected override Target.Integer.Size CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Target.Integer.Size value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public override void Setup()
        {
            this.Vector0 = new Target.Integer.Size(22, -3, 10);
            this.Vector1 = new Target.Integer.Size(12, 13, 20);
            this.Vector2 = new Target.Integer.Size(34, 10, 30);
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
            string textFromValue = new Target.Integer.SizeValue(10, 20, 30);
            Verify(textFromValue, Is.EqualTo("10, 20, 30"));
            Target.Integer.SizeValue @integerFromText = "10 20 30";
            Verify(@integerFromText.Width, Is.EqualTo(10));
            Verify(@integerFromText.Height, Is.EqualTo(20));
            Verify(@integerFromText.Depth, Is.EqualTo(30));
        }
    }
}

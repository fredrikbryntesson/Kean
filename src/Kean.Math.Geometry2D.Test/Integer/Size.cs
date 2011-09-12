using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry2D;

namespace Kean.Math.Geometry2D.Test.Integer
{
    [TestFixture]
    public class Size :
        Kean.Math.Geometry2D.Test.Abstract.Size<Size, Target.Integer.Transform, Target.Integer.TransformValue, Target.Integer.Shell, Target.Integer.ShellValue, Target.Integer.Box, Target.Integer.BoxValue, Target.Integer.Point, Target.Integer.PointValue, Target.Integer.Size, Target.Integer.SizeValue, Kean.Math.Integer, int>
    {
        protected override Kean.Math.Geometry2D.Integer.Size CastFromString(string value)
        {
            return (Kean.Math.Geometry2D.Integer.Size)value;
        }
        protected override string CastToString(Kean.Math.Geometry2D.Integer.Size value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public override void Setup()
        {
            base.Setup();
            this.Vector0 = new Kean.Math.Geometry2D.Integer.Size(22, -3);
            this.Vector1 = new Kean.Math.Geometry2D.Integer.Size(12, 13);
            this.Vector2 = new Kean.Math.Geometry2D.Integer.Size(34, 10);
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
            string textFromValue = new Target.Integer.SizeValue(10, 20);
            Expect(textFromValue, Is.EqualTo("10 20"));
            Target.Integer.SizeValue @integerFromText = "10 20";
            Expect(@integerFromText.Width, Is.EqualTo(10));
            Expect(@integerFromText.Height, Is.EqualTo(20));
        }
    }
}

using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry2D;

namespace Kean.Test.Math.Geometry2D.Integer
{
    [TestFixture]
    public class Size :
        Kean.Test.Math.Geometry2D.Abstract.Size<Kean.Math.Geometry2D.Integer.Transform, Kean.Math.Geometry2D.Integer.TransformValue, Kean.Math.Geometry2D.Integer.Size, Kean.Math.Geometry2D.Integer.SizeValue,
        Kean.Math.Integer, int>
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
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry2D.Integer.Size(22, -3);
            this.Vector1 = new Kean.Math.Geometry2D.Integer.Size(12, 13);
            this.Vector2 = new Kean.Math.Geometry2D.Integer.Size(34, 10);
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
        public void Run()
        {
            this.Run(
                base.Run,
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

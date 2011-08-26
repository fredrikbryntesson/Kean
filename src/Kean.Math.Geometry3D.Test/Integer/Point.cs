using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry3D;

namespace Kean.Math.Geometry3D.Test.Integer
{
    [TestFixture]
    public class Point :
        Kean.Math.Geometry3D.Test.Abstract.Point<Target.Integer.Transform, Target.Integer.TransformValue, Target.Integer.Point, Target.Integer.PointValue, Target.Integer.Size, Target.Integer.SizeValue, Kean.Math.Integer, int>
    {
        protected override Target.Integer.Point CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Target.Integer.Point value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Target.Integer.Point(22, -3, 10);
            this.Vector1 = new Target.Integer.Point(12, 13, 20);
            this.Vector2 = new Target.Integer.Point(34, 10, 30);
        }
        protected override int Cast(double value)
        {
            return (int)value;
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Integer.PointValue(10, 20, 30);
            Expect(textFromValue, Is.EqualTo("10 20 30"));
            Target.Integer.PointValue @integerFromText = "10 20 30";
            Expect(@integerFromText.X, Is.EqualTo(10));
            Expect(@integerFromText.Y, Is.EqualTo(20));
            Expect(@integerFromText.Z, Is.EqualTo(30));
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
            Point fixture = new Point();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

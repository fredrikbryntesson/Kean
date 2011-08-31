using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry2D;

namespace Kean.Math.Geometry2D.Test.Single
{
    [TestFixture]
    public class Box :
		Kean.Math.Geometry2D.Test.Abstract.Box<Target.Single.Transform, Target.Single.TransformValue, Target.Single.Shell, Target.Single.ShellValue, Target.Single.Box, Target.Single.BoxValue, Target.Single.Point, Target.Single.PointValue, Target.Single.Size, Target.Single.SizeValue, Kean.Math.Single, float>
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Box0 = new Target.Single.Box(1, 2, 3, 4);
            this.Box1 = new Target.Single.Box(4, 3, 2, 1);
            this.Box2 = new Target.Single.Box(2, 1, 4, 3);
        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        [Test]
        public void Casts()
        {
            // integer - single
            Target.Integer.Box integer = new Target.Integer.Box(10, 20, 30, 40);
            Target.Single.Box single = integer;
            Expect(single.Left, Is.EqualTo(10));
            Expect(single.Top, Is.EqualTo(20));
            Expect(single.Width, Is.EqualTo(30));
            Expect(single.Height, Is.EqualTo(40));
            Expect((Target.Integer.Box)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueCasts()
        {
            // integer - float
            Target.Integer.BoxValue integer = new Target.Integer.BoxValue(10, 20, 30, 40);
            Target.Single.BoxValue single = integer;
            Expect(single.Left, Is.EqualTo(10));
            Expect(single.Top, Is.EqualTo(20));
            Expect(single.Width, Is.EqualTo(30));
            Expect(single.Height, Is.EqualTo(40));
            Expect((Target.Integer.BoxValue)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.BoxValue(10, 20, 30, 40);
            Expect(textFromValue, Is.EqualTo("10 20 30 40"));
            Target.Single.BoxValue @integerFromText = "10 20 30 40";
            Expect(@integerFromText.Left, Is.EqualTo(10));
            Expect(@integerFromText.Top, Is.EqualTo(20));
            Expect(@integerFromText.Width, Is.EqualTo(30));
            Expect(@integerFromText.Height, Is.EqualTo(40));
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

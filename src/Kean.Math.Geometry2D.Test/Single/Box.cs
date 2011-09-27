using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry2D;

namespace Kean.Math.Geometry2D.Test.Single
{
    [TestFixture]
    public class Box :
		Kean.Math.Geometry2D.Test.Abstract.Box<Box, Target.Single.Transform, Target.Single.TransformValue, Target.Single.Shell, Target.Single.ShellValue, Target.Single.Box, Target.Single.BoxValue, Target.Single.Point, Target.Single.PointValue, Target.Single.Size, Target.Single.SizeValue, Kean.Math.Single, float>
    {
        string prefix = "Kean.Math.Geometry2D.Test.Single.";
        [TestFixtureSetUp]
        public override void  Setup()
        {
            base.Setup(); 
            this.Box0 = new Target.Single.Box(1, 2, 3, 4);
            this.Box1 = new Target.Single.Box(4, 3, 2, 1);
            this.Box2 = new Target.Single.Box(2, 1, 4, 3);
        }
        protected override void Run()
        {
            base.Run();
            this.Run(
                this.Casts,
                this.ValueCasts,
                this.ValueStringCasts,
                this.StringCast
                );
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
            Expect(single.Left, Is.EqualTo(10), this.prefix + "Casts.0");
            Expect(single.Top, Is.EqualTo(20), this.prefix + "Casts.1");
            Expect(single.Width, Is.EqualTo(30), this.prefix + "Casts.2");
            Expect(single.Height, Is.EqualTo(40), this.prefix + "Casts.3");
            Expect((Target.Integer.Box)single, Is.EqualTo(integer), this.prefix + "Casts.4");
        }
        [Test]
        public void ValueCasts()
        {
            // integer - float
            Target.Integer.BoxValue integer = new Target.Integer.BoxValue(10, 20, 30, 40);
            Target.Single.BoxValue single = integer;
            Expect(single.Left, Is.EqualTo(10), this.prefix + "ValueCasts.0");
            Expect(single.Top, Is.EqualTo(20), this.prefix + "ValueCasts.1");
            Expect(single.Width, Is.EqualTo(30), this.prefix + "ValueCasts.2");
            Expect(single.Height, Is.EqualTo(40), this.prefix + "ValueCasts.3");
            Expect((Target.Integer.BoxValue)single, Is.EqualTo(integer), this.prefix + "ValueCasts.4");
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.BoxValue(10, 20, 30, 40);
            Expect(textFromValue, Is.EqualTo("10,20,30,40"), this.prefix + "ValueStringCasts.0");
            Target.Single.BoxValue @integerFromText = "10 20 30 40";
            Expect(@integerFromText.Left, Is.EqualTo(10), this.prefix + "ValueStringCasts.1");
            Expect(@integerFromText.Top, Is.EqualTo(20), this.prefix + "ValueStringCasts.2");
            Expect(@integerFromText.Width, Is.EqualTo(30), this.prefix + "ValueStringCasts.3");
            Expect(@integerFromText.Height, Is.EqualTo(40), this.prefix + "ValueStringCasts.4");
        }
        [Test]
        public void StringCast()
        {
            string values = "0.05, 0.05 0.90, 0.90";
            Target.Single.Box box = values;
            Expect(box, Is.EqualTo(new Target.Single.Box(0.05f, 0.05f, 0.90f, 0.90f)), this.prefix + "StringCast.0"); 
        }
    }
}

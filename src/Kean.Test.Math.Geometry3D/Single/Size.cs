using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry3D;

namespace Kean.Test.Math.Geometry3D.Single
{
    [TestFixture]
    public class Size :
        Kean.Test.Math.Geometry3D.Abstract.Size<Target.Single.Transform, Target.Single.TransformValue, Target.Single.Size, Target.Single.SizeValue,
        Kean.Math.Single, float>
    {
        protected override Target.Single.Size CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Target.Single.Size value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Target.Single.Size(22, -3, 10);
            this.Vector1 = new Target.Single.Size(12, 13, 20);
            this.Vector2 = new Target.Single.Size(34, 10, 30);
        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        [Test]
        public void Casts()
        {
            // integer - single
            Target.Integer.Size integer = new Target.Integer.Size(10, 20, 30);
            Target.Single.Size single = integer;
            Expect(single.Width, Is.EqualTo(10));
            Expect(single.Height, Is.EqualTo(20));
            Expect(single.Depth, Is.EqualTo(30));
            Expect((Target.Integer.Size)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueCasts()
        {
            // integer - single
            Target.Integer.SizeValue integer = new Target.Integer.SizeValue(10, 20, 30);
            Target.Single.SizeValue single = integer;
            Expect(single.Width, Is.EqualTo(10));
            Expect(single.Height, Is.EqualTo(20));
            Expect(single.Depth, Is.EqualTo(30));
            Expect((Target.Integer.SizeValue)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.SizeValue(10, 20, 30);
            Expect(textFromValue, Is.EqualTo("10 20 30"));
            Target.Single.SizeValue @integerFromText = "10 20 30";
            Expect(@integerFromText.Width, Is.EqualTo(10));
            Expect(@integerFromText.Height, Is.EqualTo(20));
            Expect(@integerFromText.Depth, Is.EqualTo(30));
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
            Size fixture = new Size();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

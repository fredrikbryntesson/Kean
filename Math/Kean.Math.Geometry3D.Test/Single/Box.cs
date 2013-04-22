using System;
using NUnit.Framework;

using Target = Kean.Math.Geometry3D;

namespace Kean.Math.Geometry3D.Test.Single
{
    [TestFixture]
    public class Box :
        Kean.Math.Geometry3D.Test.Abstract.Box<Box, Target.Single.Transform, Target.Single.TransformValue, Target.Single.Box, Target.Single.BoxValue, Target.Single.Point, Target.Single.PointValue, Target.Single.Size, Target.Single.SizeValue,
        Kean.Math.Single, float>
    {
        [TestFixtureSetUp]
        public override void Setup()
        {
            this.Box0 = new Target.Single.Box(1, 2, 3, 4, 5, 6);
            this.Box1 = new Target.Single.Box(4, 3, 2, 1, 5, 6);
            this.Box2 = new Target.Single.Box(2, 1, 4, 3, 5, 6);
        }
        protected override void Run()
        {
            base.Run();
            this.Run(
             this.Casts,
             this.ValueCasts,
             this.ValueStringCasts
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
            Target.Integer.Box integer = new Target.Integer.Box(10, 20, 30, 40, 50, 60);
            Target.Single.Box single = integer;
            Verify(single.Left, Is.EqualTo(10));
            Verify(single.Top, Is.EqualTo(20));
            Verify(single.Front, Is.EqualTo(30));
            Verify(single.Width, Is.EqualTo(40));
            Verify(single.Height, Is.EqualTo(50));
            Verify(single.Depth, Is.EqualTo(60));
            Verify((Target.Integer.Box)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueCasts()
        {
            // integer - float
            Target.Integer.BoxValue integer = new Target.Integer.BoxValue(10, 20, 30, 40, 50, 60);
            Target.Single.BoxValue single = integer;
            Verify(single.Left, Is.EqualTo(10));
            Verify(single.Top, Is.EqualTo(20));
            Verify(single.Front, Is.EqualTo(30));
            Verify(single.Width, Is.EqualTo(40));
            Verify(single.Height, Is.EqualTo(50));
            Verify(single.Depth, Is.EqualTo(60));
            Verify((Target.Integer.BoxValue)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.BoxValue(10, 20, 30, 40, 50, 60);
            Verify(textFromValue, Is.EqualTo("10, 20, 30, 40, 50, 60"));
            Target.Single.BoxValue @integerFromText = "10 20 30 40 50 60";
            Verify(@integerFromText.Left, Is.EqualTo(10));
            Verify(@integerFromText.Top, Is.EqualTo(20));
            Verify(@integerFromText.Front, Is.EqualTo(30));
            Verify(@integerFromText.Width, Is.EqualTo(40));
            Verify(@integerFromText.Height, Is.EqualTo(50));
            Verify(@integerFromText.Depth, Is.EqualTo(60));
        }
    }
}

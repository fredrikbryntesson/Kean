using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry3D;

namespace Kean.Math.Geometry3D.Test.Double
{
    [TestFixture]
    public class Box :
        Kean.Math.Geometry3D.Test.Abstract.Box<Box, Target.Double.Transform, Target.Double.TransformValue, Target.Double.Box, Target.Double.BoxValue, Target.Double.Point, Target.Double.PointValue, Target.Double.Size, Target.Double.SizeValue,
        Kean.Math.Double, double>
    {
        [TestFixtureSetUp]
        public override void Setup()
        {
            base.Setup();
            this.Box0 = new Target.Double.Box(1, 2, 3, 4, 5, 6);
            this.Box1 = new Target.Double.Box(4, 3, 2, 1, 5, 6);
            this.Box2 = new Target.Double.Box(2, 1, 4, 3, 5, 6);
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
  
        protected override double Cast(double value)
        {
            return (double)value;
        }
        [Test]
        public void Casts()
        {
            // integer - double
            {
                Target.Integer.Box integer = new Target.Integer.Box(10, 20, 30, 40, 50, 60);
                Target.Double.Box @double = integer;
                Expect(@double.Left, Is.EqualTo(10));
                Expect(@double.Top, Is.EqualTo(20));
                Expect(@double.Front, Is.EqualTo(30));
                Expect(@double.Width, Is.EqualTo(40));
                Expect(@double.Height, Is.EqualTo(50));
                Expect(@double.Depth, Is.EqualTo(60));
                Expect((Target.Integer.Box)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.Box single = new Target.Single.Box(10, 20, 30, 40, 50, 60);
                Target.Double.Box @double = single;
                Expect(@double.Left, Is.EqualTo(10));
                Expect(@double.Top, Is.EqualTo(20));
                Expect(@double.Front, Is.EqualTo(30));
                Expect(@double.Width, Is.EqualTo(40));
                Expect(@double.Height, Is.EqualTo(50));
                Expect(@double.Depth, Is.EqualTo(60));
                Expect((Target.Single.Box)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueCasts()
        {
            // integer - double
            {
                Target.Integer.BoxValue integer = new Target.Integer.BoxValue(10, 20, 30, 40, 50, 60);
                Target.Double.BoxValue @double = integer;
                Expect(@double.Left, Is.EqualTo(10));
                Expect(@double.Top, Is.EqualTo(20));
                Expect(@double.Front, Is.EqualTo(30));
                Expect(@double.Width, Is.EqualTo(40));
                Expect(@double.Height, Is.EqualTo(50));
                Expect(@double.Depth, Is.EqualTo(60));
                Expect((Target.Integer.BoxValue)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.BoxValue single = new Target.Single.BoxValue(10, 20, 30, 40, 50, 60);
                Target.Double.BoxValue @double = single;
                Expect(@double.Left, Is.EqualTo(10));
                Expect(@double.Top, Is.EqualTo(20));
                Expect(@double.Front, Is.EqualTo(30));
                Expect(@double.Width, Is.EqualTo(40));
                Expect(@double.Height, Is.EqualTo(50));
                Expect(@double.Depth, Is.EqualTo(60));
                Expect((Target.Single.BoxValue)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Double.BoxValue(10, 20, 30, 40, 50, 60);
            Expect(textFromValue, Is.EqualTo("10 20 30 40 50 60"));
            Target.Double.BoxValue @doubleFromText = "10 20 30 40 50 60";
            Expect(@doubleFromText.Left, Is.EqualTo(10));
            Expect(@doubleFromText.Top, Is.EqualTo(20));
            Expect(@doubleFromText.Front, Is.EqualTo(30));
            Expect(@doubleFromText.Width, Is.EqualTo(40));
            Expect(@doubleFromText.Height, Is.EqualTo(50));
            Expect(@doubleFromText.Depth, Is.EqualTo(60));
     
        }
    }
}

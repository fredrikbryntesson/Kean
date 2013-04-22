using System;
using NUnit.Framework;

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
				Verify(@double.Left, Is.EqualTo(10));
				Verify(@double.Top, Is.EqualTo(20));
				Verify(@double.Front, Is.EqualTo(30));
				Verify(@double.Width, Is.EqualTo(40));
				Verify(@double.Height, Is.EqualTo(50));
				Verify(@double.Depth, Is.EqualTo(60));
				Verify((Target.Integer.Box)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.Box single = new Target.Single.Box(10, 20, 30, 40, 50, 60);
                Target.Double.Box @double = single;
				Verify(@double.Left, Is.EqualTo(10));
				Verify(@double.Top, Is.EqualTo(20));
				Verify(@double.Front, Is.EqualTo(30));
				Verify(@double.Width, Is.EqualTo(40));
				Verify(@double.Height, Is.EqualTo(50));
				Verify(@double.Depth, Is.EqualTo(60));
				Verify((Target.Single.Box)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueCasts()
        {
            // integer - double
            {
                Target.Integer.BoxValue integer = new Target.Integer.BoxValue(10, 20, 30, 40, 50, 60);
                Target.Double.BoxValue @double = integer;
				Verify(@double.Left, Is.EqualTo(10));
				Verify(@double.Top, Is.EqualTo(20));
				Verify(@double.Front, Is.EqualTo(30));
				Verify(@double.Width, Is.EqualTo(40));
				Verify(@double.Height, Is.EqualTo(50));
				Verify(@double.Depth, Is.EqualTo(60));
				Verify((Target.Integer.BoxValue)@double, Is.EqualTo(integer));
            }
            {
                Target.Single.BoxValue single = new Target.Single.BoxValue(10, 20, 30, 40, 50, 60);
                Target.Double.BoxValue @double = single;
				Verify(@double.Left, Is.EqualTo(10));
				Verify(@double.Top, Is.EqualTo(20));
				Verify(@double.Front, Is.EqualTo(30));
				Verify(@double.Width, Is.EqualTo(40));
				Verify(@double.Height, Is.EqualTo(50));
				Verify(@double.Depth, Is.EqualTo(60));
				Verify((Target.Single.BoxValue)@double, Is.EqualTo(single));
            }
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Double.BoxValue(10, 20, 30, 40, 50, 60);
           Verify(textFromValue, Is.EqualTo("10, 20, 30, 40, 50, 60"));
            Target.Double.BoxValue @doubleFromText = "10 20 30 40 50 60";
			Verify(@doubleFromText.Left, Is.EqualTo(10));
			Verify(@doubleFromText.Top, Is.EqualTo(20));
			Verify(@doubleFromText.Front, Is.EqualTo(30));
			Verify(@doubleFromText.Width, Is.EqualTo(40));
			Verify(@doubleFromText.Height, Is.EqualTo(50));
			Verify(@doubleFromText.Depth, Is.EqualTo(60));
     
        }
    }
}

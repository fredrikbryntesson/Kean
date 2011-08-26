using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Math.Geometry2D;

namespace Kean.Math.Geometry2D.Test.Single
{
    [TestFixture]
    public class Point :
        Kean.Math.Geometry2D.Test.Abstract.Point<Target.Single.Transform, Target.Single.TransformValue, Target.Single.Point, Target.Single.PointValue, Target.Single.Size, Target.Single.SizeValue,
        Kean.Math.Single, float>
    {
        protected override Target.Single.Point CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Target.Single.Point value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Target.Single.Point(22.221f, -3.1f);
            this.Vector1 = new Target.Single.Point(12.221f, 13.1f);
            this.Vector2 = new Target.Single.Point(34.442f, 10.0f);
        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        [Test]
        public void Casts()
        {
            // integer - single
            Target.Integer.Point integer = new Target.Integer.Point(10, 20);
            Target.Single.Point single = integer;
            Expect(single.X, Is.EqualTo(10));
            Expect(single.Y, Is.EqualTo(20));
            Expect((Target.Integer.Point)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueCasts()
        {
            // integer - single
            Target.Integer.PointValue integer = new Target.Integer.PointValue(10, 20);
            Target.Single.PointValue single = integer;
            Expect(single.X, Is.EqualTo(10));
            Expect(single.Y, Is.EqualTo(20));
            Expect((Target.Integer.PointValue)single, Is.EqualTo(integer));
        }
        [Test]
        public void ValueStringCasts()
        {
            string textFromValue = new Target.Single.PointValue(10, 20);
            Expect(textFromValue, Is.EqualTo("10 20"));
            Target.Single.PointValue @integerFromText = "10 20";
            Expect(@integerFromText.X, Is.EqualTo(10));
            Expect(@integerFromText.Y, Is.EqualTo(20));
        }
        [Test]
        public void PerformanceAddition()
        {
            /*
                Add new created class (static method): Elapsed time 12405
                Add new created struct (static method): Elapsed time 173
                Add previously created struct (static method): Elapsed time 132
                Add previously created struct (non-static method, non-reference): Elapsed time 95
                Add previously created struct (non-static method, reference): Elapsed time 92
                Multiply with static method on class: Elapsed time 11675
                Multiply with static method on struct: Elapsed time 123
                Multiply struct (non-static method, non-reference): Elapsed time 68
                Multiply struct (non-static method, reference): Elapsed time 65
            */
            Kean.Math.Geometry2D.Single.PointValue a = new Kean.Math.Geometry2D.Single.PointValue(10, 20);
            Kean.Math.Geometry2D.Single.Point b = new Kean.Math.Geometry2D.Single.Point(10, 20);
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            int n = 1920 * 1080;
            watch.Start();
            for (int i = 0; i < n; i++)
                b += new Kean.Math.Geometry2D.Single.Point(1, 1);
            watch.Stop();
            Console.WriteLine("Add new created class (static method): Elapsed time " + watch.ElapsedMilliseconds);
            watch.Reset();
            a = new Kean.Math.Geometry2D.Single.PointValue(10, 20);
            watch.Start();
            for (int i = 0; i < n; i++)
                a += new Kean.Math.Geometry2D.Single.PointValue(1, 1);
            watch.Stop();
            Console.WriteLine("Add new created struct (static method): Elapsed time " + watch.ElapsedMilliseconds);
            Kean.Math.Geometry2D.Single.PointValue c = new Kean.Math.Geometry2D.Single.PointValue(1, 1);
            watch.Reset();
            a = new Kean.Math.Geometry2D.Single.PointValue(10, 20);
            watch.Start();
            for (int i = 0; i < n; i++)
                a += c;
            watch.Stop();
            Console.WriteLine("Add previously created struct (static method): Elapsed time " + watch.ElapsedMilliseconds);
            watch.Reset();
            a = new Kean.Math.Geometry2D.Single.PointValue(10, 20);
            watch.Start();
            for (int i = 0; i < n; i++)
                a.Add(c);
            watch.Stop();
            Console.WriteLine("Add previously created struct (non-static method, non-reference): Elapsed time " + watch.ElapsedMilliseconds);
            watch.Reset();
            /*a = new Kean.Math.Geometry2D.Single.PointValue(10, 20);
            watch.Start();
            for (int i = 0; i < n; i++)
                a.Add(ref c);
            watch.Stop();
            Console.WriteLine("Add previously created struct (non-static method, reference): Elapsed time " + watch.ElapsedMilliseconds);
             */
        }
        [Test]
        public void PerformanceMultiplication()
        {
            Kean.Math.Geometry2D.Single.PointValue a = new Kean.Math.Geometry2D.Single.PointValue(10, 20);
            Kean.Math.Geometry2D.Single.Point b = new Kean.Math.Geometry2D.Single.Point(10, 20);
            float x = 5;
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            int n = 1920 * 1080;
            watch.Start();
            for (int i = 0; i < n; i++)
                b = b * x;
            watch.Stop();
            Console.WriteLine("Multiply with static method on class: Elapsed time " + watch.ElapsedMilliseconds);
            watch.Reset();
            watch.Start();
            for (int i = 0; i < n; i++)
                a = a * x;
            watch.Stop();
            Console.WriteLine("Multiply with static method on struct: Elapsed time " + watch.ElapsedMilliseconds);
            watch.Reset();
            watch.Start();
            for (int i = 0; i < n; i++)
                a.Multiply(x);
            watch.Stop();
            Console.WriteLine("Multiply struct (non-static method, non-reference): Elapsed time " + watch.ElapsedMilliseconds);
            watch.Reset();
            /*watch.Start();
            for (int i = 0; i < n; i++)
                a.Multiply(ref X);
            watch.Stop();
            Console.WriteLine("Multiply struct (non-static method, reference): Elapsed time " + watch.ElapsedMilliseconds);
            */
        }
        [Test]
        public void PointSize()
        {
            Target.Single.Point a = new Target.Single.Point(10,20) * new Target.Single.Size(2,3);
            Expect(a, Is.EqualTo(new Target.Single.Point(20,60)));
        }
        public void Run()
        {
            this.Run(
                // this.PerformanceAddition,
                // this.PerformanceMultiplication,
               this.PointSize,
                this.Casts,
                this.ValueCasts,
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

using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry2D.Single
{
    [TestFixture]
    public class Point :
        Kean.Test.Math.Geometry2D.Abstract.Point< Kean.Math.Geometry2D.Single.Transform,  Kean.Math.Geometry2D.Single.TransformValue, Kean.Math.Geometry2D.Single.Point, Kean.Math.Geometry2D.Single.PointValue,  Kean.Math.Geometry2D.Single.Size,  Kean.Math.Geometry2D.Single.SizeValue, 
        Kean.Math.Single, float>
    {
        protected override Kean.Math.Geometry2D.Single.Point CastFromString(string value)
        {
            return value;
        }
        protected override string CastToString(Kean.Math.Geometry2D.Single.Point value)
        {
            return value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry2D.Single.Point(22.221f, -3.1f);
            this.Vector1 = new Kean.Math.Geometry2D.Single.Point(12.221f, 13.1f);
            this.Vector2 = new Kean.Math.Geometry2D.Single.Point(34.442f, 10.0f);
        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        [Test]
        public void Add()
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
            a = new Kean.Math.Geometry2D.Single.PointValue(10, 20);
            watch.Start();
            for (int i = 0; i < n; i++)
                a.Add(ref c);
            watch.Stop();
            Console.WriteLine("Add previously created struct (non-static method, reference): Elapsed time " + watch.ElapsedMilliseconds);
        }
        [Test]
        public void Multiply()
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
            watch.Start();
            for (int i = 0; i < n; i++)
                a.Multiply(ref x);
            watch.Stop();
            Console.WriteLine("Multiply struct (non-static method, reference): Elapsed time " + watch.ElapsedMilliseconds);
        }
        public void Run()
        {
            this.Run
                (
                base.Run,
                this.Add,       
                this.Multiply
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

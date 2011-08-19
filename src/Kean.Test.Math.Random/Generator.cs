using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;
using Target = Kean.Math.Random;
using Collection = Kean.Core.Collection;

namespace Kean.Test.Math.Random
{
    public class Generator :
        AssertionHelper
    {
        [Test]
        public void Compare()
        {
            uint seed = (uint)DateTime.Now.Ticks;
            System.Random r = new System.Random((int)seed);
            Target.Integer.Positive positive = new Target.Integer.Positive();
            int n = 100000;
            int[] rArray = new int[n];
            System.Diagnostics.Stopwatch w = new System.Diagnostics.Stopwatch();
            w.Reset();
            w.Start();
            for (int i = 0; i < n; i++)
                rArray[i] = r.Next();
            w.Stop();
            Console.WriteLine("Dotnet Time for loop: " + w.ElapsedMilliseconds);
            w.Reset();
            w.Start();
            int[] gArray = positive.Generate(n);
            w.Stop();
            Console.WriteLine("My Time for loop: " + w.ElapsedMilliseconds);
            w.Reset();
            Target.Integer.Interval interval = new Target.Integer.Interval();
            interval.Ceiling = 100;
            w.Start();
            gArray = interval.Generate(n);
            w.Stop();
            Console.WriteLine("My Time array s: " + w.ElapsedMilliseconds);
        }
        [Test]
        public void ArraysIntegerUnique()
        {
            Target.Integer.Interval interval = new Target.Integer.Interval();
            interval.Floor = 10;
            interval.Ceiling = 500;
            int n = 20;
            int[] values = interval.GenerateUnique(n);
            bool different = true;
            int i, j;
            for (i = 0; i < n; i++)
                for (j = i+1; j < n; j++)
                {
                    different &= values[i] != values[j];
                    break;
                }
            Expect(different, Is.True, "Unique random numbers test failed");
        }
        [Test]
        public void ArraysDouble()
        {
            Target.Double.Interval interval = new Kean.Math.Random.Double.Interval();
            interval.Floor = 400;
            interval.Ceiling = 500;
            int n = 10000;
            double[] values = interval.GenerateUnique(n);
            string valuesString = "";
            for (int i = 0; i < n; i++)
            {
                valuesString += Kean.Math.Double.ToString(values[i]);
                valuesString += i != n - 1 ? ", " : "";
            }
            System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
            file.WriteLine("clear all;");
            file.WriteLine("close all;");
            file.WriteLine("x = [" + valuesString + "];");
            file.WriteLine("plot(x,'b');");
            file.Close();
        }
        [Test]
        public void NormalDouble()
        {
            Target.Double.Normal g = new Target.Double.Normal();
            int n = 50000;
            g.Mean = 10;
            g.Deviation = 2;
            double[] values = g.Generate(n);
            string valuesString = "";
            for (int i = 0; i < n; i++)
            {
                valuesString += Kean.Math.Double.ToString(values[i]);
                valuesString += i != n - 1 ? ", " : "";
            }
            System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
            file.WriteLine("clear all;");
            file.WriteLine("close all;");
            file.WriteLine("x = [" + valuesString + "];");
            file.WriteLine("plot(x,'b');");
            file.Close();
            // estimate
            double mu = 0;
            for (int i = 0; i < values.Length; i++)
                mu += values[i];
            mu /= values.Length;
            double sigma = 0;
            for (int i = 0; i < values.Length; i++)
                sigma += Kean.Math.Double.Squared(values[i] - mu);
            sigma /= values.Length;
            sigma = Kean.Math.Double.SquareRoot(sigma);
            Expect(mu, Is.EqualTo(g.Mean).Within(0.02));
            Expect(sigma, Is.EqualTo(g.Deviation).Within(0.02));
        }
        
        [Test]
        public void NormalDoublePoint()
        {
            Target.Double.Normal g = new Target.Double.Normal();
            int n = 1000;
            double[] x = new double[n];
            double[] y = new double[n];
            g.Mean = 10;
            g.Deviation = 2;
            for (int i = 0; i < n; i++)
            {
                double[] point = g.Generate(2);
                x[i] = point[0];
                y[i] = point[1];
            }
            string xs = "";
            string ys = "";
            for (int i = 0; i < n; i++)
            {
                xs += Kean.Math.Double.ToString(x[i]);
                xs += i != n - 1 ? ", " : "";
                ys += Kean.Math.Double.ToString(y[i]);
                ys += i != n - 1 ? ", " : "";
            }
                System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
                file.WriteLine("clear all;");
                file.WriteLine("close all;");
                file.WriteLine("x = [" + xs + "];");
                file.WriteLine("y = [" + ys + "];");
                file.WriteLine("scatter(x,y,'b');");
                file.Close();
        }
        
        public void Run()
        {
            this.Run(
                this.Compare,
                this.ArraysIntegerUnique,
                this.ArraysDouble,
                this.NormalDouble,
                this.NormalDoublePoint
                );
        }
        internal void Run(params System.Action[] tests)
        {
            foreach (System.Action test in tests)
                if (test.NotNull())
                    test();
        }
        public static void Test()
        {
            Generator fixture = new Generator();
            fixture.Run();
        }

    }
}

using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;
using Target = Kean.Math.Random;

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
            Target.Generator.Uniform g = new Target.Generator.Uniform(seed);
            int n = 10000000;
            int[] rArray = new int[n];
            int[] gArray = new int[n];
            System.Diagnostics.Stopwatch w = new System.Diagnostics.Stopwatch();
            w.Reset();
            w.Start();
            for (int i = 0; i < n; i++)
                rArray[i] = r.Next();
            w.Stop();
            Console.WriteLine("Dotnet Time for loop: " +w.ElapsedMilliseconds);
            w.Reset();
            w.Start();
            for (int i = 0; i < n; i++)
                gArray[i] = g.NextInt();
            w.Stop();
            Console.WriteLine("My Time for loop: " +w.ElapsedMilliseconds);
            w.Reset();
            w.Start();
            gArray = g.NextIntArray(0, 100, n);
            w.Stop();
            Console.WriteLine("My Time array s: " + w.ElapsedMilliseconds);
        }
        [Test]
        public void ArraysInt()
        {
            Target.Generator.Uniform g = new Target.Generator.Uniform();
            int[] values = g.NextIntArray(20, 100);
            values = g.NextIntArray(10, 20, 100);
            values = g.NextDifferentIntArray(20, 5);
            //values = g.NextDifferentInt(20, 100);
        }
        [Test]
        public void ArraysDouble()
        {
            Target.Generator.Uniform g = new Target.Generator.Uniform();
            double[] values = g.NextDoubleArray(10, 20, 100);
        }
        
        [Test]
        public void NormalDouble()
        {
            Target.Generator.Normal g = new Target.Generator.Normal();
            int n = 100000;
            double[] gArray = new double[n];
            double mu0 = 10;
            double sigma0 = 2;
            for (int i = 0; i < n; i++)
                gArray[i] = g.NextDouble(10,2);
            // estimate
            double mu = 0;
            for (int i = 0; i < gArray.Length; i++)
                mu += gArray[i];
            mu /= gArray.Length;
            double sigma = 0;
            for (int i = 0; i < gArray.Length; i++)
                sigma += Kean.Math.Double.Squared(gArray[i] - mu);
            sigma /= gArray.Length;
            sigma = Kean.Math.Double.SquareRoot(sigma);
            Expect(mu, Is.EqualTo(mu0).Within(0.01));
            Expect(sigma, Is.EqualTo(sigma0).Within(0.01));
        }
        [Test]
        public void NormalDoublePoint()
        {
            Target.Generator.Normal g = new Target.Generator.Normal();
            int n = 1000;
            double[] x = new double[n];
            double[] y = new double[n];
            double mu0 = 10;
            double sigma0 = 2;
            for (int i = 0; i < n; i++)
            {
                double[] point = g.NextDoublePoint(10, 2);
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
        }

        public void Run()
        {
            this.Run(
                this.Compare,
                this.ArraysInt,
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

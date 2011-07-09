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
            Target.Generator g = new Target.Generator(seed);
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
            Target.Generator g = new Target.Generator();
            int[] values = g.NextIntArray(20, 100);
            values = g.NextIntArray(10, 20, 100);
            values = g.NextDifferentIntArray(20, 5);
            //values = g.NextDifferentInt(20, 100);
        }
        [Test]
        public void ArraysDouble()
        {
            Target.Generator g = new Target.Generator();
            double[] values = g.NextDoubleArray(10, 20, 100);
        }
        /*
        [Test]
        public void NormalDouble()
        {
            int n = 100;
            double[] gArray = new double[n];
            for (int i = 0; i < n; i++)
                gArray[i] = Target.Generator.NextDoubleArrayNormal();
        }
        */
        public void Run()
        {
            this.Run(
                this.Compare,
                this.ArraysInt,
                this.ArraysDouble
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

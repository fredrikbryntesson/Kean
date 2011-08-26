using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;
using Target = Kean.Math.Regression.Minimization.LevenbergMarquardt;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Core.Collection;
using Matrix = Kean.Math.Matrix;

namespace Kean.Math.Regression.Test.Minimization
{
    public class Single :
        AssertionHelper
    {
        [Test]
        public void LevenbergMarquardt()
        {
            Matrix.Single a = new Matrix.Single(3, 3, new float[] { 3, 2, -1, 2, -2, 0.5f, -1, 4, -1 });
            Matrix.Single b = new Matrix.Single(1, 3, new float[] { 1, -2, 0 });
            Matrix.Single guess = new Matrix.Single(1, 3, new float[] { 1, 1, 1 });
            Matrix.Single result = this.Estimate(a, b, guess);
            Matrix.Single correct = new Matrix.Single(1, 3, new float[] { 1, -2, -2 });
        }
        Matrix.Single Estimate(Matrix.Single a, Matrix.Single b, Matrix.Single guess)
        {
            Func<Matrix.Single, Matrix.Single> function = x => b - a * x;
            Func<Matrix.Single, Matrix.Single> jacobian = x => -a;
            Target.Single lm = new Target.Single(function, jacobian, 200, 1e-8f, 1e-8f, 1e-3f);
            return lm.Estimate(guess);
        }
        [Test]
        public void LevenbergMarquardt2()
        {
            Matrix.Single a = new Matrix.Single(5, 5, new float[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            int n = 15;
            Matrix.Single aa = new Matrix.Single(5, n * 5);
            for (int i = 0; i < n; i++)
                aa = aa.Paste(0, 5 * i, a);
            Matrix.Single y = new Matrix.Single(1, 5, new float[] { -1, 2, -3, 4, 5 });
            Matrix.Single yy = new Matrix.Single(1, n * 5);
            for (int i = 0; i < n; i++)
                yy = yy.Paste(0, 5 * i, y);
            Matrix.Single correct = new Matrix.Single(1, 5, new float[] { -70, 231, -296, 172, -38 });
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Matrix.Single x = aa.Solve(yy);
            watch.Stop();
            Console.WriteLine("Lup solver " + watch.ElapsedMilliseconds + " error " + x.Distance(correct));
            watch.Reset();
            watch.Start();
            Matrix.Single x2 = this.Estimate(aa, yy, new Kean.Math.Matrix.Single(1, 5, new float[] { 1, 1, 1, 1, 1 }));
            watch.Stop();
            Console.WriteLine("Lm solver " + watch.ElapsedMilliseconds + " error " + x2.Distance(correct));
            //Expect(x.Distance(correct), Is.EqualTo(0).Within(0.5f));

        }
        public void Run()
        {
            this.Run(
                this.LevenbergMarquardt,
                this.LevenbergMarquardt2
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
            Single fixture = new Single();
            fixture.Run();
        }

    }
}

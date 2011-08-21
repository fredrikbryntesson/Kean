using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;
using Target = Kean.Math.Matrix.Single;

namespace Kean.Test.Math.Matrix.Algorithms
{
    public class Single :
        AssertionHelper
    {
        string prefix = "Kean.Test.Math.Matrix.Algorithms.Single";
        #region Matrix invariants
        [Test]
        public void DeterminantAndTrace()
        {
            Target a = new Target(1, 1);
            Expect(a.Trace(), Is.EqualTo(0).Within(1e-7f), this.prefix + "DeterminantAndTrace.0");
            Expect(a.Determinant(), Is.EqualTo(0).Within(1e-7f), this.prefix + "DeterminantAndTrace.1");
            a = new Target(5, 5);
            Expect(a.Determinant(), Is.EqualTo(0).Within(1e-7f), this.prefix + "DeterminantAndTrace.2");
            Expect(a.Trace(), Is.EqualTo(0).Within(1e-7f), this.prefix + "DeterminantAndTrace.3");
            a = Target.Identity(5);
            Expect(a.Determinant(), Is.EqualTo(1).Within(1e-7f), this.prefix + "DeterminantAndTrace.4");
            Expect(a.Trace(), Is.EqualTo(5).Within(1e-7f), this.prefix + "DeterminantAndTrace.5");
            a = new Target(5, 5, new float[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            a[2, 2] = 1;
            a[4, 4] = -1;
            Expect(a.Determinant(), Is.EqualTo(3250).Within(1e-7f), this.prefix + "DeterminantAndTrace.6");
            Expect(a.Trace(), Is.EqualTo(23).Within(1e-7f), this.prefix + "DeterminantAndTrace.7");
        }
        #endregion
        #region Matrix inverse
        [Test]
        public void Inverse1()
        {
            Target a = new Target(5, 5);
            Target b = a.Inverse();
        }
        [Test]
        public void Inverse2()
        {
            Target a = new Target(5, 5, new float[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target b = a.Inverse();
            Target correct = new Kean.Math.Matrix.Single(5, 5, new float[] { 5, -10, 10, -5, 1, -10, 30, -35, 19, -4, 10, -35, 46, -27, 6, -5, 19, -27, 17, -4, 1, -4, 6, -4, 1 });
            Expect(b.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "HouseHolder.0");
        }
        #endregion
        #region Least Square Solvers
        // Standard 
        // Square matrix( full rank)
        [Test]
        public void LeastSquare1()
        {
            Target a = new Target(5, 5, new float[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target y = new Target(1, 5, new float[] { -1, 2, -3, 4, 5 });
            Target correct = new Target(1, 5, new float[] { -70, 231, -296, 172, -38 });
            Target x = a.Solve(y);
            Expect(x.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare1.1");
        }
        // Overdetermined. Least square solution
        [Test]
        public void LeastSquare3()
        {
            Target a = new Target(5, 5, new float[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            int n = 15;
            Target aa = new Target(5, n * 5);
            for (int i = 0; i < n; i++)
                aa = aa.Paste(0, 5 * i, a);
            Target y = new Target(1, 5, new float[] { -1, 2, -3, 4, 5 });
            Target yy = new Target(1, n * 5);
            for (int i = 0; i < n; i++)
                yy = yy.Paste(0, 5 * i, y);
            Target correct = new Target(1, 5, new float[] { -70, 231, -296, 172, -38 });
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Target x = aa.Solve(yy);
            watch.Stop();
            long timeLup = watch.ElapsedMilliseconds;
            Expect(x.Distance(correct), Is.EqualTo(0).Within(0.5f), this.prefix + "LeastSquare3.1");
            Console.WriteLine("Time Lup " + timeLup);
            
        }
        [Test]
        public void LeastSquare4()
        {
            Target aa = new Target(4,  10, new float[] {
            -100.8856f,   19.1472f,  -58.8784f,  -59.1866f,  -59.0029f,  -38.9253f,  -19.4673f,   18.4284f,   -1.3259f,    0.5993f,
             -19.1472f, -100.8856f,   59.1866f,  -58.8784f,   38.9253f,  -59.0029f,  -18.4284f,  -19.4673f,   -0.5993f,   -1.3259f,
               1.0000f,         0,    1.0000f,         0,    1.0000f,         0,    1.0000f,         0,    1.0000f,         0,
                     0,    1.0000f,         0,    1.0000f,         0,    1.0000f,         0,    1.0000f,         0,    1.0000f});
            Target yy = new Target(1,  10, new float[] {
                 -196.6532f,   33.3493f, -101.8415f, -118.7157f, -106.0487f,  -81.0908f,  -36.6124f,   46.3590f,    5.7252f,    8.7193f});
            Target correct = new Target(1, 4, new float[] {2.0191f, 0.1504f, 7.8515f, 9.1365f });
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Target x = aa.Solve(yy);
            watch.Stop();
            long timeLup = watch.ElapsedMilliseconds;
            Expect(x.Distance(correct), Is.EqualTo(0).Within(7e-4f), this.prefix + "LeastSquare3.1");
            Console.WriteLine("Regression sample. Error " + x.Distance(correct) + " Time Lup " + timeLup);

        }
        #endregion
        #region Cholesky Factorization
        [Test]
        public void Cholesky1()
        {
            Target a = new Target(5, 5);
            try
            {
                Target b = a.Cholesky();
            }
            catch (Kean.Core.Error.Exception e)
            {
            }
        }
        [Test]
        public void Cholesky2()
        {
            Target a = new Target(5, 5, new float[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target c = a.Cholesky();
            Expect(c * c.Transpose(), Is.EqualTo(a), this.prefix + "Cholesky2.0");
            Target d = c.Transpose();
            Expect(d.Transpose() * d, Is.EqualTo(a), this.prefix + "Cholesky2.1");
        }
        #endregion
    
        public void Run()
        {
            this.Run(
                this.Cholesky1,
                this.Cholesky2,
                this.DeterminantAndTrace,
                this.Inverse1,
                this.Inverse2,
                this.LeastSquare1,
                this.LeastSquare3,
                this.LeastSquare4
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

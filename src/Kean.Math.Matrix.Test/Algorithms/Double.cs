using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;
using Target = Kean.Math.Matrix.Double;

namespace Kean.Math.Matrix.Test.Algorithms
{
    public class Double :
        AssertionHelper
    {
        string prefix = "Kean.Math.Matrix.Test.Algorithms.Double";
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
            a = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
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
            Target a = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target b = a.Inverse();
            Target correct = new Kean.Math.Matrix.Double(5, 5, new double[] {5, -10,10,-5,1,-10,30,-35,19,-4,10,-35,46,-27,6,-5,19,-27,17,-4,1,-4,6,-4,1 });
            Expect(b.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "HouseHolder.0");
        }
        #endregion
        #region QR Factorization
        [Test]
        public void QRFactorization0()
        {
            Target a = new Target(3, 5);
            Target[] qr = a.QRFactorization();
            Expect((qr[0] * qr[1]).Distance(a), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
            Expect(qr[0], Is.EqualTo(Target.Identity(5)), this.prefix + "QRFactorization.0");
            Expect(qr[1], Is.EqualTo(new Target(3, 5)), this.prefix + "QRFactorization.0");
        }
        [Test]
        public void QRFactorization1()
        {
            Target a = new Target(1, 1, new double[] { 12 });
            Target[] qr = a.QRFactorization();
            Expect((qr[0] * qr[1]).Distance(a), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
        }
        [Test]
        public void QRFactorization2()
        {
            Target a = new Target(2, 2, new double[] { 12, 6, -4, -51 });
            Target[] qr = a.QRFactorization();
            Expect((qr[0] * qr[1]).Distance(a), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
        }
        [Test]
        public void QRFactorization3()
        {
            Target a = new Target(3, 3, new double[] { 12, 6, -4, -51, 167, 24, 4, -68, -41 });
            Target[] qr = a.QRFactorization();
            Expect((qr[0] * qr[1]).Distance(a), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
            Expect((qr[0] * qr[0].Transpose()).Distance(Target.Identity(3)), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");

        }
        [Test]
        public void QRFactorization4()
        {
            Kean.Math.Random.Double.Uniform generator = new Kean.Math.Random.Double.Uniform();
            for (int i = 1; i < 20; i += 3)
            {
                Target a = new Target(i, i);
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < i; y++)
                        a[x, y] = generator.Generate();
                Target[] qr = a.QRFactorization();
                Expect((qr[0] * qr[1]).Distance(a), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
                Expect((qr[0] * qr[0].Transpose()).Distance(Target.Identity(i)), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
                for (int x = 0; x < i; x++)
                    for (int y = x + 1; y < i; y++)
                        Expect(qr[1][x, y], Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");

            }
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
            Target a = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target c = a.Cholesky();
            Expect(c * c.Transpose(), Is.EqualTo(a), this.prefix + "Cholesky2.0");
            Target d = c.Transpose();
            Expect(d.Transpose() * d, Is.EqualTo(a), this.prefix + "Cholesky2.1");
        }
        #endregion
        #region Least Square Solvers
        // Standard 
        // Square matrix( full rank)
        [Test]
        public void LeastSquare1()
        {
            Target a = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target y = new Target(1, 5, new double[] { -1, 2, -3, 4, 5 });
            Target correct = new Target(1, 5, new double[] { -70, 231, -296, 172, -38 });
            Target x = a.SolveQr(y);
            Expect(x.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare1.0");
            Target x2 = a.SolveCholesky(y);
            Expect(x2.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare1.1");
            Target x3 = a.SolveLup(y);
            Expect(x3.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare1.2");
            Target x4 = a.SolveSvd(y);
            Expect(x4.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare1.3");
        }
        // Underdetermined
        // Least norm solution
        [Test]
        public void LeastSquare2()
        {
            Target a = new Target(5, 3, new double[] { 1, 1, 1, 1, 2, 3, 1, 3, 6, 1, 4, 10, 1, 5, 15 });
            Target y = new Target(1, 3, new double[] { -1, 2, -3});
            Target correct = new Target(1, 5, new double[] { -70, 231, -296, 172, -38 });
            Target x = a.SolveQr(y);
            Target x2 = a.SolveCholesky(y);
            Expect(x.Distance(x2), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare2.0");
            Target x3 = a.SolveSvd(y);
            Expect(x.Distance(x3), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare2.1");
        }
        // Overdetermined. Least square solution
        [Test]
        public void LeastSquare3()
        {
            Target a = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            int n = 15;
            Target aa = new Target(5, n * 5);
            for(int i = 0; i < n; i++)
                aa = aa.Paste(0, 5 * i, a);
            Target y = new Target(1, 5, new double[] { -1, 2, -3, 4, 5 });
            Target yy = new Target(1, n * 5);
            for (int i = 0; i < n; i++)
                yy = yy.Paste(0, 5 * i, y);
            Target correct = new Target(1, 5, new double[] { -70, 231, -296, 172, -38 });
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Target x = aa.SolveQr(yy);
            watch.Stop();
            long timeQr = watch.ElapsedMilliseconds;
            Expect(x.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare3.0");
            watch.Reset();
            watch.Start();
            Target x2 = aa.SolveCholesky(yy);
            watch.Stop();
            long timeCholesky = watch.ElapsedMilliseconds;
            Expect(x2.Distance(correct), Is.EqualTo(0).Within(1e-6f), this.prefix + "LeastSquare3.1");
            watch.Reset();
            watch.Start();
            Target x3 = aa.SolveLup(yy);
            watch.Stop();
            long timeLup = watch.ElapsedMilliseconds;
            Expect(x3.Distance(correct), Is.EqualTo(0).Within(1e-6f), this.prefix + "LeastSquare3.2");
            watch.Reset();
            watch.Start();
            Target x4 = aa.SolveSvd(yy);
            watch.Stop();
            long timeSvd = watch.ElapsedMilliseconds;
            Expect(x4.Distance(correct), Is.EqualTo(0).Within(1e-6f), this.prefix + "LeastSquare3.3");
            //Console.WriteLine("Time Qr " + timeQr + " Cholesky  " + timeCholesky + " Lup " + timeLup + " Svd " + timeSvd);
            //Console.WriteLine("Error Qr " + x.Distance(correct) + " Cholesky " + x2.Distance(correct) + " Lup " + x3.Distance(correct) + " Svd " + x4.Distance(correct));
        }
        #endregion
        #region Auxilary Methods
        [Test]
        public void HouseHolder()
        {
            Target x = new Target(1, 3, new double[] { 12, 6, -4 });
			Target h = Target.HouseHolder(x, Kean.Math.Double.Sign(x[0, 0]) * x.Norm * Target.Basis(3, 0));
            Target correct = new Target(3, 3, new double[] { 6.0 / 7, 3.0 / 7, -2.0 / 7, 3.0 / 7, -2.0 / 7, 6.0 / 7, -2.0 / 7, 6.0 / 7, 3.0 / 7 });
            Expect(h.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "HouseHolder.0");

        }
        // Bidiagonalization
        [Test]
        public void BiDiagonalization1()
        {
            Target a = new Target(3, 4, new double[] { 1,4,7,10,2,5,8,11,3,6,9,12});
            Target[] ubv = a.BiDiagonalization();
            Target u = ubv[0];
            Target b = ubv[1];
            Target v = ubv[2];
            Expect((u * u.Transpose()).Distance(Target.Identity(u.Dimensions.Width)), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization1.0");
            Expect((u.Transpose() * u).Distance(Target.Identity(u.Dimensions.Width)), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization1.1");
            Expect((v * v.Transpose()).Distance(Target.Identity(v.Dimensions.Width)), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization1.2");
            Expect((v.Transpose() * v).Distance(Target.Identity(v.Dimensions.Width)), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization1.3");
            Expect((u.Transpose() * a * v).Distance(b), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization1.4");
            for (int x = 0; x < b.Dimensions.Width; x++)
                for (int y = 0; y < b.Dimensions.Height; y++)
                    if (y > x || x > y + 1)
                        Expect(b[x, y], Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization1.5");
        }
        [Test]
        public void BiDiagonalization2()
        {
            Target a = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target[] ubv = a.BiDiagonalization();
            Target u = ubv[0];
            Target b = ubv[1];
            Target v = ubv[2];
            Expect((u * u.Transpose()).Distance(Target.Identity(u.Dimensions.Width)), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization2.0");
            Expect((u.Transpose() * u).Distance(Target.Identity(u.Dimensions.Width)), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization2.1");
            Expect((v * v.Transpose()).Distance(Target.Identity(v.Dimensions.Width)), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization2.2");
            Expect((v.Transpose() * v).Distance(Target.Identity(v.Dimensions.Width)), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization2.3");
            Expect((u.Transpose() * a * v).Distance(b), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization2.4");
            for(int x = 0; x < b.Dimensions.Width; x++)
                for(int y = 0; y < b.Dimensions.Height; y++)
                    if(y > x || x > y + 1)
                        Expect(b[x,y], Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization2.5");
        }
        public void BiDiagonalization3()
        {
            Target a = new Target(4, 3, new double[] { 1, 4, 7, 10, 2, 5, 8, 11, 3, 6, 9, 12 });
            Target[] ubv = a.BiDiagonalization();
            Target u = ubv[0];
            Target b = ubv[1];
            Target v = ubv[2];
            Expect((u * u.Transpose()).Distance(Target.Identity(u.Dimensions.Width)), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization3.0");
            Expect((u.Transpose() * u).Distance(Target.Identity(u.Dimensions.Width)), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization3.1");
            Expect((v * v.Transpose()).Distance(Target.Identity(v.Dimensions.Width)), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization3.2");
            Expect((v.Transpose() * v).Distance(Target.Identity(v.Dimensions.Width)), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization3.3");
            Expect((u.Transpose() * a * v).Distance(b), Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization3.4");
            for (int x = 0; x < b.Dimensions.Width; x++)
                for (int y = 0; y < b.Dimensions.Height; y++)
                    if (y > x || x > y + 1)
                        Expect(b[x, y], Is.EqualTo(0).Within(1e-7f), this.prefix + "BiDiagonalization3.5");
        }
        #endregion
        #region Singular Value Decomposition
        [Test]
        public void Svd()
        {
            Target data = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target a = data;
            this.TestSvd(a);
            a = new Target(data.Dimensions.Width, 2 * data.Dimensions.Height);
            a.Set(0, 0, data);
            a.Set(0, data.Dimensions.Height, data);
            this.TestSvd(a);
            a = new Target(2,2);
            this.TestSvd(a);
            a = new Target(1, 1);
            this.TestSvd(a);
            a = Target.Identity(1);
            this.TestSvd(a);
            a = Target.Identity(11);
            this.TestSvd(a);
            a = Target.Identity(10);
            this.TestSvd(a);
            a = new Target(1, 3, new double[] { 1, 2, 3 });
            this.TestSvd(a);
            a = new Target(2 * data.Dimensions.Width, data.Dimensions.Height);
            a.Set(0, 0, data);
            a.Set(data.Dimensions.Width, 0, data);
            this.TestSvd(a);
        }
        void TestSvd(Target a)
        {
            Target[] usv = a.Svd();
            Target u = usv[0];
            Target s = usv[1];
            Target v = usv[2];
            Expect(a.Distance(u * s * v.Transpose()), Is.EqualTo(0).Within(1e-7), this.prefix + "SvdHelper.0");
            for (int x = 0; x < s.Dimensions.Width; x++)
                for (int y = 0; y < s.Dimensions.Height; y++)
                    if (x != y)
                        Expect(s[x, y], Is.EqualTo(0).Within(1e-7f), this.prefix + "SvdHelper.1");
        }
        #endregion
        #region Eigenvalue Decomposition
        [Test]
        public void Eigenvalues()
        {
            Target a = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target[] x = a.Eigenvalues();
            Expect(a.Distance(x[0] * x[1] * x[0].Transpose()), Is.EqualTo(0).Within(1e-7f), this.prefix + "Eigenvalues.1");
        }
        #endregion
        public void Run()
        {
            this.Run(
                this.DeterminantAndTrace,
                this.Inverse1,
                this.Inverse2,
                this.Svd,
                this.BiDiagonalization1,
                this.BiDiagonalization2,
                this.Eigenvalues,
                this.Cholesky1,
                this.Cholesky2,
                this.HouseHolder,
                this.QRFactorization0,
                this.QRFactorization1,
                this.QRFactorization2,
                this.QRFactorization3,
                this.QRFactorization4,
                this.LeastSquare1,
                this.LeastSquare2,
                this.LeastSquare3
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
            Double fixture = new Double();
            fixture.Run();
        }
    }
}

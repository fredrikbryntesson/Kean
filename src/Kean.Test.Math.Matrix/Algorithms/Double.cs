using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;
using Target = Kean.Math.Matrix.Double;

namespace Kean.Test.Math.Matrix.Algorithms
{
    public class Double :
        AssertionHelper
    {
        string prefix = "Kean.Test.Math.Matrix.Algorithms.Double";
        [Test]
        public void HouseHolder()
        {
            Target x = new Target(1, 3, new double[] { 12, 6, -4 });
            Target h = Target.HouseHolder(x, Kean.Math.Double.Sign(x[0, 0]) * x.Norm * Target.Basis(3, 0));
            Target correct = new Target(3, 3, new double[] { 6.0 / 7, 3.0 / 7, -2.0 / 7, 3.0 / 7, -2.0 / 7, 6.0 / 7, -2.0 / 7, 6.0 / 7, 3.0 / 7 });
            Expect(h.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "HouseHolder.0");

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
            Random generator = new Random((int)DateTime.Now.Ticks);
            for (int i = 1; i < 20; i += 3)
            {
                Target a = new Target(i, i);
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < i; y++)
                        a[x, y] = generator.NextDouble();
                Target[] qr = a.QRFactorization();
                Expect((qr[0] * qr[1]).Distance(a), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
                Expect((qr[0] * qr[0].Transpose()).Distance(Target.Identity(i)), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
                for (int x = 0; x < i; x++)
                    for (int y = x + 1; y < i; y++)
                        Expect(qr[1][x, y], Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");

            }
        }
        [Test]
        public void Cholesky()
        {
            Target a = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target c = a.Cholesky();
            Expect(c * c.Transpose(), Is.EqualTo(a), this.prefix + "Cholesky.0");
            Target d = c.Transpose();
            Expect(d.Transpose() * d, Is.EqualTo(a), this.prefix + "Cholesky.1");
        }
        // Standard 
        // Square matrix( full rank)
        [Test]
        public void LeastSquare1()
        {
            Target a = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target y = new Target(1, 5, new double[] { -1, 2, -3, 4, 5 });
            Target correct = new Target(1, 5, new double[] { -70, 231, -296, 172, -38 });
            Target x = a.LeastSquareQr(y);
            Expect(x.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare1.0");
            Target x2 = a.LeastSquareCholesky(y);
            Expect(x2.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare1.1");
        }
        // Underdetermined
        // Least norm solution
        [Test]
        public void LeastSquare2()
        {
            Target a = new Target(5, 3, new double[] { 1, 1, 1, 1, 2, 3, 1, 3, 6, 1, 4, 10, 1, 5, 15 });
            Target y = new Target(1, 3, new double[] { -1, 2, -3});
            Target correct = new Target(1, 5, new double[] { -70, 231, -296, 172, -38 });
            Target x = a.LeastSquareQr(y);
            Target x2 = a.LeastSquareCholesky(y);
            Expect(x.Distance(x2), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare2.0");
        }
        // Overdetermined. Least square solution
        [Test]
        public void LeastSquare3()
        {
            Target a = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target aa = new Target(5, 10);
            aa = aa.Paste(new Kean.Math.Geometry2D.Integer.Point(0, 0), a);
            aa = aa.Paste(new Kean.Math.Geometry2D.Integer.Point(0, 5), a);
            Target y = new Target(1, 5, new double[] { -1, 2, -3, 4, 5 });
            Target yy = new Target(1, 10);
            yy = yy.Paste(new Kean.Math.Geometry2D.Integer.Point(0, 0), y);
            yy = yy.Paste(new Kean.Math.Geometry2D.Integer.Point(0, 5), y);
            Target correct = new Target(1, 5, new double[] { -70, 231, -296, 172, -38 });
            Target x = aa.LeastSquareQr(yy);
            Expect(x.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare3.0");
            Target x2 = aa.LeastSquareCholesky(yy);
            Expect(x2.Distance(correct), Is.EqualTo(0).Within(1e-6f), this.prefix + "LeastSquare3.1");
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
        [Test]
        public void Svd()
        {
            Target a = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target[] ubv = a.Svd();
        }
        [Test]
        public void Eigenvalues()
        {
            Target a = new Target(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target[] x = a.Eigenvalues();
            Expect(a.Distance(x[0] * x[1] * x[0].Transpose()), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare3.1");
      
        }
        public void Run()
        {
            this.Run(
            //    this.Svd,
                this.BiDiagonalization1,
                this.BiDiagonalization2,
                this.Eigenvalues,
                this.Cholesky,
                this.HouseHolder,
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

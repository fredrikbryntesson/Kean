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
        public void Cholesky()
        {
            Target a = new Kean.Math.Matrix.Double(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target c = a.Cholesky();
            Expect(c * c.Transpose(), Is.EqualTo(a), this.prefix + "Cholesky.0");
            Target d = c.Transpose();
            Expect(d.Transpose() * d, Is.EqualTo(a), this.prefix + "Cholesky.1");
        }
        [Test]
        public void LeastSquare1()
        {
            Target a = new Kean.Math.Matrix.Double(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target y = new Kean.Math.Matrix.Double(1, 5, new double[] { -1, 2, -3, 4, 5 });
            Target correct = new Kean.Math.Matrix.Double(1, 5, new double[] { -70, 231, -296, 172, -38 });
            Target x = a.LeastSquare(y);
            Expect(x.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare1.0");
        }
        [Test]
        public void LeastSquare2()
        {
            Target a = new Kean.Math.Matrix.Double(5, 5, new double[] { 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 1, 3, 6, 10, 15, 1, 4, 10, 20, 35, 1, 5, 15, 35, 70 });
            Target aa = new Kean.Math.Matrix.Double(5, 10);
            aa = aa.Paste(new Kean.Math.Geometry2D.Integer.Point(0, 0), a);
            aa = aa.Paste(new Kean.Math.Geometry2D.Integer.Point(0, 5), a);
            Target y = new Kean.Math.Matrix.Double(1, 5, new double[] { -1, 2, -3, 4, 5 });
            Target yy = new Kean.Math.Matrix.Double(1, 10);
            yy = yy.Paste(new Kean.Math.Geometry2D.Integer.Point(0, 0), y);
            yy = yy.Paste(new Kean.Math.Geometry2D.Integer.Point(0, 5), y);
            Target correct = new Kean.Math.Matrix.Double(1, 5, new double[] { -70, 231, -296, 172, -38 });
            Target x = aa.LeastSquare(yy);
            Expect(x.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "LeastSquare2.0");
        }
        [Test]
        public void HouseHolder()
        {
            Target x = new Target(1, 3, new double[] { 12, 6, -4 });
            Target h = Target.HouseHolder(x);
            Target correct = new Target(3, 3, new double[] { 6.0 / 7, 3.0 / 7, -2.0 / 7, 3.0 / 7, -2.0 / 7, 6.0 / 7, -2.0 / 7, 6.0 / 7, 3.0 / 7 });
            Expect(h.Distance(correct), Is.EqualTo(0).Within(1e-7f), this.prefix + "HouseHolder.0");
                
        }
        [Test]
        public void QRFactorization1()
        {
            Target a = new Kean.Math.Matrix.Double(1, 1, new double[] { 12 });
            Target[] qr = a.QRFactorization();
            Expect((qr[0] * qr[1]).Distance(a), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
        }
        [Test]
        public void QRFactorization2()
        {
            Target a = new Kean.Math.Matrix.Double(2, 2, new double[] { 12, 6, -4, -51});
            Target[] qr = a.QRFactorization();
            Expect((qr[0] * qr[1]).Distance(a), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
        }
        [Test]
        public void QRFactorization3()
        {
            Target a = new Kean.Math.Matrix.Double(3, 3, new double[] { 12, 6, -4, -51, 167, 24, 4, -68, -41 });
            Target[] qr = a.QRFactorization();
            Expect((qr[0] * qr[1]).Distance(a), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
            Expect((qr[0] * qr[0].Transpose()).Distance(Target.Identity(3)), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
            
        }
        [Test]
        public void QRFactorization4()
        {
            Random generator = new Random((int)DateTime.Now.Ticks);
            for (int i = 1; i < 50; i++)
            {
                Target a = new Kean.Math.Matrix.Double(i, i);
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < i; y++)
                        a[x, y] = generator.NextDouble();
                Target[] qr = a.QRFactorization();
                Expect((qr[0] * qr[1]).Distance(a), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
                Expect((qr[0] * qr[0].Transpose()).Distance(Target.Identity(i)), Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
                for(int x = 0; x < i ; x++)
                    for(int y = x + 1; y < i; y++)
                        Expect(qr[1][x, y], Is.EqualTo(0).Within(1e-9f), this.prefix + "QRFactorization.0");
                
            }
        }
        public void Run()
        {
            this.Run(
                this.Cholesky,
                this.LeastSquare1,
                this.LeastSquare2,
                this.HouseHolder,
                this.QRFactorization1,
                this.QRFactorization2,
                this.QRFactorization3,
                this.QRFactorization4
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

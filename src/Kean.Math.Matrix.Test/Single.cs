using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;
using Target = Kean.Math.Matrix.Single;

namespace Kean.Math.Matrix.Test
{
    public class Single :
        Kean.Test.Fixture<Single>
    {
        protected override void Run()
        {
            this.Run(
                this.Constructor1,
                this.Constructor2,
                this.Equality,
                this.Adjoint,
                this.All,
                this.Basis,
                this.Block,
                this.Copy,
                this.Diagonal,
                this.Extract,
                this.Identity,
                this.KroneckerProduct,
                this.Minor,
                this.Paste,
                this.Set,
                this.Transpose
                );
        }
        string prefix = "Kean.Math.Matrix.Test.Single.";
        #region Constructors
        [Test]
        public void Constructor1()
        {
            Target a = new Target(3, 5);
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 5; y++)
                    Expect(a[x, y], Is.EqualTo(0), this.prefix + "Constructor1.0");
            Expect(a.Dimensions.Width, Is.EqualTo(3), this.prefix + "Constructor1.1");
            Expect(a.Dimensions.Height, Is.EqualTo(5), this.prefix + "Constructor1.2");
            Expect(a.Order, Is.EqualTo(3), this.prefix + "Constructor1.3");
            Expect(a.IsSquare, Is.False, this.prefix + "Constructor1.4");
        }
        [Test]
        public void Constructor2()
        {
            float[] values = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
            Target a = new Target(3, 5, values);
            int k = 1;
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 5; y++)
                    Expect(a[x, y], Is.EqualTo(k++), this.prefix + "Constructor2.0");
        }
        #endregion
        #region Equality
        [Test]
        public void Equality()
        {
            float[] values = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
            Target a = new Target(3, 5, values);
            float[] values2 = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
            Target b = new Target(3, 5, values2);
            Expect(a, Is.EqualTo(b), this.prefix + "Equality.0");
            b[2, 2] = -1;
            Expect(a, Is.Not.EqualTo(b), this.prefix + "Equality.1");
            float[] values3 = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
            Target c = new Target(1, 5, values3);
            Expect(a, Is.Not.EqualTo(c), this.prefix + "Equality.2");
        }
        #endregion
        #region Static Constructors
        [Test]
        public void Identity()
        {
            Target a = Target.Identity(5);
            for (int x = 0; x < 5; x++)
                for (int y = 0; y < 5; y++)
                    Expect(a[x, y], Is.EqualTo(x == y ? 1 : 0));
            Expect(a.IsDiagonal(0), Is.True, this.prefix + "Identity.0");
        }
        [Test]
        public void Basis()
        {
            Target a = Target.Basis(10, 2);
            for (int y = 0; y < 10; y++)
                Expect(a[0, y], Is.EqualTo(y == 2 ? 1 : 0), this.prefix + "Basis.0");
        }
        [Test]
        public void Diagonal()
        {
            Target a = new Target(2, 2, new float[] { 1, 2, 3, 4 });
            Target b = new Target(3, 3, new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Target c = Target.Diagonal(a, b);
            Target d = new Target(5, 5, new float[] { 1, 2, 0, 0, 0, 3, 4, 0, 0, 0, 0, 0, 1, 2, 3, 0, 0, 4, 5, 6, 0, 0, 7, 8, 9 });
            Expect(c, Is.EqualTo(d), this.prefix + "Diagonal.0");
        }
        [Test]
        public void Block()
        {
            Target a00 = new Target(2, 2, new float[] { 1, 2, 3, 4 });
            Target a01 = new Target(2, 3, new float[] { 1, 2, 3, 4 });
            Target a11 = new Target(3, 3, new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Target a10 = new Target(3, 2, new float[] { 1, 2, 3, 4 });
            Target[,] blocks = new Target[2, 2];
            blocks[0, 0] = a00;
            blocks[0, 1] = a01;
            blocks[1, 0] = a10;
            blocks[1, 1] = a11;
            Target c = Target.Block(blocks);
            Target d = new Target(5, 5);
            d.Set(0, 0, a00);
            d.Set(0, 2, a01);
            d.Set(2, 0, a10);
            d.Set(2, 2, a11);
            Expect(c, Is.EqualTo(d), this.prefix + "Block.0");
        }
        #endregion
        #region Artithmetics
        [Test]
        public void All()
        {
            Target a = new Target(2, 3, new float[] { 1, 2, 3, 4, 5, 6 });
            Target b = new Target(2, 3, new float[] { 2, 4, 6, 8, 10, 12 });
            Target c = new Target(2, 3, new float[] { -1, -2, -3, -4, -5, -6 });
            Target d = new Target(2, 3);
            Expect(a + a, Is.EqualTo(b), this.prefix + "All.0");
            Expect(-a, Is.EqualTo(c), this.prefix + "All.1");
            Expect(a - a, Is.EqualTo(d), this.prefix + "All.2");
            Expect(a * -1, Is.EqualTo(c), this.prefix + "All.3");
            Expect((-1) * a, Is.EqualTo(c), this.prefix + "All.4");
            Expect(a / -1, Is.EqualTo(c), this.prefix + "All.5");
        }
        [Test]
        public void MatrixMatrixMultiplication()
        {
            Target a = new Target(2, 3, new float[] { 1, 2, 3, 4, 5, 6 });
            Target b = new Target(2, 2, new float[] { 1, 2, 3, 4 });
            Target c = new Target(2, 3, new float[] { 9, 12, 15, 19, 26, 33 });
            Expect(a * b, Is.EqualTo(c), this.prefix + "MatrixMatrixMultiplication.0");
        }
        [Test]
        public void KroneckerProduct()
        {
            Target a = new Target(2, 3, new float[] { 1, 2, 3, 4, 5, 6 });
            Target b = new Target(2, 2, new float[] { 1, 2, 3, 4 });
            Target c = new Target(4, 6, new float[] { 1, 2, 2, 4, 3, 6, 3, 4, 6, 8, 9, 12, 4, 8, 5, 10, 6, 12, 12, 16, 15, 20, 18, 24 });
            Expect(a.Kronecker(b), Is.EqualTo(c), this.prefix + "KroneckerProduct.0");
        }
        #endregion
        #region Matrix operator
        [Test]
        public void Copy()
        {
            float[] values = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
            Target a = new Target(3, 5, values);
            Expect(a, Is.EqualTo(a.Copy()), this.prefix + "Copy.0");
        }
        [Test]
        public void Adjoint()
        {
            Target a = new Target(3, 3, new float[] { -3, -1, 3, 2, 0, -4, -5, -2, 1 });
            Target b = new Target(3, 3, new float[] { -8, -5, 4, 18, 12, -6, -4, -1, 2 });
            Expect((a.Adjoint() - b).Norm, Is.LessThan(1e-6), this.prefix + "Adjoint.0");
        }
        [Test]
        public void Transpose()
        {
            Target a = new Target(2, 3, new float[] { 1, 2, 3, 4, 5, 6 });
            Target b = new Target(3, 2, new float[] { 1, 4, 2, 5, 3, 6 });
            Expect(a.Transpose(), Is.EqualTo(b), this.prefix + "Transpose.0");
        }
        [Test]
        public void Minor()
        {
            Target a = new Target(2, 3, new float[] { 1, 2, 3, 4, 5, 6 });
            Target b = new Target(1, 2, new float[] { 5, 6 });
            Expect(a.Minor(0, 0), Is.EqualTo(b), this.prefix + "Minor.0");
        }
        [Test]
        public void Extract()
        {
            Target a = new Target(2, 3, new float[] { 1, 2, 3, 4, 5, 6 });
            Target b = new Target(2, 2, new float[] { 1, 2, 4, 5 });
            Expect(a.Extract(0, 2, 0, 2), Is.EqualTo(b), this.prefix + "Extract.0");
        }
        [Test]
        public void Paste()
        {
            Target a = new Target(2, 3).Paste(0, 0, new Target(2, 2, new float[] { 1, 2, 4, 5 }));
            Target b = new Target(2, 3, new float[] { 1, 2, 0, 4, 5, 0 });
            Expect(a, Is.EqualTo(b), this.prefix + "Paste.0");
        }
        [Test]
        public void Set()
        {
            Target a = new Target(2, 3);
            a.Set(0, 0, new Target(2, 2, new float[] { 1, 2, 4, 5 }));
            Target b = new Target(2, 3, new float[] { 1, 2, 0, 4, 5, 0 });
            Expect(a, Is.EqualTo(b), this.prefix + "Set.0");
        }
        #endregion
    }
}

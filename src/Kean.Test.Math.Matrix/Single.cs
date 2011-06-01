using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;
using Target = Kean.Math.Matrix.Single;
namespace Kean.Test.Math.Matrix
{
    public class Single :
        AssertionHelper
    {

        Target ZeroOrderThree { get; set; }
        Target TwoTwo { get; set; }
        Target ThreeThree { get; set; }
        Target TwoThree { get; set; }
        Target OneThree { get; set; }
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.ZeroOrderThree = new Target(3);
            this.TwoTwo = new Target(2);
            this.TwoTwo[0, 0] = 1;
            this.TwoTwo[1, 0] = 2;
            this.TwoTwo[0, 1] = 3;
            this.TwoTwo[1, 1] = 4;
            this.TwoThree = new Target(2, 3, new float[] { -1, -2, -3, -4, -5, -6, -7 });
            this.OneThree = new Target(1, 3, new float[] { -1, -2, -3, -4, -5, -6, -7 });
            this.ThreeThree = new Target(3, 3, new float[] { -1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }
        #region Constructors
        [Test]
        public void ConstructorsOrderTwo()
        {
            Expect(this.TwoTwo[0, 0], Is.EqualTo(1));
            Expect(this.TwoTwo[1, 0], Is.EqualTo(2));
            Expect(this.TwoTwo[0, 1], Is.EqualTo(3));
            Expect(this.TwoTwo[1, 1], Is.EqualTo(4));
        }
        [Test]
        public void ConstructorTwoThree()
        {
            Expect(this.TwoThree[0, 0], Is.EqualTo(-1));
            Expect(this.TwoThree[0, 1], Is.EqualTo(-2));
            Expect(this.TwoThree[0, 2], Is.EqualTo(-3));
            Expect(this.TwoThree[1, 0], Is.EqualTo(-4));
            Expect(this.TwoThree[1, 1], Is.EqualTo(-5));
            Expect(this.TwoThree[1, 2], Is.EqualTo(-6));
        }
        [Test]
        public void ConstructorColumn()
        {
            Expect(this.TwoThree[0, 0], Is.EqualTo(-1));
            Expect(this.TwoThree[0, 1], Is.EqualTo(-2));
            Expect(this.TwoThree[0, 2], Is.EqualTo(-3));
        }
        #endregion
        #region Special matrices
        [Test]
        public void Identity()
        {
            Target a = Target.Identity(10);
            for (int x = 0; x < a.Dimensions.Width; x++)
                for (int y = 0; y < a.Dimensions.Height; y++)
                    Expect(a[x, y], Is.EqualTo(x == y ? 1 : 0));
        }
        #endregion
        #region Equality
        [Test]
        public void Equality()
        {
            Target m = null;
            Expect(this.TwoThree.Equals(this.OneThree), Is.False);
            Expect(this.TwoThree == this.TwoThree, Is.True);
            Expect(this.TwoThree == this.OneThree, Is.False);
            Expect(this.TwoThree != m, Is.True);
            Expect(m != this.OneThree, Is.True);
        }
        [Test]
        public void HashCodeNotEqual()
        {
            Expect(this.TwoThree.GetHashCode(), Is.Not.EqualTo(this.TwoTwo.GetHashCode()));
        }
        #endregion
        [Test]
        public void Elements()
        {
            // Insert
            this.ZeroOrderThree[1, 1] = -2;
            Expect(this.ZeroOrderThree[1, 1], Is.EqualTo(-2.0f));
        }
        [Test]
        public void Transpose()
        {
            Target result = this.TwoThree.Transpose();
            Expect(result[0, 0], Is.EqualTo(-1));
            Expect(result[1, 0], Is.EqualTo(-2));
            Expect(result[2, 0], Is.EqualTo(-3));
            Expect(result[0, 1], Is.EqualTo(-4));
            Expect(result[1, 1], Is.EqualTo(-5));
            Expect(result[2, 1], Is.EqualTo(-6));
        }
        [Test]
        [ExpectedException(typeof(Kean.Math.Matrix.Exception.InvalidDimensions))]
        public void AdditionInvalidDimensions()
        {
            Target result = this.TwoTwo + this.TwoThree;
        }
        [Test]
        public void Addition()
        {
            Target result = this.TwoTwo + this.TwoTwo;
            Expect(result[0, 0], Is.EqualTo(2));
            Expect(result[1, 0], Is.EqualTo(4));
            Expect(result[0, 1], Is.EqualTo(6));
            Expect(result[1, 1], Is.EqualTo(8));
        }
        [ExpectedException(typeof(Kean.Math.Matrix.Exception.InvalidDimensions))]
        public void SubtractionInvalidDimensions()
        {
            Target result = this.TwoTwo - this.TwoThree;
        }
        [Test]
        public void Subtraction()
        {
            Target result = -this.TwoTwo - this.TwoTwo;
            Expect(result[0, 0], Is.EqualTo(-2));
            Expect(result[1, 0], Is.EqualTo(-4));
            Expect(result[0, 1], Is.EqualTo(-6));
            Expect(result[1, 1], Is.EqualTo(-8));
        }
        [Test]
        public void ScalarMultiplication()
        {
            Target result = 2 * this.TwoTwo;
            Expect(result[0, 0], Is.EqualTo(2));
            Expect(result[1, 0], Is.EqualTo(4));
            Expect(result[0, 1], Is.EqualTo(6));
            Expect(result[1, 1], Is.EqualTo(8));
        }
        [Test]
        public void ScalarDivision()
        {
            Target result = this.TwoTwo / 2;
            Expect(result[0, 0], Is.EqualTo(1 / 2.0f));
            Expect(result[1, 0], Is.EqualTo(2 / 2.0f));
            Expect(result[0, 1], Is.EqualTo(3 / 2.0f));
            Expect(result[1, 1], Is.EqualTo(4 / 2.0f));
        }
        [Test]
        [ExpectedException(typeof(Kean.Math.Matrix.Exception.DivisionByZero))]
        public void ScalarDivisionDivideByZero()
        {
            Target result = this.TwoThree / 0;
        }
        [Test]
        public void MatrixMultiplication()
        {
            Target result = this.TwoThree * this.TwoTwo;
            Expect(result[0, 0], Is.EqualTo(-13));
            Expect(result[0, 1], Is.EqualTo(-17));
            Expect(result[0, 2], Is.EqualTo(-21));
            Expect(result[1, 0], Is.EqualTo(-18));
            Expect(result[1, 1], Is.EqualTo(-24));
            Expect(result[1, 2], Is.EqualTo(-30));

        }
        [Test]
        [ExpectedException(typeof(Kean.Math.Matrix.Exception.InvalidDimensions))]
        public void MatrixMultiplicationInvalidDimensions()
        {
            Target result = this.TwoThree * this.TwoThree;
        }
        [Test]
        public void Cast()
        {
            float[] values = (float[])(this.TwoThree as Target);
            Expect(values.Length, Is.EqualTo(this.TwoThree.Dimensions.Area));
            Expect(values[0], Is.EqualTo(-1));
            Expect(values[1], Is.EqualTo(-2));
            Expect(values[2], Is.EqualTo(-3));
            Expect(values[3], Is.EqualTo(-4));
            Expect(values[4], Is.EqualTo(-5));
            Expect(values[5], Is.EqualTo(-6));
        }
        [Test]
        public void Trace()
        {
            Expect(this.TwoThree.Trace, Is.EqualTo(-6));
        }
        [Test]
        public void DeterminantOrder1()
        {
            Expect((this.OneThree.Transpose() * this.OneThree).Determinant, Is.EqualTo(14));
        }
        [Test]
        public void DeterminantOrder2()
        {
            Expect(this.TwoTwo.Determinant, Is.EqualTo(-2));
        }
        [Test]
        public void DeterminantOrder3()
        {
            Expect(this.ThreeThree.Determinant, Is.EqualTo(6));
        }
        [Test]
        public void DeterminantIdentity()
        {
            Expect(Target.Identity(4).Determinant, Is.EqualTo(1));
        }
        [Test]
        public void InverseOrder3()
        {
            Expect(this.ThreeThree.Inverse()[0, 0], Is.EqualTo(-0.5));
            Expect(this.ThreeThree.Inverse()[0, 1], Is.EqualTo(1));
            Expect(this.ThreeThree.Inverse()[0, 2], Is.EqualTo(-0.5));
            Expect(this.ThreeThree.Inverse()[1, 0], Is.EqualTo(1));
            Expect(this.ThreeThree.Inverse()[1, 1], Is.EqualTo(-5));
            Expect(this.ThreeThree.Inverse()[1, 2], Is.EqualTo(3));
            Expect(this.ThreeThree.Inverse()[2, 0], Is.EqualTo(-0.5));
            Expect(this.ThreeThree.Inverse()[2, 1], Is.EqualTo(3.66666675f));
            Expect(this.ThreeThree.Inverse()[2, 2], Is.EqualTo(-2.16666675f));
        }
        [Test]
        public void Minor()
        {
            Expect((this.OneThree.Transpose() * this.OneThree).Minor(0, 0).Dimensions.Width, Is.EqualTo(0));
            Expect((this.OneThree.Transpose() * this.OneThree).Minor(0, 0).Dimensions.Height, Is.EqualTo(0));
            Expect(this.TwoThree.Minor(1, 1)[0, 0], Is.EqualTo(-1));
            Expect(this.TwoThree.Minor(1, 1)[0, 1], Is.EqualTo(-3));
        }
        [Test]
        public void AdjointOrder1()
        {
            Expect((this.OneThree.Transpose() * this.OneThree).Adjoint()[0, 0], Is.EqualTo(1));
        }
        [Test]
        public void AdjointOrder2()
        {
            Expect(this.TwoTwo.Adjoint()[0, 0], Is.EqualTo(4));
            Expect(this.TwoTwo.Adjoint()[0, 1], Is.EqualTo(-3));
            Expect(this.TwoTwo.Adjoint()[1, 0], Is.EqualTo(-2));
            Expect(this.TwoTwo.Adjoint()[1, 1], Is.EqualTo(1));
        }
        public void Run()
        {
            this.Run(
                this.ConstructorsOrderTwo,
                this.ConstructorTwoThree,
                this.ConstructorColumn,
                this.Identity,
                this.Elements,
                this.Addition,
                this.AdditionInvalidDimensions,
                this.Equality,
                this.HashCodeNotEqual,
                this.MatrixMultiplicationInvalidDimensions,
                this.MatrixMultiplication,
                this.ScalarDivisionDivideByZero,
                this.ScalarMultiplication,
                this.Subtraction,
                this.SubtractionInvalidDimensions,
                this.Transpose,
                this.Cast,
                this.Trace,
                this.Minor,
                this.DeterminantOrder1,
                this.DeterminantOrder2,
                this.DeterminantOrder3,
                this.DeterminantIdentity,
                this.InverseOrder3,
                this.AdjointOrder1,
                this.AdjointOrder2
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
            fixture.SetupFixture();
            fixture.Run();
        }
    }
}

using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;

namespace Kean.Test.Math.Matrix
{
    public abstract class Abstract<MatrixType, R, V>
        where MatrixType : Kean.Math.Matrix.Abstract<MatrixType, R, V>, new()
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        public MatrixType ZeroOrderThree { get; set; }
        public MatrixType OrderTwo { get; set; }
        public MatrixType TwoThree { get; set; }
        public MatrixType OneThree { get; set; }
        #region Constructors
        [Test]
        public void ConstructorsOrderTwo()
        {
            Assert.That(this.OrderTwo[0, 0], Is.EqualTo(1));
            Assert.That(this.OrderTwo[1, 0], Is.EqualTo(2));
            Assert.That(this.OrderTwo[0, 1], Is.EqualTo(3));
            Assert.That(this.OrderTwo[1, 1], Is.EqualTo(4));
        }
        [Test]
        public void ConstructorTwoThree()
        {
            Assert.That(this.TwoThree[0, 0], Is.EqualTo(-1));
            Assert.That(this.TwoThree[0, 1], Is.EqualTo(-2));
            Assert.That(this.TwoThree[0, 2], Is.EqualTo(-3));
            Assert.That(this.TwoThree[1, 0], Is.EqualTo(-4));
            Assert.That(this.TwoThree[1, 1], Is.EqualTo(-5));
            Assert.That(this.TwoThree[1, 2], Is.EqualTo(-6));
        }
        [Test]
        public void ConstructorColumn()
        {
            Assert.That(this.TwoThree[0, 0], Is.EqualTo(-1));
            Assert.That(this.TwoThree[0, 1], Is.EqualTo(-2));
            Assert.That(this.TwoThree[0, 2], Is.EqualTo(-3));
        }
        #endregion
        #region Special matrices
        [Test]
        public void Identity()
        {
            MatrixType a = Kean.Math.Matrix.Abstract<MatrixType, R, V>.Identity(10);
            for(int x  = 0; x < a.Dimensions.Width; x++)
                  for(int y  = 0; y < a.Dimensions.Height; y++)
                      Assert.That(a[x,y], Is.EqualTo( x== y ? 1 : 0));
        }
        #endregion
        #region Equality
        [Test]
        public void Equality()
        {
            MatrixType m = null;
            Assert.That(this.TwoThree.Equals(this.OneThree), Is.False);
            Assert.That(this.TwoThree == this.TwoThree, Is.True);
            Assert.That(this.TwoThree.Equals(this.TwoThree.Copy() as object), Is.True);
            Assert.That(this.TwoThree == this.TwoThree.Copy(), Is.True);
            Assert.That(this.TwoThree == this.OneThree, Is.False);
            Assert.That(this.TwoThree != m, Is.True);
            Assert.That(m != this.OneThree, Is.True);
            m = this.TwoThree.Copy();
            m[1, 1] += new R().One;
            Assert.That(this.TwoThree != m, Is.True);
        }
        [Test]
        public void HashCodeEqual()
        {
            Assert.That(this.TwoThree.GetHashCode(), Is.EqualTo(this.TwoThree.Copy().GetHashCode()));
        }
        [Test]
        public void HashCodeNotEqual()
        {
            Assert.That(this.TwoThree.GetHashCode(), Is.Not.EqualTo(this.OrderTwo.Copy().GetHashCode()));
        }
        #endregion
        [Test]
        public void Elements()
        {
            // Insert
            this.ZeroOrderThree[1, 1] = -new R().Two;
            Assert.That(this.ZeroOrderThree[1, 1], Is.EqualTo(-2.0f));
        }
        [Test]
        public void Copy()
        {
            Assert.That(this.OrderTwo.Copy(), Is.Not.SameAs(this.OrderTwo));
        }
        [Test]
        public void Transpose()
        {
            MatrixType result = this.TwoThree.Transpose();
            Assert.That(result[0, 0], Is.EqualTo(-1));
            Assert.That(result[1, 0], Is.EqualTo(-2));
            Assert.That(result[2, 0], Is.EqualTo(-3));
            Assert.That(result[0, 1], Is.EqualTo(-4));
            Assert.That(result[1, 1], Is.EqualTo(-5));
            Assert.That(result[2, 1], Is.EqualTo(-6));
        }
        [Test]
        [ExpectedException(typeof(Kean.Math.Matrix.Exception.InvalidDimensions))]
        public void AdditionInvalidDimensions()
        {
            MatrixType result = this.OrderTwo + this.TwoThree;
        }
        [Test]
        public void Addition()
        {
            MatrixType result = this.OrderTwo + this.OrderTwo;
            Assert.That(result[0, 0], Is.EqualTo(2));
            Assert.That(result[1, 0], Is.EqualTo(4));
            Assert.That(result[0, 1], Is.EqualTo(6));
            Assert.That(result[1, 1], Is.EqualTo(8));
        }
        [ExpectedException(typeof(Kean.Math.Matrix.Exception.InvalidDimensions))]
        public void SubtractionInvalidDimensions()
        {
            MatrixType result = this.OrderTwo - this.TwoThree;
        }
        [Test]
        public void Subtraction()
        {
            MatrixType result = -this.OrderTwo - this.OrderTwo;
            Assert.That(result[0, 0], Is.EqualTo(-2));
            Assert.That(result[1, 0], Is.EqualTo(-4));
            Assert.That(result[0, 1], Is.EqualTo(-6));
            Assert.That(result[1, 1], Is.EqualTo(-8));
        }
        [Test]
        public void ScalarMultiplication()
        {
            MatrixType result = new R().Two * this.OrderTwo;
            Assert.That(result[0, 0], Is.EqualTo(2));
            Assert.That(result[1, 0], Is.EqualTo(4));
            Assert.That(result[0, 1], Is.EqualTo(6));
            Assert.That(result[1, 1], Is.EqualTo(8));
        }
        [Test]
        public void ScalarDivision()
        {
            MatrixType result = this.OrderTwo / new R().Two;
            Assert.That(result[0, 0], Is.EqualTo(1 / 2.0f));
            Assert.That(result[1, 0], Is.EqualTo(2 / 2.0f));
            Assert.That(result[0, 1], Is.EqualTo(3 / 2.0f));
            Assert.That(result[1, 1], Is.EqualTo(4 / 2.0f));
        }
        [Test]
        [ExpectedException(typeof(Kean.Math.Matrix.Exception.DivisionByZero))]
        public void ScalarDivisionDivideByZero()
        {
            MatrixType result = this.TwoThree / new R();
        }
        [Test]
        public void MatrixMultiplication()
        {
            MatrixType result = this.TwoThree * this.OrderTwo;
            Assert.That(result[0, 0], Is.EqualTo(-13));
            Assert.That(result[0, 1], Is.EqualTo(-17));
            Assert.That(result[0, 2], Is.EqualTo(-21));
            Assert.That(result[1, 0], Is.EqualTo(-18));
            Assert.That(result[1, 1], Is.EqualTo(-24));
            Assert.That(result[1, 2], Is.EqualTo(-30));
           
        }
        [Test]
        [ExpectedException(typeof(Kean.Math.Matrix.Exception.InvalidDimensions))]
        public void MatrixMultiplicationInvalidDimensions()
        {
            MatrixType result = this.TwoThree * this.TwoThree;
        }
        [Test]
        public void Cast()
        {
            V[] values = this.TwoThree;
            Assert.That(values.Length, Is.EqualTo(this.TwoThree.Dimensions.Area));
            Assert.That(values[0], Is.EqualTo(-1));
            Assert.That(values[1], Is.EqualTo(-2));
            Assert.That(values[2], Is.EqualTo(-3));
            Assert.That(values[3], Is.EqualTo(-4));
            Assert.That(values[4], Is.EqualTo(-5));
            Assert.That(values[5], Is.EqualTo(-6));
        }
        public void Run()
        {
            this.Run(
                this.ConstructorsOrderTwo,
                this.ConstructorTwoThree,
                this.ConstructorColumn,
                this.Identity,
                this.Copy,
                this.Elements,
                this.Addition, 
                this.AdditionInvalidDimensions,
                this.Equality,
                this.HashCodeEqual,
                this.HashCodeNotEqual,
                this.MatrixMultiplicationInvalidDimensions,
                this.MatrixMultiplication,
                this.ScalarDivisionDivideByZero,
                this.ScalarMultiplication,
                this.Subtraction,
                this.SubtractionInvalidDimensions,
                this.Transpose,
                this.Cast
            );
        }
        internal void Run(params System.Action[] tests)
        {
            foreach (System.Action test in tests)
                if (test.NotNull())
                    test();
        }
    }
}

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
        public  MatrixType ZeroOrderThree { get; set; }
        public MatrixType OrderTwo { get; set; }
        [Test]
        public void Constructors()
        {
            Assert.That(this.OrderTwo[0, 0], Is.EqualTo(1));
            Assert.That(this.OrderTwo[1, 0], Is.EqualTo(2));
            Assert.That(this.OrderTwo[0, 1], Is.EqualTo(3));
            Assert.That(this.OrderTwo[1, 1], Is.EqualTo(4));
        }
        [Test]
        public void Copy()
        {
            MatrixType r = this.OrderTwo.Copy();
            Assert.That(r, Is.Not.SameAs(this.OrderTwo)); 
        }
        [Test]
        public void Identity()
        {
            MatrixType a = Kean.Math.Matrix.Abstract<MatrixType, R, V>.Identity(10);
            for(int x  = 0; x < a.Dimension.Width; x++)
                  for(int y  = 0; y < a.Dimension.Height; y++)
                      Assert.That(a[x,y], Is.EqualTo( x== y ? 1 : 0));
        }
        [Test]
        public void Elements()
        {
            // Insert
            this.ZeroOrderThree[1, 1] = -new R().Two;
            Assert.That(this.ZeroOrderThree[1, 1], Is.EqualTo(-2.0f)); 
        }
        public void Run()
        {
            this.Run(
                this.Constructors,
                this.Identity,
                this.Copy,
                this.Elements
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

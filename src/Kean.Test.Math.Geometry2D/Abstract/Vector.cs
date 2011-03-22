using System;
using Kean.Core.Basis.Extension;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Test.Math.Geometry2D.Abstract
{
    public abstract class Vector<VectorType, R, V>
        where VectorType : Kean.Math.Geometry2D.Abstract.Vector<VectorType, R, V>, new()
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        protected virtual V Precision { get { return Kean.Math.Abstract<R, V>.Precision.Value; } }
        protected abstract V Cast(double value);

        protected VectorType Vector0 { get; set; }
        protected VectorType Vector1 { get; set; }
        protected VectorType Vector2 { get; set; }
        #region Equality
        [Test]
        public void Equality()
        {
            VectorType point = null;
            Assert.That(this.Vector0, Is.EqualTo(this.Vector0));
            Assert.That(this.Vector0.Copy(), Is.EqualTo(this.Vector0));
            Assert.That(this.Vector0.Copy().Equals(this.Vector0), Is.True);
            Assert.That(this.Vector0.Equals(this.Vector0 as object), Is.True);
            Assert.That(this.Vector0 == this.Vector0, Is.True);
            Assert.That(this.Vector0 != this.Vector1, Is.True);
            Assert.That(this.Vector0 == point, Is.False);
            Assert.That(point == point, Is.True);
            Assert.That(point == this.Vector0, Is.False);
        }
        #endregion
        #region Arithmetic
        [Test]
        public void Addition()
        {
            Assert.That(this.Vector0 + this.Vector1, Is.EqualTo(this.Vector2));
        }
        [Test]
        public void Subtraction()
        {
            Assert.That(this.Vector0 - this.Vector0, Is.EqualTo(new VectorType()));
        }
        [Test]
        public void ScalarMultitplication()
        {
            Assert.That((-Kean.Math.Abstract<R, V>.One) * this.Vector0, Is.EqualTo(-this.Vector0));
        }
        #endregion

        internal void Run(params System.Action[] tests)
        {
            foreach (System.Action test in tests)
                if (test.NotNull())
                    test();
        }

    }
}

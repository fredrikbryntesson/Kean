using System;
using Kean.Core.Basis.Extension;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Test.Math.Geometry2D.Abstract
{
    public abstract class Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>
        where BoxType : Kean.Math.Geometry2D.Abstract.Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where BoxValue : struct, Kean.Math.Geometry2D.Abstract.IBox<PointValue, SizeValue,V>
        where TransformType : Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, new()
        where TransformValue : struct, Kean.Math.Geometry2D.Abstract.ITransform<V>
        where PointType : Kean.Math.Geometry2D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointValue : struct, Kean.Math.Geometry2D.Abstract.IPoint<V>, Kean.Math.Geometry2D.Abstract.IVector<V>
        where SizeType : Kean.Math.Geometry2D.Abstract.Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, new()
        where SizeValue : struct, Kean.Math.Geometry2D.Abstract.ISize<V>, Kean.Math.Geometry2D.Abstract.IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        protected float Precision { get { return 1e-4f; } }
        protected abstract V Cast(double value);

        protected BoxType Box0 { get; set; }
        protected BoxType Box1 { get; set; }
        protected BoxType Box2 { get; set; }
        #region Equality
        [Test]
        public void Equality()
        {
            BoxType box = null;
            Assert.That(this.Box0, Is.EqualTo(this.Box0));
            Assert.That(this.Box0.Equals(this.Box0 as object), Is.True);
            Assert.That(this.Box0 == this.Box0, Is.True);
            Assert.That(this.Box0 != this.Box1, Is.True);
            Assert.That(this.Box0 == box, Is.False);
            Assert.That(box == box, Is.True);
            Assert.That(box == this.Box0, Is.False);
        }
        #endregion
        [Test]
        public void LeftTop()
        {
            PointType leftTop = this.Box0.LeftTop;
            Assert.That(leftTop.X, Is.EqualTo(1));
            Assert.That(leftTop.Y, Is.EqualTo(2));
        }
        [Test]
        public void Size()
        {
            SizeType size = this.Box0.Size;
            Assert.That(size.Width, Is.EqualTo(3));
            Assert.That(size.Height, Is.EqualTo(4));
        }
        #region Arithmetic
        [Test]
        public void Addition()
        {
        }
        [Test]
        public void Subtraction()
        {
        }
        [Test]
        public void ScalarMultitplication()
        {
        }
        #endregion
        public void Run()
        {
            this.Run(this.LeftTop, this.Size);
        }
        internal void Run(params System.Action[] tests)
        {
            foreach (System.Action test in tests)
                if (test.NotNull())
                    test();
        }

    }
}

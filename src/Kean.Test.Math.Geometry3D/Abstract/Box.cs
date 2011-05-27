using System;
using Kean.Core.Basis.Extension;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Test.Math.Geometry3D.Abstract
{
    public abstract class Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> : 
        AssertionHelper
        where BoxType : Kean.Math.Geometry3D.Abstract.Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where BoxValue : struct, Kean.Math.Geometry3D.Abstract.IBox<PointValue, SizeValue, V>
        where TransformType : Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, new()
        where TransformValue : struct, Kean.Math.Geometry3D.Abstract.ITransform<V>
        where PointType : Kean.Math.Geometry3D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointValue : struct, Kean.Math.Geometry3D.Abstract.IPoint<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
        where SizeType : Kean.Math.Geometry3D.Abstract.Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, new()
        where SizeValue : struct, Kean.Math.Geometry3D.Abstract.ISize<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
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
            Expect(this.Box0, Is.EqualTo(this.Box0));
            Expect(this.Box0.Equals(this.Box0 as object), Is.True);
            Expect(this.Box0 == this.Box0, Is.True);
            Expect(this.Box0 != this.Box1, Is.True);
            Expect(this.Box0 == box, Is.False);
            Expect(box == box, Is.True);
            Expect(box == this.Box0, Is.False);
        }
        #endregion
        [Test]
        public void LeftTop()
        {
            PointType leftTop = this.Box0.LeftTopFront;
            Expect(leftTop.X, Is.EqualTo(1));
            Expect(leftTop.Y, Is.EqualTo(2));
            Expect(leftTop.Z, Is.EqualTo(3));
        }
        [Test]
        public void Size()
        {
            SizeType size = this.Box0.Size;
            Expect(size.Width, Is.EqualTo(4));
            Expect(size.Height, Is.EqualTo(5));
            Expect(size.Depth, Is.EqualTo(6));
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

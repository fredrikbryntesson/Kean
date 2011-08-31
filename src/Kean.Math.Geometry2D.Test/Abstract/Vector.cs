using System;
using Kean.Core.Extension;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Math.Geometry2D.Test.Abstract
{
	public abstract class Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
		AssertionHelper
		where VectorType : Geometry2D.Abstract.Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, Geometry2D.Abstract.IVector<V>, new()
		where VectorValue : struct, Geometry2D.Abstract.IVector<V>
		where TransformType : Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, Geometry2D.Abstract.ITransform<V>, new()
		where TransformValue : struct, Geometry2D.Abstract.ITransform<V>
		where ShellType : Geometry2D.Abstract.Shell<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, Geometry2D.Abstract.IShell<V>, new()
		where ShellValue : struct, Geometry2D.Abstract.IShell<V>
		where BoxType : Geometry2D.Abstract.Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, Geometry2D.Abstract.IBox<PointValue, SizeValue, V>, new()
		where BoxValue : struct, Geometry2D.Abstract.IBox<PointValue, SizeValue, V>
		where PointType : Geometry2D.Abstract.Point<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, Geometry2D.Abstract.IPoint<V>, new()
		where PointValue : struct, Geometry2D.Abstract.IPoint<V>, Geometry2D.Abstract.IVector<V>
		where SizeType : Geometry2D.Abstract.Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, Geometry2D.Abstract.ISize<V>, new()
		where SizeValue : struct, Geometry2D.Abstract.ISize<V>, Geometry2D.Abstract.IVector<V>
		where R : Kean.Math.Abstract<R, V>, new()
		where V : struct
	{
        protected float Precision { get { return 1e-4f; } }
        protected abstract V Cast(double value);
        protected abstract string CastToString(VectorType value);
        protected abstract VectorType CastFromString(string value);
      
        protected VectorType Vector0 { get; set; }
        protected VectorType Vector1 { get; set; }
        protected VectorType Vector2 { get; set; }
		protected VectorType Vector3 { get { return Geometry2D.Abstract.Vector<VectorType, VectorValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(new R().CreateConstant(10), new R().CreateConstant(20)); } }
        #region Equality
        [Test]
        public void Equality()
        {
            VectorType point = null;
            Expect(this.Vector0, Is.EqualTo(this.Vector0));
            Expect(this.Vector0.Equals(this.Vector0 as object), Is.True);
            Expect(this.Vector0 == this.Vector0, Is.True);
            Expect(this.Vector0 != this.Vector1, Is.True);
            Expect(this.Vector0 == point, Is.False);
            Expect(point == point, Is.True);
            Expect(point == this.Vector0, Is.False);
        }
        #endregion
        #region Arithmetic
        [Test]
        public void Addition()
        {
            Expect((this.Vector0 + this.Vector1).Value.X, Is.EqualTo(this.Vector2.Value.X).Within(this.Precision));
            Expect((this.Vector0 + this.Vector1).Value.Y, Is.EqualTo(this.Vector2.Value.Y).Within(this.Precision));
        }
        [Test]
        public void Subtraction()
        {
            Expect(this.Vector0 - this.Vector0, Is.EqualTo(new VectorType()));
        }
        [Test]
        public void ScalarMultitplication()
        {
            Expect((-Kean.Math.Abstract<R, V>.One) * this.Vector0, Is.EqualTo(-this.Vector0));
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

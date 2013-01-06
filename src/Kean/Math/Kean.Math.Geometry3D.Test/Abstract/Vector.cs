using System;
using Kean.Core.Extension;
using NUnit.Framework;
using NUnit.Framework;

namespace Kean.Math.Geometry3D.Test.Abstract
{
    public abstract class Vector<T, TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue, R, V> :
        Kean.Test.Fixture<T>
        where T : Kean.Test.Fixture<T>, new()
        where VectorType : Kean.Math.Geometry3D.Abstract.Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType,SizeValue, R, V>, new()
        where VectorValue : struct, Kean.Math.Geometry3D.Abstract.IVector<V>
        where TransformType : Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, Kean.Math.Geometry3D.Abstract.ITransform<V>, new()
        where TransformValue : struct, Kean.Math.Geometry3D.Abstract.ITransform<V>
        where SizeType : Kean.Math.Geometry3D.Abstract.Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, Kean.Math.Geometry3D.Abstract.IVector<V>, new()
        where SizeValue : struct, Kean.Math.Geometry3D.Abstract.ISize<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
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
        protected VectorType Vector3 { get { return Kean.Math.Geometry3D.Abstract.Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue, R, V>.Create(new R().CreateConstant(10), new R().CreateConstant(20), new R().CreateConstant(30)); } }
       
        #region Equality
        [Test]
        public void Equality()
        {
            VectorType point = null;
            Expect(this.Vector0, Is.EqualTo(this.Vector0));
            Expect(this.Vector0, Is.EqualTo(this.Vector0));
            Expect(this.Vector0.Equals(this.Vector0), Is.True);
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
            Expect((this.Vector0 + this.Vector1).Value.Z, Is.EqualTo(this.Vector2.Value.Z).Within(this.Precision));
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
        #region Hash Code
        [Test]
        public void Hash()
        {
            Expect(this.Vector0.Hash(), Is.Not.EqualTo(0));
        }
        #endregion
    }
}

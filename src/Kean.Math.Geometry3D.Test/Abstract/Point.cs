using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;

namespace Kean.Math.Geometry3D.Test.Abstract
{
    public abstract class Point<T, TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V> :
        Vector<T, TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>
        where T : Kean.Test.Fixture<T>, new()
        where PointType : Kean.Math.Geometry3D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointValue : struct, Kean.Math.Geometry3D.Abstract.IPoint<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
        where TransformType : Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, Kean.Math.Geometry3D.Abstract.ITransform<V>, new()
        where TransformValue : struct, Kean.Math.Geometry3D.Abstract.ITransform<V>
        where SizeType : Kean.Math.Geometry3D.Abstract.Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, Kean.Math.Geometry3D.Abstract.IVector<V>, new()
        where SizeValue : struct, Kean.Math.Geometry3D.Abstract.ISize<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        protected override void Run()
        {
            this.Run(
                this.Equality,
                this.Addition,
                this.Subtraction,
                this.ScalarMultitplication,
                this.GetValues,
                this.ScalarProduct,
                this.CrossProduct,
                this.Casting,
                this.CastingNull
                );
        }
        [Test]
        public void GetValues()
        {
            Expect(this.Vector0.X, Is.EqualTo(this.Cast(22)).Within(this.Precision));
            Expect(this.Vector0.Y, Is.EqualTo(this.Cast(-3)).Within(this.Precision));
            Expect(this.Vector0.Z, Is.EqualTo(this.Cast(10)).Within(this.Precision));
        }
        [Test]
        public void ScalarProduct()
        {
            Expect(this.Vector0.ScalarProduct(new PointType()).Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Expect(this.Vector0.ScalarProduct(this.Vector1).Value, Is.EqualTo(this.Cast(425)).Within(this.Precision));
        }
        [Test]
        public void CrossProduct()
        {
            Expect(this.Vector0.VectorProduct(this.Vector1), Is.EqualTo(-this.Vector1.VectorProduct(this.Vector0)));
            Expect((this.Vector0.VectorProduct(this.Vector1)).X, Is.EqualTo(this.Cast(-190)).Within(this.Precision));
            Expect((this.Vector0.VectorProduct(this.Vector1)).Y, Is.EqualTo(this.Cast(-320)).Within(this.Precision));
            Expect((this.Vector0.VectorProduct(this.Vector1)).Z, Is.EqualTo(this.Cast(322)).Within(this.Precision));
        }
        [Test]
        public void Casting()
        {
            string value = "10,20,30";
            Expect(this.CastToString(this.Vector3), Is.EqualTo(value));
            Expect(this.CastFromString(value), Is.EqualTo(this.Vector3));
        }
        [Test]
        public void CastingNull()
        {
            string value = null;
            PointType point = null;
            Expect(this.CastToString(point), Is.EqualTo(value));
            Expect(this.CastFromString(value), Is.EqualTo(point));
        }
    }
}

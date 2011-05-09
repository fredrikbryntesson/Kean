using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;

namespace Kean.Test.Math.Geometry3D.Abstract
{
    public abstract class Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V> :
        Vector<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>
        where PointType : Kean.Math.Geometry3D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointValue : struct, Kean.Math.Geometry3D.Abstract.IPoint<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
        where TransformType : Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, Kean.Math.Geometry3D.Abstract.ITransform<V>, new()
        where TransformValue : struct, Kean.Math.Geometry3D.Abstract.ITransform<V>
        where SizeType : Kean.Math.Geometry3D.Abstract.Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, Kean.Math.Geometry3D.Abstract.IVector<V>, new()
        where SizeValue : struct, Kean.Math.Geometry3D.Abstract.ISize<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        [Test]
        public void GetValues()
        {
            Assert.That(this.Vector0.X, Is.EqualTo(this.Cast(22)).Within(this.Precision));
            Assert.That(this.Vector0.Y, Is.EqualTo(this.Cast(-3)).Within(this.Precision));
            Assert.That(this.Vector0.Z, Is.EqualTo(this.Cast(10)).Within(this.Precision));
        }
        [Test]
        public void ScalarProduct()
        {
            Assert.That(this.Vector0.ScalarProduct(new PointType()).Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(this.Vector0.ScalarProduct(this.Vector1).Value, Is.EqualTo(this.Cast(425)).Within(this.Precision));
        }
        [Test]
        public void CrossProduct()
        {
            Assert.That(this.Vector0 * this.Vector1, Is.EqualTo(-this.Vector1 * this.Vector0));
            Assert.That((this.Vector0 * this.Vector1).X, Is.EqualTo(this.Cast(-190)).Within(this.Precision));
            Assert.That((this.Vector0 * this.Vector1).Y, Is.EqualTo(this.Cast(-320)).Within(this.Precision));
            Assert.That((this.Vector0 * this.Vector1).Z, Is.EqualTo(this.Cast(322)).Within(this.Precision));
        }
        [Test]
        public void Casting()
        {
            string value = "10 20 30";
            Assert.That(this.CastToString(this.Vector3), Is.EqualTo(value));
            Assert.That(this.CastFromString(value), Is.EqualTo(this.Vector3));
        }
        [Test]
        public void CastingNull()
        {
            string value = null;
            PointType point = null;
            Assert.That(this.CastToString(point), Is.EqualTo(value));
            Assert.That(this.CastFromString(value), Is.EqualTo(point));
        }
        public void Run()
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
    }
}

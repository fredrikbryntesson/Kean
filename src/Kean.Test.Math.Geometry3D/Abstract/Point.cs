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
            Assert.That(this.Vector0.X.Value, Is.EqualTo(this.Cast(22.221)).Within(this.Precision));
            Assert.That(this.Vector0.Y.Value, Is.EqualTo(this.Cast(-3.1)).Within(this.Precision));
            Assert.That(this.Vector0.Z.Value, Is.EqualTo(this.Cast(10)).Within(this.Precision));
        }
        [Test]
        public void ScalarProduct()
        {
            Assert.That(this.Vector0.ScalarProduct(new PointType()).Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(this.Vector0.ScalarProduct(this.Vector1).Value, Is.EqualTo(this.Cast(430.95282)).Within(this.Precision));
        }
        [Test]
        public void CrossProduct()
        {
            Assert.That(this.Vector0 * this.Vector1, Is.EqualTo(-this.Vector1 * this.Vector0));
            Assert.That((this.Vector0 * this.Vector1).X.Value, Is.EqualTo(this.Cast(-193)).Within(this.Precision));
            Assert.That((this.Vector0 * this.Vector1).Y.Value, Is.EqualTo(this.Cast(-322.210022)).Within(this.Precision));
            Assert.That((this.Vector0 * this.Vector1).Z.Value, Is.EqualTo(this.Cast(328.980225)).Within(this.Precision));
        }
        [Test]
        public void Norm()
        {
            Assert.That(this.Vector0.Norm.Value, Is.EqualTo(this.Cast(24.56385)).Within(this.Precision));
            Assert.That(this.Vector0.Norm.Squared().Value, Is.EqualTo(this.Vector0.ScalarProduct(this.Vector0).Value).Within(this.Precision));
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
                this.Norm,
                this.CrossProduct

                );
        }
    }
}

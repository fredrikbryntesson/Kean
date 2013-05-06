using System;
using NUnit.Framework;

using Kean.Core.Extension;

namespace Kean.Math.Geometry3D.Test.Abstract
{
    public abstract class Size<T, TransformType, Transform, SizeType, Size, R, V> :
        Vector<T, TransformType, Transform, SizeType, Size, SizeType, Size, R, V>
        where T : Kean.Test.Fixture<T>, new()
        where TransformType : Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>, Kean.Math.Geometry3D.Abstract.ITransform<V>, new()
        where Transform : struct, Kean.Math.Geometry3D.Abstract.ITransform<V>
        where SizeType : Kean.Math.Geometry3D.Abstract.Size<TransformType, Transform, SizeType, Size, R, V>, Kean.Math.Geometry3D.Abstract.IVector<V>, new()
        where Size : struct, Kean.Math.Geometry3D.Abstract.ISize<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
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
                this.Casting,
                this.CastingNull,
                this.Hash
                );
        }
        [Test]
        public void GetValues()
        {
			Verify(this.Vector0.Width, Is.EqualTo(this.Cast(22)).Within(this.Precision));
			Verify(this.Vector0.Height, Is.EqualTo(this.Cast(-3)).Within(this.Precision));
			Verify(this.Vector0.Depth, Is.EqualTo(this.Cast(10)).Within(this.Precision));
        }
        [Test]
        public void Casting()
        {
            string value = "10, 20, 30";
			Verify(this.CastToString(this.Vector3), Is.EqualTo(value));
			Verify(this.CastFromString(value), Is.EqualTo(this.Vector3));
        }
        [Test]
        public void CastingNull()
        {
            string value = null;
            SizeType size = null;
			Verify(this.CastToString(size), Is.EqualTo(value));
			Verify(this.CastFromString(value), Is.EqualTo(size));
        }
    }
}

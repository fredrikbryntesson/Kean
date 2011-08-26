using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;

namespace Kean.Math.Geometry2D.Test.Abstract
{
    public abstract class Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V> : 
        Vector<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>
        where PointType : Kean.Math.Geometry2D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointValue : struct, Kean.Math.Geometry2D.Abstract.IPoint<V>, Kean.Math.Geometry2D.Abstract.IVector<V>
        where TransformType : Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, new()
        where TransformValue : struct, Kean.Math.Geometry2D.Abstract.ITransform<V>
        where SizeType : Kean.Math.Geometry2D.Abstract.Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, new()
        where SizeValue : struct, Kean.Math.Geometry2D.Abstract.ISize<V>, Kean.Math.Geometry2D.Abstract.IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        [Test]
        public void GetValues()
        {
            Expect(this.Vector0.X, Is.EqualTo(this.Cast(22.221)).Within(this.Precision));
            Expect(this.Vector0.Y, Is.EqualTo(this.Cast(-3.1)).Within(this.Precision));
        }
        [Test]
        public void Swap()
        {
            PointType result = this.Vector0.Swap();
            Expect(result.X, Is.EqualTo(this.Vector0.Y));
            Expect(result.Y, Is.EqualTo(this.Vector0.X));
        }
        [Test]
        public void Casting()
        {
            string value = "10 20";
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
        public void Run()
        {
            this.Run(
                this.Equality,
                this.Addition,
                this.Subtraction,
                this.ScalarMultitplication,
                this.GetValues,
                this.Swap,
                this.Casting,
                this.CastingNull
                );
        }
    }
}

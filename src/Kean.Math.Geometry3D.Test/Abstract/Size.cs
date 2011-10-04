using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;

namespace Kean.Math.Geometry3D.Test.Abstract
{
    public abstract class Size<T, TransformType, TransformValue, SizeType, SizeValue, R, V> :
        Vector<T, TransformType, TransformValue, SizeType, SizeValue, SizeType, SizeValue, R, V>
        where T : Kean.Test.Fixture<T>, new()
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
                this.Casting,
                this.CastingNull
                );
        }
        [Test]
        public void GetValues()
        {
            Expect(this.Vector0.Width, Is.EqualTo(this.Cast(22)).Within(this.Precision));
            Expect(this.Vector0.Height, Is.EqualTo(this.Cast(-3)).Within(this.Precision));
            Expect(this.Vector0.Depth, Is.EqualTo(this.Cast(10)).Within(this.Precision));
        }
        [Test]
        public void Casting()
        {
            string value = "10, 20, 30";
            Expect(this.CastToString(this.Vector3), Is.EqualTo(value));
            Expect(this.CastFromString(value), Is.EqualTo(this.Vector3));
        }
        [Test]
        public void CastingNull()
        {
            string value = null;
            SizeType size = null;
            Expect(this.CastToString(size), Is.EqualTo(value));
            Expect(this.CastFromString(value), Is.EqualTo(size));
        }
    }
}

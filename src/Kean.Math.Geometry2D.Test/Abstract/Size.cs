using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;

namespace Kean.Math.Geometry2D.Test.Abstract
{
    public abstract class Size<T, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
        Vector<T, SizeType, SizeValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>
        where T : Kean.Test.Fixture<T>, new()
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
        protected override void Run()
        {
            this.Run(
                this.Equality,
                this.Addition,
                this.Subtraction,
                this.ScalarMultiplication,
                this.GetValues,
                this.Swap,
                this.Casting,
                this.CastingNull
                );
        }
        string prefix = "Kean.Math.Geometry2D.Test.Abstract.Size.";
        [Test]
        public void GetValues()
        {
            Expect(this.Vector0.Width, Is.EqualTo(this.Cast(22.221)).Within(this.Precision), this.prefix + "GetValues.0");
            Expect(this.Vector0.Height, Is.EqualTo(this.Cast(-3.1)).Within(this.Precision), this.prefix + "GetValues.1");
        }
        [Test]
        public void Swap()
        {
            SizeType result = this.Vector0.Swap();
            Expect(result.Width, Is.EqualTo(this.Vector0.Height), this.prefix + "Swap.0");
            Expect(result.Height, Is.EqualTo(this.Vector0.Width), this.prefix + "Swap.1");
        }
        [Test]
        public void Casting()
        {
            string value = "10, 20";
            Expect(this.CastToString(this.Vector3), Is.EqualTo(value), this.prefix + "Casting.0");
            Expect(this.CastFromString(value), Is.EqualTo(this.Vector3), this.prefix + "Casting.1");
        }
        [Test]
        public void CastingNull()
        {
            string value = null;
            SizeType size = null;
            Expect(this.CastToString(size), Is.EqualTo(value), this.prefix + "CastingNull.0");
            Expect(this.CastFromString(value), Is.EqualTo(size), this.prefix + "CastingNull.1");
        }
    }
}

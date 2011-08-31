using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;

namespace Kean.Math.Geometry2D.Test.Abstract
{
	public abstract class Point<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
		Vector<PointType, PointValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>
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

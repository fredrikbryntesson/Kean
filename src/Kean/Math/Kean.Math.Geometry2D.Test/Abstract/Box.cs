using System;
using Kean.Core.Extension;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Math.Geometry2D.Test.Abstract
{
    public abstract class Box<T, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
        Kean.Test.Fixture<T>
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
        protected float Precision { get { return 1e-4f; } }
        protected override void  Run()
        {
            this.Run(
                this.LeftTop,
                this.Size,
                this.Hash);
        }
        string prefix = "Kean.Math.Geometry2D.Test.Abstract.Box.";
        protected abstract V Cast(double value);
    
        protected BoxType Box0 { get; set; }
        protected BoxType Box1 { get; set; }
        protected BoxType Box2 { get; set; }
        #region Equality
        [Test]
        public void Equality()
        {
            BoxType box = null;
            Expect(this.Box0, Is.EqualTo(this.Box0));
            Expect(this.Box0.Equals(this.Box0 as object), Is.True);
            Expect(this.Box0 == this.Box0, Is.True);
            Expect(this.Box0 != this.Box1, Is.True);
            Expect(this.Box0 == box, Is.False);
            Expect(box == box, Is.True);
            Expect(box == this.Box0, Is.False);
        }
        #endregion
        [Test]
        public void LeftTop()
        {
            PointType leftTop = this.Box0.LeftTop;
            Expect(leftTop.X, Is.EqualTo(1), this.prefix + "LeftTop.0");
            Expect(leftTop.Y, Is.EqualTo(2), this.prefix + "LeftTop.1");
        }
        [Test]
        public void Size()
        {
            SizeType size = this.Box0.Size;
            Expect(size.Width, Is.EqualTo(3), this.prefix + "Size.0");
            Expect(size.Height, Is.EqualTo(4), this.prefix + "Size.1");
        }
        #region Arithmetic
        [Test]
        public void Addition()
        {
        }
        [Test]
        public void Subtraction()
        {
        }
        [Test]
        public void ScalarMultitplication()
        {
        }
        #endregion
        #region Hash Code
        [Test]
        public void Hash()
        {
            Expect(this.Box0.Hash(), Is.Not.EqualTo(0));
        }
        #endregion
    }
}

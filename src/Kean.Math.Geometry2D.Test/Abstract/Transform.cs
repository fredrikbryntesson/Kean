using System;
using Kean.Core.Extension;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Math.Geometry2D.Test.Abstract
{
	public abstract class Transform<T, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
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
        protected abstract string CastToString(TransformType value);
        protected abstract TransformType CastFromString(string value);
        protected float Precision { get { return 1e-5f; } }
        protected abstract V Cast(double value);
        protected TransformType Transform0 { get; set; }
        protected TransformType Transform1 { get; set; }
        protected TransformType Transform2 { get; set; }
        protected TransformType Transform3 { get; set; }
		protected TransformType Transform4 { get { return Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(new R().CreateConstant(10), new R().CreateConstant(20), new R().CreateConstant(30), new R().CreateConstant(40), new R().CreateConstant(50), new R().CreateConstant(60)); } }
        protected PointType Point0 { get; set; }
        protected PointType Point1 { get; set; }
        protected SizeType Size0 { get; set; }
        protected override void Run()
        {
            this.Run(
                this.Equality,
                this.CreateZeroTransform,
                this.CreateIdentity,
                this.CreateRotation,
                this.CreateScale,
                this.CreateTranslation,
                this.Rotatate,
                this.Scale,
                this.Translatate,
                this.InverseTransform,
                this.MultiplicationTransformTransform,
                this.MultiplicationTransformPoint,
                this.GetValueValues,
                this.CastToArray,
                this.GetTranslation,
                this.GetScalingX,
                this.GetScalingY,
                this.GetScaling,
                this.GetRotation,
                this.Casting,
                this.CastingNull
                );
        }
        string prefix = "Kean.Math.Geometry2D.Test.Abstract.Transform.";
        #region Equality
        [Test]
        public void Equality()
        {
            TransformType transform = null;
            Expect(this.Transform0, Is.EqualTo(this.Transform0));
            Expect(this.Transform0.Equals(this.Transform0 as object), Is.True);
            Expect(this.Transform0 == this.Transform0, Is.True);
            Expect(this.Transform0 != this.Transform1, Is.True);
            Expect(this.Transform0 == transform, Is.False);
            Expect(transform == transform, Is.True);
            Expect(transform == this.Transform0, Is.False);
        }
        #endregion
        #region Arithmetic
        [Test]
        public void InverseTransform()
        {
            Expect(this.Transform0.Inverse, Is.EqualTo(this.Transform3));
        }
        [Test]
        public void MultiplicationTransformTransform()
        {
            Expect(this.Transform0 * this.Transform1, Is.EqualTo(this.Transform2));
        }
        [Test]
        public void MultiplicationTransformPoint()
        {
            Expect(this.Transform0 * this.Point0, Is.EqualTo(this.Point1));
        }
        #endregion
        [Test]
        public void CreateZeroTransform()
        {
            TransformType transform = new TransformType();
            Expect(transform.A, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.0");
            Expect(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.1");
            Expect(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.2");
            Expect(transform.D, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.3");
            Expect(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.4");
            Expect(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.5");
        }
        [Test]
        public void CreateIdentity()
        {
            TransformType transform = Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Identity;
            Expect(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "CreateIdentity.0");
            Expect(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateIdentity.1");
            Expect(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateIdentity.2");
            Expect(transform.D, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "CreateIdentity.3");
            Expect(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateIdentity.4");
            Expect(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateIdentity.5");
        }
        [Test]
        public void Rotatate()
        {
            TransformType identity = Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Identity;
            R angle = new R().CreateConstant(20).ToRadians();
            TransformType transform = Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.CreateRotation(angle);
            transform = transform.Rotate(-angle);
            Expect(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Rotatate.0");
            Expect(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Rotatate.1");
            Expect(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Rotatate.2");
            Expect(transform.D, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Rotatate.3");
            Expect(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Rotatate.4");
            Expect(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Rotatate.5");
        }
        [Test]
        public void Scale()
        {
            TransformType identity = Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Identity;
            R scale = new R().CreateConstant(20);
            TransformType transform = Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.CreateScaling(scale, scale);
            transform = transform.Scale(scale.Invert(), scale.Invert());
            Expect(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Scale.0");
            Expect(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Scale.1");
            Expect(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Scale.2");
            Expect(transform.D, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Scale.3");
            Expect(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Scale.4");
            Expect(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Scale.5");
        }
        [Test]
        public void Translatate()
        {
            R xDelta = new R().CreateConstant(40);
            R yDelta = new R().CreateConstant(-40);
            TransformType transform = Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.CreateTranslation(xDelta, yDelta);
            transform = transform.Translate(xDelta.Negate(), yDelta.Negate());
            Expect(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Translatate.0");
            Expect(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Translatate.1");
            Expect(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Translatate.2");
            Expect(transform.D, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Translatate.3");
            Expect(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Translatate.4");
            Expect(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Translatate.5");
        }
        [Test]
        public void CreateRotation()
        {
            R angle = new R().CreateConstant(20).ToRadians();
            TransformType transform = Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.CreateRotation(angle);
            Expect(transform.A, Is.EqualTo(angle.Cosinus().Value).Within(this.Precision), this.prefix + "CreateRotation.0");
            Expect(transform.B, Is.EqualTo(angle.Sinus().Value).Within(this.Precision), this.prefix + "CreateRotation.1");
            Expect(transform.C, Is.EqualTo((-angle.Sinus()).Value).Within(this.Precision), this.prefix + "CreateRotation.2");
            Expect(transform.D, Is.EqualTo(angle.Cosinus().Value).Within(this.Precision), this.prefix + "CreateRotation.3");
            Expect(transform.E, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateRotation.4");
            Expect(transform.F, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateRotation.5");
        }
        [Test]
        public void CreateScale()
        {
            V scale = (V)new R().CreateConstant(20);
            TransformType transform = Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.CreateScaling(scale, scale);
            Expect(transform.A, Is.EqualTo(scale).Within(this.Precision), this.prefix + "CreateScale.0");
            Expect(transform.B, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateScale.1");
            Expect(transform.C, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateScale.2");
            Expect(transform.D, Is.EqualTo(scale).Within(this.Precision), this.prefix + "CreateScale.3");
            Expect(transform.E, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateScale.4");
            Expect(transform.F, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateScale.5");
        }
        [Test]
        public void CreateTranslation()
        {
            V xDelta = (V)new R().CreateConstant(40);
            V yDelta = (V)new R().CreateConstant(-40);
            TransformType transform = Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.CreateTranslation(xDelta, yDelta);
            Expect(transform.A, Is.EqualTo(1).Within(this.Precision), this.prefix + "CreateTranslation.0");
            Expect(transform.B, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateTranslation.1");
            Expect(transform.C, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateTranslation.2");
            Expect(transform.D, Is.EqualTo(1).Within(this.Precision), this.prefix + "CreateTranslation.3");
            Expect(transform.E, Is.EqualTo(xDelta).Within(this.Precision), this.prefix + "CreateTranslation.4");
            Expect(transform.F, Is.EqualTo(yDelta).Within(this.Precision), this.prefix + "CreateTranslation.5");
        }
        [Test]
        public void GetValueValues()
        {
            TransformValue transform = this.Transform0.Value;
            Expect(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "GetValueValues.0");
            Expect(transform.B, Is.EqualTo(this.Cast(4)).Within(this.Precision), this.prefix + "GetValueValues.1");
            Expect(transform.C, Is.EqualTo(this.Cast(2)).Within(this.Precision), this.prefix + "GetValueValues.2");
            Expect(transform.D, Is.EqualTo(this.Cast(5)).Within(this.Precision), this.prefix + "GetValueValues.3");
            Expect(transform.E, Is.EqualTo(this.Cast(3)).Within(this.Precision), this.prefix + "GetValueValues.4");
            Expect(transform.F, Is.EqualTo(this.Cast(6)).Within(this.Precision), this.prefix + "GetValueValues.5");
        }
        [Test]
        public void GetScalingX()
        {
            V scale = this.Transform0.ScalingX;
            Expect(scale, Is.EqualTo(this.Cast(4.1231)).Within(this.Precision), this.prefix + "GetScalingX.0");
        }
        [Test]
        public void GetScalingY()
        {
            V scale = this.Transform0.ScalingY;
            Expect(scale, Is.EqualTo(this.Cast(5.38516474f)).Within(this.Precision), this.prefix + "GetScalingY.0");
        }
        [Test]
        public void GetScaling()
        {
            V scale = this.Transform0.Scaling;
            Expect(scale, Is.EqualTo(this.Cast(4.75413513f)).Within(this.Precision), this.prefix + "GetScaling.0");
        }
        [Test]
        public void GetRotation()
        {
            R angle = new R().CreateConstant(20).ToRadians();
            TransformType transform = Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.CreateRotation(angle);
            R value = ((R)transform.Rotation).ToDegrees();
            Expect(value.Value, Is.EqualTo(this.Cast(20)).Within(this.Precision), this.prefix + "GetRotation.0");
        }
        [Test]
        public void GetTranslation()
        {
            SizeType translation = this.Transform0.Translation;
            Expect(translation.Width, Is.EqualTo(this.Cast(3)).Within(this.Precision), this.prefix + "GetTranslation.0");
            Expect(translation.Height, Is.EqualTo(this.Cast(6)).Within(this.Precision), this.prefix + "GetTranslation.1");
        }
        [Test]
        public void CastToArray()
        {
            V[,] values = (V[,])((Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>)Geometry2D.Abstract.Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Identity);
            for(int x = 0; x < 3; x++)
                for(int y = 0; y < 3; y++)
                    Expect(values[x, y], Is.EqualTo(this.Cast(x == y ? 1 : 0)).Within(this.Precision), this.prefix + "CastToArray.0");
        }
        [Test]
        public void Casting()
        {
            string value = "10 20 30 40 50 60";
            Expect(this.CastToString(this.Transform4), Is.EqualTo(value), this.prefix + "Casting.0");
            Expect(this.CastFromString(value), Is.EqualTo(this.Transform4), this.prefix + "Casting.1");
        }
        [Test]
        public void CastingNull()
        {
            string value = null;
            TransformType transform = null;
            Expect(this.CastToString(transform), Is.EqualTo(value), this.prefix + "CastingNull.0");
            Expect(this.CastFromString(value), Is.EqualTo(transform), this.prefix + "CastingNull.1");
        }
    }
}

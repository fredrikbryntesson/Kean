using System;
using Kean.Core.Basis.Extension;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Test.Math.Geometry3D.Abstract
{
    public abstract class Transform<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>
        where TransformType : Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, new()
        where TransformValue : struct, Kean.Math.Geometry3D.Abstract.ITransform<V>
        where PointType : Kean.Math.Geometry3D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointValue : struct, Kean.Math.Geometry3D.Abstract.IPoint<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
        where SizeType : Kean.Math.Geometry3D.Abstract.Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, new()
        where SizeValue : struct, Kean.Math.Geometry3D.Abstract.ISize<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        protected float Precision { get { return 1e-5f; } }
        protected abstract V Cast(double value);
        protected TransformType Transform0 { get; set; }
        protected TransformType Transform1 { get; set; }
        protected TransformType Transform2 { get; set; }
        protected TransformType Transform3 { get; set; }
        protected PointType Point0 { get; set; }
        protected PointType Point1 { get; set; }
        protected SizeType Size0 { get; set; }

        #region Equality
        [Test]
        public void Equality()
        {
            TransformType transform = null;
            Assert.That(this.Transform0, Is.EqualTo(this.Transform0));
            Assert.That(this.Transform0.Equals(this.Transform0 as object), Is.True);
            Assert.That(this.Transform0 == this.Transform0, Is.True);
            Assert.That(this.Transform0 != this.Transform1, Is.True);
            Assert.That(this.Transform0 == transform, Is.False);
            Assert.That(transform == transform, Is.True);
            Assert.That(transform == this.Transform0, Is.False);
        }
        #endregion
        #region Arithmetic
        [Test]
        public void InverseTransform()
        {
            Assert.That(this.Transform0.Inverse, Is.EqualTo(this.Transform3));
        }
        [Test]
        public void MultiplicationTransformTransform()
        {
            Assert.That(this.Transform0 * this.Transform1, Is.EqualTo(this.Transform2));
        }
        [Test]
        public void MultiplicationTransformPoint()
        {
            Assert.That(this.Transform0 * this.Point0, Is.EqualTo(this.Point1));
        }
        #endregion
        [Test]
        public void CreateZeroTransform()
        {
            TransformType transform = new TransformType();
            Assert.That(transform.A.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.E.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.F.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.H.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.I.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.J.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.K.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.L.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void CreateIdentity()
        {
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.Identity;
            Assert.That(transform.A.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.E.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.F.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.H.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.I.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.J.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.K.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.L.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void Rotatate()
        {
            TransformType identity = Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.Identity;
            R angle = new R().CreateConstant(20).ToRadians();
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateRotationX(angle);
            transform = transform.RotateX(-angle);
            Assert.That(transform.A.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.E.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.F.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.H.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.I.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.J.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.K.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.L.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void Scale()
        {
            TransformType identity = Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.Identity;
            R scale = new R().CreateConstant(20);
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateScaling(scale, scale, scale);
            transform = transform.Scale(scale.Invert(), scale.Invert(), scale.Invert());
            Assert.That(transform.A.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.E.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.F.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.H.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.I.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.J.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.K.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.L.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void Translatate()
        {
            R xDelta = new R().CreateConstant(40);
            R yDelta = new R().CreateConstant(-40);
            R zDelta = new R().CreateConstant(30);
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateTranslation(xDelta, yDelta, zDelta);
            transform = transform.Translate(xDelta.Negate(), yDelta.Negate(), zDelta.Negate());
            Assert.That(transform.A.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.E.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.F.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.H.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.I.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.J.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.K.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.L.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void CreateRotation()
        {
            R angle = new R().CreateConstant(20).ToRadians();
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateRotationX(angle);
            Assert.That(transform.E, Is.EqualTo(angle.Cosinus()).Within(this.Precision));
            Assert.That(transform.F, Is.EqualTo(angle.Sinus()).Within(this.Precision));
            Assert.That(transform.H, Is.EqualTo(-angle.Sinus()).Within(this.Precision));
            Assert.That(transform.I, Is.EqualTo(angle.Cosinus()).Within(this.Precision));
        }
        [Test]
        public void CreateScale()
        {
            R scale = new R().CreateConstant(20);
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateScaling(scale, scale, scale);
            Assert.That(transform.A, Is.EqualTo(scale).Within(this.Precision));
            Assert.That(transform.E, Is.EqualTo(scale).Within(this.Precision));
            Assert.That(transform.I, Is.EqualTo(scale).Within(this.Precision));
        }
        [Test]
        public void CreateTranslation()
        {
            R xDelta = new R().CreateConstant(40);
            R yDelta = new R().CreateConstant(-40);
            R zDelta = new R().CreateConstant(30);
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateTranslation(xDelta, yDelta, zDelta);
            Assert.That(transform.J, Is.EqualTo(xDelta).Within(this.Precision));
            Assert.That(transform.K, Is.EqualTo(yDelta).Within(this.Precision));
            Assert.That(transform.L, Is.EqualTo(zDelta).Within(this.Precision));
        }
        [Test]
        public void GetValueValues()
        {
            TransformValue transform = this.Transform0.Value;
            Assert.That(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.B, Is.EqualTo(this.Cast(4)).Within(this.Precision));
            Assert.That(transform.C, Is.EqualTo(this.Cast(2)).Within(this.Precision));
            Assert.That(transform.D, Is.EqualTo(this.Cast(5)).Within(this.Precision));
            Assert.That(transform.E, Is.EqualTo(this.Cast(3)).Within(this.Precision));
            Assert.That(transform.F, Is.EqualTo(this.Cast(6)).Within(this.Precision));
        }
        [Test]
        public void GetScalingX()
        {
            V scale = this.Transform0.ScalingX;
            Assert.That(scale, Is.EqualTo(this.Cast(4.1231)).Within(this.Precision));
        }
        [Test]
        public void GetScalingY()
        {
            V scale = this.Transform0.ScalingY;
            Assert.That(scale, Is.EqualTo(this.Cast(5.38516474f)).Within(this.Precision));
        }
        [Test]
        public void GetScaling()
        {
            V scale = this.Transform0.Scaling;
            Assert.That(scale, Is.EqualTo(this.Cast(4.75413513f)).Within(this.Precision));
        }
        /*
        [Test]
        public void GetRotation()
        {
            R angle = new R().CreateConstant(20).ToRadians();
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateRotationX(angle);
            R value = ((R)transform.RotationX).ToDegrees();
            Assert.That(value.Value, Is.EqualTo(this.Cast(20)).Within(this.Precision));
        }
        */
        [Test]
        public void GetTranslation()
        {
            SizeType translation = this.Transform0.Translation;
            Assert.That(translation.Width, Is.EqualTo(this.Cast(3)).Within(this.Precision));
            Assert.That(translation.Height, Is.EqualTo(this.Cast(6)).Within(this.Precision));
        }
        [Test]
        public void CastToArray()
        {
            V[,] values = (V[,])((Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>)Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.Identity);
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    Assert.That(values[x, y], Is.EqualTo(this.Cast(x == y ? 1 : 0)).Within(this.Precision));
        }
        internal void Run(params System.Action[] tests)
        {
            foreach (System.Action test in tests)
                if (test.NotNull())
                    test();
        }
        public void Run()
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
                this.GetScaling
                );
        }
    }
}

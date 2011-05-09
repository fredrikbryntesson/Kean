using System;
using Kean.Core.Basis.Extension;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Test.Math.Geometry2D.Abstract
{
    public abstract class Transform<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>
        where TransformType : Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, new()
        where TransformValue : struct, Kean.Math.Geometry2D.Abstract.ITransform<V>
        where PointType : Kean.Math.Geometry2D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointValue : struct, Kean.Math.Geometry2D.Abstract.IPoint<V>, Kean.Math.Geometry2D.Abstract.IVector<V>
        where SizeType : Kean.Math.Geometry2D.Abstract.Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, new()
        where SizeValue : struct, Kean.Math.Geometry2D.Abstract.ISize<V>, Kean.Math.Geometry2D.Abstract.IVector<V>
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
        protected TransformType Transform4 { get { return Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.Create(new R().CreateConstant(10), new R().CreateConstant(20), new R().CreateConstant(30), new R().CreateConstant(40), new R().CreateConstant(50), new R().CreateConstant(60)); } }
        
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
            Assert.That(transform.A, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.D, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void CreateIdentity()
        {
            TransformType transform = Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.Identity;
            Assert.That(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.D, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void Rotatate()
        {
            TransformType identity = Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.Identity;
            R angle = new R().CreateConstant(20).ToRadians();
            TransformType transform = Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateRotation(angle);
            transform = transform.Rotate(-angle);
            Assert.That(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.D, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void Scale()
        {
            TransformType identity = Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.Identity;
            R scale = new R().CreateConstant(20);
            TransformType transform = Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateScaling(scale, scale);
            transform = transform.Scale(scale.Invert(), scale.Invert());
            Assert.That(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.D, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void Translatate()
        {
            R xDelta = new R().CreateConstant(40);
            R yDelta = new R().CreateConstant(-40);
            TransformType transform = Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateTranslation(xDelta, yDelta);
            transform = transform.Translate(xDelta.Negate(), yDelta.Negate());
            Assert.That(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.C, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.D, Is.EqualTo(this.Cast(1)).Within(this.Precision));
            Assert.That(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision));
            Assert.That(transform.F, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void CreateRotation()
        {
            R angle = new R().CreateConstant(20).ToRadians();
            TransformType transform = Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateRotation(angle);
            Assert.That(transform.A, Is.EqualTo(angle.Cosinus().Value).Within(this.Precision));
            Assert.That(transform.B, Is.EqualTo(angle.Sinus().Value).Within(this.Precision));
            Assert.That(transform.C, Is.EqualTo((-angle.Sinus()).Value).Within(this.Precision));
            Assert.That(transform.D, Is.EqualTo(angle.Cosinus().Value).Within(this.Precision));
            Assert.That(transform.E, Is.EqualTo(0).Within(this.Precision));
            Assert.That(transform.F, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void CreateScale()
        {
            V scale = (V)new R().CreateConstant(20);
            TransformType transform = Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateScaling(scale, scale);
            Assert.That(transform.A, Is.EqualTo(scale).Within(this.Precision));
            Assert.That(transform.B, Is.EqualTo(0).Within(this.Precision));
            Assert.That(transform.C, Is.EqualTo(0).Within(this.Precision));
            Assert.That(transform.D, Is.EqualTo(scale).Within(this.Precision));
            Assert.That(transform.E, Is.EqualTo(0).Within(this.Precision));
            Assert.That(transform.F, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void CreateTranslation()
        {
            V xDelta = (V)new R().CreateConstant(40);
            V yDelta = (V)new R().CreateConstant(-40);
            TransformType transform = Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateTranslation(xDelta, yDelta);
            Assert.That(transform.A, Is.EqualTo(1).Within(this.Precision));
            Assert.That(transform.B, Is.EqualTo(0).Within(this.Precision));
            Assert.That(transform.C, Is.EqualTo(0).Within(this.Precision));
            Assert.That(transform.D, Is.EqualTo(1).Within(this.Precision));
            Assert.That(transform.E, Is.EqualTo(xDelta).Within(this.Precision));
            Assert.That(transform.F, Is.EqualTo(yDelta).Within(this.Precision));
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
        [Test]
        public void GetRotation()
        {
            R angle = new R().CreateConstant(20).ToRadians();
            TransformType transform = Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateRotation(angle);
            R value = ((R)transform.Rotation).ToDegrees();
            Assert.That(value.Value, Is.EqualTo(this.Cast(20)).Within(this.Precision));
        }
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
            V[,] values = (V[,])((Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>)Kean.Math.Geometry2D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.Identity);
            for(int x = 0; x < 3; x++)
                for(int y = 0; y < 3; y++)
                    Assert.That(values[x,y], Is.EqualTo(this.Cast(x == y ? 1 : 0)).Within(this.Precision));
        }
        [Test]
        public void Casting()
        {
            string value = "10, 30, 50; 20, 40, 60; 0, 0, 1";
            Assert.That(this.CastToString(this.Transform4), Is.EqualTo(value));
            Assert.That(this.CastFromString(value), Is.EqualTo(this.Transform4));
        }
        [Test]
        public void CastingNull()
        {
            string value = null;
            TransformType transform = null;
            Assert.That(this.CastToString(transform), Is.EqualTo(value));
            Assert.That(this.CastFromString(value), Is.EqualTo(transform));
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
                this.GetScaling,
                this.GetRotation,
                this.Casting,
                this.CastingNull
                );
        }
    }
}

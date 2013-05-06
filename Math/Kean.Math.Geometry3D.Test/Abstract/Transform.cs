using System;
using Kean.Core.Extension;
using NUnit.Framework;


namespace Kean.Math.Geometry3D.Test.Abstract
{
    public abstract class Transform<T, TransformType, Transform, PointType, Point, SizeType, Size, R, V> :
        Kean.Test.Fixture<T>
        where T : Kean.Test.Fixture<T>, new()
        where TransformType : Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>, new()
        where Transform : struct, Kean.Math.Geometry3D.Abstract.ITransform<V>
        where PointType : Kean.Math.Geometry3D.Abstract.Point<TransformType, Transform, PointType, Point, SizeType, Size, R, V>, new()
        where Point : struct, Kean.Math.Geometry3D.Abstract.IPoint<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
        where SizeType : Kean.Math.Geometry3D.Abstract.Size<TransformType, Transform, SizeType, Size, R, V>, new()
        where Size : struct, Kean.Math.Geometry3D.Abstract.ISize<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
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
        protected TransformType Transform4 { get { return Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>.Create(new R().CreateConstant(10), new R().CreateConstant(20), new R().CreateConstant(30), new R().CreateConstant(40), new R().CreateConstant(50), new R().CreateConstant(60), new R().CreateConstant(70), new R().CreateConstant(80), new R().CreateConstant(90), new R().CreateConstant(100), new R().CreateConstant(110), new R().CreateConstant(120)); } }
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
                this.GetScalingZ,
                this.GetScaling,
                this.CastToArray,
                this.MultiplicationTransformTransform,
                this.MultiplicationTransformPoint,
                this.Casting,
                this.CastingNull,
                this.Hash
                );
        }
 
        #region Equality
        [Test]
        public void Equality()
        {
            TransformType transform = null;
			Verify(this.Transform0, Is.EqualTo(this.Transform0));
			Verify(this.Transform0.Equals(this.Transform0 as object), Is.True);
			Verify(this.Transform0 == this.Transform0, Is.True);
			Verify(this.Transform0 != this.Transform1, Is.True);
			Verify(this.Transform0 == transform, Is.False);
			Verify(transform == transform, Is.True);
			Verify(transform == this.Transform0, Is.False);
        }
        #endregion
        #region Arithmetic
        [Test]
        public void InverseTransform()
        {
            TransformType transform = this.Transform0.Inverse;
            TransformType correct = this.Transform3;
			Verify(transform.A.Value, Is.EqualTo(correct.A.Value).Within(this.Precision));
			Verify(transform.B.Value, Is.EqualTo(correct.B.Value).Within(this.Precision));
			Verify(transform.C.Value, Is.EqualTo(correct.C.Value).Within(this.Precision));
			Verify(transform.D.Value, Is.EqualTo(correct.D.Value).Within(this.Precision));
			Verify(transform.E.Value, Is.EqualTo(correct.E.Value).Within(this.Precision));
			Verify(transform.F.Value, Is.EqualTo(correct.F.Value).Within(this.Precision));
			Verify(transform.G.Value, Is.EqualTo(correct.G.Value).Within(this.Precision));
			Verify(transform.H.Value, Is.EqualTo(correct.H.Value).Within(this.Precision));
			Verify(transform.I.Value, Is.EqualTo(correct.I.Value).Within(this.Precision));
			Verify(transform.J.Value, Is.EqualTo(correct.J.Value).Within(this.Precision));
			Verify(transform.K.Value, Is.EqualTo(correct.K.Value).Within(this.Precision));
			Verify(transform.L.Value, Is.EqualTo(correct.L.Value).Within(this.Precision));
        }
        [Test]
        public void MultiplicationTransformTransform()
        {
			Verify(this.Transform0 * this.Transform1, Is.EqualTo(this.Transform2));
        }
        [Test]
        public void MultiplicationTransformPoint()
        {
			Verify(this.Transform0 * this.Point0, Is.EqualTo(this.Point1));
        }
        #endregion
        [Test]
        public void CreateZeroTransform()
        {
            TransformType transform = new TransformType();
			Verify(transform.A.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.E.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.F.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.H.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.I.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.J.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.K.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.L.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void CreateIdentity()
        {
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>.Identity;
			Verify(transform.A.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.E.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.F.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.H.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.I.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.J.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.K.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.L.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void Rotatate()
        {
            TransformType identity = Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>.Identity;
            R angle = new R().CreateConstant(20).ToRadians();
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>.CreateRotationX(angle);
            transform = transform.RotateX(-angle);
			Verify(transform.A.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.E.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.F.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.H.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.I.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.J.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.K.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.L.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void Scale()
        {
            TransformType identity = Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>.Identity;
            R scale = new R().CreateConstant(20);
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>.CreateScaling(scale, scale, scale);
            transform = transform.Scale(scale.Invert(), scale.Invert(), scale.Invert());
			Verify(transform.A.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.E.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.F.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.H.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.I.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.J.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.K.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.L.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void Translatate()
        {
            R xDelta = new R().CreateConstant(40);
            R yDelta = new R().CreateConstant(-40);
            R zDelta = new R().CreateConstant(30);
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>.CreateTranslation(xDelta, yDelta, zDelta);
            transform = transform.Translate(xDelta.Negate(), yDelta.Negate(), zDelta.Negate());
			Verify(transform.A.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.E.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.F.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.H.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.I.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.J.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.K.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.L.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void CreateRotation()
        {
            R angle = new R().CreateConstant(20).ToRadians();
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>.CreateRotationX(angle);
			Verify(transform.A.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.E, Is.EqualTo(angle.Cosinus()).Within(this.Precision));
			Verify(transform.F, Is.EqualTo(angle.Sinus()).Within(this.Precision));
			Verify(transform.H, Is.EqualTo(-angle.Sinus()).Within(this.Precision));
			Verify(transform.I, Is.EqualTo(angle.Cosinus()).Within(this.Precision));
			Verify(transform.J.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.K.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.L.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void CreateScale()
        {
            R scale = new R().CreateConstant(20);
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>.CreateScaling(scale, scale, scale);
			Verify(transform.A, Is.EqualTo(scale).Within(this.Precision));
			Verify(transform.E, Is.EqualTo(scale).Within(this.Precision));
			Verify(transform.I, Is.EqualTo(scale).Within(this.Precision));
			Verify(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.F.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.H.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.J.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.K.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.L.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
        }
        [Test]
        public void CreateTranslation()
        {
            R xDelta = new R().CreateConstant(40);
            R yDelta = new R().CreateConstant(-40);
            R zDelta = new R().CreateConstant(30);
            TransformType transform = Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>.CreateTranslation(xDelta, yDelta, zDelta);
			Verify(transform.A.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.B.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.C.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.D.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.E.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.F.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.G.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.H.Value, Is.EqualTo(this.Cast(0)).Within(this.Precision));
			Verify(transform.I.Value, Is.EqualTo(this.Cast(1)).Within(this.Precision));
			Verify(transform.J, Is.EqualTo(xDelta).Within(this.Precision));
			Verify(transform.K, Is.EqualTo(yDelta).Within(this.Precision));
			Verify(transform.L, Is.EqualTo(zDelta).Within(this.Precision));
        }
        [Test]
        public void GetValueValues()
        {
            Transform transform = this.Transform0.Value;
			Verify(transform.A, Is.EqualTo(this.Cast(-1)).Within(this.Precision));
			Verify(transform.B, Is.EqualTo(this.Cast(2)).Within(this.Precision));
			Verify(transform.C, Is.EqualTo(this.Cast(3)).Within(this.Precision));
			Verify(transform.D, Is.EqualTo(this.Cast(4)).Within(this.Precision));
			Verify(transform.E, Is.EqualTo(this.Cast(5)).Within(this.Precision));
			Verify(transform.F, Is.EqualTo(this.Cast(6)).Within(this.Precision));
			Verify(transform.G, Is.EqualTo(this.Cast(7)).Within(this.Precision));
			Verify(transform.H, Is.EqualTo(this.Cast(8)).Within(this.Precision));
			Verify(transform.I, Is.EqualTo(this.Cast(9)).Within(this.Precision));
			Verify(transform.J, Is.EqualTo(this.Cast(10)).Within(this.Precision));
			Verify(transform.K, Is.EqualTo(this.Cast(11)).Within(this.Precision));
			Verify(transform.L, Is.EqualTo(this.Cast(12)).Within(this.Precision));
        }
        [Test]
        public void GetScalingX()
        {
            V scale = this.Transform0.ScalingX;
			Verify(scale, Is.EqualTo(this.Cast(3.7416575f)).Within(this.Precision));
        }
        [Test]
        public void GetScalingY()
        {
            V scale = this.Transform0.ScalingY;
			Verify(scale, Is.EqualTo(this.Cast(8.77496433f)).Within(this.Precision));
        }
        [Test]
        public void GetScalingZ()
        {
            V scale = this.Transform0.ScalingZ;
			Verify(scale, Is.EqualTo(this.Cast(13.9283886f)).Within(this.Precision));
        }
        [Test]
        public void GetScaling()
        {
            V scale = this.Transform0.Scaling;
			Verify(scale, Is.EqualTo(this.Cast(8.8150)).Within(this.Precision));
        }
        [Test]
        public void GetTranslation()
        {
            SizeType translation = this.Transform0.Translation;
			Verify(translation.Width, Is.EqualTo(this.Cast(10)).Within(this.Precision));
			Verify(translation.Height, Is.EqualTo(this.Cast(11)).Within(this.Precision));
			Verify(translation.Depth, Is.EqualTo(this.Cast(12)).Within(this.Precision));
        }
        [Test]
        public void CastToArray()
        {
            V[,] values = (V[,])((Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>)Kean.Math.Geometry3D.Abstract.Transform<TransformType, Transform, SizeType, Size, R, V>.Identity);
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
					Verify(values[x, y], Is.EqualTo(this.Cast(x == y ? 1 : 0)).Within(this.Precision));
        }
        [Test]
        public void Casting()
        {
            string value = "10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120";
			Verify(this.CastToString(this.Transform4), Is.EqualTo(value));
			Verify(this.CastFromString(value), Is.EqualTo(this.Transform4));
        }
        [Test]
        public void CastingNull()
        {
            string value = null;
            TransformType tranform = null;
			Verify(this.CastToString(tranform), Is.EqualTo(value));
			Verify(this.CastFromString(value), Is.EqualTo(tranform));
        }
        #region Hash Code
        [Test]
        public void Hash()
        {
			Verify(this.Transform0.Hash(), Is.Not.EqualTo(0));
        }
        #endregion
    }
}

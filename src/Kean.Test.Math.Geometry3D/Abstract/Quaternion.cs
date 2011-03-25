using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;

namespace Kean.Test.Math.Geometry3D.Abstract
{
    public abstract class Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, MatrixType, R, V>
        where QuaternionType : Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where MatrixType : Kean.Math.Matrix.Abstract<MatrixType, R, V>, new()
        where PointType : Kean.Math.Geometry3D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointValue : struct, Kean.Math.Geometry3D.Abstract.IPoint<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
        where TransformType : Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, Kean.Math.Geometry3D.Abstract.ITransform<V>, new()
        where TransformValue : struct, Kean.Math.Geometry3D.Abstract.ITransform<V>
        where SizeType : Kean.Math.Geometry3D.Abstract.Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, Kean.Math.Geometry3D.Abstract.IVector<V>, new()
        where SizeValue : struct, Kean.Math.Geometry3D.Abstract.ISize<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        protected float Precision { get { return 1e-5f; } }
        protected abstract V Cast(double value);
        protected QuaternionType Q0 { get; set; }
        protected QuaternionType Q1 { get; set; }
        protected QuaternionType Q2 { get; set; }
        protected QuaternionType Q3 { get; set; }
        protected PointType P0 { get; set; }
        protected PointType P1 { get; set; }
        #region Equality
        [Test]
        public void Equality()
        {
            QuaternionType quaternion = null;
            Assert.That(this.Q0, Is.EqualTo(this.Q0));
            Assert.That(this.Q0.Copy(), Is.EqualTo(this.Q0));
            Assert.That(this.Q0.Copy().Equals(this.Q0), Is.True);
            Assert.That(this.Q0.Equals(this.Q0 as object), Is.True);
            Assert.That(this.Q0 == this.Q0, Is.True);
            Assert.That(this.Q0 != this.Q1, Is.True);
            Assert.That(this.Q0 == quaternion, Is.False);
            Assert.That(quaternion == quaternion, Is.True);
            Assert.That(quaternion == this.Q0, Is.False);
        }
        #endregion

        #region Arithmetic
        [Test]
        public void Addition()
        {
            Assert.That(this.Q0 + this.Q1, Is.EqualTo(this.Q2));
        }
        [Test]
        public void Subtraction()
        {
            Assert.That(this.Q0 - this.Q0, Is.EqualTo(new QuaternionType()));
        }
        [Test]
        public void ScalarMultitplication()
        {
            Assert.That((-Kean.Math.Abstract<R, V>.One) * this.Q0, Is.EqualTo(-this.Q0));
        }
        [Test]
        public void Multitplication()
        {
            Assert.That(this.Q0 * this.Q1, Is.EqualTo(this.Q3));
        }

        #endregion
        [Test]
        public void GetValues()
        {
            V[] values = (V[])(this.Q0 as Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>);
            Assert.That(values[0], Is.EqualTo(33).Within(this.Precision));
            Assert.That(values[1], Is.EqualTo(10).Within(this.Precision));
            Assert.That(values[2], Is.EqualTo(-12).Within(this.Precision));
            Assert.That(values[3], Is.EqualTo(54.5).Within(this.Precision));
        }
        [Test]
        public void Roll()
        {
            R angle = new R().CreateConstant(20).ToRadians();
            QuaternionType quaternion = Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.EulerRoll(angle);
            Kean.Math.Matrix.Abstract<MatrixType, R, V> rotation = (Kean.Math.Matrix.Abstract<MatrixType, R, V>)(V[,])(quaternion as Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>);
            MatrixType rotation2 = Kean.Math.Matrix.Abstract<MatrixType, R, V>.RotationX(angle);
            Assert.That(rotation.Distance(rotation2), Is.EqualTo(0).Within(this.Precision));
            Assert.That(quaternion.Roll.Value, Is.EqualTo(angle.Value).Within(this.Precision));
        }
        [Test]
        public void Pitch()
        {
            R angle = new R().CreateConstant(20).ToRadians();
            QuaternionType quaternion = Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.EulerPitch(angle);
            Kean.Math.Matrix.Abstract<MatrixType, R, V> rotation = (Kean.Math.Matrix.Abstract<MatrixType, R, V>)(V[,])(quaternion as Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>);
            MatrixType rotation2 = Kean.Math.Matrix.Abstract<MatrixType, R, V>.RotationY(-angle);
            Assert.That(rotation.Distance(rotation2), Is.EqualTo(0).Within(this.Precision));
            Assert.That(quaternion.Pitch.Value, Is.EqualTo(angle.Value).Within(this.Precision));
        }
        [Test]
        public void Yaw()
        {
            R angle = new R().CreateConstant(20).ToRadians();
            QuaternionType quaternion = Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.EulerYaw(angle);
            Kean.Math.Matrix.Abstract<MatrixType, R, V> rotation = (Kean.Math.Matrix.Abstract<MatrixType, R, V>)(V[,])(quaternion as Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>);
            MatrixType rotation2 = Kean.Math.Matrix.Abstract<MatrixType, R, V>.RotationZ(angle);
            Assert.That(rotation.Distance(rotation2), Is.EqualTo(0).Within(this.Precision));
            Assert.That(quaternion.Yaw.Value, Is.EqualTo(angle.Value).Within(this.Precision));
        }
        [Test]
        public void Action()
        {
            R roll = new R().CreateConstant(20).ToRadians();
            R pitch = new R().CreateConstant(-30).ToRadians();
            R yaw = new R().CreateConstant(45).ToRadians();
            QuaternionType quaternion = Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.EulerAngles(roll, pitch, yaw);
            PointType rotatedPointQuaternion = quaternion * this.P0;
            Kean.Math.Matrix.Abstract<MatrixType, R, V> rotation = (Kean.Math.Matrix.Abstract<MatrixType, R, V>)(V[,])(quaternion as Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>);
            Kean.Math.Matrix.Abstract<MatrixType, R, V> rotatedPoint = rotation * (MatrixType)(Kean.Math.Matrix.Abstract<MatrixType, R, V>)((V[])(this.P0 as Kean.Math.Geometry3D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType,SizeValue, R, V>));
            PointType rotatedPointRotation = (PointType)(V[])(rotatedPoint);
            Assert.That(rotatedPointQuaternion.Distance(rotatedPointRotation).Value, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void Norm()
        {
            Assert.That(this.Q0.Norm.Value, Is.EqualTo(this.Cast(65.5991592)).Within(this.Precision));
        }
        public void Run()
        {
            this.Run(
                this.Equality,
                this.Addition,
                this.Subtraction,
                this.ScalarMultitplication,
                this.Multitplication,
                this.GetValues,
                this.Roll,
                this.Pitch,
                this.Yaw,
                this.Norm,
                this.Action
                );
        }
        internal void Run(params System.Action[] tests)
        {
            foreach (System.Action test in tests)
                if (test.NotNull())
                    test();
        }

    }
}

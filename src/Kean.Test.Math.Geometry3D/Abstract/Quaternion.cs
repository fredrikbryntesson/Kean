using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;

namespace Kean.Test.Math.Geometry3D.Abstract
{
    public abstract class Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> : 
        AssertionHelper
        where QuaternionType : Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointType : Kean.Math.Geometry3D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointValue : struct, Kean.Math.Geometry3D.Abstract.IPoint<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
        where TransformType : Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, Kean.Math.Geometry3D.Abstract.ITransform<V>, new()
        where TransformValue : struct, Kean.Math.Geometry3D.Abstract.ITransform<V>
        where SizeType : Kean.Math.Geometry3D.Abstract.Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, Kean.Math.Geometry3D.Abstract.IVector<V>, new()
        where SizeValue : struct, Kean.Math.Geometry3D.Abstract.ISize<V>, Kean.Math.Geometry3D.Abstract.IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        protected abstract string CastToString(QuaternionType value);
        protected abstract QuaternionType CastFromString(string value);
        protected float Precision { get { return 1e-3f; } }
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
            Expect(this.Q0, Is.EqualTo(this.Q0));
            Expect(this.Q0.Copy(), Is.EqualTo(this.Q0));
            Expect(this.Q0.Copy().Equals(this.Q0), Is.True);
            Expect(this.Q0.Equals(this.Q0 as object), Is.True);
            Expect(this.Q0 == this.Q0, Is.True);
            Expect(this.Q0 != this.Q1, Is.True);
            Expect(this.Q0 == quaternion, Is.False);
            Expect(quaternion == quaternion, Is.True);
            Expect(quaternion == this.Q0, Is.False);
        }
        #endregion

        #region Arithmetic
        [Test]
        public void Addition()
        {
            Expect(this.Q0 + this.Q1, Is.EqualTo(this.Q2));
        }
        [Test]
        public void Subtraction()
        {
            Expect(this.Q0 - this.Q0, Is.EqualTo(new QuaternionType()));
        }
        [Test]
        public void ScalarMultitplication()
        {
            Expect((-Kean.Math.Abstract<R, V>.One) * this.Q0, Is.EqualTo(-this.Q0));
        }
        [Test]
        public void Multitplication()
        {
            Expect(this.Q0 * this.Q1, Is.EqualTo(this.Q3));
        }

        #endregion
        [Test]
        public void GetValues()
        {
            V[] values = (V[])(this.Q0 as Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>);
            Expect(values[0], Is.EqualTo(33).Within(this.Precision));
            Expect(values[1], Is.EqualTo(10).Within(this.Precision));
            Expect(values[2], Is.EqualTo(-12).Within(this.Precision));
            Expect(values[3], Is.EqualTo(54.5).Within(this.Precision));
        }
        [Test]
        public void LogarithmExponential()
        {
            R roll = new R().CreateConstant(20).ToRadians();
            R pitch = new R().CreateConstant(-30).ToRadians();
            R yaw = new R().CreateConstant(45).ToRadians();
            QuaternionType quaternion = Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.CreateRotationZ(yaw) * Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.CreateRotationY(pitch) * Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.CreateRotationX(roll);
            Expect(quaternion.Exponential().Logarithm().Real.Value, Is.EqualTo(quaternion.Real.Value).Within(this.Precision));
            Expect(quaternion.Logarithm().Exponential().Real.Value, Is.EqualTo(quaternion.Real.Value).Within(this.Precision));
            Expect(quaternion.Exponential().Logarithm().Imaginary.Distance(quaternion.Imaginary).Value, Is.EqualTo(0).Within(this.Precision));
            Expect(quaternion.Logarithm().Exponential().Imaginary.Distance(quaternion.Imaginary).Value, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void Norm()
        {
            Expect(this.Q0.Norm.Value, Is.EqualTo(this.Cast(65.5991592)).Within(this.Precision));
        }
        [Test]
        public void ActionOnVector()
        {
            PointType direction = Kean.Math.Geometry3D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(Kean.Math.Abstract<R, V>.One, Kean.Math.Abstract<R, V>.One, Kean.Math.Abstract<R, V>.One);
            QuaternionType quaternion = Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.CreateRotation(new R().CreateConstant(120).ToRadians(), direction);
            PointType point = Kean.Math.Geometry3D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(new R().CreateConstant(5), new R().CreateConstant(6), new R().CreateConstant(7));
            PointType point2 = Kean.Math.Geometry3D.Abstract.Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(new R().CreateConstant(7), new R().CreateConstant(5), new R().CreateConstant(6));
            Expect((quaternion * point).Distance(point2).Value, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void Casting()
        {
            string value = "1 2 3 4";
            QuaternionType quaternion = (Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>)(new R().CreateConstant(1))  + Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.BasisImaginaryX * new R().CreateConstant(2) + Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.BasisImaginaryY * new R().CreateConstant(3) + Kean.Math.Geometry3D.Abstract.Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.BasisImaginaryZ * new R().CreateConstant(4);
            Expect(this.CastToString(quaternion), Is.EqualTo(value));
            Expect(this.CastFromString(value), Is.EqualTo(quaternion));
        }
        [Test]
        public void CastingNull()
        {
            string value = null;
            QuaternionType quaternion = null;
            Expect(this.CastToString(quaternion), Is.EqualTo(value));
            Expect(this.CastFromString(value), Is.EqualTo(quaternion));
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
                this.Addition,
                this.Subtraction,
                this.ScalarMultitplication,
                this.Multitplication,
                this.GetValues,
                this.Norm,
                this.Casting,
                this.CastingNull,
                this.LogarithmExponential,
                this.ActionOnVector
                );
        }
       
    }
}

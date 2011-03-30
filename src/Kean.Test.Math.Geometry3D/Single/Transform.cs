using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;

namespace Kean.Test.Math.Geometry3D.Single
{
    [TestFixture]
    public class Transform :
        Kean.Test.Math.Geometry3D.Abstract.Transform<Kean.Math.Geometry3D.Single.Transform, Kean.Math.Geometry3D.Single.TransformValue, Kean.Math.Geometry3D.Single.Point, Kean.Math.Geometry3D.Single.PointValue, Kean.Math.Geometry3D.Single.Size, Kean.Math.Geometry3D.Single.SizeValue, Kean.Math.Single, float>
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Transform0 = new Kean.Math.Geometry3D.Single.Transform(-1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            this.Transform1 = new Kean.Math.Geometry3D.Single.Transform(-1, 2, 3, 4, 5, 6, 7, 8, -5, 10, 11, 12);
            this.Transform2 = new Kean.Math.Geometry3D.Single.Transform(30, 32, 36, 58, 81, 96, -10, 14, 24, 128, 182, 216);
            this.Transform3 = new Kean.Math.Geometry3D.Single.Transform(-0.5f, 1, -0.5f, 1, -5, 3, -0.5f, 3.66666666666666f, -2.16666666666667f, 0, 1, -2);
            this.Point0 = new Kean.Math.Geometry3D.Single.Point(34, 10, 30);
            this.Point1 = new Kean.Math.Geometry3D.Single.Point(226, 369, 444);
        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        [Test]
        public void InverseTransform()
        {
            Kean.Math.Matrix.Single transformMatrix = (Kean.Math.Matrix.Single)(float[,])this.Transform0;
            Kean.Math.Matrix.Single transformInverseMatrix1 = transformMatrix.Inverse();
            Kean.Math.Matrix.Single transformInverseMatrix2 = (Kean.Math.Matrix.Single)(float[,])(this.Transform0.Inverse);
            Assert.That(transformInverseMatrix1.Distance(transformInverseMatrix2), Is.LessThan(0.0000001));
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
                this.GetScalingZ,
                this.GetScaling,
                this.CastToArray,
                this.MultiplicationTransformTransform,
                this.MultiplicationTransformPoint,
                this.InverseTransform
                );
        }
        public static void Test()
        {
            Transform fixture = new Transform();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

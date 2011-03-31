using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

using Kean.Core.Basis.Extension;
using Target = Kean.Math.Geometry3D.Double.Quaternion;
namespace Kean.Test.Math.Geometry3D.Double
{
    
    [TestFixture]
    public class Quaternion :
        Kean.Test.Math.Geometry3D.Abstract.Quaternion<Kean.Math.Geometry3D.Double.Transform, Kean.Math.Geometry3D.Double.TransformValue, 
        Target, Kean.Math.Geometry3D.Double.Point, Kean.Math.Geometry3D.Double.PointValue, 
        Kean.Math.Geometry3D.Double.Size, Kean.Math.Geometry3D.Double.SizeValue, Kean.Math.Matrix.Double,  
        Kean.Math.Double, double>
    {
        protected override Kean.Math.Geometry3D.Double.Quaternion CastFromString(string value)
        {
            return (Kean.Math.Geometry3D.Double.Quaternion)value;
        }
        protected override string CastToString(Kean.Math.Geometry3D.Double.Quaternion value)
        {
            return (string)value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Q0 = new Target(33, 10, -12, 54.5f);
            this.Q1 = new Target(10, 17, -10, 14.5f);
            this.Q2 = new Target(43, 27, -22, 69);
            this.Q3 = new Target(-750.25f, 1032, 331.5f, 1127.5f);
            this.P0 = new Kean.Math.Geometry3D.Double.Point(22.221f, -3.1f, 10);
            this.P1 = new Kean.Math.Geometry3D.Double.Point(12.221f, 13.1f, 20);
        }
        [Test]
        public void InverseMatrix()
        {
            Target q = Target.CreateRotationX(Kean.Math.Double.ToRadians(30)) * Target.CreateRotationY(Kean.Math.Double.ToRadians(-20)) * Target.CreateRotationZ(Kean.Math.Double.ToRadians(50));
            Kean.Math.Matrix.Double matrix = (Kean.Math.Matrix.Double)(double[,])q;
            Kean.Math.Matrix.Double matrixInverse = (Kean.Math.Matrix.Double)(double[,])(q.Inverse);
            Kean.Math.Matrix.Double matrixInverse2 = matrix.Inverse();
            Assert.That(matrixInverse.Distance(matrixInverse2), Is.LessThan(0.000001));
        }
        [Test]
        public void ExtractRotations()
        {
            Target q = new Target(0.448822250111341, -0.54640514987049, -0.448822250111341,-0.54640514987049);

        }
        protected override double Cast(double value)
        {
            return (double)value;
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
                this.Action,
                this.CastToTransform,
                this.InverseMatrix,
                this.ExtractRotations,
                this.CastingNull
                );
        }
        internal void Run(params System.Action[] tests)
        {
            foreach (System.Action test in tests)
                if (test.NotNull())
                    test();
        }

        public static void Test()
        {
            Quaternion fixture = new Quaternion();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

using Kean.Core.Basis.Extension;
namespace Kean.Test.Math.Geometry3D.Single
{
    
    [TestFixture]
    public class Quaternion :
        Kean.Test.Math.Geometry3D.Abstract.Quaternion<Kean.Math.Geometry3D.Single.Transform, Kean.Math.Geometry3D.Single.TransformValue, 
        Kean.Math.Geometry3D.Single.Quaternion, Kean.Math.Geometry3D.Single.Point, Kean.Math.Geometry3D.Single.PointValue, 
        Kean.Math.Geometry3D.Single.Size, Kean.Math.Geometry3D.Single.SizeValue, Kean.Math.Matrix.Single,  
        Kean.Math.Single, float>
    {
        protected override Kean.Math.Geometry3D.Single.Quaternion CastFromString(string value)
        {
            return (Kean.Math.Geometry3D.Single.Quaternion)value;
        }
        protected override string CastToString(Kean.Math.Geometry3D.Single.Quaternion value)
        {
            return (string)value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Q0 = new Kean.Math.Geometry3D.Single.Quaternion(33, 10, -12, 54.5f);
            this.Q1 = new Kean.Math.Geometry3D.Single.Quaternion(10, 17, -10, 14.5f);
            this.Q2 = new Kean.Math.Geometry3D.Single.Quaternion(43, 27, -22, 69);
            this.Q3 = new Kean.Math.Geometry3D.Single.Quaternion(-750.25f, 1032, 331.5f, 1127.5f);
            this.P0 = new Kean.Math.Geometry3D.Single.Point(22.221f, -3.1f, 10);
            this.P1 = new Kean.Math.Geometry3D.Single.Point(12.221f, 13.1f, 20);
        }
        [Test]
        public void InverseMatrix()
        {
            Kean.Math.Geometry3D.Single.Quaternion q = Kean.Math.Geometry3D.Single.Quaternion.CreateRotationX(Kean.Math.Single.ToRadians(30)) * Kean.Math.Geometry3D.Single.Quaternion.CreateRotationY(Kean.Math.Single.ToRadians(-20)) * Kean.Math.Geometry3D.Single.Quaternion.CreateRotationZ(Kean.Math.Single.ToRadians(50));
            Kean.Math.Matrix.Single matrix = (Kean.Math.Matrix.Single)(float[,])q;
            Kean.Math.Matrix.Single matrixInverse = (Kean.Math.Matrix.Single)(float[,])(q.Inverse);
            Kean.Math.Matrix.Single matrixInverse2 = matrix.Inverse();
            Expect(matrixInverse.Distance(matrixInverse2), Is.LessThan(0.000001));
        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        public void Run()
        {
            this.Run(
                base.Run,
                this.InverseMatrix
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

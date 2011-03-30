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
        public void ProjectToCamera()
        {
            float roll = Kean.Math.Single.ToRadians(-90);
            float pitch = Kean.Math.Single.ToRadians(0);
            float yaw = Kean.Math.Single.ToRadians(0);
            // From ref. to camera
            Kean.Math.Geometry3D.Single.Quaternion quaternion = Kean.Math.Geometry3D.Single.Quaternion.CreateRotationX(roll) * Kean.Math.Geometry3D.Single.Quaternion.CreateRotationY(pitch) * Kean.Math.Geometry3D.Single.Quaternion.CreateRotationZ(yaw);  
            Kean.Math.Geometry3D.Single.Point uav = new Kean.Math.Geometry3D.Single.Point(10, 0, -100);
            Kean.Math.Geometry3D.Single.Transform transform = (Kean.Math.Geometry3D.Single.Transform)(quaternion.Inverse) * Kean.Math.Geometry3D.Single.Transform.CreateTranslation(uav).Inverse;
            Kean.Math.Geometry3D.Single.InfinityPoint infinityPoint0 = new Kean.Math.Geometry3D.Single.InfinityPoint(1, -1, 0); // Point on the infinity line
            Kean.Math.Geometry3D.Single.InfinityPoint infinityPoint1 = new Kean.Math.Geometry3D.Single.InfinityPoint(1, 1, 0); // Point on the infinity
            Kean.Math.Geometry3D.Single.Point point = new Kean.Math.Geometry3D.Single.Point(50, 10, -100); // Point on the infinity
            
            Kean.Math.Geometry3D.Single.InfinityPoint cameraSpacePoint0 = transform * infinityPoint0; // Transform
            Kean.Math.Geometry3D.Single.InfinityPoint cameraSpacePoint1 = transform * infinityPoint1;
            Kean.Math.Geometry3D.Single.Point cameraSpacePoint = transform * point;
            // Project onto Camera plane.
            Kean.Math.Geometry2D.Single.Point cameraProjectedPoint0 = new Kean.Math.Geometry2D.Single.Point(cameraSpacePoint0.Y / cameraSpacePoint0.X, cameraSpacePoint0.Z / cameraSpacePoint0.X);
            Kean.Math.Geometry2D.Single.Point cameraProjectedPoint1 = new Kean.Math.Geometry2D.Single.Point(cameraSpacePoint1.Y / cameraSpacePoint1.X, cameraSpacePoint1.Z / cameraSpacePoint1.X);
            // Compute angle between projected line of infinity and horizontal real-axis.
            Kean.Math.Geometry2D.Single.Point e1 = new Kean.Math.Geometry2D.Single.Point(1, 0);
            Kean.Math.Geometry2D.Single.Point v = cameraProjectedPoint1 - cameraProjectedPoint0;
            float degree = Kean.Math.Single.ToDegrees(e1.Angle(v));
            float degree2 = Kean.Math.Single.ToDegrees(Kean.Math.Single.ArcusCosinus((e1 * v) / (e1.Norm * v.Norm)));
        }
        [Test]
        public void InverseMatrix()
        {
            Kean.Math.Geometry3D.Single.Quaternion q = Kean.Math.Geometry3D.Single.Quaternion.CreateRotationX(Kean.Math.Single.ToRadians(30)) * Kean.Math.Geometry3D.Single.Quaternion.CreateRotationY(Kean.Math.Single.ToRadians(-20)) * Kean.Math.Geometry3D.Single.Quaternion.CreateRotationZ(Kean.Math.Single.ToRadians(50));
            Kean.Math.Matrix.Single matrix = (Kean.Math.Matrix.Single)(float[,])q;
            Kean.Math.Matrix.Single matrixInverse = (Kean.Math.Matrix.Single)(float[,])(q.Inverse);
            Kean.Math.Matrix.Single matrixInverse2 = matrix.Inverse();
            Assert.That(matrixInverse.Distance(matrixInverse2), Is.LessThan(0.000001));
        }
        protected override float Cast(double value)
        {
            return (float)value;
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
                this.ProjectToCamera,
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

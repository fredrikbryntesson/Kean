using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

using Kean.Core.Basis.Extension;
namespace Kean.Test.Math.Geometry3D.Double
{
    
    [TestFixture]
    public class Quaternion :
        Kean.Test.Math.Geometry3D.Abstract.Quaternion<Kean.Math.Geometry3D.Double.Transform, Kean.Math.Geometry3D.Double.TransformValue, 
        Kean.Math.Geometry3D.Double.Quaternion, Kean.Math.Geometry3D.Double.Point, Kean.Math.Geometry3D.Double.PointValue, 
        Kean.Math.Geometry3D.Double.Size, Kean.Math.Geometry3D.Double.SizeValue, Kean.Math.Matrix.Double,  
        Kean.Math.Double, double>
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Q0 = new Kean.Math.Geometry3D.Double.Quaternion(33, 10, -12, 54.5f);
            this.Q1 = new Kean.Math.Geometry3D.Double.Quaternion(10, 17, -10, 14.5f);
            this.Q2 = new Kean.Math.Geometry3D.Double.Quaternion(43, 27, -22, 69);
            this.Q3 = new Kean.Math.Geometry3D.Double.Quaternion(-750.25f, 1032, 331.5f, 1127.5f);
            this.P0 = new Kean.Math.Geometry3D.Double.Point(22.221f, -3.1f, 10);
            this.P1 = new Kean.Math.Geometry3D.Double.Point(12.221f, 13.1f, 20);
        }
        [Test]
        public void ProjectToCamera()
        {
            double roll = Kean.Math.Double.ToRadians(-90);
            double pitch = Kean.Math.Double.ToRadians(0);
            double yaw = Kean.Math.Double.ToRadians(0);
            // From ref. to camera
            Kean.Math.Geometry3D.Double.Quaternion quaternion = Kean.Math.Geometry3D.Double.Quaternion.CreateRotationX(roll) * Kean.Math.Geometry3D.Double.Quaternion.CreateRotationY(pitch) * Kean.Math.Geometry3D.Double.Quaternion.CreateRotationZ(yaw);  
            Kean.Math.Geometry3D.Double.Point uav = new Kean.Math.Geometry3D.Double.Point(10, 0, -100);
            Kean.Math.Geometry3D.Double.Transform transform = (Kean.Math.Geometry3D.Double.Transform)(quaternion.Inverse) * Kean.Math.Geometry3D.Double.Transform.CreateTranslation(uav).Inverse;
            Kean.Math.Geometry3D.Double.InfinityPoint infinityPoint0 = new Kean.Math.Geometry3D.Double.InfinityPoint(1, -1, 0); // Point on the infinity line
            Kean.Math.Geometry3D.Double.InfinityPoint infinityPoint1 = new Kean.Math.Geometry3D.Double.InfinityPoint(1, 1, 0); // Point on the infinity
            Kean.Math.Geometry3D.Double.Point point = new Kean.Math.Geometry3D.Double.Point(50, 10, -100); // Point on the infinity
            
            Kean.Math.Geometry3D.Double.InfinityPoint cameraSpacePoint0 = transform * infinityPoint0; // Transform
            Kean.Math.Geometry3D.Double.InfinityPoint cameraSpacePoint1 = transform * infinityPoint1;
            Kean.Math.Geometry3D.Double.Point cameraSpacePoint = transform * point;
            // Project onto Camera plane.
            Kean.Math.Geometry2D.Double.Point cameraProjectedPoint0 = new Kean.Math.Geometry2D.Double.Point(cameraSpacePoint0.Y / cameraSpacePoint0.X, cameraSpacePoint0.Z / cameraSpacePoint0.X);
            Kean.Math.Geometry2D.Double.Point cameraProjectedPoint1 = new Kean.Math.Geometry2D.Double.Point(cameraSpacePoint1.Y / cameraSpacePoint1.X, cameraSpacePoint1.Z / cameraSpacePoint1.X);
            // Compute angle between projected line of infinity and horizontal real-axis.
            Kean.Math.Geometry2D.Double.Point e1 = new Kean.Math.Geometry2D.Double.Point(1, 0);
            Kean.Math.Geometry2D.Double.Point v = cameraProjectedPoint1 - cameraProjectedPoint0;
            double degree = Kean.Math.Double.ToDegrees(e1.Angle(v));
            double degree2 = Kean.Math.Double.ToDegrees(Kean.Math.Double.ArcusCosinus((e1 * v) / (e1.Norm * v.Norm)));
        }
        [Test]
        public void InverseMatrix()
        {
            Kean.Math.Geometry3D.Double.Quaternion q = Kean.Math.Geometry3D.Double.Quaternion.CreateRotationX(Kean.Math.Double.ToRadians(30)) * Kean.Math.Geometry3D.Double.Quaternion.CreateRotationY(Kean.Math.Double.ToRadians(-20)) * Kean.Math.Geometry3D.Double.Quaternion.CreateRotationZ(Kean.Math.Double.ToRadians(50));
            Kean.Math.Matrix.Double matrix = (Kean.Math.Matrix.Double)(double[,])q;
            Kean.Math.Matrix.Double matrixInverse = (Kean.Math.Matrix.Double)(double[,])(q.Inverse);
            Kean.Math.Matrix.Double matrixInverse2 = matrix.Inverse();
            Assert.That(matrixInverse.Distance(matrixInverse2), Is.LessThan(0.000001));
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

using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

using Kean.Core.Basis.Extension;
using Target = Kean.Math.Geometry3D.Double;
namespace Kean.Test.Math.Geometry3D.Double
{
    
    [TestFixture]
    public class Quaternion :
        Kean.Test.Math.Geometry3D.Abstract.Quaternion<Target.Transform, Target.TransformValue, 
        Target.Quaternion, Target.Point, Target.PointValue, 
        Target.Size, Target.SizeValue,  
        Kean.Math.Double, double>
    {
        protected override Target.Quaternion CastFromString(string value)
        {
            return (Target.Quaternion)value;
        }
        protected override string CastToString(Target.Quaternion value)
        {
            return (string)value;
        }
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Q0 = new Target.Quaternion(33, 10, -12, 54.5f);
            this.Q1 = new Target.Quaternion(10, 17, -10, 14.5f);
            this.Q2 = new Target.Quaternion(43, 27, -22, 69);
            this.Q3 = new Target.Quaternion(-750.25f, 1032, 331.5f, 1127.5f);
            this.P0 = new Target.Point(22.221f, -3.1f, 10);
            this.P1 = new Target.Point(12.221f, 13.1f, 20);
        }
        [Test]
        public void InverseMatrix()
        {
            Target.Quaternion q = Target.Quaternion.CreateRotationX(Kean.Math.Double.ToRadians(30)) * Target.Quaternion.CreateRotationY(Kean.Math.Double.ToRadians(-20)) * Target.Quaternion.CreateRotationZ(Kean.Math.Double.ToRadians(50));
            Kean.Math.Matrix.Double matrix = (Kean.Math.Matrix.Double)(double[,])q;
            Kean.Math.Matrix.Double matrixInverse = (Kean.Math.Matrix.Double)(double[,])(q.Inverse);
            Kean.Math.Matrix.Double matrixInverse2 = matrix.Inverse();
            Expect(matrixInverse.Distance(matrixInverse2), Is.LessThan(0.000001));
        }
        [Test]
        public void Action()
        {
            double roll = Kean.Math.Double.ToRadians(30);
            double pitch = Kean.Math.Double.ToRadians(20);
            double yaw = Kean.Math.Double.ToRadians(-45);
            Target.Quaternion quaternion = Target.Quaternion.CreateRotationZ(yaw) * Target.Quaternion.CreateRotationY(pitch) * Target.Quaternion.CreateRotationX(roll);
            Expect(quaternion.RotationX.Value, Is.EqualTo(roll).Within(this.Precision));
            Expect(quaternion.RotationY.Value, Is.EqualTo(pitch).Within(this.Precision));
            Expect(quaternion.RotationZ.Value, Is.EqualTo(yaw).Within(this.Precision));
        }
        [Test]
        public void RotationDirectionRepresentation1()
        {
            double angle = Kean.Math.Double.ToRadians(30);
            Target.Point direction = new Target.Point(1,4,7);
            direction /= direction.Norm;
            Target.Quaternion q = Target.Quaternion.CreateRotation(angle, direction);
            Expect(q.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void RotationDirectionRepresentation2()
        {
            double angle = Kean.Math.Double.ToRadians(110);
            Target.Point direction = new Target.Point(1, 4, 7);
            direction /= direction.Norm;
            Target.Quaternion q = Target.Quaternion.CreateRotation(angle, direction);
            Expect(q.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void RotationDirectionRepresentation3()
        {
            double angle = Kean.Math.Double.ToRadians(110);
            Target.Point direction = new Target.Point(0,0,5);
            direction /= direction.Norm;
            Target.Quaternion q = Target.Quaternion.CreateRotation(angle, direction);
            Expect(q.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void RotationDirectionRepresentation4()
        {
            double angle = Kean.Math.Double.ToRadians(110);
            Target.Point direction = new Target.Point(0, 0, -5);
            direction /= direction.Norm;
            Target.Quaternion q = Target.Quaternion.CreateRotation(angle, direction);
            Expect(q.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
        }
        
        [Test]
        public void Roll()
        {
            double angle = Kean.Math.Double.ToRadians(20);
            Target.Quaternion quaternion = Target.Quaternion.CreateRotationX(angle);
            Target.Transform rotation = (Target.Transform)(quaternion);
            Target.Transform rotation2 = Target.Transform.CreateRotationX(angle);
            Expect(((Kean.Math.Matrix.Double)(double[,])(rotation)).Distance((Kean.Math.Matrix.Double)(double[,])rotation2), Is.EqualTo(0).Within(this.Precision));
            Expect(quaternion.RotationX.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(quaternion.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
        }
        [Test]
        public void Pitch()
        {
            double angle = Kean.Math.Double.ToRadians(20);
            Target.Quaternion quaternion = Target.Quaternion.CreateRotationY(angle);
            Target.Transform rotation = (Target.Transform)(quaternion);
            Target.Transform rotation2 = Target.Transform.CreateRotationY(-angle);
            Expect(((Kean.Math.Matrix.Double)(double[,])(rotation)).Distance((Kean.Math.Matrix.Double)(double[,])rotation2), Is.EqualTo(0).Within(this.Precision));
            Expect(quaternion.RotationY.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(quaternion.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
        }
        [Test]
        public void Yaw()
        {
            double angle = Kean.Math.Double.ToRadians(20);
            Target.Quaternion quaternion = Target.Quaternion.CreateRotationZ(angle);
            Target.Transform rotation = (Target.Transform)(quaternion);
            Target.Transform rotation2 = Target.Transform.CreateRotationZ(angle);
            Expect(((Kean.Math.Matrix.Double)(double[,])(rotation)).Distance((Kean.Math.Matrix.Double)(double[,])rotation2), Is.EqualTo(0).Within(this.Precision));
            Expect(quaternion.RotationZ.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(quaternion.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
        }
        [Test]
        public void Action2()
        {
            double roll = Kean.Math.Double.ToRadians(20);
            double pitch = Kean.Math.Double.ToRadians(-30);
            double yaw = Kean.Math.Double.ToRadians(45);
            Target.Quaternion quaternion = Target.Quaternion.CreateRotationZ(yaw) * Target.Quaternion.CreateRotationY(pitch) * Target.Quaternion.CreateRotationX(roll);
            Target.Point rotatedPointQuaternion = quaternion * this.P0;
            Kean.Math.Matrix.Double rotation = (Kean.Math.Matrix.Double)(double[,])(quaternion);
            Kean.Math.Matrix.Double rotatedPoint = rotation * (Kean.Math.Matrix.Double)((double[])(this.P0));
            Target.Point rotatedPointRotation = (Target.Point)(double[])(rotatedPoint);
            Expect(rotatedPointQuaternion.Distance(rotatedPointRotation).Value, Is.EqualTo(0).Within(this.Precision));
            Expect(quaternion.RotationX.Value, Is.EqualTo(roll).Within(this.Precision));
            Expect(quaternion.RotationY.Value, Is.EqualTo(pitch).Within(this.Precision));
            //    Expect(quaternion.RotationZ.Value, Is.EqualTo(yaw.Value).Within(this.Precision));
        }
        [Test]
        public void CastToTransform()
        {
            double angle = Kean.Math.Double.ToRadians(20);
            Target.Quaternion quaternion = Target.Quaternion.CreateRotationX(angle);
            Target.Transform transform0 =  (Target.Transform)(quaternion);
            Target.Transform transform1 = Target.Transform.CreateRotationX(angle);
            Kean.Math.Matrix.Double matrix0 = (Kean.Math.Matrix.Double)(double[,])(Target.Transform)(transform0);
            Kean.Math.Matrix.Double matrix1 = (Kean.Math.Matrix.Double)(double[,])(Target.Transform)(transform1);
            Expect(matrix0.Distance(matrix1), Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void CastToTransformMultiplication()
        {
            double roll = Kean.Math.Double.ToRadians(20);
            double pitch = Kean.Math.Double.ToRadians(-30);
            double yaw = Kean.Math.Double.ToRadians(45);
            Target.Quaternion quaternion1 = Target.Quaternion.CreateRotationZ(yaw) * Target.Quaternion.CreateRotationY(pitch) * Target.Quaternion.CreateRotationX(roll);
            double roll2 = Kean.Math.Double.ToRadians(40);
            double pitch2 = Kean.Math.Double.ToRadians(37);
            double yaw2 = Kean.Math.Double.ToRadians(-15);
            Target.Quaternion quaternion2 = Target.Quaternion.CreateRotationZ(yaw2) * Target.Quaternion.CreateRotationY(pitch2) * Target.Quaternion.CreateRotationX(roll2);
            Target.Transform transform1 = (Target.Transform)(Target.Quaternion)(quaternion1);
            Target.Transform transform2 = (Target.Transform)(Target.Quaternion)(quaternion2);
            Target.Transform transform = (Target.Transform)(quaternion1 * quaternion2);
            Kean.Math.Matrix.Double matrix12 = (Kean.Math.Matrix.Double)(double[,])(Target.Transform)(transform1 * transform2);
            Kean.Math.Matrix.Double matrix = (Kean.Math.Matrix.Double)(double[,])(Target.Transform)(transform);
            Expect(matrix12.Distance(matrix), Is.EqualTo(0).Within(this.Precision));
        }
        
        protected override double Cast(double value)
        {
            return (double)value;
        }
        public void Run()
        {
            this.Run(
                base.Run,
                this.InverseMatrix,
                this.RotationDirectionRepresentation1,
                this.RotationDirectionRepresentation2,
                this.RotationDirectionRepresentation3,
                this.RotationDirectionRepresentation4,
                this.Action,
                this.Action2,
                this.CastToTransform,
                this.CastToTransformMultiplication,
                this.Roll,
                this.Pitch,
                this.Yaw
                );
        }
        public static void Test()
        {
            Quaternion fixture = new Quaternion();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

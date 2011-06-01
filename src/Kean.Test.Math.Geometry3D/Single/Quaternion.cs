using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

using Kean.Core.Basis.Extension;
using Target = Kean.Math.Geometry3D.Single;
namespace Kean.Test.Math.Geometry3D.Single
{

    [TestFixture]
    public class Quaternion :
        Kean.Test.Math.Geometry3D.Abstract.Quaternion<Target.Transform, Target.TransformValue,
        Target.Quaternion, Target.Point, Target.PointValue,
        Target.Size, Target.SizeValue,
        Kean.Math.Single, float>
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
            Target.Quaternion q = Target.Quaternion.CreateRotationX(Kean.Math.Single.ToRadians(30)) * Target.Quaternion.CreateRotationY(Kean.Math.Single.ToRadians(-20)) * Target.Quaternion.CreateRotationZ(Kean.Math.Single.ToRadians(50));
            Kean.Math.Matrix.Single matrix = (Kean.Math.Matrix.Single)(float[,])q;
            Kean.Math.Matrix.Single matrixInverse = (Kean.Math.Matrix.Single)(float[,])(q.Inverse);
            Kean.Math.Matrix.Single matrixInverse2 = matrix.Inverse();
            Expect(matrixInverse.Distance(matrixInverse2), Is.LessThan(0.000001));
        }
        [Test]
        public void Action()
        {
            float roll = Kean.Math.Single.ToRadians(30);
            float pitch = Kean.Math.Single.ToRadians(20);
            float yaw = Kean.Math.Single.ToRadians(-45);
            Target.Quaternion quaternion = Target.Quaternion.CreateRotationZ(yaw) * Target.Quaternion.CreateRotationY(pitch) * Target.Quaternion.CreateRotationX(roll);
            Expect(quaternion.RotationX.Value, Is.EqualTo(roll).Within(this.Precision));
            Expect(quaternion.RotationY.Value, Is.EqualTo(pitch).Within(this.Precision));
            Expect(quaternion.RotationZ.Value, Is.EqualTo(yaw).Within(this.Precision));
        }
        [Test]
        public void RotationDirectionRepresentation1()
        {
            float angle = Kean.Math.Single.ToRadians(30);
            Target.Point direction = new Target.Point(1, 4, 7);
            direction /= direction.Norm;
            Target.Quaternion q = Target.Quaternion.CreateRotation(angle, direction);
            Expect(q.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void RotationDirectionRepresentation2()
        {
            float angle = Kean.Math.Single.ToRadians(110);
            Target.Point direction = new Target.Point(1, 4, 7);
            direction /= direction.Norm;
            Target.Quaternion q = Target.Quaternion.CreateRotation(angle, direction);
            Expect(q.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void RotationDirectionRepresentation3()
        {
            float angle = Kean.Math.Single.ToRadians(110);
            Target.Point direction = new Target.Point(0, 0, 5);
            direction /= direction.Norm;
            Target.Quaternion q = Target.Quaternion.CreateRotation(angle, direction);
            Expect(q.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void RotationDirectionRepresentation4()
        {
            float angle = Kean.Math.Single.ToRadians(110);
            Target.Point direction = new Target.Point(0, 0, -5);
            direction /= direction.Norm;
            Target.Quaternion q = Target.Quaternion.CreateRotation(angle, direction);
            Expect(q.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
        }

        [Test]
        public void Roll()
        {
            float angle = Kean.Math.Single.ToRadians(20);
            Target.Quaternion quaternion = Target.Quaternion.CreateRotationX(angle);
            Target.Transform rotation = (Target.Transform)(quaternion);
            Target.Transform rotation2 = Target.Transform.CreateRotationX(angle);
            Expect(((Kean.Math.Matrix.Single)(float[,])(rotation)).Distance((Kean.Math.Matrix.Single)(float[,])rotation2), Is.EqualTo(0).Within(this.Precision));
            Expect(quaternion.RotationX.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(quaternion.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
        }
        [Test]
        public void Pitch()
        {
            float angle = Kean.Math.Single.ToRadians(20);
            Target.Quaternion quaternion = Target.Quaternion.CreateRotationY(angle);
            Target.Transform rotation = (Target.Transform)(quaternion);
            Target.Transform rotation2 = Target.Transform.CreateRotationY(-angle);
            Expect(((Kean.Math.Matrix.Single)(float[,])(rotation)).Distance((Kean.Math.Matrix.Single)(float[,])rotation2), Is.EqualTo(0).Within(this.Precision));
            Expect(quaternion.RotationY.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(quaternion.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
        }
        [Test]
        public void Yaw()
        {
            float angle = Kean.Math.Single.ToRadians(20);
            Target.Quaternion quaternion = Target.Quaternion.CreateRotationZ(angle);
            Target.Transform rotation = (Target.Transform)(quaternion);
            Target.Transform rotation2 = Target.Transform.CreateRotationZ(angle);
            Expect(((Kean.Math.Matrix.Single)(float[,])(rotation)).Distance((Kean.Math.Matrix.Single)(float[,])rotation2), Is.EqualTo(0).Within(this.Precision));
            Expect(quaternion.RotationZ.Value, Is.EqualTo(angle).Within(this.Precision));
            Expect(quaternion.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
        }
        [Test]
        public void Action2()
        {
            float roll = Kean.Math.Single.ToRadians(20);
            float pitch = Kean.Math.Single.ToRadians(-30);
            float yaw = Kean.Math.Single.ToRadians(45);
            Target.Quaternion quaternion = Target.Quaternion.CreateRotationZ(yaw) * Target.Quaternion.CreateRotationY(pitch) * Target.Quaternion.CreateRotationX(roll);
            Target.Point rotatedPointQuaternion = quaternion * this.P0;
            Kean.Math.Matrix.Single rotation = (Kean.Math.Matrix.Single)(float[,])(quaternion);
            Kean.Math.Matrix.Single rotatedPoint = rotation * (Kean.Math.Matrix.Single)((float[])(this.P0));
            Target.Point rotatedPointRotation = (Target.Point)(float[])(rotatedPoint);
            Expect(rotatedPointQuaternion.Distance(rotatedPointRotation).Value, Is.EqualTo(0).Within(this.Precision));
            Expect(quaternion.RotationX.Value, Is.EqualTo(roll).Within(this.Precision));
            Expect(quaternion.RotationY.Value, Is.EqualTo(pitch).Within(this.Precision));
            //    Expect(quaternion.RotationZ.Value, Is.EqualTo(yaw.Value).Within(this.Precision));
        }
        [Test]
        public void CastToTransform()
        {
            float angle = Kean.Math.Single.ToRadians(20);
            Target.Quaternion quaternion = Target.Quaternion.CreateRotationX(angle);
            Target.Transform transform0 = (Target.Transform)(quaternion);
            Target.Transform transform1 = Target.Transform.CreateRotationX(angle);
            Kean.Math.Matrix.Single matrix0 = (Kean.Math.Matrix.Single)(float[,])(Target.Transform)(transform0);
            Kean.Math.Matrix.Single matrix1 = (Kean.Math.Matrix.Single)(float[,])(Target.Transform)(transform1);
            Expect(matrix0.Distance(matrix1), Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void CastToTransformMultiplication()
        {
            float roll = Kean.Math.Single.ToRadians(20);
            float pitch = Kean.Math.Single.ToRadians(-30);
            float yaw = Kean.Math.Single.ToRadians(45);
            Target.Quaternion quaternion1 = Target.Quaternion.CreateRotationZ(yaw) * Target.Quaternion.CreateRotationY(pitch) * Target.Quaternion.CreateRotationX(roll);
            float roll2 = Kean.Math.Single.ToRadians(40);
            float pitch2 = Kean.Math.Single.ToRadians(37);
            float yaw2 = Kean.Math.Single.ToRadians(-15);
            Target.Quaternion quaternion2 = Target.Quaternion.CreateRotationZ(yaw2) * Target.Quaternion.CreateRotationY(pitch2) * Target.Quaternion.CreateRotationX(roll2);
            Target.Transform transform1 = (Target.Transform)(Target.Quaternion)(quaternion1);
            Target.Transform transform2 = (Target.Transform)(Target.Quaternion)(quaternion2);
            Target.Transform transform = (Target.Transform)(quaternion1 * quaternion2);
            Kean.Math.Matrix.Single matrix12 = (Kean.Math.Matrix.Single)(float[,])(Target.Transform)(transform1 * transform2);
            Kean.Math.Matrix.Single matrix = (Kean.Math.Matrix.Single)(float[,])(Target.Transform)(transform);
            Expect(matrix12.Distance(matrix), Is.EqualTo(0).Within(this.Precision));
        }

        protected override float Cast(double value)
        {
            return (float)value;
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

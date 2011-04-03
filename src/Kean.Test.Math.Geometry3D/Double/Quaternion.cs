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
        public void Action()
        {
            double roll = Kean.Math.Double.ToRadians(30);
            double pitch = Kean.Math.Double.ToRadians(20);
            double yaw = Kean.Math.Double.ToRadians(-45);
            Target quaternion = Target.CreateRotationZ(yaw) * Target.CreateRotationY(pitch) * Target.CreateRotationX(roll);
            Assert.That(quaternion.RotationX.Value, Is.EqualTo(roll).Within(this.Precision));
            Assert.That(quaternion.RotationY.Value, Is.EqualTo(pitch).Within(this.Precision));
            Assert.That(quaternion.RotationZ.Value, Is.EqualTo(yaw).Within(this.Precision));
        }
        [Test]
        public void RotationDirectionRepresentation1()
        {
            double angle = Kean.Math.Double.ToRadians(30);
            Kean.Math.Geometry3D.Double.Point direction = new Kean.Math.Geometry3D.Double.Point(1,4,7);
            direction /= direction.Norm;
            Target q = Target.CreateRotation(angle, direction);
            Assert.That(q.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
            Assert.That(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void RotationDirectionRepresentation2()
        {
            double angle = Kean.Math.Double.ToRadians(110);
            Kean.Math.Geometry3D.Double.Point direction = new Kean.Math.Geometry3D.Double.Point(1, 4, 7);
            direction /= direction.Norm;
            Target q = Target.CreateRotation(angle, direction);
            Assert.That(q.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
            Assert.That(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void RotationDirectionRepresentation3()
        {
            double angle = Kean.Math.Double.ToRadians(110);
            Kean.Math.Geometry3D.Double.Point direction = new Kean.Math.Geometry3D.Double.Point(0,0,5);
            direction /= direction.Norm;
            Target q = Target.CreateRotation(angle, direction);
            Assert.That(q.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
            Assert.That(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
        }
        [Test]
        public void RotationDirectionRepresentation4()
        {
            double angle = Kean.Math.Double.ToRadians(110);
            Kean.Math.Geometry3D.Double.Point direction = new Kean.Math.Geometry3D.Double.Point(0, 0, -5);
            direction /= direction.Norm;
            Target q = Target.CreateRotation(angle, direction);
            Assert.That(q.Rotation.Value, Is.EqualTo(angle).Within(this.Precision));
            Assert.That(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
        }
        protected override double Cast(double value)
        {
            return (double)value;
        }
        public void Run()
        {
            this.Run(
                base.Run,
                this.CastToTransform,
                this.InverseMatrix,
                this.RotationDirectionRepresentation1,
                this.RotationDirectionRepresentation2,
                this.RotationDirectionRepresentation3,
                this.RotationDirectionRepresentation4,
                this.Action
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

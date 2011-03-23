using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry3D.Single
{
    [TestFixture]
    public class Quaternion :
        Kean.Test.Math.Geometry3D.Abstract.Quaternion<Kean.Math.Geometry3D.Single.Quaternion, Kean.Math.Matrix.Single, Kean.Math.Geometry3D.Single.Point, Kean.Math.Single, float>
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
        protected override float Cast(double value)
        {
            return (float)value;
        }
        public static void Test()
        {
            Quaternion fixture = new Quaternion();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

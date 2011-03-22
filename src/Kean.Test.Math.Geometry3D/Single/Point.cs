using System;
using NUnit.Framework;

namespace Kean.Test.Math.Geometry3D.Single
{
    [TestFixture]
    public class Point :
        Kean.Test.Math.Geometry3D.Abstract.Point<Kean.Math.Geometry3D.Single.Point, 
        Kean.Math.Single, float>
    {
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            this.Vector0 = new Kean.Math.Geometry3D.Single.Point(22.221f, -3.1f, 10);
            this.Vector1 = new Kean.Math.Geometry3D.Single.Point(12.221f, 13.1f, 20);
            this.Vector2 = new Kean.Math.Geometry3D.Single.Point(34.442f, 10.0f, 30);
        }
        protected override float Cast(double value)
        {
            return (float)value;
        }
        public static void Test()
        {
            Point fixture = new Point();
            fixture.FixtureSetup();
            fixture.Run();
        }
    }
}

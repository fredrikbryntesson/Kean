using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Test.Math.Matrix
{
    public class Single : Abstract<Kean.Math.Matrix.Single, Kean.Math.Single, float>
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.ZeroOrderThree = new Kean.Math.Matrix.Single(3);
            this.TwoTwo = new Kean.Math.Matrix.Single(2);
            this.TwoTwo[0, 0] = 1;
            this.TwoTwo[1, 0] = 2;
            this.TwoTwo[0, 1] = 3;
            this.TwoTwo[1, 1] = 4;
            this.TwoThree = new Kean.Math.Matrix.Single(2, 3, new float[] { -1, -2, -3, -4, -5, -6, -7 });
            this.OneThree = new Kean.Math.Matrix.Single(1, 3, new float[] { -1, -2, -3, -4, -5, -6, -7 });
            this.ThreeThree = new Kean.Math.Matrix.Single(3, 3, new float[] { -1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }
        public static void Test()
        {
            Single fixture = new Single();
            fixture.SetupFixture();
            fixture.Run();
        }

    }
}

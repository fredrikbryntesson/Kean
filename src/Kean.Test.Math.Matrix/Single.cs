using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Test.Math.Matrix
{
    public class Single: Abstract<Kean.Math.Matrix.Single, Kean.Math.Single, float>
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.ZeroOrderThree = new Kean.Math.Matrix.Single(3);
            this.OrderTwo = new Kean.Math.Matrix.Single(2);
            this.OrderTwo[0, 0] = 1;
            this.OrderTwo[1, 0] = 2;
            this.OrderTwo[0, 1] = 3;
            this.OrderTwo[1, 1] = 4;
        }
        public static void Test()
        {
            Single fixture = new Single();
            fixture.SetupFixture();
            fixture.Run();
        }

    }
}

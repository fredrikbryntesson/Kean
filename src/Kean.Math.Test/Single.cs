using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Math.Test
{
    public class Single : Abstract<Kean.Math.Single, float>
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.A = new Kean.Math.Single(3.5f);
            this.B = 6.6f;
        }
        public static void Test()
        {
            Single fixture = new Single();
            fixture.SetupFixture();
            fixture.Run();
        }

    }
}
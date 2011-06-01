using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Test.Math
{
    public class Double : Abstract<Kean.Math.Double, double>
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.A = new Kean.Math.Double(3.5);
            this.B = 6.6;
        }
        public static void Test()
        {
            Double fixture = new Double();
            fixture.SetupFixture();
            fixture.Run();
        }

    }
}
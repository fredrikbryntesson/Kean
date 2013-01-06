using System;
using NUnit.Framework;
using NUnit.Framework;

namespace Kean.Math.Test
{
    public class Double : Abstract<Double, Kean.Math.Double, double>
    {
        [TestFixtureSetUp]
        public override void Setup()
        {
            this.A = new Kean.Math.Double(3.5);
            this.B = 6.6;
        }
    }
}
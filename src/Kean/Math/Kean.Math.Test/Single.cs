using System;
using NUnit.Framework;
using NUnit.Framework;

namespace Kean.Math.Test
{
    public class Single : Abstract<Single, Kean.Math.Single, float>
    {
        [TestFixtureSetUp]
        public override void Setup()
        {
            this.A = new Kean.Math.Single(3.5f);
            this.B = 6.6f;
        }
    }
}
using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;
using Target = Kean.Math.Regression.Ransac;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Core.Collection;


namespace Kean.Math.Regression.Test.Filter
{
    public class Kalman :
    Kean.Test.Fixture<Kalman>
    {
        protected override void Run()
        {
            this.Run(
                this.Point
                );
        }
        string prefix = "Kean.Math.Regression.Filter.Kalman.";
        [Test]
        public void Point()
        {

        }
    }
}

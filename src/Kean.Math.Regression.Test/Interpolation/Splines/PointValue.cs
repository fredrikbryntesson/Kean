using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;
using Kean.Core;

namespace Kean.Math.Regression.Test.Interpolation.Splines
{
    public class Geometry2D :
        AssertionHelper
    {
        string prefix = "Kean.Math.Regression.Test.Interpolation.Splines.Geometry2D.";
        [Test]
        public void PointValue()
        {
            Tuple<double, Kean.Math.Geometry2D.Double.PointValue>[] measures = new Tuple<double, Kean.Math.Geometry2D.Double.PointValue>[10];
            Kean.Math.Regression.Interpolation.Splines.Geometry2D.PointValue interpolate = new Kean.Math.Regression.Interpolation.Splines.Geometry2D.PointValue(Kean.Math.Regression.Interpolation.Splines.Method.Natural, measures);

        }
        public void Run()
        {
            this.Run(
                );
        }
        internal void Run(params System.Action[] tests)
        {
            foreach (System.Action test in tests)
                if (test.NotNull())
                    test();
        }
        public static void Test()
        {
            Geometry2D fixture = new Geometry2D();
            fixture.Run();
        }
    }
}

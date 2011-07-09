using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;
using Target = Kean.Math.Random.Ransac;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Core.Collection;

namespace Kean.Test.Math.Random.Ransac
{
    public class Double : 
        AssertionHelper
    {
        [Test]
        public void Model1()
        {
            Target.Model<double, double, Kean.Math.Matrix.Double> model = new Target.Model<double, double, Kean.Math.Matrix.Double>()
            {
                RequiredMeasures = 2,
                Estimate = data =>
                {
                    Kean.Math.Matrix.Double result = new Kean.Math.Matrix.Double(2, 1);
                    double deltaX = data[0].Item1 - data[1].Item1;
                    double deltaY = data[0].Item2 - data[1].Item2;
                    double k = deltaX != 0 ? deltaY / deltaX : 0;
                    double m = data[0].Item2 - k * data[0].Item1;
                    result[0, 0] = k;
                    result[1, 0] = m;
                    return result;
                },
                FitsWell = 10,
                Threshold = .1,
                Map = (x, t) => 
                {
                    return t[0, 0] * x + t[1, 0];
                },
                Metric = (y1, y2) => Kean.Math.Double.Absolute(y1-y2)
            };
            Target.Estimate<double, double, Kean.Math.Matrix.Double> estimate = 
                new Target.Estimate<double, double, Kean.Math.Matrix.Double>(model, 50);
            Collection.IList<Kean.Core.Basis.Tuple<double, double>> points = 
                new Collection.List<Kean.Core.Basis.Tuple<double, double>>();
            double kk = 5;
            double mm = 10;
            System.Random r = new System.Random((int)DateTime.Now.Ticks);
            for(double x = 0; x < 30; x++)
            {
                double y = kk * x + mm;
                points.Add(Kean.Core.Basis.Tuple.Create<double, double>(x, y));
            }
            for (double x = 30; x < 40; x++)
            {
                double y = kk * x + mm + r.NextDouble()-0.5;
                points.Add(Kean.Core.Basis.Tuple.Create<double, double>(x, y));
            }
            estimate.Load(points);
            Target.Estimation<double, double, Kean.Math.Matrix.Double> est = estimate.Compute();
        }
        public void Run()
        {
            this.Run(
                this.Model1
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
            Double fixture = new Double();
            fixture.Run();
        }

    }
}

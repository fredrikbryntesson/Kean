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
        [Test]
        public void Model2()
        {
            Target.Model<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double> model = new Target.Model<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double>()
            {
                RequiredMeasures = 2,
                Estimate = data =>
                {
                    Kean.Math.Matrix.Double result = new Kean.Math.Matrix.Double(4, 1);
                    Geometry2D.Double.PointValue xDelta = data[0].Item1 - data[1].Item1;
                    Geometry2D.Double.PointValue yDelta = data[0].Item2 - data[1].Item2;
                    double angle = ((Geometry2D.Double.Point)xDelta).Angle(yDelta);
                    double scale = xDelta.Length != 0 ? yDelta.Length / xDelta.Length : 1;
                    Geometry2D.Double.PointValue x = data[0].Item1;
                    Geometry2D.Double.PointValue translation = data[0].Item2 - new Geometry2D.Double.PointValue(
                    scale * Kean.Math.Double.Cosinus(angle) * x.X - scale * Kean.Math.Double.Sinus(angle) * x.Y,
                    scale * Kean.Math.Double.Sinus(angle) * x.X + scale * Kean.Math.Double.Cosinus(angle) * x.Y);
                    result[0, 0] = scale;
                    result[1, 0] = angle;
                    result[2, 0] = translation.X;
                    result[3, 0] = translation.Y;
                    return result;
                },
                FitsWell = 10,
                Threshold = 1,
                Map = (x, t) =>
                {
                    double scale = t[0, 0];
                    double theta = t[1, 0];
                    double xt = t[2, 0];
                    double yt = t[3,0];
                    Geometry2D.Double.PointValue result = new Geometry2D.Double.PointValue(
                  scale * Kean.Math.Double.Cosinus(theta) * x.X - scale * Kean.Math.Double.Sinus(theta) * x.Y + xt,
                  scale * Kean.Math.Double.Sinus(theta) * x.X + scale * Kean.Math.Double.Cosinus(theta) * x.Y + yt);
                    return result;
                },
                Metric = (y1, y2) => (y1-y2).Length
            };
            Target.Estimate<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double> estimate =
                new Target.Estimate<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double>(model, 1250);
            Collection.IList<Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>> points =
                new Collection.List<Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>>();
            double s = 3;
            double thetaAngle = Kean.Math.Double.ToRadians(10);
            double xTranslation = 20;
            double yTranslation = 30;
            Kean.Math.Random.Generator.Normal normal = new Kean.Math.Random.Generator.Normal();
            for (double i = 0; i < 100; i++)
            {
                Geometry2D.Double.PointValue x = new Geometry2D.Double.PointValue(i, i);
                Geometry2D.Double.PointValue y = new Geometry2D.Double.PointValue(
                    s * Kean.Math.Double.Cosinus(thetaAngle) * x.X - s * Kean.Math.Double.Sinus(thetaAngle) * x.Y + xTranslation,
                    s * Kean.Math.Double.Sinus(thetaAngle) * x.X + s * Kean.Math.Double.Cosinus(thetaAngle) * x.Y + yTranslation);
                double[] xdd = normal.NextDoublePoint(0,0.1);
                double[] ydd = normal.NextDoublePoint(0,0.1);
                Geometry2D.Double.PointValue xd = new Kean.Math.Geometry2D.Double.PointValue(xdd[0], xdd[1]);
                Geometry2D.Double.PointValue yd = new Kean.Math.Geometry2D.Double.PointValue(ydd[0], ydd[1]);
                points.Add(Kean.Core.Basis.Tuple.Create<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>(x + xd, y + yd));
            }
            estimate.Load(points);
            Target.Estimation<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double> est = estimate.Compute();
        }
        public void Run()
        {
            this.Run(
                this.Model1,
                this.Model2
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

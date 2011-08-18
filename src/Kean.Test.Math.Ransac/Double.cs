using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;
using Target = Kean.Math.Ransac;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Core.Collection;

namespace Kean.Test.Math.Ransac
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
                Threshold = 10,
                Map = (t, x) =>
                {
                    return t[0, 0] * x + t[1, 0];
                },
                Metric = (y1, y2) => Kean.Math.Double.Absolute(y1 - y2)
            };
            Target.Estimator<double, double, Kean.Math.Matrix.Double> estimate =
                new Target.Estimator<double, double, Kean.Math.Matrix.Double>(model, 100);
            Collection.IList<Kean.Core.Basis.Tuple<double, double>> points =
                new Collection.List<Kean.Core.Basis.Tuple<double, double>>();
            double kk = 20;
            double mm = 10;
            System.Random r = new System.Random((int)DateTime.Now.Ticks);
            for (double x = 0; x < 40; x++)
            {
                double y = kk * x + mm + 40 * (r.NextDouble() - 0.5);
                points.Add(Kean.Core.Basis.Tuple.Create<double, double>(x, y));
            }
            estimate.Load(points);
            Target.Estimation<double, double, Kean.Math.Matrix.Double> best = estimate.Compute();
            if (best.NotNull())
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
                file.WriteLine("clear all;");
                file.WriteLine("close all;");
                string pointsExport = "";
                foreach (Kean.Core.Basis.Tuple<double, double> point in points)
                    pointsExport += Kean.Math.Double.ToString(point.Item1) + " " + Kean.Math.Double.ToString(point.Item2) + ";";
                pointsExport = pointsExport.TrimEnd(';');
                file.WriteLine("points = [" + pointsExport + "];");
                string consensusExport = "";
                foreach (Kean.Core.Basis.Tuple<double, double> point in best.ConsensusSet)
                    consensusExport += Kean.Math.Double.ToString(point.Item1) + " " + Kean.Math.Double.ToString(point.Item2) + ";";
                consensusExport = consensusExport.TrimEnd(';');
                file.WriteLine("consensus = [" + consensusExport + "];");
                file.WriteLine("correct = [" + Kean.Math.Double.ToString(kk) + " " + Kean.Math.Double.ToString(mm) + "];");
                file.WriteLine("bestmodel = [" + Kean.Math.Double.ToString(best.Mapping[0, 0]) + " " + Kean.Math.Double.ToString(best.Mapping[1, 0]) + "];");
                file.WriteLine("scatter(points(:,1), points(:,2),'b');");
                file.WriteLine("hold on;");
                file.WriteLine("scatter(consensus(:,1), consensus(:,2),'r');");
                file.WriteLine("xmin = min(points(:,1)); xmax = max(points(:,1)); ymin = bestmodel(1) * xmin + bestmodel(2); ymax = bestmodel(1) * xmax + bestmodel(2);");
                file.WriteLine("plot([xmin, xmax], [ymin, ymax], 'r');");
                file.WriteLine("xmin = min(points(:,1)); xmax = max(points(:,1)); ymin = bestmodel(1) * xmin + bestmodel(2); ymax = correct(1) * xmax + correct(2);");
                file.WriteLine("plot([xmin, xmax], [ymin, ymax], 'g');");
                file.WriteLine("xlabel(strcat('inliers = ', num2str(length(consensus(:,1))),' k = ',num2str(bestmodel(1)), ' m = ', num2str(bestmodel(2))))");
                file.Close();
            }


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
                FitsWell = 5,
                Threshold = 12,
                Map = (t, x) =>
                {
                    double scale = t[0, 0];
                    double theta = t[1, 0];
                    double xt = t[2, 0];
                    double yt = t[3, 0];
                    Geometry2D.Double.PointValue result = new Geometry2D.Double.PointValue(
                  scale * Kean.Math.Double.Cosinus(theta) * x.X - scale * Kean.Math.Double.Sinus(theta) * x.Y + xt,
                  scale * Kean.Math.Double.Sinus(theta) * x.X + scale * Kean.Math.Double.Cosinus(theta) * x.Y + yt);
                    return result;
                },
                Metric = (y1, y2) => (y1 - y2).Length
            };
            Target.Estimator<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double> estimate =
                new Target.Estimator<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double>(model, 50);
            Collection.IList<Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>> points =
                new Collection.List<Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>>();
            double s = 3;
            double thetaAngle = Kean.Math.Double.ToRadians(10);
            double xTranslation = 20;
            double yTranslation = 30;
            Kean.Math.Random.Double.Normal normal = new Kean.Math.Random.Double.Normal();
            for (double i = 0; i < 30; i++)
            {
                Geometry2D.Double.PointValue x = new Geometry2D.Double.PointValue(i, i);
                Geometry2D.Double.PointValue y = new Geometry2D.Double.PointValue(
                    s * Kean.Math.Double.Cosinus(thetaAngle) * x.X - s * Kean.Math.Double.Sinus(thetaAngle) * x.Y + xTranslation,
                    s * Kean.Math.Double.Sinus(thetaAngle) * x.X + s * Kean.Math.Double.Cosinus(thetaAngle) * x.Y + yTranslation);
                double[] xdd = normal.Generate(2);
                double[] ydd =  normal.Generate(2);
                Geometry2D.Double.PointValue xd = new Kean.Math.Geometry2D.Double.PointValue(xdd[0], xdd[1]);
                Geometry2D.Double.PointValue yd = new Kean.Math.Geometry2D.Double.PointValue(ydd[0], ydd[1]);
                points.Add(Kean.Core.Basis.Tuple.Create<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>(x + xd, y + yd));
            }
            points.Add(Kean.Core.Basis.Tuple.Create<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>(new Geometry2D.Double.PointValue(30, 30), new Geometry2D.Double.PointValue(70, 70)));
            estimate.Load(points);
            Target.Estimation<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double> best = estimate.Compute();

            if (best.NotNull())
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
                file.WriteLine("clear all;");
                file.WriteLine("close all;");
                string before = "";
                string after = "";
                foreach (Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue> point in points)
                {
                    before += point.Item1.ToString() + "; ";
                    after += point.Item2.ToString() + "; ";
                }
                before = before.TrimEnd(';');
                after = after.TrimEnd(';');
                file.WriteLine("before = [" + before + "];");
                file.WriteLine("after = [" + after + "];");
                file.WriteLine("scatter(before(:,1),before(:,2),'b');");
                file.WriteLine("hold on;");
                file.WriteLine("scatter(after(:,1),after(:,2),'r');");
                string consensus = "";
                foreach (Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue> point in best.ConsensusSet)
                    consensus += point.Item2.ToString() + "; ";
                consensus = consensus.TrimEnd(';');
                file.WriteLine("consensus = [" + consensus + "];");
                file.WriteLine("scatter(consensus(:,1),consensus(:,2),'g');");
                file.WriteLine("bestmodel = [" + best.Mapping + "];");
                file.WriteLine("xlabel(strcat(' s= ',num2str(bestmodel(1)), ' angle= ', num2str(180 * bestmodel(2) / pi), ' x= ', num2str(bestmodel(3)), ' y= ', num2str(bestmodel(4))))");
                file.Close();
            }

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

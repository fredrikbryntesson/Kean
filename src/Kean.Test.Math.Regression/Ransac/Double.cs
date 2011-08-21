using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Basis.Extension;
using Target = Kean.Math.Regression.Ransac;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Core.Collection;

namespace Kean.Test.Math.Regression.Ransac
{
    public class Double :
        AssertionHelper
    {
        [Test]
        public void RobustPolynomialRegression()
        {
            int degree = 2 + 1;
            System.Func<Kean.Math.Matrix.Double, double, double> map = (t, x) =>
            {
                double result = 0;
                for (int i = 0; i < t.Dimensions.Height; i++)
                    result += t[0, i] * Kean.Math.Double.Power(x, i);
                return result;
            };
            Target.Model<double, double, Kean.Math.Matrix.Double> model = new Target.Model<double, double, Kean.Math.Matrix.Double>()
            {
                RequiredMeasures = 10,
                Estimate = data =>
                {
                    Kean.Math.Matrix.Double result = null;
                    int n = data.Count;
                    Kean.Math.Matrix.Double a = new Kean.Math.Matrix.Double(degree, n);
                    Kean.Math.Matrix.Double b = new Kean.Math.Matrix.Double(1, n);
                    for (int i = 0; i < n; i++)
                    {
                        double x = data[i].Item1;
                        b[0, i] = data[i].Item2;
                        for (int j = 0; j < degree; j++)
                            a[j, i] = Kean.Math.Double.Power(x, j);
                    }
                    result = a.SolveLup(b) ?? new Kean.Math.Matrix.Double(1, degree);
                    return result;
                },
                FitsWell = 45,
                Threshold = 900,
                Map = map,
                Metric = (y1, y2) => Kean.Math.Double.Squared(y1 - y2)
            };
            Target.Estimator<double, double, Kean.Math.Matrix.Double> estimate =
                new Target.Estimator<double, double, Kean.Math.Matrix.Double>(model, 120);
            Collection.IList<Kean.Core.Basis.Tuple<double, double>> points =
                new Collection.List<Kean.Core.Basis.Tuple<double, double>>();
            Kean.Math.Matrix.Double coefficients = new Kean.Math.Matrix.Double(1, degree, new double[] { 20, 10, 5, 10});
            Kean.Math.Random.Double.Normal generator = new Kean.Math.Random.Double.Normal(0, 30);
            for (double x = -10; x < 10; x+=0.1)
            {
                double y = map(coefficients, x) + generator.Generate();
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
                foreach (Kean.Core.Basis.Tuple<double, double> point in best.Inliers)
                    consensusExport += Kean.Math.Double.ToString(point.Item1) + " " + Kean.Math.Double.ToString(point.Item2) + ";";
                consensusExport = consensusExport.TrimEnd(';');
                file.WriteLine("consensus = [" + consensusExport + "];");

                string bestModelExport = "";
                for (int y = 0; y < degree; y++)
                    bestModelExport += Kean.Math.Double.ToString(best.Mapping[0, y]) + ";";
                bestModelExport = bestModelExport.TrimEnd(';');
                file.WriteLine("bestModel = [" + bestModelExport + "];");
                file.WriteLine("scatter(points(:,1), points(:,2),'b');");

                string correctModelExport = "";
                for (int y = 0; y < degree; y++)
                    correctModelExport += Kean.Math.Double.ToString(coefficients[0, y]) + ";";
                correctModelExport = correctModelExport.TrimEnd(';');
                file.WriteLine("correctModel = [" + correctModelExport + "];");

                file.WriteLine("hold on;");
                file.WriteLine("scatter(consensus(:,1), consensus(:,2),'r');");
                file.WriteLine("plot(points(:,1), polyval(fliplr(correctModel'),points(:,1)), 'r');");
                file.WriteLine("plot(points(:,1), polyval(fliplr(bestModel'),points(:,1)), 'g');");
                file.Close();
            }
        }
        [Test]
        public void ScaleRotationTranslationRegression()
        {
            Target.Model<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double> model = new Target.Model<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double>()
            {
                RequiredMeasures = 5,
                Estimate = data =>
                {
                    Kean.Math.Matrix.Double result = null;
                    int count = data.Count;
                    Kean.Math.Matrix.Double a = new Kean.Math.Matrix.Double(4, 2 * count);
                    Kean.Math.Matrix.Double b = new Kean.Math.Matrix.Double(1, 2 * count);
                    int j = 0;
                    for(int i = 0; i < count; i++)
                    {
                        Geometry2D.Double.PointValue previous = data[i].Item1;
                        Geometry2D.Double.PointValue y = data[i].Item2;
                        a[0, j] = previous.X;
                        a[1, j] = -previous.Y;
                        a[2, j] = 1;
                        a[3, j] = 0;
                        b[0, j++] = y.X;
                        a[0, j] = previous.Y;
                        a[1, j] = previous.X;
                        a[2, j] = 0;
                        a[3, j] = 1;
                        b[0, j++] = y.Y;
                    }
                    Kean.Math.Matrix.Double estimation  = a.Solve(b);
                    if (estimation.NotNull())
                    {
                        double angle = Kean.Math.Double.ArcusTangensExtended(estimation[0, 1], estimation[0, 0]);
                        double scale = Kean.Math.Double.SquareRoot(Kean.Math.Double.Squared(estimation[0, 0]) + Kean.Math.Double.Squared(estimation[0, 1]));
                        Geometry2D.Double.PointValue translation = new Geometry2D.Double.PointValue(estimation[0, 2], estimation[0, 3]);
                        result = new Kean.Math.Matrix.Double(1, 4, new double[] { scale, angle, translation.X, translation.Y });
                    }
                    else
                        result = new Kean.Math.Matrix.Double(1, 4);
                    return result;
                },
                FitsWell = 10,
                Threshold = 20,
                Map = (t, x) =>
                {
                    double scale = t[0, 0];
                    double theta = t[0, 1];
                    double xt = t[0, 2];
                    double yt = t[0, 3];
                    Geometry2D.Double.PointValue result = new Geometry2D.Double.PointValue(
                    scale * Kean.Math.Double.Cosinus(theta) * x.X - scale * Kean.Math.Double.Sinus(theta) * x.Y + xt,
                    scale * Kean.Math.Double.Sinus(theta) * x.X + scale * Kean.Math.Double.Cosinus(theta) * x.Y + yt);
                    return result;
                },
                Metric = (y1, y2) => Kean.Math.Double.Squared((y1 - y2).Length)
            };
            Target.Estimator<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double> estimate =
                new Target.Estimator<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double>(model, 20);
            Collection.IList<Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>> previousCurrentPoints =
                new Collection.List<Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>>();
            double s = 2;
            double thetaAngle = Kean.Math.Double.ToRadians(5);
            double xTranslation = 7;
            double yTranslation = 10;
            Kean.Math.Random.Double.Normal normal = new Kean.Math.Random.Double.Normal(0,1);
            Kean.Core.Collection.IList<Geometry2D.Double.PointValue> previousPoints = new Kean.Core.Collection.List<Geometry2D.Double.PointValue>();
            for (int x = -100; x < 100; x+=20)
                for (int y = -100; y < 100; y+=20)
                    previousPoints.Add(new Geometry2D.Double.PointValue(x, y));
            for (int i = 0; i < previousPoints.Count; i++)
            {
                Geometry2D.Double.PointValue x = previousPoints[i];
                Geometry2D.Double.PointValue y = new Geometry2D.Double.PointValue(
                    s * Kean.Math.Double.Cosinus(thetaAngle) * x.X - s * Kean.Math.Double.Sinus(thetaAngle) * x.Y + xTranslation,
                    s * Kean.Math.Double.Sinus(thetaAngle) * x.X + s * Kean.Math.Double.Cosinus(thetaAngle) * x.Y + yTranslation);
                double[] xdd = normal.Generate(2);
                double[] ydd = normal.Generate(2);
                Geometry2D.Double.PointValue xd = new Kean.Math.Geometry2D.Double.PointValue(xdd[0], xdd[1]);
                Geometry2D.Double.PointValue yd = new Kean.Math.Geometry2D.Double.PointValue(ydd[0], ydd[1]);
                previousCurrentPoints.Add(Kean.Core.Basis.Tuple.Create<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>(x + xd, y + yd));
            }
            previousCurrentPoints.Add(Kean.Core.Basis.Tuple.Create<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>(new Geometry2D.Double.PointValue(130, 130), new Geometry2D.Double.PointValue(720, 70)));
            estimate.Load(previousCurrentPoints);
            Target.Estimation<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Kean.Math.Matrix.Double> best = estimate.Compute();

            if (best.NotNull())
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
                file.WriteLine("clear all;");
                file.WriteLine("close all;");
                string before = "";
                string after = "";
                foreach (Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue> point in previousCurrentPoints)
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
                foreach (Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue> point in best.Inliers)
                    consensus += point.Item2.ToString() + "; ";
                consensus = consensus.TrimEnd(';');
                file.WriteLine("consensus = [" + consensus + "];");
                file.WriteLine("scatter(consensus(:,1),consensus(:,2),'g');");
                file.WriteLine("bestmodel = [" + best.Mapping + "];");
                file.WriteLine("xlabel(strcat(' s= ',num2str(bestmodel(1)), ' angle= ', num2str(180 * bestmodel(2) / pi), ' x= ', num2str(bestmodel(3)), ' y= ', num2str(bestmodel(4))))");
                file.Close();
            }
        }
        [Test]
        public void TranslationRegression()
        {
            Target.Model<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Geometry2D.Double.PointValue> model = new Target.Model<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>()
            {
                RequiredMeasures = 5,
                Estimate = data =>
                {
                    int count = data.Count;
                    Geometry2D.Double.PointValue result = new Geometry2D.Double.PointValue();
                    for(int i = 0; i < count; i++)
                        result += data[i].Item2 - data[i].Item1;
                    result.X /= (double)count;
                    result.Y /= (double)count;
                    return result;
                },
                FitsWell = 10,
                Threshold = 8,
                Map = (t, x) => t + x,
                Metric = (y1, y2) => Kean.Math.Double.Squared((y1 - y2).Length)
            };
            Target.Estimator<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Geometry2D.Double.PointValue> estimate =
                new Target.Estimator<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>(model, 20);
            Collection.IList<Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>> previousCurrentPoints =
                new Collection.List<Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>>();
            Geometry2D.Double.PointValue translation = new Geometry2D.Double.PointValue(7, 10);
            Kean.Math.Random.Double.Normal normal = new Kean.Math.Random.Double.Normal(0, 2);
            Kean.Core.Collection.IList<Geometry2D.Double.PointValue> previousPoints = new Kean.Core.Collection.List<Geometry2D.Double.PointValue>();
            for (int x = -100; x < 100; x += 20)
                for (int y = -100; y < 100; y += 20)
                    previousPoints.Add(new Geometry2D.Double.PointValue(x, y));
            for (int i = 0; i < previousPoints.Count; i++)
            {
                Geometry2D.Double.PointValue x = previousPoints[i];
                Geometry2D.Double.PointValue y = translation + x;
                double[] xdd = normal.Generate(2);
                double[] ydd = normal.Generate(2);
                Geometry2D.Double.PointValue xd = new Kean.Math.Geometry2D.Double.PointValue(xdd[0], xdd[1]);
                Geometry2D.Double.PointValue yd = new Kean.Math.Geometry2D.Double.PointValue(ydd[0], ydd[1]);
                previousCurrentPoints.Add(Kean.Core.Basis.Tuple.Create<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>(x + xd, y + yd));
            }
            previousCurrentPoints.Add(Kean.Core.Basis.Tuple.Create<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue>(new Geometry2D.Double.PointValue(107, 107), new Geometry2D.Double.PointValue(120, 130)));
            estimate.Load(previousCurrentPoints);
            Target.Estimation<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue, Geometry2D.Double.PointValue> best = estimate.Compute();

            if (best.NotNull())
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
                file.WriteLine("clear all;");
                file.WriteLine("close all;");
                string before = "";
                string after = "";
                foreach (Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue> point in previousCurrentPoints)
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
                foreach (Kean.Core.Basis.Tuple<Geometry2D.Double.PointValue, Geometry2D.Double.PointValue> point in best.Inliers)
                    consensus += point.Item2.ToString() + "; ";
                consensus = consensus.TrimEnd(';');
                file.WriteLine("consensus = [" + consensus + "];");
                file.WriteLine("scatter(consensus(:,1),consensus(:,2),'g');");
                file.WriteLine("bestmodel = [" + Kean.Math.Double.ToString(best.Mapping.X) + " " + Kean.Math.Double.ToString(best.Mapping.Y) + "];");
                file.WriteLine("xlabel(strcat(' x= ', num2str(bestmodel(1)), ' y= ', num2str(bestmodel(2))))");
                file.Close();
            }
        }
        public void Run()
        {
            this.Run(
                this.RobustPolynomialRegression,
                this.TranslationRegression,
                this.ScaleRotationTranslationRegression
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

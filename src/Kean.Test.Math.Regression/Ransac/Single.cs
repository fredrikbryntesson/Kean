using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Kean.Core.Extension;
using Target = Kean.Math.Regression.Ransac;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Core.Collection;

namespace Kean.Test.Math.Regression.Ransac
{
    public class Single :
        AssertionHelper
    {
        [Test]
        public void RobustPolynomialRegression()
        {
            int degree = 2 + 1;
            System.Func<Kean.Math.Matrix.Single, float, float> map = (t, x) =>
            {
                float result = 0;
                for (int i = 0; i < t.Dimensions.Height; i++)
                    result += t[0, i] * Kean.Math.Single.Power(x, i);
                return result;
            };
            Target.Model<float, float, Kean.Math.Matrix.Single> model = new Target.Model<float, float, Kean.Math.Matrix.Single>()
            {
                RequiredMeasures = 10,
                Estimate = data =>
                {
                    Kean.Math.Matrix.Single result = null;
                    int n = data.Count;
                    Kean.Math.Matrix.Single a = new Kean.Math.Matrix.Single(degree, n);
                    Kean.Math.Matrix.Single b = new Kean.Math.Matrix.Single(1, n);
                    for (int i = 0; i < n; i++)
                    {
                        float x = data[i].Item1;
                        b[0, i] = data[i].Item2;
                        for (int j = 0; j < degree; j++)
                            a[j, i] = Kean.Math.Single.Power(x, j);
                    }
                    result = a.Solve(b) ?? new Kean.Math.Matrix.Single(1, degree);
                    return result;
                },
                FitsWell = 45,
                Threshold = 900,
                Map = map,
                Metric = (y1, y2) => Kean.Math.Single.Squared(y1 - y2)
            };
            Target.Estimator<float, float, Kean.Math.Matrix.Single> estimate =
                new Target.Estimator<float, float, Kean.Math.Matrix.Single>(model, 120);
            Collection.IList<Kean.Core.Tuple<float, float>> points =
                new Collection.List<Kean.Core.Tuple<float, float>>();
            Kean.Math.Matrix.Single coefficients = new Kean.Math.Matrix.Single(1, degree, new float[] { 20, 10, 5, 10 });
            Kean.Math.Random.Single.Normal generator = new Kean.Math.Random.Single.Normal(0, 30);
            for (float x = -10; x < 10; x += 0.1f)
            {
                float y = map(coefficients, x) + generator.Generate();
                points.Add(Kean.Core.Tuple.Create<float, float>(x, y));
            }
            estimate.Load(points);
            Target.Estimation<float, float, Kean.Math.Matrix.Single> best = estimate.Compute();
            if (best.NotNull())
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
                file.WriteLine("clear all;");
                file.WriteLine("close all;");
                string pointsExport = "";
                foreach (Kean.Core.Tuple<float, float> point in points)
                    pointsExport += Kean.Math.Single.ToString(point.Item1) + " " + Kean.Math.Single.ToString(point.Item2) + ";";
                pointsExport = pointsExport.TrimEnd(';');
                file.WriteLine("points = [" + pointsExport + "];");
                string consensusExport = "";
                foreach (Kean.Core.Tuple<float, float> point in best.Inliers)
                    consensusExport += Kean.Math.Single.ToString(point.Item1) + " " + Kean.Math.Single.ToString(point.Item2) + ";";
                consensusExport = consensusExport.TrimEnd(';');
                file.WriteLine("consensus = [" + consensusExport + "];");

                string bestModelExport = "";
                for (int y = 0; y < degree; y++)
                    bestModelExport += Kean.Math.Single.ToString(best.Mapping[0, y]) + ";";
                bestModelExport = bestModelExport.TrimEnd(';');
                file.WriteLine("bestModel = [" + bestModelExport + "];");
                file.WriteLine("scatter(points(:,1), points(:,2),'b');");

                string correctModelExport = "";
                for (int y = 0; y < degree; y++)
                    correctModelExport += Kean.Math.Single.ToString(coefficients[0, y]) + ";";
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
            Target.Model<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue, Kean.Math.Matrix.Single> model = new Target.Model<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue, Kean.Math.Matrix.Single>()
            {
                RequiredMeasures = 5,
                Estimate = data =>
                {
                    int count = data.Count;
                    Kean.Math.Matrix.Single a = new Kean.Math.Matrix.Single(4, 2 * count);
                    Kean.Math.Matrix.Single b = new Kean.Math.Matrix.Single(1, 2 * count);
                    int j = 0;
                    for (int i = 0; i < count; i++)
                    {
                        Geometry2D.Single.PointValue previous = data[i].Item1;
                        Geometry2D.Single.PointValue y = data[i].Item2;
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
                    return a.Solve(b) ?? new Kean.Math.Matrix.Single(1, 4);
                },
                FitsWell = 10,
                Threshold = 20,
                Map = (t, x) => new Geometry2D.Single.PointValue(t[0, 0] * x.X - t[0, 1] * x.Y + t[0, 2], t[0, 1] * x.X + t[0, 0] * x.Y + t[0, 3]),
                Metric = (y1, y2) => Kean.Math.Single.Squared((y1 - y2).Length)
            };
            Target.Estimator<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue, Kean.Math.Matrix.Single> estimate =
                new Target.Estimator<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue, Kean.Math.Matrix.Single>(model, 20);
            Collection.IList<Kean.Core.Tuple<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue>> previousCurrentPoints =
                new Collection.List<Kean.Core.Tuple<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue>>();
            float s = 2;
            float thetaAngle = Kean.Math.Single.ToRadians(5);
            float xTranslation = 7;
            float yTranslation = 10;
            Kean.Math.Random.Single.Normal normal = new Kean.Math.Random.Single.Normal(0, 1);
            Kean.Core.Collection.IList<Geometry2D.Single.PointValue> previousPoints = new Kean.Core.Collection.List<Geometry2D.Single.PointValue>();
            for (int x = -100; x < 100; x += 20)
                for (int y = -100; y < 100; y += 20)
                    previousPoints.Add(new Geometry2D.Single.PointValue(x, y));
            for (int i = 0; i < previousPoints.Count; i++)
            {
                Geometry2D.Single.PointValue x = previousPoints[i];
                Geometry2D.Single.PointValue y = new Geometry2D.Single.PointValue(
                    s * Kean.Math.Single.Cosinus(thetaAngle) * x.X - s * Kean.Math.Single.Sinus(thetaAngle) * x.Y + xTranslation,
                    s * Kean.Math.Single.Sinus(thetaAngle) * x.X + s * Kean.Math.Single.Cosinus(thetaAngle) * x.Y + yTranslation);
                float[] xdd = normal.Generate(2);
                float[] ydd = normal.Generate(2);
                Geometry2D.Single.PointValue xd = new Kean.Math.Geometry2D.Single.PointValue(xdd[0], xdd[1]);
                Geometry2D.Single.PointValue yd = new Kean.Math.Geometry2D.Single.PointValue(ydd[0], ydd[1]);
                previousCurrentPoints.Add(Kean.Core.Tuple.Create<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue>(x + xd, y + yd));
            }
            previousCurrentPoints.Add(Kean.Core.Tuple.Create<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue>(new Geometry2D.Single.PointValue(130, 130), new Geometry2D.Single.PointValue(720, 70)));
            estimate.Load(previousCurrentPoints);
            Target.Estimation<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue, Kean.Math.Matrix.Single> best = estimate.Compute();

            if (best.NotNull())
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
                file.WriteLine("clear all;");
                file.WriteLine("close all;");
                string before = "";
                string after = "";
                foreach (Kean.Core.Tuple<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue> point in previousCurrentPoints)
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
                foreach (Kean.Core.Tuple<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue> point in best.Inliers)
                    consensus += point.Item2.ToString() + "; ";
                consensus = consensus.TrimEnd(';');
                file.WriteLine("consensus = [" + consensus + "];");
                file.WriteLine("scatter(consensus(:,1),consensus(:,2),'g');");
                file.WriteLine("bestmodel = [" + best.Mapping + "];");
                file.Close();




                int count = best.Inliers.Count;
                Func<Kean.Math.Matrix.Single, Kean.Math.Matrix.Single> f = beta =>
                {
                    float scale = beta[0, 0];
                    float theta = beta[0, 1];
                    float xt = beta[0, 2];
                    float yt = beta[0, 3];
                    Kean.Math.Matrix.Single result = new Kean.Math.Matrix.Single(1, 2 * count);
                    for (int i = 0; i < count; i++)
                    {
                        Geometry2D.Single.PointValue previous = best.Inliers[i].Item1;
                        Geometry2D.Single.PointValue current = best.Inliers[i].Item2;
                        result[0, 2 * i + 0] = scale * Kean.Math.Single.Cosinus(theta) * previous.X - scale * Kean.Math.Single.Sinus(theta) * previous.Y + xt - current.X;
                        result[0, 2 * i + 1] = scale * Kean.Math.Single.Sinus(theta) * previous.X + scale * Kean.Math.Single.Cosinus(theta) * previous.Y + yt - current.Y;
                    }
                    return result;
                };
                Func<Kean.Math.Matrix.Single, Kean.Math.Matrix.Single> j = beta =>
                {
                    float scale = beta[0, 0];
                    float theta = beta[0, 1];
                    float xt = beta[0, 2];
                    float yt = beta[0, 3];
                    Kean.Math.Matrix.Single result = new Kean.Math.Matrix.Single(4, 2 * count);
                    for (int i = 0; i < count; i++)
                    {
                        Geometry2D.Single.PointValue previous = best.Inliers[i].Item1;
                        result[0, 2 * i + 0] = Kean.Math.Single.Cosinus(theta) * previous.X - Kean.Math.Single.Sinus(theta) * previous.Y;
                        result[1, 2 * i + 0] = -scale * Kean.Math.Single.Sinus(theta) * previous.X - scale * Kean.Math.Single.Cosinus(theta) * previous.Y;
                        result[2, 2 * i + 0] = 1;
                        result[3, 2 * i + 0] = 0;
                        result[0, 2 * i + 1] = Kean.Math.Single.Sinus(theta) * previous.X + Kean.Math.Single.Cosinus(theta) * previous.Y;
                        result[1, 2 * i + 1] = scale * Kean.Math.Single.Cosinus(theta) * previous.X - scale * Kean.Math.Single.Sinus(theta) * previous.Y;
                        result[2, 2 * i + 1] = 0;
                        result[3, 2 * i + 1] = 1;
                    }
                    return result;
                };

                Kean.Math.Regression.Minimization.LevenbergMarquardt.Single lm =
                    new Kean.Math.Regression.Minimization.LevenbergMarquardt.Single(f, j,
                        200, 1e-8f, 1e-8f, 1e-3f);
                float scaleGuess = Kean.Math.Single.SquareRoot(Kean.Math.Single.Squared(best.Mapping[0, 0]) + Kean.Math.Single.Squared(best.Mapping[0, 1]));
                float thetaGuess = Kean.Math.Single.ArcusTangensExtended(best.Mapping[0, 1], best.Mapping[0, 0]);
                float xTranslationGuess = best.Mapping[0, 2];
                float yTranslationGuess = best.Mapping[0, 3];
                Kean.Math.Matrix.Single guess = new Kean.Math.Matrix.Single(1, 4, new float[] { 1, 1, 1, 1 }); // new Kean.Math.Matrix.Single(1, 4, new float[] { scaleGuess, thetaGuess, xTranslationGuess, yTranslationGuess }); 
                Kean.Math.Matrix.Single estimation = lm.Estimate(guess);
            }
        }
        [Test]
        public void TranslationRegression()
        {
            Target.Model<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue, Geometry2D.Single.PointValue> model = new Target.Model<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue, Geometry2D.Single.PointValue>()
            {
                RequiredMeasures = 5,
                Estimate = data =>
                {
                    int count = data.Count;
                    Geometry2D.Single.PointValue result = new Geometry2D.Single.PointValue();
                    for (int i = 0; i < count; i++)
                        result += data[i].Item2 - data[i].Item1;
                    result.X /= (float)count;
                    result.Y /= (float)count;
                    return result;
                },
                FitsWell = 10,
                Threshold = 8,
                Map = (t, x) => t + x,
                Metric = (y1, y2) => Kean.Math.Single.Squared((y1 - y2).Length)
            };
            Target.Estimator<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue, Geometry2D.Single.PointValue> estimate =
                new Target.Estimator<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue, Geometry2D.Single.PointValue>(model, 20);
            Collection.IList<Kean.Core.Tuple<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue>> previousCurrentPoints =
                new Collection.List<Kean.Core.Tuple<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue>>();
            Geometry2D.Single.PointValue translation = new Geometry2D.Single.PointValue(7, 10);
            Kean.Math.Random.Single.Normal normal = new Kean.Math.Random.Single.Normal(0, 2);
            Kean.Core.Collection.IList<Geometry2D.Single.PointValue> previousPoints = new Kean.Core.Collection.List<Geometry2D.Single.PointValue>();
            for (int x = -100; x < 100; x += 20)
                for (int y = -100; y < 100; y += 20)
                    previousPoints.Add(new Geometry2D.Single.PointValue(x, y));
            for (int i = 0; i < previousPoints.Count; i++)
            {
                Geometry2D.Single.PointValue x = previousPoints[i];
                Geometry2D.Single.PointValue y = translation + x;
                float[] xdd = normal.Generate(2);
                float[] ydd = normal.Generate(2);
                Geometry2D.Single.PointValue xd = new Kean.Math.Geometry2D.Single.PointValue(xdd[0], xdd[1]);
                Geometry2D.Single.PointValue yd = new Kean.Math.Geometry2D.Single.PointValue(ydd[0], ydd[1]);
                previousCurrentPoints.Add(Kean.Core.Tuple.Create<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue>(x + xd, y + yd));
            }
            previousCurrentPoints.Add(Kean.Core.Tuple.Create<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue>(new Geometry2D.Single.PointValue(107, 107), new Geometry2D.Single.PointValue(120, 130)));
            estimate.Load(previousCurrentPoints);
            Target.Estimation<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue, Geometry2D.Single.PointValue> best = estimate.Compute();

            if (best.NotNull())
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("test.m");
                file.WriteLine("clear all;");
                file.WriteLine("close all;");
                string before = "";
                string after = "";
                foreach (Kean.Core.Tuple<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue> point in previousCurrentPoints)
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
                foreach (Kean.Core.Tuple<Geometry2D.Single.PointValue, Geometry2D.Single.PointValue> point in best.Inliers)
                    consensus += point.Item2.ToString() + "; ";
                consensus = consensus.TrimEnd(';');
                file.WriteLine("consensus = [" + consensus + "];");
                file.WriteLine("scatter(consensus(:,1),consensus(:,2),'g');");
                file.WriteLine("bestmodel = [" + Kean.Math.Single.ToString(best.Mapping.X) + " " + Kean.Math.Single.ToString(best.Mapping.Y) + "];");
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
            Single fixture = new Single();
            fixture.Run();
        }

    }
}

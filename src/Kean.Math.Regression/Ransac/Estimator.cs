// 
//  Estimate.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

// RANSAC Method implemented according to described method given at http://en.wikipedia.org/wiki/RANSAC
using System;
using Collection = Kean.Core.Collection;
using Kean.Core.Extension;
using Kean.Core.Collection.Extension;
namespace Kean.Math.Regression.Ransac
{
    public class Estimator<Domain, Range, Transform>
    {
        Model<Domain, Range, Transform> model;
        int maximumIterations;
        Collection.IList<Kean.Core.Tuple<Domain, Range>> data;
        Random.IInterval<int> random;
        double confidence;
        public Estimator(Model<Domain, Range, Transform> model, int maximumIterations) : this(model, maximumIterations, 0.99) { }
        public Estimator(Model<Domain, Range, Transform> model, int maximumIterations, double confidence)
        {
            this.model = model;
            this.maximumIterations = maximumIterations;
            this.confidence = confidence;
            this.random = new Random.Integer.Interval();
        }
        public void Load(Collection.IList<Kean.Core.Tuple<Domain, Range>> data)
        {
            this.data = data;
        }
        public void Reset()
        {
            this.data = null;
        }
        public Estimation<Domain, Range, Transform> Compute()
        {
            Estimation<Domain, Range, Transform> result = null;
            if (this.model.IsNull() || this.model.Estimate.IsNull() || this.model.Map.IsNull() || this.model.Metric.IsNull())
                throw new Exception.ModelSetup();
            if (this.data.IsNull() || this.model.IsNull())
                throw new Exception.InputData();
            if (this.data.Count >= this.model.RequiredMeasures)
            {
                int iterations = this.maximumIterations;
                int numberOfInliers = 0;
                int count = this.data.Count;
                int neededMeasures = this.model.FitsWell == 0 ? this.model.RequiredMeasures : this.model.FitsWell;
                for (int d = 0; d < iterations; d++)
                {
                    Kean.Core.Tuple<Collection.IList<Kean.Core.Tuple<Domain, Range>>, Collection.IList<Kean.Core.Tuple<Domain, Range>>> maybeInliersOutliers = this.InliersOutliers();
                    Transform maybeModel = this.model.Estimate(maybeInliersOutliers.Item1);
                    Collection.IList<Kean.Core.Tuple<Domain, Range>> consensusSet = maybeInliersOutliers.Item1;
                    foreach (Kean.Core.Tuple<Domain, Range> outlier in maybeInliersOutliers.Item2)
                        if (this.model.Metric(this.model.Map(maybeModel, outlier.Item1), outlier.Item2) < this.model.Threshold)
                            consensusSet.Add(outlier);
                    if (consensusSet.Count > Kean.Math.Integer.Maximum(neededMeasures - 1, numberOfInliers))
                    {
                        Transform thisModel = this.model.Estimate(consensusSet);
                        double thisError = 0;
                        foreach (Kean.Core.Tuple<Domain, Range> datum in consensusSet)
                            thisError += this.model.Metric(this.model.Map(thisModel, datum.Item1), datum.Item2);
                        numberOfInliers = consensusSet.Count;
                        result = new Estimation<Domain, Range, Transform>(consensusSet, thisError, thisModel, (double)numberOfInliers / count);
                        iterations = this.UpdateNumberOfIterations((double)(count - numberOfInliers) / count, iterations, neededMeasures);
                    }
                }
            }
            return result;
        }
        int UpdateNumberOfIterations(double outlierProbability, int iterations, int neededMeasures)
        {
            int result = 0;
            double nominator = Kean.Math.Double.Maximum(1 - this.confidence, double.Epsilon);
            double denominator = 1 - Kean.Math.Double.Power(1 - outlierProbability, neededMeasures);
            if (denominator >= double.Epsilon)
            {
                nominator = Kean.Math.Double.Logarithm(nominator);
                denominator = Kean.Math.Double.Logarithm(denominator);
                result = denominator >= 0 || -nominator >= maximumIterations * (-denominator) ?
                    iterations : Kean.Math.Integer.Round(nominator / denominator);
            }
            return result;
        }

        Kean.Core.Tuple<Collection.IList<Kean.Core.Tuple<Domain, Range>>, Collection.IList<Kean.Core.Tuple<Domain, Range>>> InliersOutliers()
        {
            Kean.Core.Tuple<Collection.IList<Kean.Core.Tuple<Domain, Range>>, Collection.IList<Kean.Core.Tuple<Domain, Range>>> result;
            Collection.IList<Kean.Core.Tuple<Domain, Range>> inliers = new Collection.List<Kean.Core.Tuple<Domain, Range>>(this.data.Count);
            Collection.IList<Kean.Core.Tuple<Domain, Range>> outliers = new Collection.List<Kean.Core.Tuple<Domain, Range>>(this.data.Count);
            this.random.Ceiling = this.data.Count - 1;
            int[] inlierIndexes = this.random.GenerateUnique(this.model.RequiredMeasures);
            Array.Sort(inlierIndexes);
            int[] outlierIndexes = new int[this.data.Count - inlierIndexes.Length];
            int k = 0;
            for (int j = 0; j < inlierIndexes[0]; j++)
                outlierIndexes[k++] = j;
            for (int i = 0; i < inlierIndexes.Length - 1; i++)
                for (int j = inlierIndexes[i] + 1; j < inlierIndexes[i + 1]; j++)
                    outlierIndexes[k++] = j;
            for (int j = inlierIndexes[inlierIndexes.Length - 1] + 1; j < this.data.Count; j++)
                outlierIndexes[k++] = j;
            for (int i = 0; i < inlierIndexes.Length; i++)
                inliers.Add(this.data[inlierIndexes[i]]);
            for (int i = 0; i < outlierIndexes.Length; i++)
                outliers.Add(this.data[outlierIndexes[i]]);
            result = Kean.Core.Tuple.Create<Collection.IList<Kean.Core.Tuple<Domain, Range>>, Collection.IList<Kean.Core.Tuple<Domain, Range>>>(inliers, outliers);
            return result;
        }
    }
}

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
using Kean.Core.Basis.Extension;
using Kean.Core.Collection.Extension;
namespace Kean.Math.Ransac
{
    public class Estimator<Domain, Range, Transform>
    {
        Model<Domain, Range, Transform> model;
        int iterations;
        Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>> data;
        Random.IInterval<int> random;
        public Estimator(Model<Domain, Range, Transform> model, int iterations)
        {
            this.model = model;
            this.iterations = iterations;
            this.random = new Random.Integer.Interval();
        }
        public void Load(Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>> data)
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
                Estimation<Domain, Range, Transform>[] estimate = new Estimation<Domain, Range, Transform>[this.iterations];
                
                for(int d = 0; d < this.iterations; d++)
                {
                    Kean.Core.Basis.Tuple<Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>, Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>> maybeInliersOutliers = this.InliersOutliers();
                    Transform maybeModel = this.model.Estimate(maybeInliersOutliers.Item1);
                    Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>> consensusSet = maybeInliersOutliers.Item1;
                    foreach (Kean.Core.Basis.Tuple<Domain, Range> outlier in maybeInliersOutliers.Item2)
                        if (this.model.Metric(this.model.Map(maybeModel, outlier.Item1), outlier.Item2) < this.model.Threshold)
                            consensusSet.Add(outlier);
                    if (consensusSet.Count > this.model.FitsWell)
                    {
                        Transform thisModel = this.model.Estimate(consensusSet);
                        double thisError = 0;
                        foreach (Kean.Core.Basis.Tuple<Domain, Range> datum in consensusSet)
                            thisError = Kean.Math.Double.Maximum(thisError, this.model.Metric(this.model.Map(thisModel, datum.Item1), datum.Item2));
                        estimate[d] = new Estimation<Domain, Range, Transform>(consensusSet, thisError, thisModel);
                    }
                }
                double error = double.MaxValue;
                for (int i = 0; i < estimate.Length; i++)
                {
                    if (estimate[i].NotNull() && estimate[i].Error < error)
                    {
                        result = estimate[i];
                        error = result.Error;
                    }
                }
            }
            return result;
        }
        Kean.Core.Basis.Tuple<Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>, Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>> InliersOutliers()
        {
            Kean.Core.Basis.Tuple<Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>, Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>> result;
            Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>> inliers = new Collection.List<Kean.Core.Basis.Tuple<Domain, Range>>(this.data.Count);
            Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>> outliers = new Collection.List<Kean.Core.Basis.Tuple<Domain, Range>>(this.data.Count);
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
            result = Kean.Core.Basis.Tuple.Create<Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>, Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>>(inliers, outliers);
            return result;
        }
    }
}

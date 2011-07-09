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
namespace Kean.Math.Random.Ransac
{
    public class Estimate<Domain, Range, Transform>
    {
        Model<Domain, Range, Transform> model;
        int iterations;
        Estimation<Domain, Range, Transform>[] estimate;
        Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>> data;
        Generator.Uniform random;
        public Estimate(Model<Domain, Range, Transform> model) : this(model, 200) { }
        public Estimate(Model<Domain, Range, Transform> model, int iterations)
        {
            this.model = model;
            this.iterations = iterations;
            this.estimate = new Estimation<Domain, Range, Transform>[this.iterations];
            this.random = new Generator.Uniform();
        }
        public void Load(Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>> data)
        {
            this.data = data;
        }
        public Estimation<Domain, Range, Transform> Compute()
        {
            Estimation<Domain, Range, Transform> result = null;
            if (this.model.IsNull() || this.model.Estimate.IsNull() || this.model.Map.IsNull() || this.model.Metric.IsNull())
                throw new Exception.ModelSetup();
            if (this.data.IsNull() || this.model.IsNull() || this.data.Count < this.model.RequiredMeasures)
                throw new Exception.InputData();
            new Action<int>(d =>
            {
                Kean.Core.Basis.Tuple<Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>, Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>> maybeInliersOutliers = this.InliersOutliers();
                Transform maybeModel = this.model.Estimate(maybeInliersOutliers.Item1);
                Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>> consensusSet = maybeInliersOutliers.Item1;
                foreach (Kean.Core.Basis.Tuple<Domain, Range> outlier in maybeInliersOutliers.Item2)
                    if (this.model.Metric(this.model.Map(outlier.Item1, maybeModel), outlier.Item2) < this.model.Threshold)
                        consensusSet.Add(outlier);
                if (consensusSet.Count > this.model.FitsWell)
                {
                    Transform thisModel = this.model.Estimate(consensusSet);
                    double thisError = 0;
                    foreach (Kean.Core.Basis.Tuple<Domain, Range> datum in consensusSet)
                        thisError += this.model.Metric(this.model.Map(datum.Item1, thisModel), datum.Item2);
                    this.estimate[d] = new Estimation<Domain, Range, Transform>(consensusSet, thisError, thisModel);
                }
            }).For(this.estimate.Length);
            double error = double.MaxValue;
            for (int i = 0; i < this.estimate.Length; i++)
            {
                if (this.estimate[i].NotNull() && this.estimate[i].Error < error)
                {
                    result = this.estimate[i];
                    error = result.Error;
                }
            }
            return result;
        }
        Kean.Core.Basis.Tuple<Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>, Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>> InliersOutliers()
        {
            Kean.Core.Basis.Tuple<Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>, Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>>> result;
            Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>> inliers = new Collection.List<Kean.Core.Basis.Tuple<Domain, Range>>(this.data.Count);
            Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>> outliers = new Collection.List<Kean.Core.Basis.Tuple<Domain, Range>>(this.data.Count);
            int[] inlierIndexes = this.random.NextDifferentIntArray(this.data.Count, this.model.RequiredMeasures);
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

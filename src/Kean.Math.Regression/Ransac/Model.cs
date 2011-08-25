// 
//  Model.cs
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
using System;
using Collection = Kean.Core.Collection;
using Kean.Core.Extension;

namespace Kean.Math.Regression.Ransac
{
    public class Model<Domain,Range, Transform>
    {
        public Func<Collection.IList<Kean.Core.Tuple<Domain, Range>>, Transform> Estimate { get; set; }
        public Func<Transform, Domain, Range> Map { get; set; }
        public Func<Range,Range, double> Metric { get; set; }
        public double Threshold { get; set; }
        public int FitsWell { get; set; }
        public int RequiredMeasures { get; set; }
        public Model() { }
        public Model(
            Func<Collection.IList<Kean.Core.Tuple<Domain, Range>>, Transform> estimate,
            Func<Transform, Domain, Range> map,
            Func<Range, Range, double> metric,
            double threshold,
            int fitsWell,
            int requiredMeasures)
        {
            this.Estimate = estimate;
            this.Map = map;
            this.Metric = metric;
            this.Threshold = threshold;
            this.FitsWell = fitsWell;
            this.RequiredMeasures = requiredMeasures;
        }
    }
}

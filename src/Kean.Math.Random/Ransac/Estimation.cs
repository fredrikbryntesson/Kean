// 
//  Estimation.cs
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
using Kean.Core.Basis.Extension;

namespace Kean.Math.Random.Ransac
{
    public class Estimation<Domain, Range, Transform>
    {
        public Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>> ConsensusSet { get; set; }
        public double Error { get; set; }
        public Transform Mapping { get; set; }
        public Estimation(Collection.IList<Kean.Core.Basis.Tuple<Domain, Range>> consensusSet, double error, Transform mapping)
        {
            this.ConsensusSet = consensusSet;
            this.Error = error;
            this.Mapping = mapping;
        }
        #region Object overrides
        public override int GetHashCode()
        {
            return this.ConsensusSet.GetHashCode()^this.Error.GetHashCode()^this.Mapping.GetHashCode();
        }
        public override string ToString()
        {
            return "Transform: " + this.Mapping.ToString() + " Error:" + Kean.Math.Double.ToString(this.Error);
        }
        #endregion
    }
}

// 
//  Bins.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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

using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Kean.Core.Extension;

namespace Kean.Core.Buffer.Recycle
{
	class Bins<T> : 
		IBin<T>
		where T : struct
	{
		int[] thresholds;
		IBin<T>[] bins;
		IBin<T> this[int size]
		{
			get
			{
				int index = this.thresholds.Index(value => size < value);
				if (index < 0)
					index = this.thresholds.Length;
				return this.bins[index];
			}
		}
		public int Capacity 
		{
			get { return this.bins[0].Capacity; }
			set { this.bins.Apply(bin => bin.Capacity  = value); }
		}
		public Bins(int capacity, params int[] thresholds)
		{
			this.thresholds = thresholds;
			this.bins = new IBin<T>[thresholds.Length + 1].Initialize(() => new Bin<T>(capacity));
		}
		public T[] Create(int size)
		{
			return this[size].Create(size);
		}
		public void Recycle(T[] item)
		{
			this[item.Length].Recycle(item);
		}
	}
}

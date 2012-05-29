// 
//  Bin.cs
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
using System;

namespace Kean.Core.Recycle
{
	public class Bin<T, S> :
		IBin<T, S>
	{
		public int Capacity { get; set; }
		Collection.IList<T> recycled;
		Func<T, S, bool> comparer;
		public Bin(int capacity, Func<T, S, bool> comparer)
		{
			this.Capacity = capacity;
			this.comparer = comparer;
			this.recycled = new Collection.Synchronized.List<T>();
		}
		public T Find(S specifier)
		{
			return this.recycled.RemoveFirst(item => this.comparer(item, specifier));
		}
		public void Recycle(T item)
		{
			if (item.NotNull())
			{
				while (this.recycled.Count >= this.Capacity)
					this.recycled.Remove(this.Capacity - 1);
				this.recycled.Add(item);
			}
		}
	}
}

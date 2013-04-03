//
//  WasteBin.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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
using Kean.Core;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Recycle = Kean.Core.Recycle;

namespace Kean.Draw.OpenGL.Backend
{
	class WasteBin<T> :
		Synchronized
	{
		Collection.IList<T> content = new Collection.List<T>();
		Action<T> free;
		public WasteBin(Action<T> free)
		{
			this.free = free;
		}
		public virtual void Add(T item)
		{
			lock (this.Lock)
				this.content.Add(item);
		}
		public virtual void Free()
		{
			Collection.IList<T> waste;
			lock (this.Lock)
			{
				waste = this.content;
				this.content = new Collection.List<T>();
			}
			foreach (var item in waste)
				this.free(item);
		}
	}
}

// 
//  ListExtension.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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
using Kean.Core.Basis.Extension;
namespace Kean.Core.Collection.Extension
{
	public static class ListExtension
	{
		public static void Add<T>(this IList<T> data, System.Collections.Generic.IEnumerable<T> items)
		{
			foreach (T item in items)
				data.Add(item);
		}
		public static bool Remove<T>(this IList<T> data, Func<T, bool> predicate)
		{
			bool result = false;
			int i = 0;
			while (i < data.Count)
			{
				T item = data[i];
				if (predicate(item))
					result = item.NotNull() ? item.Equals(data.Remove(i)) : (data.Remove(i) == null);
				else
					i++;
			}
			return result;
		}
		public static void Clear<T>(this IList<T> data)
		{
			while (data.Count > 0)
				data.Remove();
		}
	}
}

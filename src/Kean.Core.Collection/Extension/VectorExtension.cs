// 
//  ReadOnlyVectorExtension.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System;
using Kean.Core.Basis.Extension;
namespace Kean.Core.Collection.Extension
{
	public static class ReadOnlyVectorExtension
	{
		public static void Apply<T>(this IReadOnlyVector<T> data, Action<T> function)
		{
			foreach (T element in data)
				function(element);
		}
		public static void Map<T, S>(this IReadOnlyVector<T> input, IVector<S> output, Func<T, S> function)
		{
			int minimumLength = (input.Count > output.Count) ? output.Count : input.Count;
			for (int i = 0; i < minimumLength; i++)
				output[i] = function(input[i]);
		}
		public static IReadOnlyVector<S> Map<T, S>(this IReadOnlyVector<T> input, Func<T, S> function)
		{
			IVector<S> result = new Vector<S>(input.Count);
			for (int i = 0; i < input.Count; i++)
				result[i] = function(input[i]);
			return new Wrap.ReadOnlyVector<S>(result);
		}
		public static int Index<T>(this IReadOnlyVector<T> data, Func<T, bool> function)
		{
			int result = -1;
			for (int i = 0; data.NotNull() && i < data.Count; i++)
				if (function(data[i]))
				{
					result = i;
					break;
				}
			return result;
		}
		public static T Find<T>(this IReadOnlyVector<T> data, Func<T, bool> function)
		{
			T result = default(T);
			foreach (T element in data)
				if (function(element))
				{
					result = element;
					break;
				}
			return result;
		}
		public static S Find<T, S>(this IReadOnlyVector<T> data, Func<T, S> function)
		{
			S result = default(S);
			foreach (T element in data)
				if ((result = function(element)) != null)
					break;
			return result;
		}
		public static bool Exists<T>(this IReadOnlyVector<T> data, Func<T, bool> function)
		{
			bool result = false;
			foreach (T element in data)
				if (function(element))
				{
					result = true;
					break;
				}
			return result;
		}
		public static bool All<T>(this IReadOnlyVector<T> data, Func<T, bool> function)
		{
			bool result = true;
			foreach (T element in data)
				if (!function(element))
				{
					result = false;
					break;
				}
			return result;
		}
		public static S Fold<T, S>(this IReadOnlyVector<T> data, Func<T, S, S> function, S initial)
		{
			foreach (T element in data)
				initial = function(element, initial);
			return initial;
		}
		public static T[] ToArray<T>(this IReadOnlyVector<T> data)
		{
			T[] result = new T[data.Count];
			for (int i = 0; i < result.Length; i++)
				result[i] = data[i];
			return result;
		}
	}
}

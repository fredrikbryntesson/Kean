// 
//  VectorExtension.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
	public static class ArrayExtension
	{
		public static void Reverse<T>(this T[] data)
		{
			int half = data.Length / 2;
			for (int i = 0; i < half; i++)
			{
				T t = data[i];
				data[i] = data[data.Length - i - 1];
				data[data.Length - i - 1] = t;
			}
		}
		public static void Apply<T>(this T[] data, Action<T> function)
		{
			foreach (T element in data)
				function(element);
		}
		public static void Modify<T>(this T[] data, Func<T, T> function)
		{
			for (int i = 0; i < data.Length; i++)
				data[i] = function(data[i]);
		}
		public static void Map<T, S>(this T[] input, S[] output, Func<T, S> function)
		{
			int minimumLength = (input.Length > output.Length) ? output.Length : input.Length;
			for (int i = 0; i < minimumLength; i++)
				output[i] = function(input[i]);
		}
		public static S[] Map<T, S>(this T[] input, Func<T, S> function)
		{
			S[] result = new S[input.Length];
			for (int i = 0; i < input.Length; i++)
				result[i] = function(input[i]);
			return result;
		}
		public static int Index<T>(this T[] data, Func<T, bool> function)
		{
			int result = -1;
			for (int i = 0; i < data.Length; i++)
				if (function(data[i]))
				{
					result = i;
					break;
				}
			return result;
		}
		public static T Find<T>(this T[] data, Func<T, bool> function)
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
		public static S Find<T, S>(this T[] data, Func<T, S> function)
		{
			S result = default(S);
			foreach (T element in data)
				if ((result = function(element)) != null)
					break;
			return result;
		}
		public static bool Exists<T>(this T[] data, Func<T, bool> function)
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
		public static bool All<T>(this T[] data, Func<T, bool> function)
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
		public static S Fold<T, S>(this T[] data, Func<T, S, S> function, S initial)
		{
			foreach (T element in data)
				initial = function(element, initial);
			return initial;
		}
		public static T[] Copy<T>(this T[] data)
		{
			T[] result = new T[data.Length];
			for (int i = 0; i < result.Length; i++)
				result[i] = data[i];
			return result;
		}
	}
}

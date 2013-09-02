// 
//  EnumerableExtension.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Kean.Core.Extension;

namespace Kean.Core.Extension
{
	public static class EnumerableExtension
	{
		public static T First<T> (this System.Collections.Generic.IEnumerable<T> me)
		{
			T result;
			if (me.NotNull())
			{
				using (System.Collections.Generic.IEnumerator<T> enumerator = me.GetEnumerator())
					result = enumerator.MoveNext() ? enumerator.Current : default(T);
			}
			else
				result = default(T);
			return result;
		}

		public static void Apply<T> (this System.Collections.Generic.IEnumerable<T> me, Action<T> function)
		{
			foreach (T element in me)
				function(element);
		}

		public static System.Collections.Generic.IEnumerable<S> Map<T, S> (this System.Collections.Generic.IEnumerable<T> me, Func<T, S> function)
		{
			foreach (T element in me)
				yield return function(element);
		}

		public static int Index<T> (this System.Collections.Generic.IEnumerable<T> me, Func<T, bool> function)
		{
			int result = -1;
			int i = 0;
			foreach (T element in me)
				if (function(element))
				{
					result = i;
					break;
				}
				else
					i++;
			return result;
		}

		public static int Index<T> (this System.Collections.Generic.IEnumerable<T> me, T needle)
		{
			return me.Index(element => element.SameOrEquals(needle));
		}

		public static int Index<T> (this System.Collections.Generic.IEnumerable<T> me, params T[] needles) 
			where T : IEquatable<T>
		{
			return me.Index(element => needles.Contains(element));
		}

		public static bool Contains<T> (this System.Collections.Generic.IEnumerable<T> me, T needle) 
			where T : IEquatable<T>
		{
			bool result = false;
			foreach (T element in me)
				if (needle.SameOrEquals(element))
				{
					result = true;
					break;
				}
			return result;
		}

		public static bool Contains<T> (this System.Collections.Generic.IEnumerable<T> me, params T[] needles) 
			where T : IEquatable<T>
		{
			bool result = false;
			foreach (T element in me)
				if (needles.Contains(element))
				{
					result = true;
					break;
				}
			return result;
		}

		public static T Find<T> (this System.Collections.Generic.IEnumerable<T> me, Func<T, bool> function)
		{
			T result = default(T);
			foreach (T element in me)
				if (function(element))
				{
					result = element;
					break;
				}
			return result;
		}

		public static S Find<T, S> (this System.Collections.Generic.IEnumerable<T> me, Func<T, S> function)
		{
			S result = default(S);
			foreach (T element in me)
				if ((result = function(element)) != null)
					break;
			return result;
		}

		public static bool Exists<T> (this System.Collections.Generic.IEnumerable<T> me, Func<T, bool> function)
		{
			bool result = false;
			foreach (T element in me)
				if (function(element))
				{
					result = true;
					break;
				}
			return result;
		}

		public static bool All<T> (this System.Collections.Generic.IEnumerable<T> me, Func<T, bool> function)
		{
			bool result = true;
			foreach (T element in me)
				if (!function(element))
				{
					result = false;
					break;
				}
			return result;
		}

		public static bool All<T> (this System.Collections.Generic.IEnumerable<T> me, Func<T, bool, bool> function)
		{
			bool result = true;
			System.Collections.Generic.IEnumerator<T> enumerator = me.GetEnumerator();
			bool notLast = enumerator.MoveNext();
			while (notLast)
			{
				T current = enumerator.Current;
				if (!function(current, !(notLast = enumerator.MoveNext())))
				{
					result = false;
					break;
				}
			}
			return result;
		}

		public static S Fold<T, S> (this System.Collections.Generic.IEnumerable<T> me, Func<T, S, S> function, S initial)
		{
			foreach (T element in me)
				initial = function(element, initial);
			return initial;
		}

		public static System.Collections.Generic.IEnumerable<T> Append<T> (this System.Collections.Generic.IEnumerable<T> me, System.Collections.Generic.IEnumerable<T> other)
		{
			foreach (T item in me)
				yield return item;
			foreach (T item in other)
				yield return item;
		}

		public static System.Collections.Generic.IEnumerable<T> Prepend<T> (this System.Collections.Generic.IEnumerable<T> me, System.Collections.Generic.IEnumerable<T> other)
		{
			foreach (T item in other)
				yield return item;
			foreach (T item in me)
				yield return item;
		}

		public static string Join (this System.Collections.Generic.IEnumerable<string> me, string seperator)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			System.Collections.Generic.IEnumerator<string> enumerator = me.GetEnumerator();
			if (enumerator.MoveNext())
			{
				result.Append(enumerator.Current);
				while (enumerator.MoveNext())
					result.Append(seperator).Append(enumerator.Current);
			}
			return result.ToString();
		}
	}
}

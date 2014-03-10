﻿// 
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
using Kean.Extension;
using Generic = System.Collections.Generic;

namespace Kean.Extension
{
	public static class EnumerableExtension
	{
		public static T First<T> (this Generic.IEnumerable<T> me)
		{
			T result;
			if (me.NotNull())
			{
				using (Generic.IEnumerator<T> enumerator = me.GetEnumerator())
					result = enumerator.MoveNext() ? enumerator.Current : default(T);
			}
			else
				result = default(T);
			return result;
		}

		public static void Apply<T> (this Generic.IEnumerable<T> me, Action<T> function)
		{
			foreach (T element in me)
				function(element);
		}

		public static Generic.IEnumerable<S> Map<T, S> (this Generic.IEnumerable<T> me, Func<T, S> function)
		{
			foreach (T element in me)
				yield return function(element);
		}

		public static int Index<T> (this Generic.IEnumerable<T> me, Func<T, bool> function)
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

		public static int Index<T> (this Generic.IEnumerable<T> me, T needle)
		{
			return me.Index(element => element.SameOrEquals(needle));
		}

		public static int Index<T> (this Generic.IEnumerable<T> me, params T[] needles) 
			where T : IEquatable<T>
		{
			return me.Index(element => needles.Contains(element));
		}

		public static bool Contains<T> (this Generic.IEnumerable<T> me, T needle) 
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

		public static bool Contains<T> (this Generic.IEnumerable<T> me, params T[] needles) 
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

		public static T Find<T> (this Generic.IEnumerable<T> me, Func<T, bool> function)
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

		public static S Find<T, S> (this Generic.IEnumerable<T> me, Func<T, S> function)
		{
			S result = default(S);
			foreach (T element in me)
				if ((result = function(element)) != null)
					break;
			return result;
		}

		public static bool Exists<T> (this Generic.IEnumerable<T> me, Func<T, bool> function)
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

		public static bool All<T> (this Generic.IEnumerable<T> me, Func<T, bool> function)
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

		public static bool All<T> (this Generic.IEnumerable<T> me, Func<T, bool, bool> function)
		{
			bool result = true;
			Generic.IEnumerator<T> enumerator = me.GetEnumerator();
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

		public static S Fold<T, S> (this Generic.IEnumerable<T> me, Func<T, S, S> function, S initial)
		{
			foreach (T element in me)
				initial = function(element, initial);
			return initial;
		}

		public static Generic.IEnumerable<T> Append<T> (this Generic.IEnumerable<T> me, params T[] other)
		{
			return me.Append(other);
		}

		public static Generic.IEnumerable<T> Append<T> (this Generic.IEnumerable<T> me, Generic.IEnumerable<T> other)
		{
			if (me.NotNull())
				foreach (T item in me)
					yield return item;
			if (other.NotNull())
				foreach (T item in other)
					yield return item;
		}

		public static Generic.IEnumerable<T> Prepend<T> (this Generic.IEnumerable<T> me, params T[] other)
		{
			return me.Prepend((Generic.IEnumerable<T>)other);
		}

		public static Generic.IEnumerable<T> Prepend<T> (this Generic.IEnumerable<T> me, Generic.IEnumerable<T> other)
		{
			if (other.NotNull())
				foreach (T item in other)
					yield return item;
			if (me.NotNull())
				foreach (T item in me)
					yield return item;
		}

		public static Generic.IEnumerable<T> Where<T> (this Generic.IEnumerable<T> me, Func<T, bool> predicate)
		{
			foreach (T element in me)
				if (predicate(element))
					yield return element;
		}

		public static string Join (this Generic.IEnumerable<string> me)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			Generic.IEnumerator<string> enumerator = me.GetEnumerator();
			while (enumerator.MoveNext())
				result.Append(enumerator.Current);
			return result.ToString();
		}

		public static string Join (this Generic.IEnumerable<string> me, string seperator)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			Generic.IEnumerator<string> enumerator = me.GetEnumerator();
			if (enumerator.MoveNext())
			{
				result.Append(enumerator.Current);
				while (enumerator.MoveNext())
					result.Append(seperator).Append(enumerator.Current);
			}
			return result.ToString();
		}

		public static Generic.IEnumerable<char> Decode (this Generic.IEnumerable<byte> me)
		{
			return me.Decode(System.Text.Encoding.UTF8);
		}

		public static Generic.IEnumerable<char> Decode (this Generic.IEnumerable<byte> me, System.Text.Encoding encoding)
		{
			byte[] buffer = new byte[3];
			int pointer = 0;
			Generic.IEnumerator<byte> enumerator = me.GetEnumerator();
			while (enumerator.MoveNext())
			{
				buffer[pointer++] = enumerator.Current;
				if (enumerator.Current == 0xef && enumerator.MoveNext())
				{
					buffer[pointer++] = enumerator.Current;
					if (enumerator.Current == 0xbb && enumerator.MoveNext())
					{
						buffer[pointer++] = enumerator.Current;
						if (enumerator.Current == 0xbf)
							pointer = 0;
					}
				}
				foreach (char c in encoding.GetChars(buffer, 0, pointer))
					yield return c;
				pointer = 0;
			}
		}

		public static string Join (this Generic.IEnumerable<char> me)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			foreach (char c in me)
				result.Append(c);
			return result.ToString();
		}
	}
}

// 
//  EnumerableExtension.cs
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

using System;
using Kean.Core.Extension;
using Generic = System.Collections.Generic;

namespace Kean.Core.Extension
{
	public static class EnumeratorExtension
	{
		public static Generic.IEnumerator<T> Restart<T>(this Generic.IEnumerator<T> me)
		{
			me.Reset();
			return me;
		}
		public static void Apply<T>(this Generic.IEnumerator<T> me, Action<T> function)
		{
			while (me.MoveNext())
				function(me.Current);
		}
		public static Generic.IEnumerator<S> Map<T, S>(this Generic.IEnumerator<T> me, Func<T, S> function)
		{
			while (me.MoveNext()) 
				yield return function(me.Current);
		}
		public static int Index<T>(this Generic.IEnumerator<T> me, Func<T, bool> function)
		{
			int result = -1;
			int i = 0;
			while (me.MoveNext())
				if (function(me.Current))
				{
					result = i;
					break;
				}
				else
					i++;
			return result;
		}
		public static int Index<T>(this Generic.IEnumerator<T> me, T needle)
		{
			return me.Index(element => element.SameOrEquals(needle));
		}
		public static int Index<T>(this Generic.IEnumerator<T> me, params T[] needles) 
			where T : IEquatable<T>
		{
			return me.Index(element => needles.Contains(element));
		}
		public static bool Contains<T>(this Generic.IEnumerator<T> me, T needle) 
			where T : IEquatable<T>
		{
			bool result = false;
			while (me.MoveNext())
				if (needle.SameOrEquals(me.Current))
				{
					result = true;
					break;
				}
			return result;
		}
		public static bool Contains<T>(this Generic.IEnumerator<T> me, params T[] needles) 
			where T : IEquatable<T>
		{
			bool result = false;
			while (me.MoveNext())
				if (needles.Contains(me.Current))
				{
					result = true;
					break;
				}
			return result;
		}
		public static T Find<T>(this Generic.IEnumerator<T> me, Func<T, bool> function)
		{
			T result = default(T);
			while (me.MoveNext())
				if (function(me.Current))
				{
					result = me.Current;
					break;
				}
			return result;
		}
		public static S Find<T, S>(this Generic.IEnumerator<T> me, Func<T, S> function)
		{
			S result = default(S);
			while (me.MoveNext())
				if ((result = function(me.Current)) != null)
					break;
			return result;
		}
		public static bool Exists<T>(this Generic.IEnumerator<T> me, Func<T, bool> function)
		{
			bool result = false;
			while (me.MoveNext())
				if (function(me.Current))
				{
					result = true;
					break;
				}
			return result;
		}
		public static bool All<T>(this Generic.IEnumerator<T> me, Func<T, bool> function)
		{
			bool result = true;
			while (me.MoveNext())
				if (!function(me.Current))
				{
					result = false;
					break;
				}
			return result;
		}
		public static S Fold<T, S>(this Generic.IEnumerator<T> me, Func<T, S, S> function, S initial)
		{
			while (me.MoveNext())
				initial = function(me.Current, initial);
			return initial;
		}
		public static string Join(this Generic.IEnumerator<string> me, string seperator)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			if (me.MoveNext())
			{
				result.Append(me.Current);
				while (me.MoveNext())
					result.Append(seperator).Append(me.Current);
			}
			return result.ToString();
		}
	}
}

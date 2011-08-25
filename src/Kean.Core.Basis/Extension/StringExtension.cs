// 
//  String.cs
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
using RegularExpressions = System.Text.RegularExpressions;
using Kean.Core.Basis.Extension;

namespace Kean.Core.Basis.Extension
{
	public static class StringExtension
	{
		public static bool NotEmpty(this string me)
		{
			return me.NotNull() && me != "";
		}
		public static bool IsEmpty(this string me)
		{
			return me.IsNull() || me == "";
		}
		public static bool Like(this string me, string pattern)
		{
			return new RegularExpressions.Regex("^" + RegularExpressions.Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$", RegularExpressions.RegexOptions.IgnoreCase | RegularExpressions.RegexOptions.Singleline).IsMatch(me);
		}
		public static int Index(this string me, string needle)
		{
			return me.Index(0, needle);
		}
		public static int Index(this string me, int start, string needle)
		{
			return me.Index(start, me.Length - 1, needle);
		}
		public static int Index(this string me, int start, int end, string needle)
		{
			int length = me.Length - 1;
			if (start < 0)
				start = length + start;
			if (end < 0)
				end = length + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (me[i] == needle[0] && me.Substring(i, needle.Length) == needle)
				{
					result = i;
					break;
				}
			return result;
		}
		public static int Index(this string me, char needle)
		{
			return me.Index(0, needle);
		}
		public static int Index(this string me, int start, char needle)
		{
			return me.Index(start, me.Length - 1, needle);
		}
		public static int Index(this string me, int start, int end, char needle)
		{
			int length = me.Length;
			if (start < 0)
				start = length + start;
			if (end < 0)
				end = length + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (me[i] == needle)
				{
					result = i;
					break;
				}
			return result;
		}
		public static int Index(this string me, params char[] needles)
		{
			return me.Index(0, needles);
		}
		public static int Index(this string me, int start, params char[] needles)
		{
			return me.Index(start, me.Length - 1, needles);
		}
		public static int Index(this string me, int start, int end, params char[] needles)
		{
			int length = me.Length;
			if (start < 0)
				start = length + start;
			if (end < 0)
				end = length + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (needles.Contains(me[i]))
				{
					result = i;
					break;
				}
			return result;
		}
	}
}

// 
//  StringExtension.cs
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
using Kean.Core.Extension;

namespace Kean.Core.Extension
{
	public static class StringExtension
	{
		public static bool NotEmpty (this string me)
		{
			return me.NotNull () && me != "";
		}

		public static bool IsEmpty (this string me)
		{
			return me.IsNull () || me == "";
		}

		public static bool Like (this string me, string pattern)
		{
			return new RegularExpressions.Regex ("^" + RegularExpressions.Regex.Escape (pattern).Replace (@"\*", ".*").Replace (@"\?", ".") + "$", RegularExpressions.RegexOptions.IgnoreCase | RegularExpressions.RegexOptions.Singleline).IsMatch (me);
		}

		public static void Save (this string me, string path)
		{
			System.IO.File.WriteAllText (path, me);
		}

		public static string Format (this string me, object argument)
		{
			return string.Format (me, argument);
		}

		public static string Format (this string me, object argument0, object argument1)
		{
			return string.Format (me, argument0, argument1);
		}

		public static string Format (this string me, object argument0, object argument1, object argument2)
		{
			return string.Format (me, argument0, argument1, argument2);
		}

		public static string Format (this string me, params object[] arguments)
		{
			return string.Format (me, arguments);
		}

		public static int Index (this string me, string needle)
		{
			return me.Index (0, needle);
		}

		public static int Index (this string me, int start, string needle)
		{
			return me.Index (start, me.Length - 1, needle);
		}

		public static int Index (this string me, int start, int end, string needle)
		{
			int length = me.Length - 1;
			if (start < 0)
				start = length + start;
			if (end < 0)
				end = length + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (me [i] == needle [0] && me.Substring (i, needle.Length) == needle) {
					result = i;
					break;
				}
			return result;
		}

		public static int Index (this string me, char needle)
		{
			return me.Index (0, needle);
		}

		public static int Index (this string me, int start, char needle)
		{
			return me.Index (start, me.Length - 1, needle);
		}

		public static int Index (this string me, int start, int end, char needle)
		{
			int length = me.Length;
			if (start < 0)
				start = length + start;
			if (end < 0)
				end = length + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (me [i] == needle) {
					result = i;
					break;
				}
			return result;
		}

		public static int Index (this string me, params char[] needles)
		{
			return me.Index (0, needles);
		}

		public static int Index (this string me, int start, params char[] needles)
		{
			return me.Index (start, me.Length - 1, needles);
		}

		public static int Index (this string me, int start, int end, params char[] needles)
		{
			int length = me.Length;
			if (start < 0)
				start = length + start;
			if (end < 0)
				end = length + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (needles.Contains (me [i])) {
					result = i;
					break;
				}
			return result;
		}

		public static string[] SplitAt (this string me)
		{
			return me.SplitAt (' ');
		}

		public static string[] SplitAt (this string me, params char[] separators)
		{
			string[] result = null;
			System.Collections.Generic.IList<string> parts = new System.Collections.Generic.List<string> ();
			System.Text.StringBuilder current = new System.Text.StringBuilder ();
			me = me.Trim ();
			int index = 0;
			while (index < me.Length) {
				char unit = me [index];
				switch (unit) {
				case '\\':
					if (++index < me.Length)
						switch (me [index]) {
						case 'r':
							current.Append ('\r');
							break;
						case 'n':
							current.Append ('\n');
							break;
						case 't':
							current.Append ('\t');
							break;
						case 'b':
							current.Append ('\b');
							break;
						case '\\':
							current.Append ('\\');
							break;
						case '"':
							current.Append ('"');
							break;
						}
					break;
				case '"':
					while (++index < me.Length && (unit = me[index]) != '"')
						current.Append (unit);
					break;
				default:
					if (separators.Contains (unit)) {
						parts.Add (current.ToString ());
						current = new System.Text.StringBuilder ();
					} else
						current.Append (unit);
					break;
				}
				index++;
			}
			if (current.Length > 0)
				parts.Add (current.ToString ());
			result = new string[parts.Count];
			parts.CopyTo (result, 0);
			return result;
		}

		public static T Parse<T> (this string me)
		{
			T result;
			System.Type type = typeof(T);
			if (me is T)
				result = me.ConvertType<T> ();
			else if (type == typeof(char))
				result = me.ToString ().ConvertType<T> ();
			else if (type == typeof(bool)) {
				bool value;
				result = (bool.TryParse (me, out value) && value).ConvertType<T> ();
			} else if (type == typeof(byte)) {
				byte value;
				result = (byte.TryParse (me, out value) ? value : (byte)0).ConvertType<T> ();
			} else if (type == typeof(sbyte)) {
				sbyte value;
				result = (sbyte.TryParse (me, out value) ? value : (sbyte)0).ConvertType<T> ();
			} else if (type == typeof(short)) {
				short value;
				result = (short.TryParse (me, out value) ? value : (short)0).ConvertType<T> ();
			} else if (type == typeof(int)) {
				int value;
				result = (int.TryParse (me, out value) ? value : 0).ConvertType<T> ();
			} else if (type == typeof(uint)) {
				uint value;
				result = (uint.TryParse (me, out value) ? value : 0).ConvertType<T> ();
			} else if (type == typeof(long)) {
				long value;
				result = (long.TryParse (me, out value) ? value : 0).ConvertType<T> ();
			} else if (type == typeof(ulong)) {
				ulong value;
				result = (ulong.TryParse (me, out value) ? value : 0).ConvertType<T> ();
			} else if (type == typeof(float)) {
				float value;
				result = (float.TryParse (me, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out value) ? value : 0).ConvertType<T> ();
			} else if (type == typeof(double)) {
				double value;
				result = (double.TryParse (me, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out value) ? value : 0).ConvertType<T> ();
			} else if (type == typeof(decimal)) {
				decimal value;
				result = (decimal.TryParse (me, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out value) ? value : 0).ConvertType<T> ();
			} else if (type == typeof(DateTime)) {
				DateTime value;
				result = (DateTime.TryParse (me, System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat, System.Globalization.DateTimeStyles.AssumeLocal, out value) ? value : new DateTime ()).ConvertType<T> ();
			} else if (type == typeof(DateTimeOffset)) {
				DateTimeOffset value;
				result = (DateTimeOffset.TryParse (me, System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat, System.Globalization.DateTimeStyles.AssumeLocal, out value) ? value : new DateTimeOffset ()).ConvertType<T> ();
			} else if (type == typeof(TimeSpan)) {
				TimeSpan value;
				result = (TimeSpan.TryParse (me, System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat, out value) ? value : new TimeSpan ()).ConvertType<T> ();
			} else if (type == typeof(IntPtr)) {
				int value;
				result = (int.TryParse (me, out value) ? (IntPtr)value : IntPtr.Zero).ConvertType<T> ();
			} else if (type.IsEnum) {
				object value = Enum.Parse (type, me, true);
				result = (value.NotNull () && value.GetType () == typeof(T)) ? value.ConvertType<T> () : default(T);
			} else if (type.GetInterface (typeof(IString).FullName).NotNull ()) {
				result = System.Activator.CreateInstance (typeof(T)).ConvertType<T> ();
				(result as IString).String = me;
			} else {
				Func<string, object> cast = typeof(T).FromStringCast ();
				if (cast.NotNull ())
					result = cast (me).ConvertType<T> ();
				else
					result = me.ConvertType<T> ();
			}
			return result;
		}
	}
}

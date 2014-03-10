﻿//
//  ByteInDeviceExtension.cs
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
using Kean;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Generic = System.Collections.Generic;

namespace Kean.IO.Extension
{
	public static class ByteInDeviceExtension
	{
		public static bool MovePast (this IByteInDevice me, params byte[] separator)
		{
			byte? next;
			int position = 0;
			while ((next = me.Read()).HasValue)
			{
				if (next != separator[position++])
					position = 0;
				else if (separator.Length == position)
					break;
			}
			return !me.Empty;
		}
		public static Generic.IEnumerable<byte> ReadPast (this IByteInDevice me, params byte[] separator)
		{
			byte? next;
			int position = 0;
			while ((next = me.Read()).HasValue)
			{
				if (next != separator[position++])
					position = 0;
				else if (separator.Length == position)
					break;
				yield return next.Value;
			}
		}
		public static Generic.IEnumerable<byte> AsEnumerable (this IByteInDevice me)
		{
			byte? next;
			while ((next = me.Read()).HasValue)
				yield return next.Value;
		}
		public static Generic.IEnumerator<byte> AsEnumerator (this IByteInDevice me)
		{
			byte? next;
			while ((next = me.Read()).HasValue)
				yield return next.Value;
		}
	}
}

// 
//  Request.cs
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
using Uri = Kean.Uri;
using Generic = System.Collections.Generic;
using Kean.IO.Extension;
using Kean.Collection.Extension;

namespace Kean.IO.Net.Http
{
	public struct Request
	{
		public Method Method { get; private set; }
		public Uri.Path Path { get; private set; }
		public string Protocol { get; private set; }
		#region Headers
		Collection.IDictionary<string, string> headers;
		public string this [string key]
		{ 
			get { return this.headers.NotNull() ? this.headers[key] : null; } 
			private set
			{ 
				if (key.NotEmpty())
				{
					if (this.headers.IsNull())
						this.headers = new Collection.Dictionary<string, string>();
					this.headers[key] = value; 
				}
			}
		}
		#endregion
		Uri.Locator url;
		public Uri.Locator Url
		{ 
			get
			{
				if (this.url.IsNull())
					this.url = new Uri.Locator("http", this["Host"], this.Path);
				return this.url;
			}
		}
		public static Request Parse(IByteInDevice device)
		{
			Request result = new Request();
			string[] firstLine = Request.ReadLine(device).Decode().Join().Split(' ');
			if (firstLine.Length == 3)
			{
				result.Method = firstLine[0].Parse<Method>();
				result.Path = firstLine[1];
				result.Protocol = firstLine[2];
				string line;
				while ((line = Request.ReadLine(device).Decode().Join()).NotEmpty())
				{
					string[] parts = line.Split(new char[] { ':' }, 2);
					result[parts[0].Trim()] = parts[1].Trim();
				}
			}
			return result;
		}
		static Generic.IEnumerable<byte> ReadLine(IByteInDevice device)
		{
			foreach (byte b in device.Read(13, 10))
				if (b != 13 && b != 10)
					yield return b;
			//byte? next = device.Peek();
			//if (next.HasValue && next.Value == 32 || next.Value == 9) // lines broken into several lines must start with space (SP) or horizontal tab (HT)
			//	foreach (byte b in this.ReadLine(device))
			//		yield return b;
		}
	}
}

// 
//  ByteDevice.cs
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
using Kean.Core;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Uri = Kean.Core.Uri;

namespace Kean.IO
{
	public class ByteDevice :
		IByteDevice
	{
		byte? peeked;
		System.IO.Stream stream;

		#region Constructors
		public ByteDevice(System.IO.Stream stream)
		{
			this.stream = stream;
		}
		#endregion
		#region IByteDevice Members
		public bool Readable { get { return this.stream.NotNull() && this.stream.CanRead; } }
		public bool Writeable { get { return this.stream.NotNull() && this.stream.CanWrite; } }
		#endregion
		byte? Convert(int value)
		{
			return value < 0 ? null : (byte?)value;
		}
		#region IByteInDevice Members
		public byte? Peek()
		{
			return this.peeked.HasValue ? this.peeked : this.peeked = this.Convert(this.stream.ReadByte());
		}
		public byte? Read()
		{
			byte? result;
			if (this.peeked.HasValue)
			{
				result = this.peeked;
				this.peeked = null;
			}
			else
				result = this.Convert(this.stream.ReadByte());
			return result;
		}
		#endregion
		#region IByteOutDevice Members
		public bool Write(System.Collections.Generic.IEnumerable<byte> buffer)
		{
			bool result = true;
			try
			{
				Collection.IList<byte> list = new Collection.List<byte>();
				foreach (byte b in buffer)
					list.Add(b);
				byte[] array = list.ToArray();
				this.stream.Write(array, 0, array.Length);
				this.stream.Flush();
			}
			catch (System.Exception) { result = false; }
			return result;
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return !this.Peek().HasValue; } }
		#endregion
		#region IDevice Members
		public virtual bool Opened { get { return this.Readable || this.Writeable; } }
		public virtual bool Close()
		{
			bool result;
			if (result = this.stream.NotNull())
			{
				this.stream.Close();
				this.stream = null;
			}
			return result;
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		#region Static Open
		public static IByteDevice Open(Uri.Locator resource)
		{
			IByteDevice result = null;
			switch (resource.Scheme)
			{
				case "assembly":
					result = resource.Authority == "" ? ByteDevice.Open(System.Reflection.Assembly.GetEntryAssembly(), resource.Path) : ByteDevice.Open(System.Reflection.Assembly.LoadWithPartialName(resource.Authority), resource.Path);
					break;
				case "file":
					result = new ByteDevice(System.IO.File.Open(resource.Path, System.IO.FileMode.OpenOrCreate));
					break;
			}
			return result;
		}
		public static IByteDevice Open(System.Reflection.Assembly assembly, Uri.Path resource)
		{
			return new ByteDevice(assembly.GetManifestResourceStream(assembly.GetName().Name + "." + ((string)resource).Replace('/', '.')));
		}
		#endregion
	}
}

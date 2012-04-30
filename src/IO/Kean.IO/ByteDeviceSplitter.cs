// 
//  ByteDeviceSplitter.cs
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
using Kean.Core;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Uri = Kean.Core.Uri;

namespace Kean.IO
{
	public class ByteDeviceSplitter :
		IByteDevice
	{
		IByteInDevice inDevice;
		IByteOutDevice outDevice;

		#region Constructors
		protected ByteDeviceSplitter(IByteInDevice inDevice) :
			this(inDevice, null)
		{ }
		protected ByteDeviceSplitter(IByteInDevice inDevice, IByteOutDevice outDevice)
		{
			this.inDevice = inDevice;
			this.outDevice = outDevice;
		}
		#endregion
		#region IByteDevice Members
		public bool Readable { get { return this.inDevice.NotNull() && this.inDevice.Opened; } }
		public bool Writeable { get { return this.outDevice.NotNull() && this.outDevice.Opened; } }
		#endregion
		#region IByteInDevice Members
		public byte? Peek()
		{
			return this.inDevice.NotNull() ? this.inDevice.Peek() : null;
		}
		public byte? Read()
		{
			return this.inDevice.NotNull() ? this.inDevice.Read() : null;
		}
		#endregion
		#region IByteOutDevice Members
		public bool Write(System.Collections.Generic.IEnumerable<byte> buffer)
		{
			return this.outDevice.NotNull() && this.outDevice.Write(buffer);
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return this.inDevice.IsNull() || this.inDevice.Empty; } }
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return this.inDevice.Resource; } }
		public virtual bool Opened { get { return this.Readable || this.Writeable; } }
		public virtual bool Close()
		{
			bool result;
			if (result = this.inDevice.NotNull())
			{
				this.inDevice.Close();
				this.inDevice = null;
			}
			if (this.outDevice.NotNull())
			{
				result |= this.outDevice.NotNull();
				this.outDevice.Close();
				this.outDevice = null;
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
		public static IByteDevice Open(IByteInDevice inDevice) { return ByteDeviceSplitter.Open(inDevice, null); }
		public static IByteDevice Open(IByteInDevice inDevice, IByteOutDevice outDevice) { return inDevice.NotNull() || outDevice.NotNull() ? new ByteDeviceSplitter(inDevice, outDevice) : null; }
		#endregion
	}
}

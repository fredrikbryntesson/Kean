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
		protected ByteDevice(System.IO.Stream stream)
		{
			this.stream = stream;
		}
		#endregion
		#region IByteDevice Members
		public bool Readable { get { return this.stream.NotNull() && this.stream.CanRead; } }
		public bool Writeable { get { return this.stream.NotNull() && this.stream.CanWrite; } }
		#endregion
        byte? RawRead()
        {
            byte? result;
            try { result = this.stream.IsNull() ? null : this.Convert(this.stream.ReadByte()); }
            catch (ObjectDisposedException) { result = null;  }
            return result;
        }
		byte? Convert(int value)
		{
			return value < 0 ? null : (byte?)value;
		}
		#region IByteInDevice Members
		public byte? Peek()
		{
			return this.peeked.HasValue ? this.peeked : this.peeked = this.RawRead();
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
                result = this.RawRead();
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
		public Uri.Locator Resource { get; private set; }
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
		#region Static Open & Create
		public static IByteDevice Open(System.IO.Stream stream)
		{
			return stream.NotNull() ? new ByteDevice(stream) : null;
		}
		public static IByteDevice Open(Uri.Locator resource)
		{
			return ByteDevice.Open(resource, System.IO.FileMode.Open);
		}
		public static IByteDevice Open(Uri.Locator input, Uri.Locator output)
		{
			return ByteDeviceSplitter.Open(ByteDevice.Open(input), ByteDevice.Create(output));
		}
		public static IByteDevice Create(Uri.Locator resource)
		{
			IByteDevice result = ByteDevice.Open(resource, System.IO.FileMode.Create);
			if (result.IsNull() && resource.NotNull())
			{
				System.IO.Directory.CreateDirectory(resource.Path.FolderPath.PlatformPath);
				result = ByteDevice.Open(resource, System.IO.FileMode.Create);
			}
			return result;
		}
		static IByteDevice Open(Uri.Locator resource, System.IO.FileMode mode)
		{
			IByteDevice result = null;
			if (resource.NotNull())
				switch (resource.Scheme)
				{
					case "assembly":
						result = resource.Authority == "" ? ByteDevice.Open(System.Reflection.Assembly.GetEntryAssembly(), resource.Path) : ByteDevice.Open(System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(resource.Authority)), resource.Path);
						break;
					case "file":
						try 
						{
							System.IO.FileStream stream = System.IO.File.Open(resource.Path.PlatformPath, mode, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
							if (stream.NotNull())
								result = new ByteDevice(stream) { Resource = resource }; 
						}
						catch (System.IO.DirectoryNotFoundException) { result = null; }
						catch (System.IO.FileNotFoundException) { result = null; }
						break;
					case "http":
					case "https":
						if (mode == System.IO.FileMode.Open)
						{
							try
							{
								using (System.Net.WebClient client = new System.Net.WebClient())
                                    result = new ByteDevice(new System.IO.MemoryStream(client.DownloadData(resource))) { Resource = resource };
							}
							catch (System.Net.WebException) { result = null; }
						}
						break;
				}
			return result;
		}
		public static IByteDevice Open(System.Reflection.Assembly assembly, Uri.Path resource)
		{
			return new ByteDevice(assembly.GetManifestResourceStream(assembly.GetName().Name + ((string)resource).Replace('/', '.'))) { Resource = new Uri.Locator("assembly", assembly.GetName().Name, resource) };
		}
		#endregion
	}
}

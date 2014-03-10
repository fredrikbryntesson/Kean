﻿// 
//  BlockDevice.cs
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

namespace Kean.IO
{
	public class BlockDevice :
		IBlockDevice
	{
		System.IO.Stream stream;
		public bool Wrapped { get; set; }
		object peekedLock = new object();
		Collection.IVector<byte> peeked;
		#region Constructors

		protected BlockDevice(System.IO.Stream stream)
		{
			this.stream = stream;
			this.Resource = "stream:///";
		}

		#endregion
		#region IBlockDevice
		public bool Readable { get { return this.stream.NotNull() && this.stream.CanRead; } }
		public bool Writeable { get { return this.stream.NotNull() && this.stream.CanWrite; } }
		#endregion
		#region IBlockInDevice
		Collection.IVector<byte> RawRead()
		{
			byte[] buffer = new byte[64 * 1024];
			int count = this.stream.Read(buffer, 0, buffer.Length);
			return new Collection.Slice<byte>(buffer, 0, count);
		}
		public Collection.IVector<byte> Peek()
		{
			lock (this.peekedLock)
				return this.peeked ?? (this.peeked = this.RawRead());
		}
		public Collection.IVector<byte> Read()
		{
			lock (this.peekedLock)
			{
				Collection.IVector<byte> result = this.peeked ?? this.RawRead();
				this.peeked = null;
				return result;
			}
		}
		#endregion
		#region IBlockOutDevice
		public bool Write(Collection.IVector<byte> buffer)
		{
			bool result = true;
			try
			{
				byte[] array = buffer.ToArray(); // TODO: fix this with some kind of array-slice api
				this.stream.Write(array, 0, array.Length);
				if (this.AutoFlush)
					this.Flush();
			}
			catch (System.Exception)
			{
				result = false;
			}
			return result;
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return !this.Peek().NotNull(); } }
		#endregion
		#region IOutDevice Members
		public bool AutoFlush { get; set; }
		public bool Flush()
		{
			this.stream.Flush();
			return !this.AutoFlush; 
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get; private set; }
		public virtual bool Opened { get { return this.Readable || this.Writeable; } }
		public virtual bool Close()
		{
			bool result;
			if (result = this.stream.NotNull())
			{
				this.Flush();
				if (!this.Wrapped)
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
		#region Static Open, Wrap & Create
		#region Open
		public static IBlockDevice Open(System.IO.Stream stream)
		{
			return stream.NotNull() ? new BlockDevice(stream) : null;
		}
		public static IBlockDevice Open(Uri.Locator resource)
		{
			return BlockDevice.Open(resource, System.IO.FileMode.Open);
		}
		static IBlockDevice Open(Uri.Locator resource, System.IO.FileMode mode)
		{
			IBlockDevice result = null;
			if (resource.NotNull())
				switch (resource.Scheme)
				{
					case "assembly":
						result = resource.Authority == "" ? BlockDevice.Open(System.Reflection.Assembly.GetEntryAssembly(), resource.Path) : BlockDevice.Open(System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(resource.Authority)), resource.Path);
						break;
					case "file":
						try
						{
							System.IO.FileStream stream = System.IO.File.Open(System.IO.Path.GetFullPath(resource.PlatformPath), mode, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
							if (stream.NotNull())
								result = new BlockDevice(stream) { Resource = resource }; 
						}
						catch (System.IO.DirectoryNotFoundException)
						{
							result = null;
						}
						catch (System.IO.FileNotFoundException)
						{
							result = null;
						}
						break;
					case "http":
					case "https":
						if (mode == System.IO.FileMode.Open)
						{
							try
							{
								using (System.Net.WebClient client = new System.Net.WebClient())
									result = new BlockDevice(new System.IO.MemoryStream(client.DownloadData(resource))) { Resource = resource };
							}
							catch (System.Net.WebException)
							{
								result = null;
							}
						}
						break;
				}
			return result;
		}
		public static IBlockDevice Open(System.Reflection.Assembly assembly, Uri.Path resource)
		{
			return new BlockDevice(assembly.GetManifestResourceStream(assembly.GetName().Name + ((string)resource).Replace('/', '.'))) { Resource = new Uri.Locator("assembly", assembly.GetName().Name, resource) };
		}
		#endregion
		#region Create
		public static IBlockDevice Create(Uri.Locator resource)
		{
			IBlockDevice result = BlockDevice.Open(resource, System.IO.FileMode.Create);
			if (result.IsNull() && resource.NotNull())
			{
				System.IO.Directory.CreateDirectory(resource.Path.FolderPath.PlatformPath);
				result = BlockDevice.Open(resource, System.IO.FileMode.Create);
			}
			return result;
		}
		#endregion
		#region Wrap
		public static IBlockDevice Wrap(System.IO.Stream stream)
		{
			return stream.NotNull() ? new BlockDevice(stream) { Wrapped = true } : null;
		}
		#endregion
		#endregion
	}
}

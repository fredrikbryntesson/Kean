// 
//  CharacterDevice.cs
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
	public class CharacterDevice :
		ICharacterDevice
	{
		IByteDevice backend;
		Decoder decoder;
		System.Text.Encoding encoding;

		#region Constructors
		public CharacterDevice(System.IO.Stream stream) :
			this(new ByteDevice(stream))
		{ }
		public CharacterDevice(IByteDevice backend) :
			this(backend, System.Text.Encoding.UTF8)
		{ }
		public CharacterDevice(IByteDevice backend, System.Text.Encoding encoding)
		{
			this.backend = backend;
			this.decoder = new Decoder(this.backend, encoding);
			this.encoding = encoding;
		}
		~CharacterDevice()
		{
			this.Close();
		}
		#endregion
		#region ICharacterDevice Members
		public bool Readable { get { return this.backend.NotNull() && this.backend.Readable; } }
		public bool Writeable { get { return this.backend.NotNull() && this.backend.Writeable; } }
		#endregion
		#region ICharacterOutDevice Members
		public bool Write(System.Collections.Generic.IEnumerable<char> buffer)
		{
			return this.backend.Write(new Collection.Enumerable<byte>(() => new Encoder(buffer.GetEnumerator(), this.encoding)));
		}
		#endregion
		#region ICharacterInDevice Members
		public char? Peek()
		{
			return this.decoder.Peek();
		}
		public char? Read()
		{
			return this.decoder.Read();
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return this.decoder.NotNull() && this.decoder.Empty; } }
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return this.backend.Resource; } }
		public virtual bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }
		public virtual bool Close()
		{
			bool result;
			if (result = this.backend.NotNull() && this.backend.Close() && this.decoder.Close())
			{
				this.backend = null;
				this.decoder = null;
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
		public static ICharacterDevice Open(Uri.Locator resource)
		{
			return new CharacterDevice(ByteDevice.Open(resource));
		}
		public static ICharacterDevice Create(Uri.Locator resource)
		{
			return new CharacterDevice(ByteDevice.Create(resource));
		}
		#endregion

	}
}

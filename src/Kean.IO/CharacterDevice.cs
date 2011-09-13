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
using Text = System.Text;

namespace Kean.IO
{
	public class CharacterDevice :
		ICharacterDevice
	{
		System.IO.TextReader reader;
		System.IO.TextWriter writer;

		#region Constructors
		public CharacterDevice(System.IO.Stream stream, Text.Encoding encoding) :
			this(new System.IO.StreamReader(stream, encoding), new System.IO.StreamWriter(stream, encoding))
		{ }
		public CharacterDevice(System.IO.Stream stream) :
			this(new System.IO.StreamReader(stream), new System.IO.StreamWriter(stream))
		{ }
		public CharacterDevice(System.IO.TextReader reader, System.IO.TextWriter writer)
		{
			this.reader = reader;
			this.writer = writer;
		}
		#endregion
		#region ICharacterDevice Members
		public bool Readable { get { return this.reader.NotNull(); } }
		public bool Writeable { get { return this.writer.NotNull(); } }
		#endregion
		#region ICharacterOutDevice Members
		public bool Write(System.Collections.Generic.IEnumerable<char> buffer)
		{
			bool result;
			if (result = this.writer.NotNull())
			{
				foreach (char c in buffer)
					this.writer.Write(c);
				this.writer.Flush();
			}
			return result;
		}
		#endregion
		#region ICharacterInDevice Members
		public char? Peek()
		{
			int result = -1;
			if (this.reader.NotNull())
				result = this.reader.Peek();
			return result < 0 ? null : (char?)result;
		}
		public char? Read()
		{
			int result = -1;
			if (this.reader.NotNull())
				result = this.reader.Read();
			return result < 0 ? null : (char?)result;
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return this.reader.IsNull() || this.reader.Peek() < 0; } }
		#endregion
		#region IDevice Members
		public virtual bool Opened { get { return this.reader.NotNull() && this.writer.NotNull(); } }
		public virtual bool Close()
		{
			bool result;
			if (result = this.reader.NotNull())
			{
				this.reader.Dispose();
				this.reader = null;
			}
			if (result |= this.writer.NotNull())
			{
				this.writer.Dispose();
				this.writer = null;
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

	}
}

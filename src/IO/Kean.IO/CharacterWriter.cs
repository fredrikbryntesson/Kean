// 
//  CharacterWriter.cs
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
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Uri = Kean.Core.Uri;

namespace Kean.IO
{
	public class CharacterWriter :
		ICharacterWriter
	{
		ICharacterOutDevice backend;

		public char[] NewLine { get; set; }

		public Uri.Locator Resource { get { return this.backend.Resource; } }
		public bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }

		protected CharacterWriter(ICharacterOutDevice backend)
		{
			this.backend = backend;
			this.NewLine = new char[] { '\n' };
		}
		~CharacterWriter()
		{
			this.Close();
		}
		public bool Close()
		{
			bool result;
			if (result = this.backend.NotNull() && this.backend.Close())
				this.backend = null;
			return result;
		}
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion

		#region ICharacterWriter Members
		public bool Write(params char[] buffer)
		{
			return this.Write((System.Collections.Generic.IEnumerable<char>)buffer);
		}
		public bool Write(string value)
		{
			return value.IsNull() || this.Write(value.ToCharArray());
		}
		public bool Write<T>(T value) where T : IConvertible
		{
			return this.Write(value.ToString((IFormatProvider)System.Globalization.CultureInfo.InvariantCulture.GetFormat(typeof(T))));
		}
		public bool Write(string format, params object[] arguments)
		{
			return this.Write(string.Format(format, arguments));
		}
		public bool Write(System.Collections.Generic.IEnumerable<char> buffer)
		{
			return this.backend.Write(new Collection.Enumerable<char>(() => new NewLineConverter(buffer.GetEnumerator(), this.NewLine)));
		}
		public bool WriteLine()
		{
			return this.Write(new char[] { '\n' });
		}
		public bool WriteLine(params char[] buffer)
		{
			return this.Write((System.Collections.Generic.IEnumerable<char>) buffer.Merge(new char[] {'\n'}));
		}
		public bool WriteLine(string value)
		{
			return this.Write(value + "\n");
		}
		public bool WriteLine<T>(T value) where T : IConvertible
		{
			return this.WriteLine(value.ToString((IFormatProvider)System.Globalization.CultureInfo.InvariantCulture.GetFormat(typeof(T))));
		}
		public bool WriteLine(string format, params object[] arguments)
		{
			return this.WriteLine(string.Format(format, arguments));
		}
		public bool WriteLine(System.Collections.Generic.IEnumerable<char> buffer)
		{
			return this.Write((System.Collections.Generic.IEnumerable<char>)buffer) && this.WriteLine();
		}
		#endregion
		#region NewLineConverter Class
		class NewLineConverter :
			System.Collections.Generic.IEnumerator<char>
		{
			System.Collections.Generic.IEnumerator<char> backend;
			char[] newLine;
			int index;
			#region Constructors
			public NewLineConverter(System.Collections.Generic.IEnumerator<char> backend, params char[] newLine)
			{
				this.backend = backend;
				this.newLine = newLine;
			}
			#endregion
			#region IEnumerator<char> Members
			public char Current { get; private set; }
			#endregion
			#region IDisposable Members
			public void Dispose()
			{
				this.backend.Dispose();
			}
			#endregion
			#region IEnumerator Members
			object System.Collections.IEnumerator.Current { get { return this.Current; } }
			public bool MoveNext()
			{
				bool result = true;
				if (this.index > 0)
				{
					this.Current = this.newLine[this.index++];
					if (this.index >= this.newLine.Length)
						this.index = 0;
				}
				else if (this.backend.MoveNext())
				{
					if (this.backend.Current == '\n')
					{
						this.Current = this.newLine[0];
						if (this.newLine.Length > 1)
							this.index = 1;
					}
					else
						this.Current = this.backend.Current;
				}
				else
					result = false;
				return result;
			}
			public void Reset()
			{
				this.backend.Reset();
				this.index = 0;
			}
			#endregion
		}
		#endregion
		#region Static Open & Create
		public static ICharacterWriter Open(Uri.Locator resource)
		{
			return CharacterWriter.Open(CharacterDevice.Open(resource));
		}
		public static ICharacterWriter Create(Uri.Locator resource)
		{
			return CharacterWriter.Open(CharacterDevice.Create(resource));
		}
		public static ICharacterWriter Open(ICharacterOutDevice backend)
		{
			return backend.NotNull() ? new CharacterWriter(backend) : null;
		}
		#endregion
	}
}

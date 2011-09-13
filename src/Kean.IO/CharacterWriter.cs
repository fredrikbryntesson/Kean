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

namespace Kean.IO
{
	public class CharacterWriter :
		ICharacterWriter
	{
		ICharacterOutDevice backend;

		public char[] NewLine { get; set; }

		public bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }

		public CharacterWriter(ICharacterOutDevice backend)
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
			return this.backend.Write(buffer);
		}
		public bool Write(string value)
		{
			return this.Write(value.ToCharArray());
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
			return this.backend.Write(buffer);
		}
		public bool WriteLine()
		{
			return this.Write(this.NewLine);
		}
		public bool WriteLine(params char[] buffer)
		{
			return this.backend.Write(buffer.Merge(this.NewLine));
		}
		public bool WriteLine(string value)
		{
			return this.Write(value + new string(this.NewLine));
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
			return this.backend.Write(buffer) && this.WriteLine();
		}
		#endregion

	}
}

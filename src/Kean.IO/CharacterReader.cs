// 
//  CharacterReader.cs
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
using Uri = Kean.Core.Uri;

namespace Kean.IO
{
	public class CharacterReader :
		ICharacterReader
	{
		ICharacterInDevice backend;

		public Uri.Locator Resource { get; private set; }
		public int Row { get; private set; }
		public int Column { get; private set; }

		public bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }
		public bool Empty { get { return this.backend.NotNull() && this.backend.Empty; } }
		public char Last { get; private set; }

		public CharacterReader(ICharacterInDevice backend)
		{
			this.backend = backend;
			this.Row = 1;
		}
		~CharacterReader()
		{
			this.Close();
		}
		public bool Next()
		{
			if (this.Last == '\n')
			{
				this.Row++;
				this.Column = 1;
			}
			else
				this.Column++;
			char? next = this.backend.Read();
			bool result;
			if (!next.HasValue)
				this.Last = '\0';
			else if (next == '\r' && this.backend.Peek().HasValue && this.backend.Peek() == '\n')
			{
				this.backend.Read();
				this.Last = '\n';
			}
			else if (next == '\r')
			{
				this.Last = '\n';
			}
			else
				this.Last = (char)next;
			return next.HasValue;
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
		#region Static Open
		public static ICharacterReader Open(Uri.Locator resource)
		{
			return new CharacterReader(CharacterDevice.Open(resource)) { Resource = resource };
		}
		#endregion
	}
}

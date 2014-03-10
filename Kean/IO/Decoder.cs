﻿// 
//  Decoder.cs
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
using Kean;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Error = Kean.Error;
using Kean.IO.Extension;

namespace Kean.IO
{
	class Decoder :
		ICharacterInDevice
	{
		IByteInDevice backend;
		System.Text.Encoding encoding;
		Collection.IQueue<char> queue = new Collection.Queue<char>();

		#region Constructors
		public Decoder(IByteInDevice backend, System.Text.Encoding encoding)
		{
			this.backend = backend;
			this.encoding = encoding;
		}
		~Decoder() { Error.Log.Wrap((Func<bool>)this.Close)(); }
		#endregion
		void FillQueue()
		{
			this.queue.Enqueue(this.backend.AsEnumerable().Decode(this.encoding));
		}
		#region ICharacterInDevice Members
		public char? Peek()
		{
			if (this.queue.Empty)
				this.FillQueue();
			return this.queue.Empty ? (char?)null : this.queue.Peek();
		}
		public char? Read()
		{
			if (this.queue.Empty)
				this.FillQueue();
			char? result = this.queue.Empty ? (char?)null : this.queue.Dequeue();
			return result;
		}
		#endregion

		#region IInDevice Members
		public bool Empty
		{
			get { return this.queue.Empty && (this.backend.IsNull() || this.backend.Empty); }
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return this.backend.Resource; } }
		public bool Opened
		{
			get { return !this.queue.Empty || this.backend.NotNull() && this.backend.Opened; }
		}
		public bool Close()
		{
			bool result;
			if (result = this.backend.NotNull() && this.backend.Close())
				this.backend = null;
			return result;
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose() { this.Close(); }
		#endregion
	}
}

// 
//  Stream.cs
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

namespace Kean.IO
{
	public abstract class Stream :
		IDisposable
	{
		public abstract bool Opened { get; }
		public abstract bool Ended { get; }
		protected Stream()
		{ }
		~Stream()
		{
			(this as IDisposable).Dispose();
		}
		public abstract int Read();
		public abstract int Peek();
		public abstract void Write(byte value);
		public abstract void Close();


		#region IDisposable Members
		void IDisposable.Dispose()
		{
			if (this.Opened)
				this.Close();
		}
		#endregion
	}
}

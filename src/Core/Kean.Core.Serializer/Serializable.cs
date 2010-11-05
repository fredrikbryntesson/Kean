// 
//  Serializable.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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

namespace Kean.Core.Serializer
{
	public abstract class Serializable<T> :
		Storage.ISerializable,
		IEquatable<Storage.ISerializable>
		where T : new()
	{
		internal ulong Identifier { get; set; }
		ulong Storage.ISerializable.Identifier { get { return this.Identifier; } }
		protected Serializable() { }
		
		void Storage.ISerializable.Serialize(System.IO.Stream stream)
		{
		}
		
		public bool Equals(Storage.ISerializable other)
		{
			return (other is Storage.ISerializable) && this.Identifier == other.Identifier;
		}
	}
}
 
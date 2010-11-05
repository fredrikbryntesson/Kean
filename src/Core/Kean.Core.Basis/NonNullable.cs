// 
//  NonNullable.cs
//  
//  Author:
//       smika <${AuthorEmail}>
//  
//  Copyright (c) 2010 smika
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

namespace Kean.Core.Basis
{
	public struct NonNullable<T>
		where T : class, new()
	{
		T nullable;
		private NonNullable(T nullable)
		{
			this.nullable = nullable;
		}
		public static implicit operator NonNullable<T>(T nullable)
		{
			return new NonNullable<T>(nullable);
		}
		public static implicit operator T(NonNullable<T> nonNullable)
		{
			if (nonNullable.nullable == null)
				nonNullable.nullable = new T();
			return nonNullable.nullable;
		}
	}
}

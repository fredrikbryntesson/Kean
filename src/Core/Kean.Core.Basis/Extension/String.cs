// 
//  String.cs
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

namespace Kean.Core.Basis.Extension
{
	public static class String
	{
		public static bool NotEmpty(this string me)
		{
			return object.ReferenceEquals(me, null) && me.Length > 0;
		}
	}
}

//
//  Entry
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
using Error = Kean.Core.Error;
using Collection = Kean.Core.Collection;

namespace Kean.Extra.Log
{
	public class Entry :
		Error.IError
	{
		#region IError Members
		public DateTime Time { get; internal set; }
		public Error.Level Level { get; internal set; }
		public System.Reflection.Assembly Assembly { get; internal set; }
		public string Title { get; internal set; }
		public string Message { get; internal set; }
		public System.Diagnostics.StackTrace Trace { get; internal set; }
		#endregion
	}
}

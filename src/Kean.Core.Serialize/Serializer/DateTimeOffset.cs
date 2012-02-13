﻿// 
//  DateTimeOffset.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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

namespace Kean.Core.Serialize.Serializer
{
	public class DateTimeOffset :
		ISerializer
	{
		public DateTimeOffset()
		{ }
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type)
		{
			return type == "System.DateTimeOffset" ? this : null;
		}
		public Data.Node Serialize(Storage storage, Reflect.Type type, object data)
		{
			return new Data.DateTimeOffset(data, type);
		}
		public object Deserialize(Storage storage, Data.Node data)
		{
			return data is Data.DateTimeOffset ? (data as Data.DateTime).Value :
				data is Data.Binary ? new System.DateTimeOffset(BitConverter.ToInt64((data as Data.Binary).Value, 0), new System.TimeSpan(BitConverter.ToInt64((data as Data.Binary).Value, 8))) :
				data is Data.String ? System.DateTimeOffset.Parse((data as Data.String).Value) :
				new System.DateTimeOffset();
		}
		#endregion
	}
}


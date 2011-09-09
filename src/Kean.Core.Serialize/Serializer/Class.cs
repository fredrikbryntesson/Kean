// 
//  Class.cs
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
using Kean.Core;
using Kean.Core.Extension;
using Kean.Core.Reflect.Extension;

namespace Kean.Core.Serialize.Serializer
{
	public class Class :
		Abstract
	{
		public Class()
		{
		}
		protected override bool Accepts (Reflect.Type type)
		{
			return type.Category == Reflect.TypeCategory.Class;
		}
		protected override T Deserialize<T> (Storage storage, Reflect.Type type, Data.Node data)
		{
			throw new NotImplementedException ();
		}
		protected override Data.Node Serialize<T> (Storage storage, Reflect.Type type, T data)
		{
			Data.Branch result = new Data.Branch() { Type = type };
			foreach (Reflect.Property property in data.GetProperties())
			{
				ParameterAttribute[] attributes = property.GetAttributes<ParameterAttribute>();
				if (attributes.Length == 1)
				{
//					Data.Node data = storage
				}
			}
			return result;
		}
	}
}


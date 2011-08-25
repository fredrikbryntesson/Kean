// 
//  StringInterface.cs
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

namespace Kean.Core.Serialize.Serializer
{
	public class StringInterface :
		Abstract
	{
		public StringInterface()
		{
		}
		public override bool Accepts(Type type)
		{
			return type is IString;
		}
		protected override Data.Node Serialize<T> (Storage storage, Reflect.TypeName type, T data)
		{
			return new Data.Leaf<string>((data as IString).String);
		}
		protected override T Deserialize<T> (Storage storage, Reflect.TypeName type, Data.Node data)
		{
			T result;
			if (data is Data.Leaf<string>)
			{
				result = type.Create<T>();
				(result as IString).String = (data as Data.Leaf<string>).Value;
			}
			else
				result = default(T);
			return result;
		}
	}
}


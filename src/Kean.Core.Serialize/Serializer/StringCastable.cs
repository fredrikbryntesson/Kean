// 
//  StringCastable.cs
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

namespace Kean.Core.Serialize.Serializer
{
	public class StringCastable :
		Abstract
	{
		public StringCastable()
		{
		}
		public override bool Accepts(Type type)
		{
			System.Reflection.MethodInfo toString = this.GetToStringCast(type);
			return this.GetFromStringCast(type).NotNull() &&
					this.GetToStringCast(type).NotNull();
		}
		protected override Data.Node Serialize<T> (Storage storage, Reflect.TypeName type, T data)
		{
			return new Data.Leaf<string>((string)this.GetToStringCast(type).Invoke(null, System.Reflection.BindingFlags.Static, null, new object[] { data }, System.Globalization.CultureInfo.InvariantCulture));
		}
		protected override T Deserialize<T> (Storage storage, Reflect.TypeName type, Data.Node data)
		{
			return (T)this.GetFromStringCast(type).Invoke(null, System.Reflection.BindingFlags.Static, null, new object[] { data is Data.Leaf<string> ? (data as Data.Leaf<string>).Value : null }, System.Globalization.CultureInfo.InvariantCulture);
		}
		System.Reflection.MethodInfo GetFromStringCast(Type type)
		{
			return type.GetMethod("op_Implicit", new Type[] { typeof(string) }) ?? type.GetMethod("op_Explicit", new Type[] { typeof(string) });
		}
		System.Reflection.MethodInfo GetToStringCast(Type type)
		{
			System.Reflection.MethodInfo result = null;
			foreach (System.Reflection.MethodInfo method in type.GetMethods())
			{
				if (method.IsStatic && method.ReturnType == typeof(string) && (method.Name == "op_Implicit" || method.Name == "op_Explicit"))
				{
					System.Reflection.ParameterInfo[] parameters = method.GetParameters();
					if (parameters.Length == 1 && parameters[0].ParameterType == type)
					{
						result = method;
						break;
					}
				}
			}
			return result;
		}
	}
}


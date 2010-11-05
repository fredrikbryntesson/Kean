// 
//  Configure.cs
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
using Kean.Core.Basis.Extension;

namespace Kean.Core.Configure
{
	public class Configurator :
		IConfigurable
	{
		public object Target { get; set; }
		public object this[string path] 
		{
			get 
			{
				return null;
			}
			set
			{
			}
		}
		public Configurator()
		{
		}
		public Configurator(object target)
		{
			this.Target = target;
		}
		public static object Get(object target, string path)
		{
			object result = null;
			string[] splitted = path.Split(new char[] { '.' }, 2);
			if (splitted.NotNull() && splitted.Length > 0 && splitted[0].NotEmpty()) {
				Parameter parameter = new Parameter(target, splitted[0]);
				if (splitted.Length > 1) {
					object v = parameter.Value;
					if (v is IConfigurable)
						result = (v as IConfigurable)[splitted[1]];
				}
				else	
					result = parameter.Value;
			}
			return result;
		}
		public static void Set(object target, string path, object value)
		{
			string[] splitted = path.Split(new char[] { '.' }, 2);
			if (splitted.NotNull() && splitted.Length > 0 && splitted[0].NotEmpty()) 
			{
				Parameter parameter = new Parameter(target, splitted[0]);
				if (splitted.Length > 1) 
				{
					object v = parameter.Value;
					if (v is IConfigurable)
						(v as IConfigurable)[splitted[1]] = value;
				}
				else
					parameter.Value = value;
			}
		}
	}
}


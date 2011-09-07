// 
//  Abstract.cs
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
using Reflect = Kean.Core.Reflect;

namespace Kean.Cli.Processor.Parameter
{
	abstract class Abstract
	{
		public Reflect.Type Type { get; private set; }
		Reflect.Parameter parameter;
		protected Abstract(Reflect.Type type, Reflect.Parameter parameter)
		{
			this.Type = type;
			this.parameter = parameter;
		}
		public abstract string Complete(string incomplete);
		public abstract string Help(string incomplete);
		public static Abstract Create(Reflect.Parameter parameter)
		{
			Abstract result = null;
			Reflect.Type type = parameter.Type;
			if (type == typeof(Boolean))
				result = new Boolean(type, parameter);
			else if (type == typeof(string))
				result = new String(type, parameter);
			return null;
		}
	}
}

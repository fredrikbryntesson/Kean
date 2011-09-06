// 
//  Property.cs
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
using Reflect = Kean.Core.Reflect;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;

namespace Kean.Cli.Processor
{
	class Property :
		Member
	{
		Func<string> get;
		Action<string> set; 
		public string Value
		{
			get { return this.get(); }
			set { this.set(value); }
		}
		public Property(PropertyAttribute attribute, Reflect.Property backend, Object parent) :
			base(attribute, backend, parent)
		{
			if (backend.Type == typeof(bool))
			{
				Reflect.Property<bool> b = backend.Convert<bool>();
				this.get = () => b.Value ? "true" : "false";
				this.set = value => b.Value = value.Trim().ToLower().Contains("true");
			}
			else if (backend.Type == typeof(string))
			{
				Reflect.Property<string> b = backend.Convert<string>();
				this.get = () => b.Value;
				this.set = value => b.Value = value;
			}
		}
	}
}

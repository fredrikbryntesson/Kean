// 
//  Property.cs
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
namespace Kean.Core.Reflect
{
	public class Property
	{
		object parent;
		Type parentType;
		System.Reflection.PropertyInfo information;
		public string Name { get; private set; }
		public object Value
		{
			get { return this.information.GetValue(this.parent, null); }
			set { this.information.SetValue(this.parent, value, null); }
		}
		internal Property(object parent, string name)
		{
			this.parent = parent;
			this.parentType = this.parent.GetType();
			this.Name = name;
			this.information = this.parentType.GetProperty(name);
		}
		internal Property(object parent, System.Reflection.PropertyInfo property)
		{
			this.parent = parent;
			this.parentType = this.parent.GetType();
			this.Name = property.Name;
			this.information = property;
		}
	}
}


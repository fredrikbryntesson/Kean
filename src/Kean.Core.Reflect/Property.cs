// 
//  Property.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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
	public class Property : 
		Member
	{
		protected System.Reflection.PropertyInfo PropertyInformation { get; private set; }
		public object Data
		{
			get { return this.PropertyInformation.GetValue(this.Parent, null); }
			set { this.PropertyInformation.SetValue(this.Parent, value, null); }
		}
		public bool Readable { get { return this.PropertyInformation.CanRead; } }
		public bool Writable { get { return this.PropertyInformation.CanWrite; } }
		public Type Type { get { return this.PropertyInformation.PropertyType; } }

		internal Property(object parent, Type parentType, System.Reflection.PropertyInfo propertyInformation) :
			base(parent, parentType, propertyInformation)
		{
			this.PropertyInformation = propertyInformation;
		}
		public Property<T> Convert<T>()
		{
			return new Property<T>(this.Parent, this.ParentType, this.PropertyInformation);
		}
		internal static Property Create(object parent, Type parentType, System.Reflection.PropertyInfo propertyInformation)
		{
			Property result;
			if (propertyInformation.PropertyType == typeof(int))
				result = new Property<int>(parent, parentType, propertyInformation);
			else
				result = new Property(parent, parentType, propertyInformation);
			return result;
		}
	}
	public class Property<T> :
		Property
	{
		public T Value
		{
			get { return (T)this.PropertyInformation.GetValue(this.Parent, null); }
			set { this.PropertyInformation.SetValue(this.Parent, value, null); }
		}
		internal Property(object parent, Type parentType, System.Reflection.PropertyInfo propertyInformation) :
			base(parent, parentType, propertyInformation)
		{ }
	}
}


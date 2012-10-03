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

using Kean.Core;
using Kean.Core.Extension;

namespace Kean.Core.Reflect
{
	public class Property : 
		Member,
		IComparable<Property>
	{
		protected System.Reflection.PropertyInfo Information { get; private set; }
		public object Data
		{
			get { return this.Information.GetValue(this.Parent, null); }
			set { this.Information.SetValue(this.Parent, value, null); }
		}
		public bool Readable { get { return this.Information.CanRead; } }
		public bool Writable { get { return this.Information.CanWrite; } }
		public Type Type { get { return this.Information.PropertyType; } }

		protected Property(object parent, Type parentType, System.Reflection.PropertyInfo information) :
			base(parent, parentType, information)
		{
			this.Information = information;
		}
		public Property<T> Convert<T>()
		{
			return new Property<T>(this.Parent, this.ParentType, this.Information);
		}
		internal static Property Create(object parent, Type parentType, System.Reflection.PropertyInfo information)
		{
			Property result;
			if (information.PropertyType == typeof(int))
				result = new Property<int>(parent, parentType, information);
			else
				result = new Property(parent, parentType, information);
			return result;
		}
		#region IComparable<Property> Members
		Order IComparable<Property>.Compare(Property other)
		{
			return this.Name.CompareWith(other.NotNull() ? other.Name : null);
		}
		#endregion
	}
	public class Property<T> :
		Property
	{
		public T Value
		{
			get { return (T)this.Information.GetValue(this.Parent, null); }
			set { this.Information.SetValue(this.Parent, value, null); }
		}
		internal Property(object parent, Type parentType, System.Reflection.PropertyInfo information) :
			base(parent, parentType, information)
		{ }
	}
}


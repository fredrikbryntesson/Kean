// 
//  Member.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;

using Kean.Core;
using Kean.Core.Extension;
using Kean.Core.Reflect.Extension;

namespace Kean.Core.Reflect
{
	public abstract class Member :
		IComparable<Member>
	{
		protected object Parent { get; private set; }
		protected Type ParentType { get; private set; }
		System.Reflection.MemberInfo information;
		public string Name { get; private set; }
		private System.Attribute[] attributes;
		public System.Attribute[] Attributes
		{
			get
			{
				if (this.attributes.IsNull())
					this.attributes = this.information.GetCustomAttributes(true).Map(attribute => attribute as System.Attribute);
				return this.attributes;
			}
		}
		internal Member(object parent, Type parentType, System.Reflection.MemberInfo information)
		{
			this.Parent = parent;
			this.ParentType = parentType;
			this.information = information;
			this.Name = information.Name;
		}
		public T[] GetAttributes<T>()
			where T : System.Attribute
		{
			return this.information.GetCustomAttributes(typeof(T), true).Map(attribute => attribute as T);
		}

		#region IComparable<Member> Members
		public Order Compare(Member other)
		{
			return this.Name.CompareWith(other.NotNull() ? other.Name : null);
		}
		#endregion
		internal static Member Create(object parent, Type parentType, System.Reflection.MemberInfo memberInformation)
		{
			return Member.Create(parent, parentType, memberInformation, MemberFilter.Field | MemberFilter.Property | MemberFilter.Method);
		}
		internal static Member Create(object parent, Type parentType, System.Reflection.MemberInfo memberInformation, MemberFilter filter)
		{
			Member result = null;
			if (memberInformation is System.Reflection.PropertyInfo && filter.Contains(MemberFilter.Property))
				result = Property.Create(parent, parentType, memberInformation as System.Reflection.PropertyInfo);
			else if (memberInformation is System.Reflection.FieldInfo && filter.Contains(MemberFilter.Property))
				result = Field.Create(parent, parentType, memberInformation as System.Reflection.FieldInfo);
			else if (memberInformation is System.Reflection.MethodInfo && filter.Contains(MemberFilter.Method))
				result = Method.Create(parent, parentType, memberInformation as System.Reflection.MethodInfo);
			return result;
		}
	}
}

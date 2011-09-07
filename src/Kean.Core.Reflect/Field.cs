// 
//  Field.cs
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

namespace Kean.Core.Reflect
{
	public class Field :
		Member
	{
		protected System.Reflection.FieldInfo FieldInformation { get; private set; }
		public object Data
		{
			get { return this.FieldInformation.GetValue(this.Parent); }
			set { this.FieldInformation.SetValue(this.Parent, value); }
		}
		public Type Type { get { return this.FieldInformation.FieldType; } }
		internal Field(object parent, Type parentType, System.Reflection.FieldInfo fieldInformation) :
			base(parent, parentType, fieldInformation)
		{
			this.FieldInformation = fieldInformation;
		}
		public Field<T> Convert<T>()
		{
			return new Field<T>(this.Parent, this.ParentType, this.FieldInformation);
		}
		internal static Field Create(object parent, Type parentType, System.Reflection.FieldInfo fieldInformation)
		{
			Field result;
			if (fieldInformation.FieldType == typeof(int))
				result = new Field<int>(parent, parentType, fieldInformation);
			else
				result = new Field(parent, parentType, fieldInformation);
			return result;
		}
	}
	public class Field<T> :
		Field
	{
		public T Value
		{
			get { return (T)this.FieldInformation.GetValue(this.Parent); }
			set { this.FieldInformation.SetValue(this.Parent, value); }
		}
		internal Field(object parent, Type parentType, System.Reflection.FieldInfo fieldInformation) :
			base(parent, parentType, fieldInformation)
		{ }
	}
}


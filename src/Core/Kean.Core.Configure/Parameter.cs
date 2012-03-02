// 
//  Parameter.cs
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

namespace Kean.Core.Configure
{
	public class Parameter
	{
		object target;
		System.Reflection.PropertyInfo property;
		ParameterAttribute attribute;
		bool stringSerializable;

		public string Name { get; private set; }
		public object Default { get { return this.attribute.Default; } }
		public object Value
		{
			get
			{
				object result = property.GetValue(this, null);
	        	if(this.stringSerializable && result is IString)
	        		result = (result as IString).String;
				return result;
			}
			set
			{
	        	if (this.stringSerializable)
	        	{
	        		IString v = System.Activator.CreateInstance(this.property.PropertyType) as IString;
	        		v.String = value as string;
	        		property.SetValue(this.target, v, null);                                
	        	}
	        	else
	        		property.SetValue(this.target, value, null);                                
			}
		}
		public Parameter(object target, string name)
		{
			this.Name = name;
			this.target = target;
			this.property = this.target.GetType().GetProperty(name);
			if (this.property != null)
			{
                ParameterAttribute[] attributes = (property.GetCustomAttributes(typeof(ParameterAttribute), true) as ParameterAttribute[]);
				if (attributes.Length == 1)
					this.attribute = attributes[0];
				else
					this.property = null;
			}
			if (this.property == null)
	            foreach (System.Reflection.PropertyInfo property in this.GetType().GetProperties())
	            {
	                ParameterAttribute[] attributes = property.GetCustomAttributes(typeof(ParameterAttribute), true) as ParameterAttribute[];
					if (attributes.Length == 1 && attributes[0].Name == this.Name)
					{
						this.property = property;
						this.attribute = attributes[0];
						break;
					}
				}
			this.stringSerializable = this.property.PropertyType.GetInterface(typeof(IString).Name) != null;
		}

		public void SetDefault()
		{
			this.Value = this.Default;
		}

	}
}

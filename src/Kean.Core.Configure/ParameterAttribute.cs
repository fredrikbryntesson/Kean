// 
//  ParameterAttribute.cs
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
	[System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class ParameterAttribute :
		Attribute
	{
    	public string Name { get; set; }
		/// <summary>
        /// If set the property is initialized to the given value.
        /// </summary>
        public object Default { get; set; }
        /// <summary>
        /// True if property shall be initialized to default value. Defaults to true.
        /// </summary>
        public bool Initialize { get; set; }
		
		public ParameterAttribute()
		{
        	this.Initialize = true;
		}
		public ParameterAttribute(object defaultValue) :
			this()
		{
			this.Default = defaultValue;
		}
		public ParameterAttribute(string name, object defaultValue) :
			this()
		{
			this.Name = name;
			this.Default = defaultValue;
		}
	}
}

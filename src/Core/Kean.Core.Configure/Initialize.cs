// 
//  Initialize.cs
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
	public abstract class Initialize :
		Configurable
	{
		/// <summary>
		/// Constructor that scans for default values for properties and sets them.
		/// </summary>
        protected Initialize()
        {
            foreach (System.Reflection.PropertyInfo property in this.GetType().GetProperties())
            {
                ParameterAttribute[] attributes = property.GetCustomAttributes(typeof(ParameterAttribute), true) as ParameterAttribute[];

                if (attributes.Length > 0 && attributes[0].Default != null && attributes[0].Initialize)
                {
					bool found = false;
					foreach (System.Type type in property.PropertyType.GetInterfaces())
						if (type == typeof(IString))
						{
							found = true;
						    break;
						};
                	if(found)
                	{
                		IString value = System.Activator.CreateInstance(property.PropertyType) as IString;
                		value.String = attributes[0].Default as string;
                		property.SetValue(this, value, null);                                
                	}
                	else
                		property.SetValue(this, attributes[0].Default, null);                                
                }
            }
        }
	}

}

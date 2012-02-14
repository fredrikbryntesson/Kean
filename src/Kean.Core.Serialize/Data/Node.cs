// 
//  Node.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2012 Simon Mika
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
using Kean.Core.Extension;

namespace Kean.Core.Serialize.Data
{
	public abstract class Node
	{
		public Uri.Locator Locator { get; set; }
		string name;
		public string Name 
		{
			get { return (this.Attribute.NotNull() ? this.Attribute.Name : null) ?? this.name ?? (this.Type.NotNull() ? this.Type.ShortName : null); }
			set { this.name = value; } 
		}
		public ParameterAttribute Attribute { get; set; }
		public Reflect.Type Type { get; set; }
		protected Node()
		{
		}
		public Node UpdateName(string name)
		{
			this.Name = name;
			return this;
		}
		public Node UpdateAttribute(ParameterAttribute attribute)
		{
			this.Attribute = attribute;
			return this;
		}
		public Node DefaultType(Reflect.Type type)
		{
			if (this.Type.IsNull())
				this.Type = type;
			return this;
		}
	}
}

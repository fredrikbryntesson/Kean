﻿// 
//  Embedded.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
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

using System;

namespace Kean.Html.Dom
{
	public class Embedded :
		Element
	{
		protected override string TagName { get { return "embed"; } }
		public string Height { get; set; }
		public string Source { get; set; }
		public string Type { get; set; }
		public string Width { get; set; }
		protected override string FormatAttributes()
		{
			return
				 base.FormatAttributes() +
				 this.FormatAttribute("height", this.Height) +
				 this.FormatAttribute("type", this.Type) +
				 this.FormatAttribute("width", this.Width) +
				 this.FormatAttribute("src", this.Source);
		}
	}
}

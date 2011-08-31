// 
//  Bgr.cs
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
//  You should have received a copy of the GNU Lesser General Public Licenseusing System;
using System;

namespace Kean.Draw.Color
{
	public struct Bgr :
		IColor
	{
		public byte blue;
		public byte green;
		public byte red;
		public Bgr(byte blue, byte green, byte red)
		{
			this.blue = blue;
			this.green = green;
			this.red = red;
		}
	}
}

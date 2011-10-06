// 
//  Bgra.cs
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
	public struct Bgra :
		IColor
	{
		public Bgr color;
		public byte alpha;
		public Bgra(byte blue, byte green, byte red, byte alpha) : this(new Bgr(blue, green, red), alpha) { }
		public Bgra(Bgr color, byte alpha)
		{
			this.color = color;
			this.alpha = alpha;
		}
		#region IColor Members
		public IColor Copy()
		{
			return new Bgra(this.color, this.alpha);
		}
		public T Convert<T>() where T : IColor, new()
		{
			T result = default(T);
			if (typeof(T) == typeof(Y))
				Color.Convert.FromBgr((Y v) => result = (T)(IColor)v)(this.color);
			else if (typeof(T) == typeof(Yuv))
				Color.Convert.FromBgr((Yuv v) => result = (T)(IColor)v)(this.color);
			else if (typeof(T) == typeof(Bgr))
				result = (T)(IColor)this.color;
			else if (typeof(T) == typeof(Bgra))
				result = (T)(IColor)this;
			return result;
		}
		#endregion
		#region Object Overides
		public override string ToString()
		{
			return this.color.ToString() + " " + this.alpha;
		}
		#endregion
	}
}

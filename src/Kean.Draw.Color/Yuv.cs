// 
//  Yuv.cs
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
	public struct Yuv :
		IColor
	{
		public byte y;
		public byte u;
		public byte v;
		public Yuv(byte y, byte u, byte v)
		{
			this.y = y;
			this.u = u;
			this.v = v;
		}
		#region IColor Members
		public IColor Copy()
		{
			return new Yuv(this.y, this.u, this.v);
		}
		public T Convert<T>() where T : IColor, new()
		{
			T result = default(T);
			if (typeof(T) == typeof(Y))
				Color.Convert.FromYuv((Y v) => result = (T)(IColor)v)(this);
			else if (typeof(T) == typeof(Yuv))
				result = (T)(IColor)this;
			else if (typeof(T) == typeof(Bgr))
				Color.Convert.FromYuv((Bgr v) => result = (T)(IColor)v)(this);
			else if (typeof(T) == typeof(Bgra))
			{
				Bgr color = new Bgr();
				Color.Convert.FromYuv((Bgr v) => color = (Bgr)(IColor)v)(this);
				result = (T)(IColor)new Bgra(color, 255);
			}
			return result;
		}
		#endregion
		#region Object Overides
		public override string ToString()
		{
			return this.y + " " + this.u + " " + this.v;
		}
		#endregion
	}
}

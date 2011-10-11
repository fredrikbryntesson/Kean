﻿// 
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

		#region IColor Members
		public IColor Copy()
		{
			return new Bgr(this.blue, this.red, this.green);
		}
		public void Set<T>(T color) where T : IColor
		{
			Bgr c = color.Convert<Bgr>();
			this.blue = c.blue;
			this.green = c.green;
			this.red = c.red;
		}
		public T Convert<T>() where T : IColor, new()
		{
			T result = default(T);
			if (typeof(T) == typeof(Y))
				Color.Convert.FromBgr((Y v) => result = (T)(IColor)v)(this);
			else if (typeof(T) == typeof(Yuv))
				Color.Convert.FromBgr((Yuv v) => result = (T)(IColor)v)(this);
			else if (typeof(T) == typeof(Bgr))
				result = (T)(IColor)this;
			else if (typeof(T) == typeof(Bgra))
				result = (T)(IColor)new Bgra(this, 255);
			return result;
		}
		public IColor Blend(float factor, IColor other)
		{
			Bgr c = other.Convert<Bgr>();
			return new Bgr((byte)(this.blue * (1 - factor) + c.blue * factor), (byte)(this.green * (1 - factor) + c.green * factor), (byte)(this.red * (1 - factor) + c.red * factor));
		}
		public float Distance(IColor other)
		{
			Bgr c = other.Convert<Bgr>();
			return Math.Single.SquareRoot((Math.Single.Squared(this.blue - c.blue) + Math.Single.Squared(this.green - c.green) + Math.Single.Squared(this.red - c.red)) / 3);
		}
		#endregion
		#region Object Overides
		public override string ToString()
		{
			return this.blue + " " + this.green + " " + this.red;
		}
		#endregion
	}
}

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
		public void Set<T>(T color) where T : IColor
		{
			Yuv c = color.Convert<Yuv>();
			this.y = c.y;
			this.u = c.u;
			this.v = c.v;
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
		public IColor Blend(float factor, IColor other)
		{
			Yuv c = other.Convert<Yuv>();
			return new Yuv((byte)(this.y * (1 - factor) + c.y * factor), (byte)(this.u * (1 - factor) + c.u * factor), (byte)(this.v * (1 - factor) + c.v * factor));
		}
		public float Distance(IColor other)
		{
			Yuv c = other.Convert<Yuv>();
			return Math.Single.SquareRoot((Math.Single.Squared(this.y - c.y) + Math.Single.Squared(this.u - c.u) + Math.Single.Squared(this.v - c.v)) / 3);
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

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
		public Bgra(byte blue, byte green, byte red) : this(blue, green, red, 255) { }
		public Bgra(byte blue, byte green, byte red, byte alpha) : this(new Bgr(blue, green, red), alpha) { }
		public Bgra(Bgr color, byte alpha)
		{
			this.color = color;
			this.alpha = alpha;
		}
		#region IColor Members
		public void Set<T>(T color) where T : IColor
		{
			Bgra c = color.Convert<Bgra>();
			this.alpha = c.alpha;
			this.color.Set(c.color);
		}
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
		public IColor Blend(float factor, IColor other)
		{
			Bgra c = other.Convert<Bgra>();
			return new Bgra(this.color.Blend(factor, c.color).Convert<Bgr>(), (byte)(this.alpha * (1 - factor) + c.alpha * factor));
		}
		public float Distance(IColor other)
		{
			Bgra c = other.Convert<Bgra>();
			return Math.Single.SquareRoot((Math.Single.Squared(this.color.Distance(c.color)) + Math.Single.Squared(this.alpha - c.alpha)) / 2);
		}
		#endregion
		#region Object Overides
		public override string ToString()
		{
			return this.color.ToString() + " " + this.alpha;
		}
		#endregion
		#region Static Creators
		public static Bgra White { get { return new Bgra(255, 255, 255); } }
		public static Bgra Black { get { return new Bgra(0, 0, 0); } }
		public static Bgra Blue { get { return new Bgra(255, 0, 0); } }
		public static Bgra Green { get { return new Bgra(0, 255, 0); } }
		public static Bgra Red { get { return new Bgra(0, 0, 255); } }
		public static Bgra Magenta { get { return new Bgra(255, 0, 255); } }
		public static Bgra Yellow { get { return new Bgra(255, 255, 0); } }
		public static Bgra Cyan { get { return new Bgra(255, 255, 0); } }
		#endregion
	}
}

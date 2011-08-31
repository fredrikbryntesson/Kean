// 
//  Bgra.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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
using Buffer = Kean.Core.Buffer;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Raster
{
	public class Bgra :
		Packed
	{
		protected override int BytesPerPixel { get { return 4; } }
		public Bgra(Geometry2D.Integer.Size resolution) :
			base(new byte[Packed.CalculateLength(resolution, 4)], resolution) { }
		public Bgra(Geometry2D.Integer.Size resolution, CoordinateSystem coordinateSystem) :
			base(new Buffer.Vector<byte>(Packed.CalculateLength(resolution, 4)), resolution, coordinateSystem) { }
		public Bgra(byte[] data, Geometry2D.Integer.Size resolution) :
			base(data, resolution) { }
		public Bgra(IntPtr pointer, Geometry2D.Integer.Size resolution) :
			this(new Buffer.Sized(pointer, resolution.Area * 4), resolution) { }
		public Bgra(Buffer.Sized buffer, Geometry2D.Integer.Size resolution) :
			base(buffer, resolution, CoordinateSystem.Default) { }
		public Bgra(Buffer.Sized buffer, Geometry2D.Integer.Size resolution, CoordinateSystem coordinateSystem) :
			base(buffer, resolution, coordinateSystem) { }
		protected Bgra(Bgra original) :
			base(original) { }
		internal Bgra(Image original) :
			this(original.Resolution, original.CoordinateSystem)
		{
			unsafe
			{
				int* destination = (int*)this.Pointer;
				original.Apply(color => *((Color.Bgra*)destination++) = new Color.Bgra(color, 255));
			}
		}
		public override Draw.Image Copy()
		{
			return new Bgra(this);
		}
		public override void Apply(Action<Color.Bgr> action)
		{
			unsafe
			{
				int* end = (int*)this.Pointer + this.Resolution.Area;
				for (int* source = (int*)this.Pointer; source < end; source++)
					action(*((Color.Bgr*)source));
			}
		}
		public override void Apply(Action<Color.Yuv> action)
		{
			this.Apply(Color.Convert.FromBgr(action));
		}
		public override void Apply(Action<Color.Y> action)
		{
			this.Apply(Color.Convert.FromBgr(action));
		}
	}
}

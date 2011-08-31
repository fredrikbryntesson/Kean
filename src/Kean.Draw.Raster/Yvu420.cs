// 
//  Yvu420.cs
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
using Buffer = Kean.Core.Buffer;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Raster
{
	public class Yvu420 :
		Yuv420
	{
		protected Yvu420(Yvu420 original) :
			base(original) { }
		public Yvu420(Geometry2D.Integer.Size resolution) :
			this(new Buffer.Vector<byte>(Packed.CalculateLength(resolution, 1) + 2 * Packed.CalculateLength(resolution / 2, 1)), resolution) { }
		public Yvu420(Geometry2D.Integer.Size resolution, CoordinateSystem coordinateSystem) :
			this(new Buffer.Vector<byte>(Packed.CalculateLength(resolution, 1) + 2 * Packed.CalculateLength(resolution / 2, 1)), resolution, coordinateSystem) { }
		public Yvu420(byte[] data, Geometry2D.Integer.Size resolution) :
			this(new Buffer.Vector<byte>(data), resolution) { }
		public Yvu420(Buffer.Sized buffer, Geometry2D.Integer.Size resolution) :
			this(buffer, resolution, CoordinateSystem.Default) { }
		public Yvu420(Buffer.Sized buffer, Geometry2D.Integer.Size resolution, CoordinateSystem coordinateSystem) :
			base(buffer, resolution, coordinateSystem) { }
		internal Yvu420(Image original) :
			base(original) { }
		protected override Monochrome CreateU()
		{
			return base.CreateV();
		}
		protected override Monochrome CreateV()
		{
			return base.CreateU();
		}
		public override Draw.Image Copy()
		{
			return new Yvu420(this);
		}

		public override float Distance(Draw.Image other)
		{
			return other is Yvu420 ? base.Distance(other) : float.MaxValue;
		}
		public override bool Equals(Draw.Image other)
		{
			return other is Yvu420 && base.Equals(other);
		}
	}
}

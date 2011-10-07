﻿// 
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
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using Buffer = Kean.Core.Buffer;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Raster
{
	public class Yvu420 :
		Yuv420
	{
		protected Yvu420(Yvu420 original) :
			base(original) { }
		public Yvu420(Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(Packed.CalculateLength(size, 1) + 2 * Packed.CalculateLength(size / 2, 1)), size) { }
		public Yvu420(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			this(new Buffer.Vector<byte>(Packed.CalculateLength(size, 1) + 2 * Packed.CalculateLength(size / 2, 1)), size, coordinateSystem) { }
		public Yvu420(byte[] data, Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(data), size) { }
		public Yvu420(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			this(buffer, size, CoordinateSystem.Default) { }
		public Yvu420(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(buffer, size, coordinateSystem) { }
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
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Yvu420(size) { Crop = this.Crop, Wrap = this.Wrap };
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

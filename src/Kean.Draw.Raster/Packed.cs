// 
//  Packed.cs
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
	public abstract class Packed :
		Image
	{
		protected abstract int BytesPerPixel { get; }

		public int Stride { get; private set; }

		protected Packed(byte[] data, Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(data), size, CoordinateSystem.Default) { }
		protected Packed(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(buffer, size, coordinateSystem)
		{
			this.Stride = Packed.CalculateStride(size, this.BytesPerPixel);
		}
		protected Packed(Packed original) :
			base(original)
		{
			this.Stride = original.Stride;
		}

		public override bool Equals(Draw.Image other)
		{
			return other is Packed &&
				this.BytesPerPixel == (other as Packed).BytesPerPixel &&
				base.Equals(other);
		}
		public override int GetHashCode()
		{
			return this.BytesPerPixel.GetHashCode() ^ base.GetHashCode();
		}
		public override float Distance(Draw.Image other)
		{
			return other is Packed && this.BytesPerPixel == (other as Packed).BytesPerPixel ? base.Distance(other) : float.MaxValue;
		}

		internal static int CalculateStride(Geometry2D.Integer.Size size, int bytesPerPixel)
		{
			return size.Width * bytesPerPixel + (4 - (size.Width * bytesPerPixel) % 4) % 4;
		}
		internal static int CalculateLength(Geometry2D.Integer.Size size, int bytesPerPixel)
		{
			return Packed.CalculateStride(size, bytesPerPixel) * size.Height;
		}
	}
}

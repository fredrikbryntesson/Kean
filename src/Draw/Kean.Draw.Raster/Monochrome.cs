// 
//  Monochrome.cs
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

using System;
using Buffer = Kean.Core.Buffer;
using Geometry2D = Kean.Math.Geometry2D;


namespace Kean.Draw.Raster
{
	public class Monochrome :
		Packed
	{
		protected override int BytesPerPixel { get { return 1; } }

		public Monochrome(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default) { }
		public Monochrome(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(new Buffer.Vector<byte>(Packed.CalculateLength(size, 1)), size, coordinateSystem) { }
		public Monochrome(byte[] data, Geometry2D.Integer.Size size) :
			base(data, size) { }
		public Monochrome(IntPtr pointer, Geometry2D.Integer.Size size) :
			this(new Buffer.Sized(pointer, Packed.CalculateLength(size, 1)), size) { }
		public Monochrome(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			base(buffer, size, CoordinateSystem.Default) { }
		public Monochrome(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(buffer, size, coordinateSystem) { }
		protected Monochrome(Monochrome original) :
			base(original) { }
		internal Monochrome(Image original) :
			this(original.Size, original.CoordinateSystem)
		{
			unsafe
			{
				byte* row = (byte*)this.Pointer;
				int rowLength = this.Size.Width;
				byte* rowEnd = row + rowLength;
				byte* destination = row;
				original.Apply((color) =>
				{
					*(destination++) = color;
					if (destination >= rowEnd)
					{
						row += this.Stride;
						destination = row;
						rowEnd = row + rowLength;
					}
				});
			}
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Monochrome(size) { Crop = this.Crop, Wrap = this.Wrap };
		}
		public override Draw.Image Copy()
		{
			return new Monochrome(this);
		}
		public override void Apply(Action<Color.Bgr> action)
		{
			this.Apply(Color.Convert.FromY(action));
		}
		public override void Apply(Action<Color.Yuv> action)
		{
			this.Apply(Color.Convert.FromY(action));
		}
		public override void Apply(Action<Color.Y> action)
		{
			unsafe
			{
				byte* end = (byte*)this.Pointer + this.Length;
				int rowLength = this.Size.Width;
				for (byte* row = (byte*)this.Pointer; row < end; row += this.Stride)
				{
					byte* rowEnd = row + rowLength;
					for (byte* source = row; source < rowEnd; source++)
						action(*source);
				}
			}
		}
	}
}

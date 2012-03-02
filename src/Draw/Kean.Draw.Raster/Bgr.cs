// 
//  Bgr.cs
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
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Buffer = Kean.Core.Buffer;
using Geometry2D = Kean.Math.Geometry2D;


namespace Kean.Draw.Raster
{
	public class Bgr :
		Packed
	{
		protected override int BytesPerPixel { get { return 3; } }
		public Bgr(Geometry2D.Integer.Size size) :
			base(new byte[Packed.CalculateLength(size, 3)], size) { }
		public Bgr(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(new Buffer.Vector<byte>(Packed.CalculateLength(size, 3)), size, coordinateSystem) { }
		public Bgr(byte[] data, Geometry2D.Integer.Size size) :
			base(data, size) { }
		public Bgr(IntPtr pointer, Geometry2D.Integer.Size size) :
			this(new Buffer.Sized(pointer, Packed.CalculateLength(size, 3)), size) { }
		public Bgr(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			base(buffer, size, CoordinateSystem.Default) { }
		public Bgr(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(buffer, size, coordinateSystem) { }
		protected Bgr(Bgr original) :
			base(original) { }
		internal Bgr(Image original) :
			this(original.Size, original.CoordinateSystem)
		{
			unsafe
			{
				byte* row = (byte*)this.Pointer;
				int rowLength = this.Size.Width;
				Color.Bgr* rowEnd = (Color.Bgr*)row + rowLength;
				Color.Bgr* destination = (Color.Bgr*)row;
				original.Apply((color) =>
				{
					*(destination++) = color;
					if (destination >= rowEnd)
					{
						row += this.Stride;
						destination = (Color.Bgr*)row;
						rowEnd = (Color.Bgr*)row + rowLength;
					}
				});
			}
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Bgr(size) { Crop = this.Crop, Wrap = this.Wrap };
		}
		public override Draw.Image Copy()
		{
			return new Bgr(this);
		}
		public override void Apply(Action<Color.Bgr> action)
		{
			unsafe
			{
				byte* end = (byte*)this.Pointer + this.Length;
				int rowLength = this.Size.Width;
				for (byte* row = (byte*)this.Pointer; row < end; row += this.Stride)
				{
					Color.Bgr* rowEnd = (Color.Bgr*)row + rowLength;
					for (Color.Bgr* source = (Color.Bgr*)row; source < rowEnd; source++)
						action(*source);
				}
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

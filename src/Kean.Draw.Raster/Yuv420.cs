// 
//  Yuv420.cs
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
using Buffer = Kean.Core.Buffer;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Raster
{
	public class Yuv420 :
		Planar
	{
		Monochrome yBuffer;
		Monochrome uBuffer;
		Monochrome vBuffer;

		public Yuv420(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default) { }
		public Yuv420(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			this(new Buffer.Vector<byte>(Packed.CalculateLength(size, 1) + 2 * Packed.CalculateLength(size / 2, 1)), size, coordinateSystem) { }
		public Yuv420(byte[] data, Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(data), size) { }
		public Yuv420(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			this(buffer, size, CoordinateSystem.Default) { }
		public Yuv420(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(buffer, size, coordinateSystem)
		{
			this.yBuffer = this.CreateY();
			this.uBuffer = this.CreateU();
			this.vBuffer = this.CreateV();
		}
		protected Yuv420(Yuv420 original) :
			base(original)
		{
			this.yBuffer = this.CreateY();
			this.uBuffer = this.CreateU();
			this.vBuffer = this.CreateV();
		}
		internal Yuv420(Image original) :
			this(original.Size, original.CoordinateSystem)
		{
			unsafe
			{
				int y = 0;
				int x = 0;
				int width = this.Size.Width;

				byte* yRow = (byte*)this.yBuffer.Pointer;
				byte* yDestination = yRow;

				byte* uRow = (byte*)this.uBuffer.Pointer;
				byte* uDestination = uRow;

				byte* vRow = (byte*)this.vBuffer.Pointer;
				byte* vDestination = vRow;

				original.Apply(color =>
				{
					*(yDestination++) = color.y;
					if (x % 2 == 0 && y % 2 == 0)
					{
						*(uDestination++) = color.u;
						*(vDestination++) = color.v;
					}
					x++;
					if (x >= width)
					{
						x = 0;
						y++;

						yRow += this.yBuffer.Stride;
						yDestination = yRow;
						if (y % 2 == 0)
						{
							uRow += this.uBuffer.Stride;
							uDestination = uRow;

							vRow += this.vBuffer.Stride;
							vDestination = vRow;
						}
					}
				});
			}
		}
		protected virtual Monochrome CreateY()
		{
			return new Monochrome(this.Pointer, this.Size);
		}
		protected virtual Monochrome CreateU()
		{
			return new Monochrome(new IntPtr(this.Pointer.ToInt32() + Packed.CalculateLength(this.Size, 1)), this.Size / 2);
		}
		protected virtual Monochrome CreateV()
		{
			return new Monochrome(new IntPtr(this.Pointer.ToInt32() + Packed.CalculateLength(this.Size, 1) + Packed.CalculateLength(this.Size / 2, 1)), this.Size / 2);
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Yuv420(size) { Crop = this.Crop, Wrap = this.Wrap };
		}
		public override Draw.Image Copy()
		{
			return new Yuv420(this);
		}
		public override void Apply(Action<Color.Bgr> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
		public override void Apply(Action<Color.Yuv> action)
		{
			unsafe
			{
				byte* yRow = (byte*)this.yBuffer.Pointer;
				byte* ySource = yRow;

				byte* uRow = (byte*)this.uBuffer.Pointer;
				byte* uSource = uRow;

				byte* vRow = (byte*)this.vBuffer.Pointer;
				byte* vSource = vRow;

				int width = this.Size.Width;
				int height = this.Size.Height;

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						action(new Color.Yuv(*(ySource++), *uSource, *vSource));
						if (x % 2 == 1)
						{
							uSource++;
							vSource++;
						}
					}
					yRow += this.yBuffer.Stride;
					if (y % 2 == 1)
					{
						uRow += this.uBuffer.Stride;
						vRow += this.vBuffer.Stride;
					}
					ySource = yRow;
					uSource = uRow;
					vSource = vRow;
				}
			}
		}
		public override void Apply(Action<Color.Y> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
		public override float Distance(Draw.Image other)
		{
			return other is Yuv420 ? base.Distance(other) : float.MaxValue;
		}
		public override bool Equals(Draw.Image other)
		{
			return other is Yuv420 && base.Equals(other);
		}
	}
}

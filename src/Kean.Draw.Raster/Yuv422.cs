// 
//  Yuv422.cs
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
	/// <summary>
	/// Yuv422 is the format mentioned as YUY2 (see fourcc.org). Order yuyvyuyvyuyv
	/// </summary>
	public class Yuv422 :
		Packed
	{
		protected override int BytesPerPixel { get { return 2; } }

		public Yuv422(Geometry2D.Integer.Size resolution) :
			this(resolution, CoordinateSystem.Default) { }
		public Yuv422(Geometry2D.Integer.Size resolution, CoordinateSystem coordinateSystem) :
			this(new Buffer.Vector<byte>(Packed.CalculateLength(resolution, 2)), resolution, coordinateSystem) { }
		public Yuv422(byte[] data, Geometry2D.Integer.Size resolution) :
			base(data, resolution) { }
		public Yuv422(Buffer.Sized buffer, Geometry2D.Integer.Size resolution) :
			base(buffer, resolution, CoordinateSystem.Default) { }
		public Yuv422(Buffer.Sized buffer, Geometry2D.Integer.Size resolution, CoordinateSystem coordinateSystem) :
			base(buffer, resolution, coordinateSystem) { }
		protected Yuv422(Yuv422 original) :
			base(original) { }
		internal Yuv422(Image original) :
			this(original.Resolution, original.CoordinateSystem)
		{
			unsafe
			{
				int y = 0;
				int x = 0;
				int width = this.Resolution.Width;

				byte* row = (byte*)this.Pointer;
				byte* yDestination = row;
				byte* uDestination = row + 1;
				byte* vDestination = row + 3;

				original.Apply(color =>
				{
					*yDestination = color.y;
					yDestination += 2;
					if (x % 2 == 0)
					{
						*uDestination = color.u;
						*vDestination = color.v;
						uDestination += 4;
						vDestination += 4;
					}
					x++;
					if (x >= width)
					{
						x = 0;
						y++;

						row += this.Stride;
						yDestination = row;
						uDestination = row + 1;
						vDestination = row + 3;
					}
				});
			}
		}

		public override Draw.Image Copy()
		{
			return new Yuv422(this);
		}
		public override void Apply(Action<Color.Bgr> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
		public override void Apply(Action<Color.Yuv> action)
		{
			unsafe
			{
				byte* row = (byte*)this.Pointer;
				byte* ySource = row;
				byte* uSource = row + 1;
				byte* vSource = row + 3;

				int width = this.Resolution.Width;
				int height = this.Resolution.Height;

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						action(new Color.Yuv(*ySource, *uSource, *vSource));
						ySource += 2;
						if (x % 2 == 1)
						{
							uSource += 4;
							vSource += 4;
						}
					}
					row += this.Stride;
					ySource = row;
					uSource = row + 1;
					vSource = row + 3;
				}
			}
		}
		public override void Apply(Action<Color.Y> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
	}
}

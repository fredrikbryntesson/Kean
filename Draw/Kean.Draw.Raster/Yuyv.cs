// 
//  Yuyv.cs
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
	/// <summary>
	/// Yuyv is a packed version of Yuv422 mentioned as YUY2 (see fourcc.org). Order yuyvyuyvyuyv
	/// </summary>
	[System.Runtime.InteropServices.ComVisible(true)]
	public class Yuyv :
		Packed
	{
		protected override int BytesPerPixel { get { return 2; } }

		public Yuyv(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default) { }
		public Yuyv(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			this(new Buffer.Vector<byte>(Packed.CalculateLength(size, 2)), size, coordinateSystem) { }
		public Yuyv(byte[] data, Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(data), size) { }
		public Yuyv(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			base(buffer, size, CoordinateSystem.Default) { }
		public Yuyv(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(buffer, size, coordinateSystem) { }
		protected Yuyv(Yuyv original) :
			base(original) { }
		internal Yuyv(Image original) :
			this(original.Size, original.CoordinateSystem)
		{
			unsafe
			{
				int y = 0;
				int x = 0;
				int width = this.Size.Width;

				byte* row = (byte*)this.Pointer;
				byte* yDestination = row;
				byte* uDestination = row + 1;
				byte* vDestination = row + 3;

				original.Apply(color =>
				{
					*yDestination = color.Y;
					yDestination += 2;
					if (x % 2 == 0)
					{
						*uDestination = color.U;
						*vDestination = color.V;
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
			return new Yuyv(this);
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Yuyv(size) { Crop = this.Crop, Wrap = this.Wrap };
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

				int width = this.Size.Width;
				int height = this.Size.Height;

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
		#region Static Open
		public static new Yuyv OpenResource(System.Reflection.Assembly assembly, string name)
		{
			return Image.OpenResource<Yuyv>(assembly, name);
		}
		public static new Yuyv OpenResource(string name)
		{
			return Image.OpenResource<Yuyv>(System.Reflection.Assembly.GetCallingAssembly(), name);
		}
		public static new Yuyv Open(string filename)
		{
			return Image.Open<Yuyv>(filename);
		}
		public static new Yuyv Open(System.IO.Stream stream)
		{
			return Image.Open<Yuyv>(stream);
		}
		#endregion
	}
}

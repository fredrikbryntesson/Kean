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
using Kean.Core.Extension;
using Buffer = Kean.Core.Buffer;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Core.Collection;
using Integer = Kean.Math.Integer;
using Single = Kean.Math.Single;

namespace Kean.Draw.Raster
{
	[System.Runtime.InteropServices.ComVisible(true)]
	public class Bgr :
		Packed
	{
		public Color.Bgr this[Geometry2D.Integer.Point position]
		{
			get { return this[position.X, position.Y]; }
			set { this[position.X, position.Y] = value; }
		}
		public Color.Bgr this[int x, int y]
		{
			get { unsafe { return *((Color.Bgr*)((byte*)this.Buffer + y * this.Stride) + x); } }
			set { unsafe { *((Color.Bgr*)((byte*)this.Buffer + y * this.Stride) + x) = value; } }
		}
		public Color.Bgr this[Geometry2D.Single.Point position]
		{
			get { return this[position.X, position.Y]; }
		}
		public Color.Bgr this[float x, float y]
		{
			get
			{
				float left = x - Integer.Floor(x);
				float top = y - Integer.Floor(y);

				Color.Bgr topLeft = this[Integer.Floor(x), Integer.Floor(y)];
				Color.Bgr bottomLeft = this[Integer.Floor(x), Integer.Ceiling(y)];
				Color.Bgr topRight = this[Integer.Ceiling(x), Integer.Floor(y)];
				Color.Bgr bottomRight = this[Integer.Ceiling(x), Integer.Ceiling(y)];

				float b, g, r;
				b = top * (left * topLeft.Blue + (1 - left) * topRight.Blue) + (1 - top) * (left * bottomLeft.Blue + (1 - left) * bottomRight.Blue);
				g = top * (left * topLeft.Green + (1 - left) * topRight.Green) + (1 - top) * (left * bottomLeft.Green + (1 - left) * bottomRight.Green);
				r = top * (left * topLeft.Red + (1 - left) * topRight.Red) + (1 - top) * (left * bottomLeft.Red + (1 - left) * bottomRight.Red);

				return new Color.Bgr((byte)b, (byte)g, (byte)r);
			}
		}
		protected override int BytesPerPixel { get { return 3; } }
		public Bgr(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default) { }
		public Bgr(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(new Buffer.Vector<byte>(Packed.CalculateLength(size, 3)), size, coordinateSystem) { }
		public Bgr(byte[] data, Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(data), size) { }
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
		protected override Draw.Cairo.Image CreateCairoImage(Buffer.Sized buffer, Geometry2D.Integer.Size size)
		{
			return new Cairo.Bgr(buffer, size);
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
		public override float Distance(Draw.Image other)
		{
			float result = 0;
			if (other is Bgr && this.Size == other.Size)
			{
				for (int y = 0; y < this.Size.Height; y++)
					for (int x = 0; x < this.Size.Width; x++)
					{
						Color.Bgr pixel = this[x, y];
						float minimumDistance = float.MaxValue;
						for (int otherY = Integer.Maximum(0, y - 1); minimumDistance > 0 && otherY < Integer.Minimum(y + 1, this.Size.Height); otherY++)
							for (int otherX = Integer.Maximum(0, x - 1); minimumDistance > 0 && otherX < Integer.Minimum(x + 1, this.Size.Width); otherX++)
								minimumDistance = Single.Minimum(minimumDistance, pixel.Distance((other as Bgr)[otherX, otherY]));
						result += minimumDistance;
					}
				result /= this.Size.Length;
			}
			else
				result = float.PositiveInfinity;
			return result;
		}
		#region Static Open
		public static new Bgr OpenResource(System.Reflection.Assembly assembly, string name)
		{
			return Image.OpenResource<Bgr>(assembly, name);
		}
		public static new Bgr OpenResource(string name)
		{
			return Image.OpenResource<Bgr>(System.Reflection.Assembly.GetCallingAssembly(), name);
		}
		public static new Bgr Open(string filename)
		{
			return Image.Open<Bgr>(filename);
		}
		public static new Bgr Open(System.IO.Stream stream)
		{
			return Image.Open<Bgr>(stream);
		}
		#endregion
	}
}

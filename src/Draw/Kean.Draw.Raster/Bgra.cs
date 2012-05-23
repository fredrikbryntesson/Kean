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
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Kean.Core.Extension;
using Buffer = Kean.Core.Buffer;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Raster
{
	public class Bgra :
		Packed
	{
		protected override int BytesPerPixel { get { return 4; } }
		public Bgra(Geometry2D.Integer.Size size) :
			base(new byte[Packed.CalculateLength(size, 4)], size) { }
		public Bgra(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(new Buffer.Vector<byte>(Packed.CalculateLength(size, 4)), size, coordinateSystem) { }
		public Bgra(byte[] data, Geometry2D.Integer.Size size) :
			base(data, size) { }
		public Bgra(IntPtr pointer, Geometry2D.Integer.Size size) :
			this(new Buffer.Sized(pointer, size.Area * 4), size) { }
		public Bgra(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			base(buffer, size, CoordinateSystem.Default) { }
		public Bgra(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(buffer, size, coordinateSystem) { }
		protected Bgra(Bgra original) :
			base(original) { }
		internal Bgra(Image original) :
			this(original.Size, original.CoordinateSystem)
		{
			unsafe
			{
				int* destination = (int*)this.Pointer;
				original.Apply(color => *((Color.Bgra*)destination++) = new Color.Bgra(color, 255));
			}
		}
		protected override Draw.Cairo.Image CreateCairoImage(Buffer.Sized buffer, Geometry2D.Integer.Size size)
		{
			return new Cairo.Bgra(buffer, size);
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Bgra(size) { Crop = this.Crop, Wrap = this.Wrap };
		}
		public override Draw.Image Copy()
		{
			return new Bgra(this);
		}
		public override void Apply(Action<Color.Bgr> action)
		{
			unsafe
			{
				int* end = (int*)this.Pointer + this.Size.Area;
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
		#region Static Open
		public static new Bgra OpenResource(System.Reflection.Assembly assembly, string name)
		{
			return Image.OpenResource<Bgra>(assembly, name);
		}
		public static new Bgra OpenResource(string name)
		{
			return Image.OpenResource<Bgra>(System.Reflection.Assembly.GetCallingAssembly(), name);
		}
		public static new Bgra Open(string filename)
		{
			return Image.Open<Bgra>(filename);
		}
		public static new Bgra Open(System.IO.Stream stream)
		{
			return Image.Open<Bgra>(stream);
		}
		#endregion
	}
}

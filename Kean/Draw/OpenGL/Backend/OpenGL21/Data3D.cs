//
//  Data3D.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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

using Kean.Collection.Extension;
using System;
using Collection = Kean.Collection;
using Error = Kean.Error;
using GL = OpenTK.Graphics.OpenGL.GL;
using Geometry2D = Kean.Math.Geometry2D;
using Geometry3D = Kean.Math.Geometry3D;
using Raster = Kean.Draw.Raster;
using Kean.Extension;

namespace Kean.Draw.OpenGL.Backend.OpenGL21
{
	public class Data3D :
		Backend.Data3D
	{
		public Data3D(Backend.Context context) :
			base(context)
		{ }
		protected override int CreateIdentifier()
		{
			return GL.GenTexture();
		}
		protected override void Allocate()
		{
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.Linear);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, (int)OpenTK.Graphics.OpenGL.TextureMagFilter.Linear);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapR, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.MirroredRepeat);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.MirroredRepeat);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.MirroredRepeat);
			GL.TexImage3D(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, 0, OpenTK.Graphics.OpenGL.PixelInternalFormat.Luminance, this.Size.Depth, this.Size.Width, this.Size.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Red, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, IntPtr.Zero);
			//			GL.TexImage3D(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, 0, OpenTK.Graphics.OpenGL.PixelInternalFormat.Luminance, this.Size.Width, this.Size.Height, this.Size.Depth, 0, OpenTK.Graphics.OpenGL.PixelFormat.Red, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, IntPtr.Zero);
		}
		protected override void Allocate(IntPtr pointer)
		{
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.Linear);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, (int)OpenTK.Graphics.OpenGL.TextureMagFilter.Linear);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapR, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.MirroredRepeat);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.MirroredRepeat);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.MirroredRepeat);
			GL.TexImage3D(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, 0, this.PixelInternalFormat, this.Size.Width, this.Size.Height, this.Size.Depth, 0, this.PixelFormat, this.PixelType, pointer);
			//GL.TexImage3D(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, 0, this.PixelInternalFormat, this.Size.Depth, this.Size.Width, this.Size.Height, 0, this.PixelFormat, this.PixelType, pointer);
			//			GL.TexImage3D(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, 0, OpenTK.Graphics.OpenGL.PixelInternalFormat.Luminance, this.Size.Width, this.Size.Height, this.Size.Depth, 0, OpenTK.Graphics.OpenGL.PixelFormat.Red, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, IntPtr.Zero);
		}
		protected override void Load(byte[, ,] data)
		{
			unsafe
			{
				for (int x = 0; x < this.Size.Width; x++)
					for (int y = 0; y < this.Size.Height; y++)
						fixed (byte* pointer = &data[x, y, 0])
							GL.TexSubImage3D(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, 0, 0, x, y, 256, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.Luminance, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, new IntPtr((void*)pointer));
			}
			//GL.TexSubImage3D(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, 0, 0, 0, 0, this.Size.Width, this.Size.Height, this.Size.Depth, OpenTK.Graphics.OpenGL.PixelFormat.Luminance, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, data);
		}
		protected override void Load(IntPtr pointer)
		{
			GL.TexSubImage3D(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, 0, 0, 0, 0, this.Size.Width, this.Size.Height, this.Size.Depth, OpenTK.Graphics.OpenGL.PixelFormat.Luminance, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, pointer);
		}
		public override void Use()
		{
			GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, this.Identifier);
		}
		public override void UnUse()
		{
			GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, 0);
		}
		protected internal override void Delete()
		{
			GL.DeleteTexture(this.Identifier);
			base.Delete();
		}
	}
}

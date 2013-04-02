//
//  Texture.cs
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

using Kean.Core.Collection.Extension;
using System;
using Collection = Kean.Core.Collection;
using Error = Kean.Core.Error;
using GL = OpenTK.Graphics.OpenGL.GL;
using Geometry2D = Kean.Math.Geometry2D;
using Raster = Kean.Draw.Raster;

namespace Kean.Draw.OpenGL.Backend.OpenGL21
{
	public class Texture :
		Backend.Texture
	{
		protected OpenTK.Graphics.OpenGL.PixelInternalFormat InternalFormat { get; private set; }
		protected OpenTK.Graphics.OpenGL.PixelFormat Format { get; private set; }
		protected OpenTK.Graphics.OpenGL.TextureWrapMode WrapMode { get; private set; }
		bool wrap;
		public override bool Wrap 
		{
			get { return this.wrap; }
			set { this.WrapMode = (wrap = value) ? OpenTK.Graphics.OpenGL.TextureWrapMode.MirroredRepeat : OpenTK.Graphics.OpenGL.TextureWrapMode.Clamp; }
		}
		internal protected Texture(Context context) :
			base(context)
		{ }
		protected override int CreateIdentifier()
		{
			return GL.GenTexture();
		}
		public override void Use()
		{
			GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, this.Identifier);
		}
		public override void UnUse()
		{
			GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0);
		}
		public override void Configure()
		{
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.Linear);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, (int)OpenTK.Graphics.OpenGL.TextureMagFilter.Linear);
		}
		protected override void SetFormat(TextureType type, Geometry2D.Integer.Size size)
		{
			switch (this.Type = type)
			{
				default:
				case TextureType.Argb:
					this.InternalFormat = OpenTK.Graphics.OpenGL.PixelInternalFormat.Rgba;
					this.Format = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
					break;
				case TextureType.Rgb:
					this.InternalFormat = OpenTK.Graphics.OpenGL.PixelInternalFormat.Rgb;
					this.Format = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
					break;
				case TextureType.Monochrome:
					this.InternalFormat = OpenTK.Graphics.OpenGL.PixelInternalFormat.Rgba;
					this.Format = OpenTK.Graphics.OpenGL.PixelFormat.Luminance;
					break;
			}
			this.Size = this.Context.ClampTextureSize(size);
		}
		protected override void Create(IntPtr data)
		{
			GL.TexImage2D(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0, this.InternalFormat, this.Size.Width, this.Size.Height, 0, this.Format, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, data);
		}
		protected override void Load(IntPtr data, Geometry2D.Integer.Box region, TextureType type)
		{
			OpenTK.Graphics.OpenGL.PixelFormat format;
			switch (type)
			{
				default:
				case TextureType.Argb:
					format = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
					break;
				case TextureType.Rgb:
					format = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
					break;
				case TextureType.Monochrome:
					format = OpenTK.Graphics.OpenGL.PixelFormat.Luminance;
					break;
			}
			GL.TexSubImage2D(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0, region.Left, region.Top, region.Width, region.Height, format, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, data);
		}
        protected override void Read(IntPtr pointer, Geometry2D.Integer.Box region)
		{
			GL.ReadPixels(region.Left, region.Top, region.Width, region.Height, this.Format, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, pointer);
		}
		public override void Render(Geometry2D.Single.PointValue leftTop, Geometry2D.Single.PointValue rightTop, Geometry2D.Single.PointValue leftBottom, Geometry2D.Single.PointValue rightBottom, Geometry2D.Single.Box rectangle)
		{
			GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, (int)this.WrapMode);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, (int)this.WrapMode);
			GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
			GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.One, OpenTK.Graphics.OpenGL.BlendingFactorDest.OneMinusSrcAlpha);
			GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Quads);
			GL.TexCoord2(leftTop.X, leftTop.Y); GL.Vertex2(rectangle.Left, rectangle.Top);
			GL.TexCoord2(rightTop.X, rightTop.Y); GL.Vertex2(rectangle.Right, rectangle.Top);
			GL.TexCoord2(rightBottom.X, rightBottom.Y); GL.Vertex2(rectangle.Right, rectangle.Bottom);
			GL.TexCoord2(leftBottom.X, leftBottom.Y); GL.Vertex2(rectangle.Left, rectangle.Bottom);
			GL.End();
		}
	}
}

// 
//  Image.cs
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
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using GL = OpenTK.Graphics.OpenGL.GL;
using Error = Kean.Core.Error;
using Log = Kean.Extra.Log;
using Draw = Kean.Draw;
using Gpu = Kean.Draw.Gpu;
using Raster = Kean.Draw.Raster;
using Kean.Gui.OpenGL.Backend.Extension;

namespace Kean.Gui.OpenGL.Backend
{
	public abstract class Texture :
		Gpu.Backend.ITexture
	{

		protected Texture(Factory factory, Gpu.Backend.TextureType type, Geometry2D.Integer.Size resolution, Draw.CoordinateSystem coordinateSystem) :
			this(factory, type, resolution, coordinateSystem, IntPtr.Zero)
		{ }
		protected Texture(Factory factory, Gpu.Backend.TextureType type, Geometry2D.Integer.Size size, Draw.CoordinateSystem coordinateSystem, IntPtr data)
		{
			this.Factory = factory;
			this.Type = type;
			this.Size = size;
			this.Identifier = this.CreateIdentifier();
			this.Bind();
			this.Load(this.Type, this.Size, data);
			this.SetParameters();
		}

		#region Implementors interface
		public uint Identifier { get; private set; }
		protected virtual Geometry2D.Integer.Size AdaptResolution(Geometry2D.Integer.Size resolution)
		{
			int maximumTextureSize;
			GL.GetInteger(OpenTK.Graphics.OpenGL.GetPName.MaxTextureSize, out maximumTextureSize);
			if (resolution.Width > maximumTextureSize)
			{
				Log.Cache.Log(Error.Level.Warning, "Texture Width Exceeds Limit", string.Format("The requested texture size \"{0}\" is bigger than the maximum texture size \"{1}, {1}\". The textures width will be clamped.", resolution, maximumTextureSize));
				resolution = new Geometry2D.Integer.Size(maximumTextureSize, resolution.Height);
			}
			if (resolution.Height > maximumTextureSize)
			{
				Log.Cache.Log(Error.Level.Warning, "Texture Height Exceeds Limit", string.Format("The requested texture size \"{0}\" is bigger than the maximum texture size \"{1}, {1}\". The textures height will be clamped.", resolution, maximumTextureSize));
				resolution = new Geometry2D.Integer.Size(resolution.Width, maximumTextureSize);
			}
			return resolution;
		}
		protected virtual uint CreateIdentifier()
		{
			uint result;
			GL.GenTextures(1, out result);
			return result;
		}
		protected abstract FrameBuffer CreateCanvas();
		protected virtual void SetActive(int channel)
		{
			switch (channel)
			{
				default:
				case 0: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture0); break;
				case 1: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture1); break;
				case 2: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture2); break;
				case 3: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture3); break;
				case 4: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture4); break;
				case 5: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture5); break;
				case 6: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture6); break;
				case 7: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture7); break;
				case 8: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture8); break;
				case 9: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture9); break;
				case 10: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture10); break;
				case 11: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture11); break;
				case 12: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture12); break;
				case 13: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture13); break;
				case 14: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture14); break;
				case 15: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture15); break;
				case 16: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture16); break;
				case 17: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture17); break;
				case 18: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture18); break;
				case 19: GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture19); break;
			}
		}
        protected virtual void Bind()
		{
			GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, this.Identifier);
		}
		protected virtual void Unbind()
		{
			GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0);
		}
		protected virtual void Load(Gpu.Backend.TextureType type, Geometry2D.Integer.Size size, IntPtr data)
		{
			GL.TexImage2D(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0, type.PixelInternalFormat(), this.Size.Width, this.Size.Height, 0, type.PixelFormat(), OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, data);
		}
		protected virtual void SetParameters()
		{
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.Linear);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, (int)OpenTK.Graphics.OpenGL.TextureMagFilter.Linear);
			if (this.Wrap)
			{
				GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.MirroredRepeat);
				GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.MirroredRepeat);
			}
			else
			{
				GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToBorder);
				GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToBorder);
			}
		}
		protected virtual void Read(IntPtr pointer)
		{
			GL.GetTexImage(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0, this.Type.PixelFormat(), OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, pointer); 
		}
		protected abstract Texture Create();
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
		}
		#endregion

		#region ITexture Members
		public void Use()
		{
			this.Bind();
		}
		public void Use(int channel)
		{
			this.SetActive(channel);
			this.Bind();
		}
		public void Unuse()
		{
			this.Unbind();
		}
		public void Unuse(int channel)
		{
			this.Unbind();
			this.SetActive(0);
		}
		public bool Wrap { get; set; }
		public Gpu.Backend.IFactory Factory { get; private set; }
		Gpu.Backend.IFrameBuffer frameBuffer;
		public Kean.Draw.Gpu.Backend.IFrameBuffer FrameBuffer
		{
			get 
			{
				if (this.frameBuffer.IsNull())
					this.frameBuffer = this.CreateCanvas();
				return this.frameBuffer;
			}
		}
		public Draw.CoordinateSystem CoordinateSystem { get; set; }
		public Geometry2D.Integer.Size Size { get; private set; }
		public Gpu.Backend.TextureType Type { get; private set; }
        // TODO Support more raster formats
        public void Load(Geometry2D.Integer.Point offset, Raster.Image image)
		{
            this.Bind();
            if(image is Raster.Monochrome && this.Type == Kean.Draw.Gpu.Backend.TextureType.Monochrome ||
                image is Raster.Bgr && this.Type == Kean.Draw.Gpu.Backend.TextureType.Bgr ||
                image is Raster.Bgra && this.Type == Kean.Draw.Gpu.Backend.TextureType.Bgra)
            GL.TexSubImage2D(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0, offset.X, offset.Y, image.Size.Width, image.Size.Height, this.Type.PixelFormat(), OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, image.Pointer);
            this.Unbind();
        }
		public Raster.Image Read()
		{
			this.Bind();
			Raster.Image result;
			switch (this.Type)
			{
				case Gpu.Backend.TextureType.Bgra:
					result = new Raster.Bgra(this.Size);
					break;
				case Gpu.Backend.TextureType.Bgr:
					result = new Raster.Bgr(this.Size);
					break;
				case Gpu.Backend.TextureType.Monochrome:
					result = new Raster.Monochrome(this.Size);
					break;
				default:
					result = null;
					break;
			}
			if (result.NotNull())
				this.Read(result.Pointer);
			return result;
		}
		public void Render()
		{
			this.Render(new Geometry2D.Single.Box(0.0f, 0.0f, this.Size.Width, this.Size.Height), new Geometry2D.Single.Box(0.0f, 0.0f, this.Size.Width, this.Size.Height));
		}
		public void Render(Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			this.Bind();
			Geometry2D.Single.PointValue leftTop = new Geometry2D.Single.PointValue(source.Left / this.Size.Width, source.Top / this.Size.Height);
			Geometry2D.Single.PointValue rightTop = new Geometry2D.Single.PointValue(source.Right / this.Size.Width, source.Top / this.Size.Height);
			Geometry2D.Single.PointValue leftBottom = new Geometry2D.Single.PointValue(source.Left / this.Size.Width, source.Bottom / this.Size.Height);
			Geometry2D.Single.PointValue rightBottom = new Geometry2D.Single.PointValue(source.Right / this.Size.Width, source.Bottom / this.Size.Height);
           	GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
			GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.One, OpenTK.Graphics.OpenGL.BlendingFactorDest.OneMinusSrcAlpha);
			GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Quads);
			GL.TexCoord2(leftTop.X, leftTop.Y); GL.Vertex2(destination.Left, destination.Top);
			GL.TexCoord2(rightTop.X, rightTop.Y); GL.Vertex2(destination.Right, destination.Top);
			GL.TexCoord2(rightBottom.X, rightBottom.Y); GL.Vertex2(destination.Right, destination.Bottom);
			GL.TexCoord2(leftBottom.X, leftBottom.Y); GL.Vertex2(destination.Left, destination.Bottom);
			GL.End();
			this.Unbind();
		}
		#endregion
	}
}

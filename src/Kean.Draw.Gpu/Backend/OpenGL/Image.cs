using System;
using Geometry2D = Kean.Math.Geometry2D;
using GL = OpenTK.Graphics.OpenGL.GL;
using Error = Kean.Core.Error;
using Log = Kean.Extra.Log;
using Kean.Draw.Gpu.Backend.OpenGL.Extension;

namespace Kean.Draw.Gpu.Backend.OpenGL
{
	public abstract class Image :
		IImage
	{

		protected Image(ImageType type, Geometry2D.Integer.Size resolution) :
			this(type, resolution, IntPtr.Zero)
		{ }
		protected Image(ImageType type, Geometry2D.Integer.Size resolution, IntPtr data)
		{
			this.Type = type;
			this.Resolution = resolution;
			this.Identifier = this.CreateIdentifier();
			this.Bind();
			this.Load(this.Type, this.Resolution, data);
		}

		#region Implementors interface
		protected int Identifier { get; private set; }
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
		protected virtual int CreateIdentifier()
		{
			return GL.GenTexture();
		}
		protected virtual void Bind()
		{
			GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
			GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, this.Identifier);
		}
		protected virtual void Load(ImageType type, Geometry2D.Integer.Size size, IntPtr data)
		{
			GL.TexImage2D(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0, type.PixelInternalFormat(), this.Resolution.Width, this.Resolution.Height, 0, type.PixelFormat(), OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, data);
		}
		#endregion

		#region IImage Members
		public ImageType Type { get; private set; }
		public Geometry2D.Integer.Size Resolution { get; private set; }

		public void Load(IntPtr data, Geometry2D.Integer.Size resolution, Geometry2D.Integer.Point offset, ImageType type)
		{
		}

		public void Read(IntPtr data)
		{
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
		}
		#endregion
	}
}

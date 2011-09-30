using System;

namespace Kean.Draw.Gpu.Backend.OpenGL.Extension
{
	public static class ImageTypeExtension
	{
		public static OpenTK.Graphics.OpenGL.PixelInternalFormat PixelInternalFormat(this ImageType me)
		{
			OpenTK.Graphics.OpenGL.PixelInternalFormat result;
			switch (me)
			{
				default:
				case ImageType.Argb:
					result = OpenTK.Graphics.OpenGL.PixelInternalFormat.Rgba;
					break;
				case ImageType.Rgb:
					result = OpenTK.Graphics.OpenGL.PixelInternalFormat.Rgb;
					break;
				case ImageType.Monochrome:
					result = OpenTK.Graphics.OpenGL.PixelInternalFormat.Luminance8;
					break;
			}
			return result;
		}
		public static OpenTK.Graphics.OpenGL.PixelFormat PixelFormat(this ImageType me)
		{
			OpenTK.Graphics.OpenGL.PixelFormat result;
			switch (me)
			{
				default:
				case ImageType.Argb:
					result = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
					break;
				case ImageType.Rgb:
					result = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
					break;
				case ImageType.Monochrome:
					result = OpenTK.Graphics.OpenGL.PixelFormat.Luminance;
					break;
			}
			return result;
		}
	}
}

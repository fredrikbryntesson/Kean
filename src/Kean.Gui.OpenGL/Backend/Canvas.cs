// 
//  Canvas.cs
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
	public abstract class Canvas :
		Gpu.Backend.ICanvas
	{
		Image depth;
		public uint Framebuffer { get; private set; }

		protected Canvas(Image image)
		{
			this.Image = image;
			this.depth = this.CreateDepth();
			this.Framebuffer = this.CreateFramebuffer(image, this.depth);
		}

		#region Inheritors Interface
		protected abstract Image CreateDepth();
		protected abstract uint CreateFramebuffer(Image color, Image depth);
		protected abstract void Bind();
		protected abstract void Unbind();
		#endregion

		#region ICanvas Members
		public Gpu.Backend.IImage Image { get; private set; }
		public Gpu.Backend.IFactory Factory { get { return this.Image.Factory; } }
		
		public Raster.Image Read(Geometry2D.Integer.Box region)
		{
			Raster.Image result;
			switch (this.Image.Type)
			{
				case Gpu.Backend.ImageType.Bgra:
					result = new Raster.Bgra(region.Size);
					break;
				default:
					result = null;
					break;
			}
			if (result.NotNull())
				GL.ReadPixels(region.Left, region.Top, region.Width, region.Height, this.Image.Type.PixelFormat(), OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, result.Pointer);
			return result;
		}
		public void Draw(Kean.Draw.IColor color)
		{
			this.Bind();
			//GL.ClearColor();
			GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL.ClearBufferMask.StencilBufferBit);
		}

		#endregion
	}
}

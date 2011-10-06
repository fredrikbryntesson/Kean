// 
//  Window.cs
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
using Geometry2D = Kean.Math.Geometry2D;
using Gpu = Kean.Draw.Gpu;
using GL = OpenTK.Graphics.OpenGL.GL;
using Raster = Kean.Draw.Raster;
using Kean.Gui.OpenGL.Backend.Extension;

namespace Kean.Gui.OpenGL.Backend.OpenGL21
{
	class Window :
		Backend.Window
	{
		class ScreenImage :
			Gpu.Backend.IImage
		{
			class ScreenCanvas :
				OpenGL21.Canvas
			{
				public ScreenCanvas(Gpu.Backend.IImage image) :
					base(image)
				{ }
				protected override Backend.Image CreateDepth()
				{
					return null;
				}
				protected override uint CreateFramebuffer(Kean.Draw.Gpu.Backend.IImage color, Kean.Gui.OpenGL.Backend.Image depth)
				{
					return 0;
				}
				protected override void Bind()
				{
					GL.Ext.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, 0);
					Exception.Framebuffer.Check();
				}
				protected override void Unbind()
				{
				}
			}
			Func<Geometry2D.Integer.Size> getSize;
			public ScreenImage(Func<Geometry2D.Integer.Size> getSize)
			{
				this.getSize = getSize;
				this.Canvas = new ScreenCanvas(this);
			}
			#region IImage Members
			public bool Wrap { get { return false; } set { ; } }
			public Gpu.Backend.IFactory Factory { get; private set; }
			public Gpu.Backend.ICanvas Canvas { get; private set; }
			public Draw.CoordinateSystem CoordinateSystem { get; set; }
			public Geometry2D.Integer.Size Size { get { return this.getSize(); } }
			public Draw.Gpu.Backend.ImageType Type { get { return Gpu.Backend.ImageType.Bgra; } }
			public void Load(Geometry2D.Integer.Point offset, Draw.Raster.Image image)
			{
				// TODO: implement this by using GL.WritePixels.
			}
			public Raster.Image Read()
			{
				// TODO: implement this by using GL.ReadPixels.
				return null;
			}
			public Gpu.Backend.IImage Copy()
			{
				return null;
			}
			public void Render()
			{
			}
			public void Render(Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
			{
			}
			#endregion
			#region IDisposable Members
			public void Dispose()
			{ }
			#endregion
		}
		public Window(Geometry2D.Integer.Size size, string title) :
			base(size, title, OpenTK.GameWindowFlags.Default, OpenTK.Graphics.GraphicsMode.Default, OpenTK.DisplayDevice.Default)
		{ }
		protected override OpenTK.Graphics.GraphicsContext CreateContext(OpenTK.Platform.IWindowInfo windowInformation)
		{
			return new OpenTK.Graphics.GraphicsContext(OpenTK.Graphics.GraphicsMode.Default, windowInformation, 2, 1, OpenTK.Graphics.GraphicsContextFlags.Default);
		}
		public override Gpu.Backend.IImage CreateImage()
		{
			return new ScreenImage(() => new Geometry2D.Integer.Size(this.Width, this.Height));
		}
	}
}

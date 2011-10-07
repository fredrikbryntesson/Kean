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
		public Geometry2D.Single.Box Clip { get; set; }
		public Geometry2D.Single.Transform Transform { get; set; }
		protected Canvas(Gpu.Backend.IImage image)
		{
			this.Image = image;
			this.depth = this.CreateDepth();
			this.Framebuffer = this.CreateFramebuffer(image, this.depth);
		}

		#region Inheritors Interface
		protected abstract Image CreateDepth();
		protected abstract uint CreateFramebuffer(Gpu.Backend.IImage color, Image depth);
		protected abstract void Bind();
		protected virtual void SetupViewport()
		{
			GL.PushAttrib(OpenTK.Graphics.OpenGL.AttribMask.AllAttribBits);
			GL.Viewport(0, 0, this.Image.Size.Width, this.Image.Size.Height);
			GL.Ortho(0.0, 0.0, 1.0, 1.0, 0.0, 0.0);
			GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
			GL.PushMatrix();
			(new Geometry2D.Single.Transform(2.0f / this.Image.Size.Width, 0.0f, 0.0f, 2.0f / this.Image.Size.Height, -1.0f, -1.0f)).Load();
			GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
			GL.PushMatrix();
			GL.LoadIdentity();
			GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
			GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.SrcAlpha, OpenTK.Graphics.OpenGL.BlendingFactorDest.OneMinusSrcAlpha);
		}
		protected virtual void TeardownViewport()
		{
			GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
			GL.PopMatrix();
			GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
			GL.PopMatrix();
			GL.PopAttrib();
		}
		protected abstract void Unbind();
		protected virtual void SetupClippingPlanes()
		{
			if (this.Clip.NotNull())
			{
				double[] left = new double[] { 1.0, 0.0, 0.0, -this.Clip.Left };
				GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane0, ref left[0]);
				GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane0);
				double[] right = new double[] { -1.0, 0.0, 0.0, this.Clip.Right };
				GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane1, ref right[0]);
				GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane1);
				double[] top = new double[] { 0.0, 1.0, 0.0, -this.Clip.Top };
				GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane2, ref top[0]);
				GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane2);
				double[] bottom = new double[] { 0.0, -1.0, 0.0, this.Clip.Bottom };
				GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane3, ref bottom[0]);
				GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane3);
			}
		}
		protected virtual void TearDownClippingPlanes()
		{
			if (this.Clip.NotNull())
			{
				GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane0);
				GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane1);
				GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane2);
				GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane3);
			}
		}
		protected virtual void SetupTransform()
		{
			if (this.Transform.NotNull())
			{

				Geometry2D.Single.Transform translation = Geometry2D.Single.Transform.CreateTranslation((Geometry2D.Single.Size)(this.Image.Size) / 2.0f);
				(translation * this.Transform * translation.Inverse).Load();
			}
		}
		protected virtual void TearDownTransform()
		{
			if (this.Transform.NotNull())
				Geometry2D.Single.Transform.Identity.Load();
		}
		#endregion
		void Setup()
		{
			this.Bind();
			this.SetupViewport();
			this.SetupClippingPlanes();
			this.SetupTransform();
		}
		void Teardown()
		{
			this.Unbind();
			this.TearDownClippingPlanes();
			this.TearDownTransform();
			this.TeardownViewport();
		}
		#region ICanvas Members
		public Gpu.Backend.IImage Image { get; private set; }
		public Gpu.Backend.IFactory Factory { get { return this.Image.Factory; } }
		
		public Raster.Image Read(Geometry2D.Integer.Box region)
		{
			Raster.Image result;
			Kean.Draw.Gpu.Backend.ImageType type = this.Image.Type;
			switch (type)
			{
				case Gpu.Backend.ImageType.Bgra:
					result = new Raster.Bgra(region.Size);
					break;
				case Gpu.Backend.ImageType.Bgr:
					result = new Raster.Bgr(region.Size);
					break;
				case Gpu.Backend.ImageType.Monochrome:
					result = new Raster.Monochrome(region.Size);
					break;
				default:
					result = null;
					break;
			}
			if (result.NotNull())
			{
				this.Setup();
				GL.ReadPixels(region.Left, region.Top, region.Width, region.Height, type.PixelFormat(), OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, result.Pointer);
				this.Teardown();
			}
			return result;
		}
		public void Draw(Kean.Draw.IColor color)
		{
			this.Setup();
			Draw.Color.Bgra bgra = color.Convert<Draw.Color.Bgra>();
			GL.ClearColor(new OpenTK.Graphics.Color4(bgra.color.red, bgra.color.green, bgra.color.blue, bgra.alpha));
			GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL.ClearBufferMask.StencilBufferBit);
			this.Teardown();
		}
        public void Draw(Draw.IColor color, Geometry2D.Single.Box region)
        {
			this.Setup();
			GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
			Draw.Color.Bgra bgra = color.Convert<Draw.Color.Bgra>();
			GL.Color4(new OpenTK.Graphics.Color4(bgra.color.red, bgra.color.green, bgra.color.blue, bgra.alpha));
			GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.One, OpenTK.Graphics.OpenGL.BlendingFactorDest.Zero);
            GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            GL.Vertex2(region.Left, region.Top);
            GL.Vertex2(region.Right, region.Top);
            GL.Vertex2(region.Right, region.Bottom);
            GL.Vertex2(region.Left, region.Bottom);
            GL.End();
			GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D); 
			GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.SrcAlpha, OpenTK.Graphics.OpenGL.BlendingFactorDest.OneMinusSrcAlpha);
			this.Teardown();
		}
        public void Draw(Gpu.Backend.IImage image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
        {
			this.Setup();
			image.Render(source, destination);
			this.Teardown();
		}
		
		public void Blend(float factor)
		{
			this.Setup();
			GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
			GL.Color4(new OpenTK.Graphics.Color4(factor, factor, factor, factor));
			GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.One, OpenTK.Graphics.OpenGL.BlendingFactorDest.One);
			GL.BlendEquation(OpenTK.Graphics.OpenGL.BlendEquationMode.FuncReverseSubtract);
			GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Quads);
			GL.Vertex2(0, 0);
			GL.Vertex2(this.Image.Size.Width, 0);
			GL.Vertex2(this.Image.Size.Width, this.Image.Size.Height);
			GL.Vertex2(0, this.Image.Size.Height);
			GL.End();
			GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
			GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.SrcAlpha, OpenTK.Graphics.OpenGL.BlendingFactorDest.OneMinusSrcAlpha);
			GL.BlendEquation(OpenTK.Graphics.OpenGL.BlendEquationMode.FuncAdd);
			this.Teardown();
		}
		#endregion
	}
}

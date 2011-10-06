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

namespace Kean.Gui.OpenGL.Backend.OpenGL21
{
	public class Canvas :
		Backend.Canvas
	{
		internal Canvas(Image image) :
			base(image)
		{
		}
		protected override Backend.Image CreateDepth()
		{
			return new Depth(this.Factory as OpenGL21.Factory, this.Image.Size);
		}
		protected override uint CreateFramebuffer(Backend.Image color, Backend.Image depth)
		{
			uint result;
			GL.GenFramebuffers(1, out result);
			GL.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, result);
			GL.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.ColorAttachment0Ext, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, color.Identifier, 0);
			GL.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.DepthAttachmentExt, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, depth.Identifier, 0);
			Exception.Framebuffer.Check();
			GL.Ext.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, 0);
			return result;
		}
		protected override void Bind()
		{
            GL.PushAttrib(OpenTK.Graphics.OpenGL.AttribMask.AllAttribBits);
            GL.Ext.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, this.Framebuffer);
            Exception.Framebuffer.Check();
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
		protected override void Unbind()
		{
            GL.Ext.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, 0);
            Exception.Framebuffer.Check();
            GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
            GL.PopMatrix();
            GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
            GL.PopMatrix();
            GL.PopAttrib();
		}
	}
}

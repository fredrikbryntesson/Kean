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
using Log = Kean.Platform.Log;
using Draw = Kean.Draw;
using Gpu = Kean.Draw.Gpu;
using Raster = Kean.Draw.Raster;
using Kean.Gui.OpenGL.Backend.Extension;
using Kean.Core.Collection.Extension;

namespace Kean.Gui.OpenGL.Backend.OpenGL21
{
	public class FrameBuffer :
		Backend.FrameBuffer
	{
		internal FrameBuffer(params Gpu.Backend.ITexture[] textures) :
			base(textures)
		{
		}
		protected override Backend.Texture CreateDepth()
		{
			return new Depth(this.Factory as OpenGL21.Factory, this.Size);
		}
		protected override uint CreateFrameBuffer(Gpu.Backend.ITexture[] color, Backend.Texture depth)
		{
			uint result;
			GL.GenFramebuffers(1, out result);
			GL.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, result);
            this.BindTextures(color);
            GL.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.DepthAttachmentExt, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, (depth as Backend.Texture).Identifier, 0);
			Exception.Framebuffer.Check();
			GL.Ext.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, 0);
			return result;
		}
		protected override void DeleteFrameBuffer(uint identifier)
		{
			GL.Ext.DeleteFramebuffers(1, ref identifier);
		}
        protected override void BindTextures(Gpu.Backend.ITexture[] color)
        {
            GL.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.ColorAttachment0Ext, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, (color[0] as Backend.Texture).Identifier, 0);
            if (color.Length > 1)
            {
                GL.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.ColorAttachment1Ext, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, (color[1] as Backend.Texture).Identifier, 0);
                if (color.Length > 2)
                {
                    GL.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.ColorAttachment2Ext, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, (color[1] as Backend.Texture).Identifier, 0);
                    if (color.Length > 3)
                    {
                        GL.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.ColorAttachment3Ext, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, (color[1] as Backend.Texture).Identifier, 0);
                        if (color.Length > 4)
                            GL.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.ColorAttachment4Ext, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, (color[1] as Backend.Texture).Identifier, 0);
                    }
                }
            }
        }
		protected override void Bind()
		{
			GL.Ext.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, this.Identifier);
            this.BindTextures(this.Textures.ToArray());
            Exception.Framebuffer.Check();
		}
		protected override void Unbind()
		{
			GL.Ext.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, 0);
			Exception.Framebuffer.Check();
		}
	}
}

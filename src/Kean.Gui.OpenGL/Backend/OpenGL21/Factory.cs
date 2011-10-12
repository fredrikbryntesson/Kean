// 
//  Factory.cs
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
using GL = OpenTK.Graphics.OpenGL.GL;
using Error = Kean.Core.Error;
using Log = Kean.Extra.Log;
using Draw = Kean.Draw;
using Gpu = Kean.Draw.Gpu;
using Raster = Kean.Draw.Raster;
using Kean.Gui.OpenGL.Backend.Extension;

namespace Kean.Gui.OpenGL.Backend.OpenGL21
{
	public class Factory :
		OpenGL.Backend.Factory
	{
		#region Constructors
		public Factory()
		{ }
		#endregion
		#region Inheritors Interface
		protected override Shader.Vertex DefaultVertex { get { return new Shader.Vertex(@"void main() { gl_Position = ftransform(); gl_TexCoord[0] = gl_MultiTexCoord0; }"); } }
        protected override Shader.Fragment MonochromeToBgrFragment
        {
            get
            {
                return new Shader.Fragment(@"uniform sampler2D monochrome0; void main() { vec4 value = texture2D(monochrome0, gl_TexCoord[0].xy); float y = value.x; gl_FragColor = vec4(y, y, y, 1.0); }");
            }
        }
        protected override Shader.Fragment BgrToMonochromeFragment 
		{
			get 
			{
				return new Shader.Fragment(@"uniform sampler2D bgr0; void main() { vec4 value = texture2D(bgr0, gl_TexCoord[0].xy); float y = value.x * 0.299 + value.y * 0.587 + value.z * 0.114; gl_FragColor = vec4(y, y, y, 1.0); }");
			} 
		}
		protected override Shader.Fragment BgrToUFragment
		{
			get
			{
				return new Shader.Fragment(@"uniform sampler2D bgr0; void main() { vec4 value = texture2D(bgr0, gl_TexCoord[0].xy); float u = value.x * (-0.168736) + value.y * (-0.331264) + value.z * 0.5000 + 0.5; gl_FragColor = vec4(u, u, u, 1.0); }");
			}
		}
		protected override Shader.Fragment BgrToVFragment
		{
			get
			{
				return new Shader.Fragment(@"uniform sampler2D bgr0; void main() { vec4 value = texture2D(bgr0, gl_TexCoord[0].xy); float v = value.x * 0.5 + value.y * (-0.418688) + value.z * (-0.081312) + 0.5; gl_FragColor = vec4(v, v, v, 1.0); }");
			}
		}
		protected override Shader.Fragment BgrToYuv420Fragment
		{
			get
			{
				return new Shader.Fragment(@"
								uniform sampler2D bgr0;
                                vec4 YuvToRgba(vec4 t);
                                void main()
                                {
                                    vec2 center = gl_TexCoord[0].xy;
                                    gl_FragData[0] = vec4(0.5, 0.0, 0.0, 1.0);
                                    gl_FragData[1] = vec4(0.4, 0.0, 0.0, 1.0);
                                    gl_FragData[2] = vec4(0.3, 0.0, 0.0, 1.0);
                                }
                                // Convert yuva to rgba
                                vec4 YuvToRgba(vec4 t)
                                {	
	                                 mat4 matrix = mat4(1,	                1,                  1,   	                    0,
						                                -0.000001218894189, -0.344135678165337,  1.772000066073816,         0,
						                                1.401999588657340, -0.714136155581812,   0.000000406298063,         0, 
								                        0,	                0,                  0,                          1);   
                                		
	                                return matrix * t;
                                }");
			}
		}
		protected override Shader.Fragment Yuv420ToBgrFragment
		{
			get 
			{
				return new Shader.Fragment(@"
								uniform sampler2D monochrome0Y0;
                                uniform sampler2D monochrome0U1;
                                uniform sampler2D monochrome0V2;
                                vec4 YuvToRgba(vec4 t);
                                void main()
                                {
                                    vec2 center = gl_TexCoord[0].xy;
                                    gl_FragColor = YuvToRgba(vec4(texture2D(monochrome0Y0, center).x, texture2D(monochrome0U1, center).x-0.5, texture2D(monochrome0V2, center).x-0.5, 1.0));
                                }
                                // Convert yuva to rgba
                                vec4 YuvToRgba(vec4 t)
                                {	
	                                 mat4 matrix = mat4(1,	                1,                  1,   	                    0,
						                                -0.000001218894189, -0.344135678165337,  1.772000066073816,         0,
						                                1.401999588657340, -0.714136155581812,   0.000000406298063,         0, 
								                        0,	                0,                  0,                          1);   
                                		
	                                return matrix * t;
                                }"); 
			} 
		}
		#endregion
		#region IFactory Members
		public override Gpu.Backend.ITexture CreateImage(Gpu.Backend.TextureType type, Geometry2D.Integer.Size size, Draw.CoordinateSystem coordinateSystem)
		{
			return new Texture(this, type, size, coordinateSystem);
		}
		public override Gpu.Backend.ITexture CreateImage(Raster.Image image)
		{
			return new Texture(this, image);
		}
		public override Gpu.Backend.IFrameBuffer CreateFrameBuffer(params Kean.Draw.Gpu.Backend.ITexture[] textures)
		{
			return new FrameBuffer(textures);
		}
		#endregion
	}
}

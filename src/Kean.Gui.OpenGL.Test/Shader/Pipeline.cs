// 
//  Pipeline.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
using Target = Kean.Gui.OpenGL;
using Raster = Kean.Draw.Raster;
using Geometry2D = Kean.Math.Geometry2D;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Gui.OpenGL.Test.Shader
{
	public class Pipeline :
		Abstract<Pipeline>
	{
		protected override void Run()
		{
			this.Run(
                this.BgraToYuv,
                this.ToMonochrome,
                this.YuvToBgra
				//this.PaintAllRed,
				//this.SetVariables,
				//this.Multitexture
				);
		}
		[Test]
		public void ToMonochrome()
		{
			Target.Backend.OpenGL21.Factory factory = new Target.Backend.OpenGL21.Factory();
			Target.Backend.Image source = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Bgra, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
			source.Load(new Geometry2D.Integer.Point(0, 0), Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>());
			Target.Backend.Image destination = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Monochrome, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
			Kean.Draw.Gpu.Backend.ICanvas destinationCanvas = destination.Canvas;
            Backend.BgraToY map = new Backend.BgraToY();
            map.Apply(source, destinationCanvas as Backend.Canvas);
            Kean.Draw.Raster.Image result = destinationCanvas.Image.Read();
            result.Save("test.png");
            //Expect(result, Is.EqualTo(Raster.Bgra.OpenResource("Correct.PaintAllRed.png")));
		}
        [Test]
        public void YuvToBgra()
        {
            Target.Backend.OpenGL21.Factory factory = new Target.Backend.OpenGL21.Factory();
            Target.Backend.Image y = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Monochrome, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
            Target.Backend.Image u = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Monochrome, new Geometry2D.Integer.Size(639, 426) / 2, Kean.Draw.CoordinateSystem.Default);
            Target.Backend.Image v = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Monochrome, new Geometry2D.Integer.Size(639, 426) / 2, Kean.Draw.CoordinateSystem.Default);
            Raster.Yuv420 yuv = Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Yuv420>();
            y.Load(new Geometry2D.Integer.Point(0, 0), yuv.Y);
            u.Load(new Geometry2D.Integer.Point(0, 0), yuv.U);
            v.Load(new Geometry2D.Integer.Point(0, 0), yuv.V);
            Target.Backend.Image destination = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Bgra, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
            Kean.Draw.Gpu.Backend.ICanvas destinationCanvas = destination.Canvas;
            Backend.YuvToBgra map = new Backend.YuvToBgra();
            map.Apply(y,u,v, destinationCanvas as Backend.Canvas);
            Kean.Draw.Raster.Image result = destinationCanvas.Image.Read();
            result.Save("test.png");
            //Expect(result, Is.EqualTo(Raster.Bgra.OpenResource("Correct.PaintAllRed.png")));
        }
        [Test]
        public void BgraToYuv()
        {
            Target.Backend.OpenGL21.Factory factory = new Target.Backend.OpenGL21.Factory();
            Target.Backend.Image source = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Bgra, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
            source.Load(new Geometry2D.Integer.Point(0, 0), Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>());
            Target.Backend.Image destination1 = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Monochrome, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
            Target.Backend.Image destination2 = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Monochrome, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
            Target.Backend.Image destination3 = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Monochrome, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
            
            Backend.BgraToYuv map = new Backend.BgraToYuv();
            map.Apply(source, destination1, destination2, destination3);
            destination1.Canvas.Image.Read().Save("y.png");
            destination2.Canvas.Image.Read().Save("u.png");
            destination3.Canvas.Image.Read().Save("v.png");
            //Expect(result, Is.EqualTo(Raster.Bgra.OpenResource("Correct.PaintAllRed.png")));
        }
        
		[Test]
		public void SetVariables()
		{
			Target.Backend.Shader.Program program = new Target.Backend.Shader.Program();
			program.Shaders.Add
			   (
				   new Target.Backend.Shader.Fragment
						(
							@"
                            uniform float a; 
                            uniform vec2 b; 
                            uniform vec3 c; 
                            uniform vec4 d; 
                            uniform sampler2D tex0; 
                            void main()
                            {
                                gl_FragColor = d + vec4(b, b) + vec4(a, c);
                            }"
						)
			   );
			program.Shaders.Add
				(
				new Target.Backend.Shader.Vertex
					(
						@"
                            void main()
                                {
                                    gl_Position = ftransform(); 
                                    gl_TexCoord[0] = gl_MultiTexCoord0;
                                }
                         "
					)
				);
			Target.Backend.OpenGL21.Factory factory = new Target.Backend.OpenGL21.Factory();
			Target.Backend.Image source = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Bgra, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
			source.Load(new Geometry2D.Integer.Point(0, 0), Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>());
			Target.Backend.Image destination = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Bgra, new Geometry2D.Integer.Size(320, 256), Kean.Draw.CoordinateSystem.Default);
			Kean.Draw.Gpu.Backend.ICanvas destinationCanvas = destination.Canvas;
			program.SetVariable("a", 0.5f);
			program.SetVariable("b", -0.5f, 0.0f);
			program.SetVariable("c", -1.0f, -0.5f, 0.0f);
			program.SetVariable("d", 1.0f, 1.0f, 1.0f, 1.0f);
			program.Use();
			destinationCanvas.Draw(source);
			program.UnUse();
			Raster.Image result = destinationCanvas.Image.Read();
			Expect(result, Is.EqualTo(Raster.Bgra.OpenResource("Correct.SetVariables.png")));
		}
        /*
		[Test]
		public void Multitexture()
		{
			Target.Backend.Shader.Program program = new Target.Backend.Shader.Program();
			program.Shaders.Add
			   (
				   new Target.Backend.Shader.Fragment
						(
							@"
                                uniform sampler2D textureA;
                                uniform sampler2D textureB;
                                void main()
                                {
                                    vec2 center = gl_TexCoord[0].xy;
                                    gl_FragColor = (texture2D(textureA, center) + texture2D(textureB, center)) / 2.0;
                                }
                                "
						)
			   );
			program.Shaders.Add
				(
				new Target.Backend.Shader.Vertex
					(
						@"
                            void main()
                                {
                                    gl_Position = ftransform(); 
                                    gl_TexCoord[0] = gl_MultiTexCoord0;
                                }
                         "
					)
				);
			Target.Backend.OpenGL21.Factory factory = new Target.Backend.OpenGL21.Factory();
			Target.Backend.Image sourceA = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Bgra, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
			sourceA.Load(new Geometry2D.Integer.Point(0, 0), Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>());
			Target.Backend.Image sourceB = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Bgra, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
			sourceB.Load(new Geometry2D.Integer.Point(0, 0), Raster.Image.OpenResource("Input.ElephantSeal.jpg").Convert<Raster.Bgra>());
			Target.Backend.Image destination = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Bgra, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
			Kean.Draw.Gpu.Backend.ICanvas destinationCanvas = destination.Canvas;
			
			OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture1);
			sourceB.Bind();
			OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture0);
			sourceA.Bind();
			
			program.SetVariable("textureA", 0); // Tell the fragment shader the unit numbers. 
			program.SetVariable("textureB", 1);
			
			program.Use();
			destinationCanvas.Draw(sourceA);
			program.UnUse();
			
			OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture1);
			sourceB.Unbind();
			OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture0);
			sourceA.Unbind();
			
			Raster.Image result = destinationCanvas.Image.Read();
			result.Save("Multitexture.png");

			//Expect(result, Is.EqualTo(Raster.Bgra.OpenResource("Correct.SetVariables.png")));
		}
		[Test]
		public void ShaderTest()
		{
			Target.Backend.Shader.Program program = new Target.Backend.Shader.Program();
			program.Shaders.Add
			   (
				   new Target.Backend.Shader.Fragment
					   (
							@"
                                uniform sampler2D tex0; 
                                void main()
                                    {
                                        gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);
                                    }
                             "
						)
			   );
			program.Shaders.Add
				(
				new Target.Backend.Shader.Vertex
					(
						@"
                            void main()
                                {
                                    gl_Position = ftransform(); 
                                    gl_TexCoord[0] = gl_MultiTexCoord0;
                                }
                         "
					)
				);
			Target.Backend.OpenGL21.Factory factory = new Target.Backend.OpenGL21.Factory();
			Target.Backend.Image source = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Bgra, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
			source.Load(new Geometry2D.Integer.Point(0, 0), Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>());
			Target.Backend.Image destination = new Target.Backend.OpenGL21.Image(factory, Kean.Draw.Gpu.Backend.ImageType.Bgra, new Geometry2D.Integer.Size(639, 426), Kean.Draw.CoordinateSystem.Default);
			Kean.Draw.Gpu.Backend.ICanvas destinationCanvas = destination.Canvas;
			Kean.Draw.Map map = new Kean.Gui.OpenGL.Map<Kean.Draw.Gpu.Image, Kean.Draw.Gpu.Image>();
			//destinationCanvas.Draw(map, source);
			Kean.Draw.Raster.Image result = destinationCanvas.Image.Read();
			//Expect(result, Is.EqualTo(Raster.Bgra.OpenResource("Correct.PaintAllRed.png")));
		}*/
	}
}

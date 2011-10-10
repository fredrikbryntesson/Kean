using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.Gui.OpenGL.Backend
{
    public class YuvToBgra :
       Map<Image, Image, Image, Canvas>
    {
        public YuvToBgra()
        {
            this.Program = new Shader.Program();
            this.Program.Shaders.Add
               (
                   new Shader.Fragment
                       (
                            @"
                                uniform sampler2D textureY; 
                                uniform sampler2D textureU; 
                                uniform sampler2D textureV; 
                                vec4 YuvToRgba(vec4 t);
                                void main()
                                    {
                                        vec2 center = gl_TexCoord[0].xy;
                                        gl_FragColor = YuvToRgba(vec4(texture2D(textureY, center).x, texture2D(textureU, center).x-0.5, texture2D(textureV, center).x-0.5, 1.0));
                                    }
                                // Convert yuva to rgba
                                vec4 YuvToRgba(vec4 t)
                                {	
	                                 mat4 matrix = mat4(1,	                1,                  1,   	                    0,
						                                -0.000001218894189, -0.344135678165337,  1.772000066073816,         0,
						                                1.401999588657340, -0.714136155581812,   0.000000406298063,         0, 
								                        0,	                0,                  0,                          1);   
                                		
	                                return matrix * t;
                                }
                                
                             "
                        )
               );
            this.Program.Shaders.Add
                (
                new Shader.Vertex
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
        }
        public override void Apply(Image input1, Image input2, Image input3, Canvas result)
        {
            OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture2);
            input3.Bind();
            OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture1);
            input2.Bind();
            OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture0);
            input1.Bind();
            result.Setup();
            this.Program.SetVariable("textureY", 0); // Tell the fragment shader the unit numbers. 
            this.Program.SetVariable("textureU", 1);
            this.Program.SetVariable("textureV", 2);
            this.Program.Use();
            result.Draw(input1);
            this.Program.UnUse();
            OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture2);
            input3.Unbind();
            OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture1);
            input2.Unbind();
            OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture0);
            input1.Unbind();
            result.Teardown();
        }
    }
}

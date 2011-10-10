using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.Gui.OpenGL.Backend
{
    public class BgraToYuv :
       Map2<Image, Image, Image, Image>
    {
        public BgraToYuv()
        {
            this.Program = new Shader.Program();
            this.Program.Shaders.Add
               (
                   new Shader.Fragment
                       (
                            @"
                                #version 110
                                uniform sampler2D tex0; 
                                void main()
                                    {
                                        vec4 input = texture2D(tex0, gl_TexCoord[0].xy);
                                        float y = input.x * 0.299 + input.y * 0.587 + input.z * 0.114;
                                        float u = input.x * (-0.168736) + input.y * (-0.331264) + input.z * 0.5;
                                        float v = input.x * 0.5 + input.y * (-0.418688) + input.z * (-0.081312);
                                        gl_FragData[0] = vec4(y, y, y, 1.0);
                                        gl_FragData[1] = vec4(u + 0.5, u + 0.5, u + 0.5, 1.0);
                                        gl_FragData[2] = vec4(v + 0.5, v + 0.5, v + 0.5, 1.0);
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
        public override void Apply(Image input, Image result1, Image result2, Image result3)
        {
            OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture0);
            input.Bind();
            this.Program.SetVariable("tex0", 0); // Tell the fragment shader the unit numbers. 
            
            Backend.Canvas canvas = result1.Canvas as Backend.Canvas;
            canvas.Setup();
            OpenTK.Graphics.OpenGL.GL.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.ColorAttachment0, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, result1.Identifier, 0);
            OpenTK.Graphics.OpenGL.GL.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.ColorAttachment1, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, result2.Identifier, 0);
            OpenTK.Graphics.OpenGL.GL.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.ColorAttachment2, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, result3.Identifier, 0);
            OpenTK.Graphics.OpenGL.DrawBuffersEnum[] buffers = new OpenTK.Graphics.OpenGL.DrawBuffersEnum[] { OpenTK.Graphics.OpenGL.DrawBuffersEnum.ColorAttachment0, OpenTK.Graphics.OpenGL.DrawBuffersEnum.ColorAttachment1, OpenTK.Graphics.OpenGL.DrawBuffersEnum.ColorAttachment2 };
            OpenTK.Graphics.OpenGL.GL.DrawBuffers(3, buffers);
            this.Program.Use();
            canvas.Draw(input);
            this.Program.UnUse();
            OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture0);
            input.Unbind();
            canvas.Teardown();
        }
    }
}

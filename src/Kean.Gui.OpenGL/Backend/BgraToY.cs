using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.Gui.OpenGL.Backend
{
    public class BgraToY :
        Map<Image, Canvas>
    {
        public BgraToY()
        {
            this.Program = new Shader.Program();
            this.Program.Shaders.Add
               (
                   new Shader.Fragment
                       (
                            @"
                                uniform sampler2D tex0; 
                                void main()
                                    {
                                        vec4 input = texture2D(tex0, gl_TexCoord[0].xy);
                                        float y = input.x * 0.299 + input.y * 0.587 + input.z * 0.114;
                                        gl_FragColor = vec4(y, y, y, 1.0);
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
        public override void Apply(Image input, Canvas result)
        {
            input.Bind();
            result.Setup();
            this.Program.Use();
            result.Draw(input);
            this.Program.UnUse();
            input.Unbind();
            result.Teardown();
        }
    }
}

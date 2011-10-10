using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.Gui.OpenGL.Backend
{
    public abstract class Map
    {
        protected Shader.Program Program { get; set; }
    }
    public abstract class Map<T, Result> :
        Map
        where T : Image
        where Result : Canvas
    {
        public abstract void Apply(T input, Result result);
    }
    public abstract class Map<T1, T2, Result> :
        Map
        where T1 : Image
        where T2 : Image
        where Result : Canvas
    {
        public abstract void Apply(T1 input1, T2 input2, Result result);
    }
    public abstract class Map<T1, T2, T3, Result> :
        Map
        where T1 : Image
        where T2 : Image
        where T3 : Image
        where Result : Canvas
    {
        public abstract void Apply(T1 input1, T2 input2, T3 input3, Result result);
    }
    public abstract class Map2<T, R1, R2, R3> :
        Map
        where T : Image
        where R1 : Image
        where R2 : Image
        where R3 : Image
    {
        public abstract void Apply(T input, R1 result1, R2 result2, R3 result3);
    }

}

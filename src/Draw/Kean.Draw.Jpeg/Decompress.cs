// 
//  Decompress.cs
//  
//  Author:
//      Anders Frisk <andersfrisk77@gmail.com>
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
using Geometry2D = Kean.Math.Geometry2D;
using Raster = Kean.Draw.Raster;

namespace Kean.Draw.Jpeg
{
    public static class Decompress
    {
        public static Raster.Image OpenResource(System.Reflection.Assembly assembly, string name)
        {
            name = assembly.GetName().Name + "." + name.Replace('\\', '.').Replace('/', '.');
            Raster.Image result;
            using (System.IO.Stream stream = assembly.GetManifestResourceStream(name))
                result = Decompress.Open(stream);
            return result;
        }
        public static Raster.Image OpenResource(string name)
        {
            string[] splitted = name.Split(new char[] { ':' }, 2);
            Raster.Image result;
            if (splitted.Length > 1)
                result = Decompress.OpenResource(System.Reflection.Assembly.LoadWithPartialName(splitted[0]), splitted[1]);
            else
                result = Decompress.OpenResource(System.Reflection.Assembly.GetCallingAssembly(), name);
            return result;
        }
        public static Raster.Image Open(System.IO.Stream stream)
        {
            Raster.Image result;
            try
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);    
                result = Decompress.Open(buffer);
            }
            catch (ArgumentException)
            {
                result = null;
            }
            return result;
        }
        public static Raster.Image Open(byte[] buffer)
        {
            Raster.Image result = null;
            IntPtr handler = Api.InitDecompressor();
            int width, height;
            Sampler sampler;
            Api.DecompressHeader(handler, buffer, (uint)buffer.Length, out width, out height, out sampler);
            switch (sampler)
            {
                case Sampler.Yuv420:
                    result = new Raster.Yuv420(new Geometry2D.Integer.Size(width, height));
                    break;
                case Sampler.Yuv422:
                    result = new Raster.Yuv422(new Geometry2D.Integer.Size(width, height));
                    break;
                case Sampler.Gray:
                    result = new Raster.Monochrome(new Geometry2D.Integer.Size(width, height));
                    break;
            }
            Api.DecompressToYuv(handler, buffer, (uint)buffer.Length, result.Pointer, 0);
            Api.Destroy(handler);
            return result;
        }
    }
}

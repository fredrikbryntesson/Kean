// 
//  Api.cs
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
using InteropServices = System.Runtime.InteropServices;

namespace Kean.Draw.Jpeg
{
    internal static class Api
    {
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjInitDecompress")]
        public static extern IntPtr InitDecompressor();
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDestroy")]
        public static extern int Destroy(IntPtr handle);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjGetErrorStr")]
        public static extern string GetError();
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDecompressHeader2")]
        public static extern int DecompressHeader(IntPtr handle, IntPtr jpegBuffer, uint jpegBufferSize, out int width, out int height, out Sampler jpegSubsampler);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDecompressToYUV")]
        public static extern int DecompressToYuv(IntPtr handle, IntPtr jpegBuffer, uint jpegBufferSize, IntPtr destinationBuffer, int flags);
    }
}

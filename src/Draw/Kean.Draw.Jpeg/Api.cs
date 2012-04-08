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
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjInitCompress")]
        public static extern IntPtr InitCompressor();
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjInitDecompress")]
        public static extern IntPtr InitDecompressor();
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjInitTransform")]
        public static extern IntPtr InitTransform();
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDestroy")]
        public static extern int Destroy(IntPtr handle);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjGetErrorStr")]
        public static extern string GetError();
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDecompressHeader2")]
        public static extern int DecompressHeader(IntPtr handle, byte[] jpegBuffer, uint jpegBufferSize, out int width, out int height, out Sampler jpegSubsampler);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDecompressHeader2")]
        public static extern int DecompressHeader(IntPtr handle, IntPtr jpegBuffer, uint jpegBufferSize, out int width, out int height, out Sampler jpegSubsampler);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDecompress2")]
        public static extern int Decompress(IntPtr handle, byte[] jpegBuffer, uint jpegBufferSize, byte[] destinationBuffer, int width, int pitch, int height, int pixelFormat, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDecompress2")]
        public static extern int Decompress(IntPtr handle, IntPtr jpegBuffer, uint jpegBufferSize, byte[] destinationBuffer, int width, int pitch, int height, int pixelFormat, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDecompress2")]
        public static extern int Decompress(IntPtr handle, byte[] jpegBuffer, uint jpegBufferSize, IntPtr destinationBuffer, int width, int pitch, int height, int pixelFormat, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDecompress2")]
        public static extern int Decompress(IntPtr handle, IntPtr jpegBuffer, uint jpegBufferSize, IntPtr destinationBuffer, int width, int pitch, int height, int pixelFormat, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDecompressToYUV")]
        public static extern int DecompressToYuv(IntPtr handle, byte[] jpegBuffer, uint jpegBufferSize, byte[] destinationBuffer, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDecompressToYUV")]
        public static extern int DecompressToYuv(IntPtr handle, byte[] jpegBuffer, uint jpegBufferSize, IntPtr destinationBuffer, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDecompressToYUV")]
        public static extern int DecompressToYuv(IntPtr handle, IntPtr jpegBuffer, uint jpegBufferSize, byte[] destinationBuffer, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjDecompressToYUV")]
        public static extern int DecompressToYuv(IntPtr handle, IntPtr jpegBuffer, uint jpegBufferSize, IntPtr destinationBuffer, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjBufSizeYUV")]
        public static extern uint BufferSizeYuv(int width, int height, Sampler jpegSubsampler);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjBufSize")]
        public static extern uint BufferSize(int width, int height, Sampler jpegSubsampler);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjCompress2")]
        public static extern int Compress(IntPtr handle, byte[] sourceBuffer, int width, int pitch, int height, int pixelFormat, ref IntPtr jpegBuffer, out uint jpegBufferSize, Sampler jpegSubsampler, int jpegQual, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjCompress2")]
        public static extern int Compress(IntPtr handle, IntPtr sourceBuffer, int width, int pitch, int height, int pixelFormat, ref IntPtr jpegBuffer, out uint jpegBufferSize, Sampler jpegSubsampler, int jpegQual, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjEncodeYUV2")]
        public static extern int EncodeYuv(IntPtr handle, byte[] sourceBuffer, int width, int pitch, int height, int pixelFormat, byte[] destinationBuffer, Sampler subsampler, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjEncodeYUV2")]
        public static extern int EncodeYuv(IntPtr handle, IntPtr sourceBuffer, int width, int pitch, int height, int pixelFormat, byte[] destinationBuffer, Sampler subsampler, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjEncodeYUV2")]
        public static extern int EncodeYuv(IntPtr handle, byte[] sourceBuffer, int width, int pitch, int height, int pixelFormat, IntPtr destinationBuffer, Sampler subsampler, int flags);
        [InteropServices.DllImport("turbojpeg", EntryPoint = "tjEncodeYUV2")]
        public static extern int EncodeYuv(IntPtr handle, IntPtr sourceBuffer, int width, int pitch, int height, int pixelFormat, IntPtr destinationBuffer, Sampler subsampler, int flags);
    }
}

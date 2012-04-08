using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.Draw.Jpeg
{
    public static class Compress
    {
        public static void Save(Raster.Image image)
        {
            IntPtr handler = Api.InitCompressor();
            image = image.Convert<Raster.Bgr>() as Raster.Image;
            int pitch = image.Length / image.Size.Height;
            int pixelFormat = 1;
            Sampler sampler = Sampler.Yuv420;
            int quality = 70;
            int flags = 1024;
            uint jpegBufferSize = Api.BufferSize(image.Size.Width, image.Size.Height, sampler);
            Kean.Core.Buffer.Vector<byte> buffer = new Kean.Core.Buffer.Vector<byte>((int)jpegBufferSize);
            IntPtr pointer = buffer;
            Api.Compress(handler, image.Pointer, image.Size.Width, pitch, image.Size.Height, pixelFormat, ref pointer, out jpegBufferSize, sampler, quality, flags);    
            Api.Destroy(handler);
            System.IO.StreamWriter writer = new System.IO.StreamWriter("test3.jpg");
            writer.BaseStream.Write(buffer.ToArray(), 0, buffer.Size);
            writer.Close();
        }
    }
}

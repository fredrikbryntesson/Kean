// 
//  Jpeg.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2012 Anders Frisk
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

namespace Kean.Draw.Net.Rtsp
{
    public struct Jpeg
    {
        public int TypeSpecific;
        public int FragmentOffset;
        public int JpegType;
        public int Quality;
        public int Width;
        public int Height;
        public int PayloadOffset;
        public int PayloadLength;
        public byte[] QuantizationTable;
        public Jpeg(byte[] buffer, int offset, int length, bool first)
        {
            this.TypeSpecific = buffer[offset + 0];
            this.FragmentOffset = System.BitConverter.ToInt32(new byte[] { buffer[offset + 3], buffer[offset + 2], buffer[offset + 1], 0 }, 0);
            this.JpegType = buffer[offset + 4];
            this.Quality = buffer[offset + 5];
            this.Width = buffer[offset + 6] * 8;
            this.Height = buffer[offset + 7] * 8;
            if (first)
            {
                int mbz = buffer[offset + 8];
                int precision = buffer[offset + 9];
                int quantizationTableLength = (ushort)((buffer[offset + 10]) * 256 + buffer[offset + 11]);
                this.QuantizationTable = new byte[quantizationTableLength];
                Array.Copy(buffer, offset + 12, this.QuantizationTable, 0, this.QuantizationTable.Length); 
                this.PayloadOffset = offset + 12 + quantizationTableLength;
                this.PayloadLength = length - 12 - quantizationTableLength;   
            }
            else
            {
                this.QuantizationTable = null;
                this.PayloadOffset = offset + 8;
                this.PayloadLength = length - 8;
            }
        }
        public byte[] CreateHeader()
        {
            byte[] lumaQuantization = new byte[64];
            byte[] chromaQuantization = new byte[64];
            Array.Copy(this.QuantizationTable, lumaQuantization, 64);
            Array.Copy(this.QuantizationTable, 64, chromaQuantization, 0, 64);
            return this.CreateHeader(lumaQuantization, chromaQuantization);
        }
        public byte[] CreateHeader(byte[] lumaQuantization, byte[] chromaQuantization)
        {
            byte[] result;
            result = Jpeg.CreateHeader(this.Quality, this.JpegType, this.Height / 8, this.Width / 8, lumaQuantization, chromaQuantization);
            return result;
        }
        static byte[] CreateHeader(int quality, int jpegType, int height, int width, byte[] lumaQuantizationTable, byte[] chromaQuantizationTable)
        {
            byte[] result;
            byte[] header = new byte[1024];
            int factor = quality;
            if (quality < 1)
                factor = 1;
            if (quality > 99)
                factor = 99;
            if (quality < 50)
                quality = 5000 / factor;
            else
                quality = 200 - factor * 2;

            int offset = 0;
            /** convert from blocks to pixels **/
            width *= 8;
            height *= 8;
            header[offset++] = (byte)0xff;
            header[offset++] = (byte)0xd8;            /** SOI **/

            offset = Jpeg.CreateQuantizationHeader(header, lumaQuantizationTable, 0, offset);
            offset = Jpeg.CreateQuantizationHeader(header, chromaQuantizationTable, 1, offset);

            //            if (dri != 0)
            //                off = MakeDRIHeader(header, dri, off);

            header[offset++] = (byte)0xff; // Start of Image
            header[offset++] = (byte)0xc0;            /** SOF **/
            header[offset++] = (byte)0;               /** length msb **/
            header[offset++] = (byte)17;              /** length lsb **/
            header[offset++] = (byte)8;               /** 8-bit precision **/
            header[offset++] = (byte)(height >> 8);          /** height msb **/
            header[offset++] = (byte)height;               /** height lsb **/
            header[offset++] = (byte)(width >> 8);          /** width msb **/
            header[offset++] = (byte)width;               /** wudth lsb **/
            header[offset++] = (byte)3;               /** number of components **/
            header[offset++] = (byte)0;               /** comp 0 **/
            if (jpegType == 0)
                header[offset++] = (byte)0x21;    /** hsamoff = 2, vsamoff = 1 **/
            else
                header[offset++] = (byte)0x22;    /** hsamoff = 2, vsamoff = 2 **/
            header[offset++] = (byte)0;               /** quant table 0 **/
            header[offset++] = (byte)1;               /** comp 1 **/
            header[offset++] = (byte)0x11;            /** hsamoff = 1, vsamoff = 1 **/
            header[offset++] = (byte)1;               /** quant table 1 **/
            header[offset++] = (byte)2;               /** comp 2 **/
            header[offset++] = (byte)0x11;            /** hsamoff = 1, vsamoff = 1 **/
            header[offset++] = (byte)1;               /** quant table 1 **/

            offset = Jpeg.CreateHuffmanHeader(header, Jpeg.lum_dc_codelens, Jpeg.lum_dc_symbols, 0, 0, offset);
            offset = Jpeg.CreateHuffmanHeader(header, Jpeg.lum_ac_codelens, Jpeg.lum_ac_symbols, 0, 1, offset);
            offset = Jpeg.CreateHuffmanHeader(header, Jpeg.chm_dc_codelens, Jpeg.chm_dc_symbols, 1, 0, offset);
            offset = Jpeg.CreateHuffmanHeader(header, Jpeg.chm_ac_codelens, Jpeg.chm_ac_symbols, 1, 1, offset);

            header[offset++] = (byte)0xff;
            header[offset++] = (byte)0xda;            /** SOS **/
            header[offset++] = (byte)0;               /** length msb **/
            header[offset++] = (byte)12;              /** length lsb **/
            header[offset++] = (byte)3;               /** 3 components **/
            header[offset++] = (byte)0;               /** comp 0 **/
            header[offset++] = (byte)0;               /** huffman table 0 **/
            header[offset++] = (byte)1;               /** comp 1 **/
            header[offset++] = (byte)0x11;            /** huffman table 1 **/
            header[offset++] = (byte)2;               /** comp 2 **/
            header[offset++] = (byte)0x11;            /** huffman table 1 **/
            header[offset++] = (byte)0;               /** first DCT coeff **/
            header[offset++] = (byte)63;              /** last DCT coeff ***/
            header[offset++] = (byte)0;               /** sucessive approx. ***/

            result = new byte[offset];
            Array.Copy(header, result, offset);
            return result;
        }
        static int CreateQuantizationHeader(byte[] header, byte[] quantizationTable, int tableNumber, int offset)
        {
            header[offset++] = (byte)0xff;
            header[offset++] = (byte)0xdb;            /* DQT */
            header[offset++] = (byte)0;               /* length msb */
            header[offset++] = (byte)67;              /* length lsb */
            header[offset++] = (byte)tableNumber;
            Array.Copy(quantizationTable, 0, header, offset, quantizationTable.Length);
            offset += quantizationTable.Length;
            return offset;
        }
        static int MakeDRIHeader(byte[] header, ushort defineRestartInterval, int offset)
        {
            header[offset++] = (byte)0xff;
            header[offset++] = (byte)0xdd;            /* DRI */
            header[offset++] = (byte)0x0;             /* length msb */
            header[offset++] = (byte)4;               /* length lsb */
            header[offset++] = (byte)(defineRestartInterval >> 8);        /* dri msb */
            header[offset++] = (byte)(defineRestartInterval & 0xff);      /* dri lsb */
            return offset;
        }
        static int CreateHuffmanHeader(byte[] header, byte[] codelens, byte[] symbols, int tableNumber, int tableClass, int offset)
        {
            header[offset++] = (byte)0xff;
            header[offset++] = (byte)0xc4;            /* DHT */
            header[offset++] = (byte)0;               /* length msb */
            header[offset++] = (byte)(3 + codelens.Length + symbols.Length); /* length lsb */
            header[offset++] = (byte)((tableClass << 4) | tableNumber);
            Array.Copy(codelens, 0, header, offset, codelens.Length);
            offset += codelens.Length;
            Array.Copy(symbols, 0, header, offset, symbols.Length);
            offset += symbols.Length;
            return offset;
        }


        /*
         * Table K.1 from JPEG spec.
         */
        static int[] jpegLumaQuantizationTable = {
        16, 11, 10, 16, 24, 40, 51, 61,
        12, 12, 14, 19, 26, 58, 60, 55,
        14, 13, 16, 24, 40, 57, 69, 56,
        14, 17, 22, 29, 51, 87, 80, 62,
        18, 22, 37, 56, 68, 109, 103, 77,
        24, 35, 55, 64, 81, 104, 113, 92,
        49, 64, 78, 87, 103, 121, 120, 101,
        72, 92, 95, 98, 112, 100, 103, 99
        };

        /*
         * Table K.2 from JPEG spec.
         */
        static int[] jpegChromaQuantizationTable = {
        17, 18, 24, 47, 99, 99, 99, 99,
        18, 21, 26, 66, 99, 99, 99, 99,
        24, 26, 56, 99, 99, 99, 99, 99,
        47, 66, 99, 99, 99, 99, 99, 99,
        99, 99, 99, 99, 99, 99, 99, 99,
        99, 99, 99, 99, 99, 99, 99, 99,
        99, 99, 99, 99, 99, 99, 99, 99,
        99, 99, 99, 99, 99, 99, 99, 99
        };


        static byte[] lum_dc_codelens = { 0, 1, 5, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, };

        static byte[] lum_dc_symbols = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, };

        static byte[] lum_ac_codelens = { 0, 2, 1, 3, 3, 2, 4, 3, 5, 5, 4, 4, 0, 0, 1, 0x7d, };

        static byte[] lum_ac_symbols = {
                                   0x01, 0x02, 0x03, 0x00, 0x04, 0x11, 0x05, 0x12,
        0x21, 0x31, 0x41, 0x06, 0x13, 0x51, 0x61, 0x07,
        0x22, 0x71, 0x14, 0x32, 0x81, 0x91, 0xa1, 0x08,
        0x23, 0x42, 0xb1, 0xc1, 0x15, 0x52, 0xd1, 0xf0,
        0x24, 0x33, 0x62, 0x72, 0x82, 0x09, 0x0a, 0x16,
        0x17, 0x18, 0x19, 0x1a, 0x25, 0x26, 0x27, 0x28,
        0x29, 0x2a, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39,
        0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49,
        0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59,
        0x5a, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
        0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79,
        0x7a, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89,
        0x8a, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98,
        0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7,
        0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6,
        0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3, 0xc4, 0xc5,
        0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xd2, 0xd3, 0xd4,
        0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xe1, 0xe2,
        0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea,
        0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
        0xf9, 0xfa,
};

        static byte[] chm_dc_codelens = { 0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, };

        static byte[] chm_dc_symbols = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, };

        static byte[] chm_ac_codelens = { 0, 2, 1, 2, 4, 4, 3, 4, 7, 5, 4, 4, 0, 1, 2, 0x77, };

        static byte[] chm_ac_symbols = {
        0x00, 0x01, 0x02, 0x03, 0x11, 0x04, 0x05, 0x21,
        0x31, 0x06, 0x12, 0x41, 0x51, 0x07, 0x61, 0x71,
        0x13, 0x22, 0x32, 0x81, 0x08, 0x14, 0x42, 0x91,
        0xa1, 0xb1, 0xc1, 0x09, 0x23, 0x33, 0x52, 0xf0,
        0x15, 0x62, 0x72, 0xd1, 0x0a, 0x16, 0x24, 0x34,
        0xe1, 0x25, 0xf1, 0x17, 0x18, 0x19, 0x1a, 0x26,
        0x27, 0x28, 0x29, 0x2a, 0x35, 0x36, 0x37, 0x38,
        0x39, 0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48,
        0x49, 0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58,
        0x59, 0x5a, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68,
        0x69, 0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78,
        0x79, 0x7a, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87,
        0x88, 0x89, 0x8a, 0x92, 0x93, 0x94, 0x95, 0x96,
        0x97, 0x98, 0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5,
        0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4,
        0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3,
        0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xd2,
        0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda,
        0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9,
        0xea, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
        0xf9, 0xfa,
};










    }
}

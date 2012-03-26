// 
//  Client.cs
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
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;
using Raster = Kean.Draw.Raster;
using Parallel = Kean.Core.Parallel;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
namespace Kean.Draw.Net.Rtsp
{
    public class Client
    {
        public event Action<byte[]> NewData;
        public Status Status { get; private set; }
        Parallel.Thread thread;
        Protocol.Rtsp protocol;
        public Client()
        { }
        public bool Open(Uri.Locator locator)
        {
            bool result = false;
            this.Initialize();
            try
            {
                Draw.Net.Client.Tcp client = new Net.Client.Tcp();
                if (result = client.Open(locator))
                {
                    this.protocol = new Protocol.Rtsp(locator, client.SendMessage, client.RecieveMessage);
                    result &= this.Setup();
                    if (result)
                    {
                        this.Status = Status.Playing;
                        if(this.Play())
                            this.Start();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            return result;
        }
        void Start()
        {
            Net.Client.Rtp<Protocol.Jpeg> client = new Net.Client.Rtp<Protocol.Jpeg>();
            client.Open(this.protocol.Port);
            this.thread = Parallel.Thread.Start("Draw.Net", () =>
            {
                int width = 0;
                int height = 0;
                int jpegType = 0;
                int quality = 0;
                byte[] quantizationTable = null;
                byte[] jpegPayload = new byte[512 * 1024];
                int jpegPayloadLength = 0;
                bool frame = false;
                while (this.Status != Status.Closed)
                {
                    if (client.Avaliable)
                    {
                        Protocol.Rtp<Protocol.Jpeg> rtp = client.Recieve();
                        //Console.WriteLine("Counter " + rtp.SequenceNumber + " offset" + rtp.Payload.FragmentOffset + " " + rtp.Marker + " " + rtp.Payload.Width + " " + rtp.Payload.Height);
                        //Console.WriteLine("rtp " + rtp.SequenceNumber + " marker " + rtp.Marker);
                        // Leave the first partial image (which will never will be completed).
                        if (rtp.Marker && !frame)
                            frame = true;
                        else if (!rtp.Marker && frame)
                        {
                            if (rtp.Payload.First)
                            {
                                width = rtp.Payload.Width;
                                height = rtp.Payload.Height;
                                jpegType = rtp.Payload.JpegType;
                                quality = rtp.Payload.Quality;
                                quantizationTable = rtp.Payload.QuantizationTable;
                            }
                            Array.Copy(rtp.Payload.Payload, 0, jpegPayload, rtp.Payload.FragmentOffset, rtp.Payload.Payload.Length);
                            jpegPayloadLength += rtp.Payload.Payload.Length;
                        }
                        else if (rtp.Marker && frame)
                        {
                            Array.Copy(rtp.Payload.Payload, 0, jpegPayload, rtp.Payload.FragmentOffset, rtp.Payload.Payload.Length);
                            jpegPayloadLength += rtp.Payload.Payload.Length;

                            byte[] image = new byte[512 * 1024];
                            byte[] header = Protocol.Jpeg.CreateHeader(quality, jpegType, width / 8, height / 8, quantizationTable);
                            Array.Copy(header, image, header.Length);
                            Array.Copy(jpegPayload, 0, image, header.Length, jpegPayloadLength);
                            this.NewData.Call(image);
                            jpegPayloadLength = 0;
                        }
                    }
                }
            });
        }
        bool Setup()
        {
            return this.protocol.Options() && this.protocol.Description() && this.protocol.Setup();
        }
        public bool Play()
        {
            return this.Status != Status.Closed && this.protocol.Play();
        }
        public bool Pause()
        {
            return this.Status != Status.Closed && this.protocol.Pause();
        }
        public bool Close()
        {
            return this.Status != Status.Closed && this.protocol.Teardown();
        }
        protected virtual void Initialize()
        { }
    }
}

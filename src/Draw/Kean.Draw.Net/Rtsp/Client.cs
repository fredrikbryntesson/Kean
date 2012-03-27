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
        public event Action<Raster.Image> NewFrame;
        public Status Status { get; private set; }
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
            Parallel.ThreadPool pool = new Parallel.ThreadPool("Draw.Net.Pool");
            Parallel.Thread.Start("Draw.Net", () =>
            {
                bool frame = false;
                Protocol.Jpeg image = new Protocol.Jpeg();
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
                                image = new Protocol.Jpeg(
                                    rtp.Payload.TypeSpecific,
                                    0,
                                    rtp.Payload.JpegType,
                                    rtp.Payload.Quality,
                                    rtp.Payload.Size,
                                    new byte[512 * 1024],
                                    rtp.Payload.QuantizationTable);
                            }
                            Array.Copy(rtp.Payload.Payload, 0, image.Payload, rtp.Payload.FragmentOffset, rtp.Payload.Payload.Length);
                            image.PayloadLength = Kean.Math.Integer.Maximum(image.PayloadLength, rtp.Payload.FragmentOffset + rtp.Payload.Payload.Length);
                        }
                        else if (rtp.Marker && frame)
                        {
                            Array.Copy(rtp.Payload.Payload, 0, image.Payload, rtp.Payload.FragmentOffset, rtp.Payload.Payload.Length);
                            image.PayloadLength = Kean.Math.Integer.Maximum(image.PayloadLength, rtp.Payload.FragmentOffset + rtp.Payload.Payload.Length);
                            pool.Enqueue(() =>
                            {
                                byte[] header = image.CreateHeader();
                                byte[] all = new byte[header.Length + image.PayloadLength];
                                Array.Copy(header, all, header.Length);
                                Array.Copy(image.Payload, 0, all, header.Length, image.PayloadLength);
                                this.NewFrame.Call(Raster.Image.Open(new System.IO.MemoryStream(all)));
                            });
                        }
                    }
                    else
                        System.Threading.Thread.Sleep(20);
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

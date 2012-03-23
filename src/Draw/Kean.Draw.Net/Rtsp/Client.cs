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
        TcpClient tcpClient;
        Parallel.Thread thread;
        Protocol protocol;
        public Client()
        { }
        public bool Open(Uri.Locator locator)
        {
            bool result = false;
            this.Initialize();
            try
            {
                this.tcpClient = new TcpClient();
                if (result = this.tcpClient.Open(locator))
                {
                    this.protocol = new Protocol(locator, this.tcpClient.SendMessage, this.tcpClient.RecieveMessage);
                    result &= this.Setup();
                    if (result)
                    {
                        this.Status = Status.Playing;
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
            System.Net.Sockets.UdpClient udpClient = new System.Net.Sockets.UdpClient(this.protocol.Port);
            //udpClient.Client.Blocking = true;
            //udpClient.Client.ReceiveTimeout = 1000;
            System.Net.IPEndPoint endPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, this.protocol.Port);
            this.thread = Parallel.Thread.Start("Draw.Net", () =>
            {
                byte[] jpegData = new byte[512 * 1024];
                int jpegDataLength = 0;
                bool frame = false;
                bool first = true;
                while (this.Status != Status.Closed)
                {
                    if (udpClient.Available > 0)
                    {
                        byte[] udp = udpClient.Receive(ref endPoint);
                        Rtp rtp = new Rtp(udp, udp.Length);
                        //Console.WriteLine("rtp " + rtp.SequenceNumber + " marker " + rtp.Marker);
                        // Leave the first partial image (which will never will be completed).
                        if (rtp.Marker && !frame)
                            frame = true;
                        else if (!rtp.Marker && frame)
                        {
                            Net.Jpeg.Protocol jpeg = new Net.Jpeg.Protocol(udp, rtp.PayloadOffset, rtp.PayloadLength, first);
                            // first part of jpeg image
                            if (first)
                            {
                                byte[] header = jpeg.CreateHeader();
                                Array.Copy(header, jpegData, header.Length);
                                jpegDataLength += header.Length;
                                first = false;
                            }
                            // parts in between
                            Array.Copy(udp, jpeg.PayloadOffset, jpegData, jpegDataLength, jpeg.PayloadLength);
                            jpegDataLength += jpeg.PayloadLength;
                        }
                        else if (rtp.Marker && frame)
                        {
                            // last part of jpeg image
                            Net.Jpeg.Protocol jpeg = new Net.Jpeg.Protocol(udp, rtp.PayloadOffset, rtp.PayloadLength, false);
                            Array.Copy(udp, jpeg.PayloadOffset, jpegData, jpegDataLength, jpeg.PayloadLength);
                            jpegDataLength += jpeg.PayloadLength;

                            // complete jpeg image data
                            byte[] data = new byte[jpegDataLength];
                            Array.Copy(jpegData, 0, data, 0, jpegDataLength);
                            this.NewData.Call(data);
                            jpegDataLength = 0;
                            first = true;
                        }
                    }
                }
            });
        }
        bool Setup()
        {
            return this.protocol.Description() && this.protocol.Setup();
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

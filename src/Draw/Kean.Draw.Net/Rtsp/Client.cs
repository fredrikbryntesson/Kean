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
        bool stopped;
        public event Action<byte[]> NewData;
        public bool Running { get { return !this.stopped; } }
        Uri.Locator locator;
        System.Net.Sockets.TcpClient clientConnection;
        System.Net.Sockets.NetworkStream clientStream;
        int port = 9000;
        int counter = 0;
        string session;
        Parallel.Thread thread;
        Protocol protocol;
        public Client(Uri.Locator locator)
        {
            this.locator = locator;
            this.clientConnection = new System.Net.Sockets.TcpClient(this.locator.Authority.Endpoint.Host.ToString(), this.locator.Authority.Endpoint.Port.HasValue ? (int)this.locator.Authority.Endpoint.Port.Value : 554);
            this.clientStream = clientConnection.GetStream();
            this.protocol = new Protocol(locator, this.SendMessage, this.RecieveMessage);
        }
        public bool Start()
        {
            bool result = false;
            this.Initialize();
            if (result = this.Setup() && this.Play())
            {
                System.Net.Sockets.Socket server = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                         System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
                System.Net.EndPoint endPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Parse("192.168.1.65"), this.port);
                server.Bind(endPoint);
                this.stopped = false;
                this.thread = Parallel.Thread.Start("Draw.Net", () =>
                {
                    Console.WriteLine("Started");
                    server.ReceiveBufferSize = 8192 * 15;
                    server.ReceiveTimeout = 200;
                    byte[] jpegData = new byte[512 * 1024];
                    int jpegDataLength = 0;
                    byte[] buffer = new byte[server.ReceiveBufferSize];
                    bool frame = false;
                    bool first = true;
                    int index = 0;
                    while (this.Running)
                    {
                        if (server.Available > 0)
                        {
                            int read = server.Receive(buffer);
                            Rtp rtp = new Rtp(buffer, read);
                            if (rtp.Marker && !frame)
                                frame = true;
                            else if (!rtp.Marker && frame)
                            {
                                Net.Jpeg.Protocol jpeg = new Net.Jpeg.Protocol(buffer, rtp.PayloadOffset, rtp.PayloadLength, first);
                                // first
                                if (first)
                                {
                                    byte[] header = jpeg.CreateHeader();
                                    Array.Copy(header, jpegData, header.Length);
                                    jpegDataLength += header.Length;
                                }
                                first = false;
                                // between
                                Array.Copy(buffer, jpeg.PayloadOffset, jpegData, jpegDataLength, jpeg.PayloadLength);
                                jpegDataLength += jpeg.PayloadLength;
                            }
                            else if (rtp.Marker && frame)
                            {
                                // last
                                Net.Jpeg.Protocol jpeg = new Net.Jpeg.Protocol(buffer, rtp.PayloadOffset, rtp.PayloadLength, false);
                                Array.Copy(buffer, jpeg.PayloadOffset, jpegData, jpegDataLength, jpeg.PayloadLength);
                                jpegDataLength += jpeg.PayloadLength;

                                byte[] data = new byte[jpegDataLength];
                                Array.Copy(jpegData, 0, data, 0, jpegDataLength);
                                this.NewData.Call(data);
                                /*
                                Raster.Image image = Raster.Image.Open(new System.IO.MemoryStream(jpegData, 0, jpegDataLength));
                                image.Save("test" + (index++) + ".png");
                                Console.WriteLine("image found " + image.NotNull() + " time " + rtp.Timestamp);
                                */
                                jpegDataLength = 0;
                                first = true;
                            }
                        }
                    }
                    Console.WriteLine("Stopped");
                });
            }
            return result;
        }
        public bool Stop()
        {
            return this.Pause() && this.TearDown();
        }
        bool Setup()
        {
            bool result = false;
            result = this.protocol.Description();
            return result;
        }
        bool Play()
        {
            bool result = false;
            int? port = this.protocol.Setup();
            if (port.HasValue)
            {
                this.port = port.Value;
                result = this.protocol.Play();
            }
            return result;
        }
        bool Pause()
        {
            return this.protocol.Pause(); 
        }
        bool TearDown()
        {
            return this.protocol.Teardown();
        }
        void SendMessage(string message)
        {
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(message);
            this.clientStream.Write(buffer, 0, buffer.Length);
            System.Threading.Thread.Sleep(200);
        }
        string RecieveMessage()
        {
            string result = null;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            System.Threading.Thread.Sleep(200);
            int trials = 0;
            while (this.clientConnection.Connected && this.clientStream.CanRead && this.clientStream.DataAvailable)
                {
                    int value = this.clientStream.ReadByte();
                    if (value >= 0)
                        builder.Append(System.Text.Encoding.ASCII.GetChars(new byte[] { (byte)value }));
                    System.Threading.Thread.Sleep(10);
                }
            result = builder.ToString();
            return result;
        }
        protected virtual void Initialize()
        { }
    }
}

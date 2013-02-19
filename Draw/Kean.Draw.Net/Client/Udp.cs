// 
//  Udp.cs
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

namespace Kean.Draw.Net.Client
{
    public class Udp
    {
        public bool Avaliable { get { return this.client.Available > 0; } }
        System.Net.IPEndPoint endPoint;
        System.Net.Sockets.UdpClient client;
        public Udp()
        { }
        public bool Open(int port)
        {
            bool result = false;
            try
            {
                this.client = new System.Net.Sockets.UdpClient();
                this.client.Client.ReceiveTimeout = 1000;
                this.client.Client.ReceiveBufferSize = 100000;
                this.endPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, port);
                this.client.Client.Bind(this.endPoint);
                result = true;
            }
            catch (Exception)
            { }
            return result;
        }
        public void Send(byte[] buffer)
        {
            this.client.Send(buffer, buffer.Length);
        }
        public byte[] Recieve()
        {
            byte[] result = null;
            if(this.client.Available > 0 ) 
                result = this.client.Receive(ref this.endPoint);
            return result;
        }
    }
}
// 
//  Tcp.cs
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
using Collection = Kean.Core.Collection;
namespace Kean.Draw.Net.Client
{
    public class Tcp
    {
        System.Net.Sockets.TcpClient client;
        byte[] buffer;
        public Tcp()
        { }
        public bool Open(Uri.Locator locator)
        {
            bool result = false;
            try
            {
                this.client = new System.Net.Sockets.TcpClient();
                this.buffer = new byte[this.client.ReceiveBufferSize];
                this.client.Connect(locator.Authority.Endpoint.Host.ToString(), locator.Authority.Endpoint.Port.HasValue ? (int)locator.Authority.Endpoint.Port.Value : 554);
                result = true;
            }
            catch (Exception)
            {}
            return result;
        }
        public void Send(byte[] buffer)
        {
            this.client.Client.Send(buffer);
        }
        public void SendMessage(string message)
        {
            this.Send(System.Text.Encoding.ASCII.GetBytes(message));
        }
        public byte[] Recieve()
        {
            byte[] result = null;
            int read = this.client.Client.Receive(this.buffer);
            if (read >= 0)
            {
                result = new byte[read];
                Array.Copy(this.buffer, 0, result, 0, read);
            }
            return result;
        }
        public string RecieveMessage()
        {
            string result = null;
            byte[] answer = this.Recieve();
            if (answer.NotEmpty())
                result = System.Text.Encoding.ASCII.GetString(answer);
            return result;
        }
    }
}

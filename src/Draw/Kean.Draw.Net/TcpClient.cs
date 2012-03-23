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
namespace Kean.Draw.Net
{
    public class TcpClient
    {
        Uri.Locator locator;
        System.Net.Sockets.Socket tcpClient;
        public TcpClient()
        { }
        public bool Open(Uri.Locator locator)
        {
            bool result = false;
            this.locator = locator;
            try
            {
                this.tcpClient = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                this.tcpClient.Connect(this.locator.Authority.Endpoint.Host.ToString(), this.locator.Authority.Endpoint.Port.HasValue ? (int)this.locator.Authority.Endpoint.Port.Value : 554);
                result = true;
            }
            catch (Exception e)
            {}
            return result;
        }
        public void SendMessage(string message)
        {
            this.tcpClient.Send(System.Text.Encoding.ASCII.GetBytes(message));
        }
        public string RecieveMessage()
        {
            string result = null;
            byte[] buffer = new byte[10000];
            int read = this.tcpClient.Receive(buffer);
            result = System.Text.Encoding.ASCII.GetString(buffer, 0, read);
            return result;
        }
    }
}

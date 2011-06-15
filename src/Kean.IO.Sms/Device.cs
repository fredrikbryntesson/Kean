// 
//  Device.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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
using Kean.Core.Basis.Extension;

namespace Kean.IO.Sms
{
	public class Device
	{
		System.IO.Ports.SerialPort port;

		public bool IsOpen { get { return this.port.NotNull() && this.port.IsOpen; } }
		public Device()
		{
		}

		public bool Connect(string resource)
		{
			this.port = new System.IO.Ports.SerialPort(resource, 57600, System.IO.Ports.Parity.None);
			this.port.Open();
			this.port.DataReceived += (sender, e) => Console.WriteLine(this.port.ReadExisting());
			this.port.WriteLine("AT\r");
			Console.WriteLine("AT");
			Console.WriteLine(this.port.ReadExisting());
			return this.IsOpen;

		}
		public void Close()
		{
			if (this.IsOpen)
				this.port.Close();
		}
		public void Send(Message message)
		{
			Console.WriteLine(this.port.ReadExisting());
			this.port.WriteLine("AT+CMGS=\"" +  message.Receiver + "\"");
			Console.WriteLine("AT+CMGS=\"" +  message.Receiver + "\"");
			this.port.WriteLine(message.Body);
			this.port.Write(new byte[] { 0x1a }, 0, 1);
			Console.WriteLine(this.port.ReadExisting());
		}
	}
}


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
		Modem.Mobile modem;

		public bool IsOpen { get { return this.modem.NotNull() && this.modem.IsOpen; } }
		public Device()
		{
		}

		public bool Open(string resource)
		{
			this.modem = new Modem.Mobile();
			Serial.IPort port = new Serial.Debug(new Serial.Port(resource));
			return port.Open("57600 8n1") && this.modem.Open(port) && this.modem.Attention() && this.IsOpen;
		}
		public bool Close()
		{
			return this.IsOpen && this.modem.Close();
		}
		public void Send(Message message)
		{
			Console.WriteLine(this.modem.SmsMode);
			this.modem.SmsMode = Modem.SmsMode.Text;
			Console.WriteLine(this.modem.SmsMode);
			this.modem.SendMessage(message.Receiver, message.Body);
			//this.port.WriteLine("AT+CMGS=\"" +  message.Receiver + "\"");
			//this.port.WriteLine(message.Body);
			//this.port.Write(0x1a);
		}
	}
}


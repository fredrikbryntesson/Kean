// 
//  Message.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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

namespace Kean.IO.Modem.Command
{
	public class Message :
		Execute
	{
		string message;
		protected override string Command { get { return base.Command; } }
		public Message(string name, string message, params string[] parameters) :
			base(name, parameters)
		{
			this.message = message;
		}
		protected override void Send(Serial.IPort port)
		{
			base.Send (port);
			port.Write(this.message);
			port.Write(0x1a);
		}
	}
}

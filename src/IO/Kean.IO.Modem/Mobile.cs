// 
//  Mobile.cs
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
using Collection = Kean.Core.Collection;

namespace Kean.IO.Modem
{
	public class Mobile :
		Standard
	{
		bool TextMode { get { return this.smsMode == SmsMode.Text; } }
		SmsMode smsMode = SmsMode.Pdu;
		public SmsMode SmsMode
		{
			get
			{
				this.smsMode = (SmsMode) int.Parse(this.Read("+CMGF")[0]);
				return this.smsMode;
			}
			set
			{
				this.smsMode = value;
				this.Set("+CMGF", ((int)value).ToString());
			}
		}

		public Mobile()
		{
		}

		public bool SendMessage(string receiver, string message)
		{
			return this.TextMode ?
				this.Message("+CMGS", message, "\"" + receiver + "\"") :
				false;
		}
	}
}


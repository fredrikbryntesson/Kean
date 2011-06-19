// 
//  MyClass.cs
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
using Kean.Core.Basis.Extension;
using Kean.Core.Collection.Extension;

namespace Kean.IO.Modem
{
	public class Connection
	{
		Serial.IPort port;
		public bool IsOpen { get { return this.port.NotNull() && this.port.IsOpen; } }

		public Connection()
		{
		}
		public bool Open(Serial.IPort port)
		{
			this.port = port;
			return this.port.NotNull();
		}
		public bool Open(string resource, Serial.Settings settings)
		{
			return this.Open(Serial.Port.Open(resource, settings));
		}
		public bool Close()
		{
			return this.IsOpen && this.Close();
		}
		public string[] Send(params string[] commands)
		{
			this.port.WriteLine(commands.Fold((command, accumulated) => accumulated + command + ";", "AT").TrimEnd(";"));
			return this.port.Read().Split(new char[] { '\n', '\r' },StringSplitOptions.RemoveEmptyEntries);
		}
	}
}


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
using Collection = Kean.Core.Collection;
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
			return this.IsOpen;
		}
		public bool Open(string resource, Serial.Settings settings)
		{
			return this.Open(Serial.Port.Open(resource, settings));
		}
		public bool Close()
		{
			return this.IsOpen && this.port.Close();
		}
		public string[] Send(params string[] commands)
		{
			this.port.WriteLine(commands.Fold((command, accumulated) => accumulated + command + ";", "AT").TrimEnd(';'));
			return this.port.Read().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
		}
		public bool Execute(string command, params string[] parameters)
		{
			Command.Execute result = new Command.Execute(command, parameters);
			result.Run(this.port);
			return result.Succeded;
		}
		public bool Message(string command, string message, params string[] parameters)
		{
			Command.Execute result = new Command.Message(command, message, parameters);
			result.Run(this.port);
			return result.Succeded;
		}
		public bool Set(string variable, params string[] values)
		{
			Command.Set result = new Command.Set(variable, values);
			result.Run(this.port);
			return result.Succeded;
		}
		public Collection.IReadOnlyVector<string> Read(string variable)
		{
			Command.Read result = new Command.Read(variable);
			result.Run(this.port);
			return result.Parameters;
		}
		public Collection.IReadOnlyVector<string> Test(string command)
		{
			Command.Test result = new Command.Test(command);
			result.Run(this.port);
			return result.Parameters;
		}
	}
}


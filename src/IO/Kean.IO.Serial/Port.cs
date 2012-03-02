// 
//  Port.cs
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
using Ports = System.IO.Ports;
using Kean.Core.Extension;
using Kean.Core.Collection.Extension;

namespace Kean.IO.Serial
{
	public class Port :
		IPort,
		IDisposable
	{
		Ports.SerialPort backend;

		public bool IsOpen { get { return this.backend.NotNull() && this.backend.IsOpen; } }

		public string Device { get; private set; }
		public Settings Settings { get; private set; }

		public Port(string device)
		{
			this.Device = device;
		}
		~Port ()
		{
			this.Close();
		}
		public bool Open(Settings settings)
		{
			this.Settings = settings;
			System.IO.Ports.Parity parity;
			switch (settings.Parity)
			{
			default:
			case Parity.None: parity = System.IO.Ports.Parity.None; break;
			case Parity.Odd: parity = System.IO.Ports.Parity.Odd; break;
			case Parity.Even: parity = System.IO.Ports.Parity.Even; break;
			}
			System.IO.Ports.StopBits stopBits;
			switch (settings.StopBits)
			{
			default:
			case 0: stopBits = System.IO.Ports.StopBits.None; break;
			case 1: stopBits = System.IO.Ports.StopBits.One; break;
			case 2: stopBits = System.IO.Ports.StopBits.Two; break;
			}

			this.backend = new System.IO.Ports.SerialPort(this.Device, settings.BaudRate, parity, settings.DataBits, stopBits);
			this.backend.Open();
			this.backend.ReadTimeout = 400;
			this.backend.NewLine = "\r";
			return this.IsOpen;
		}
		public bool Close()
		{
			bool result = !this.IsOpen;
			if (!result)
			{
				this.backend.Close();
				result = !this.backend.IsOpen;
			}
			if (result && this.backend.NotNull())
			{
				this.backend.Dispose();
				this.backend = null;
			}
			return result;
		}
		public void Write(params byte[] value)
		{
			this.backend.Write(value, 0, value.Length);
		}
		public void Write(string value)
		{
			this.backend.Write(value);
		}
		public void WriteLine(string value)
		{
			this.backend.WriteLine(value);
		}
		public string Read()
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			try
			{
				while (true)
					result.Append((char) (byte) this.backend.ReadByte());
			}
			catch (TimeoutException)
			{
			}
			return result.ToString();
		}
		void IDisposable.Dispose()
		{
			this.Close();
		}
		public static Port Open(string device, Settings settings)
		{
			Port result = new Port(device);
			if (!result.Open(settings))
			{
				result.Close();
				result = null;
			}
			return result;
		}
		public static Port[] Available { get { return System.IO.Ports.SerialPort.GetPortNames().Map(name => new Port(name)); } }
	}
}


// 
//  Program.cs
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
using Parallel = Kean.Core.Parallel;

namespace Kean.IO.Net.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Program.Proxy(23, "192.168.1.101:23");
			//Program.Proxy();
		}
		static void Proxy()
		{
			System.Net.Sockets.TcpListener server = new System.Net.Sockets.TcpListener(23);
			server.Start();
			while (true)
			{
				Console.Write("Waiting for a connection... ");
				System.Net.Sockets.TcpClient serverConnection = server.AcceptTcpClient();
				System.Net.Sockets.NetworkStream serverStream = serverConnection.GetStream();
				Console.WriteLine("Connected");
				System.Net.Sockets.TcpClient clientConnection = new System.Net.Sockets.TcpClient("192.168.1.101", 23);
				System.Net.Sockets.NetworkStream clientStream = clientConnection.GetStream();

				Parallel.Thread send = Parallel.Thread.Start(() =>
				{
					while (serverConnection.Connected && clientConnection.Connected)
					{
						int value = clientStream.ReadByte();
						if (value >= 0)
						{
							Console.WriteLine("< " + value + "\t" + new string((System.Text.Encoding.ASCII.GetChars(new byte[] { (byte)value }))));
							serverStream.WriteByte((byte)value);
						}
					}
				});
				while (serverConnection.Connected && clientConnection.Connected)
				{
					int value = serverStream.ReadByte();
					if (value >= 0)
					{
						Console.WriteLine("> " + value + "\t" + new string((System.Text.Encoding.ASCII.GetChars(new byte[] { (byte)value }))));
						clientStream.WriteByte((byte)value);
					}
				}
			}
		}
		static void Proxy(uint listenPort, string connectTo)
		{
			Console.Write("Waiting for a connection... ");
			new Kean.IO.Net.Tcp.Server(server =>
			{
				Tcp.Connection client = Tcp.Connection.Connect(connectTo);
				Console.WriteLine("Connected");
				Parallel.Thread send = Parallel.Thread.Start(() =>
				{
					while (server.Opened && client.Opened)
					{
						char? value = client.Read();
						if (value.HasValue)
						{
							Console.WriteLine("< " + System.Text.Encoding.ASCII.GetBytes(new char[] { value.Value })[0] + "\t" + value.Value);
							server.Write(new char[] { value.Value });
						}
					}
				});
				while (server.Opened && client.Opened)
				{
					char? value = server.Read();
					if (value.HasValue)
					{
						Console.WriteLine("> " + System.Text.Encoding.ASCII.GetBytes(new char[] { value.Value })[0] + "\t" + value.Value);
						client.Write(new char[] { value.Value });
					}
				}
				send.Abort();
			}
			, listenPort);
		}
		static void Terminal()
		{
			Tcp.Connection connection = Tcp.Connection.Connect("192.168.1.101:23");
			char? incomming;
			while ((incomming = connection.Read()).HasValue)
				Console.Write(incomming.Value);
		}
		static void Echo()
		{
			new Kean.IO.Net.Tcp.Server(connection =>
			{
				while (connection.Opened)
				{
					char? value = connection.Read();
					if (value.HasValue)
					{
						Console.Write(value.Value);
						connection.Write(new char[] { char.ToUpper(value.Value) });
					}
				}
			}
			, 7);
		}

	}
}

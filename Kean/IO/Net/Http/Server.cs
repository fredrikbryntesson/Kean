// 
//  Server.cs
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
using Kean;
using Kean.Extension;
using Uri = Kean.Uri;
using Kean.IO.Extension;
using Generic = System.Collections.Generic;

namespace Kean.IO.Net.Http
{
	public class Server :
		IDisposable
	{
		public event Action Closed 
		{ 
			add { this.connection.Closed += value; } 
			remove { this.connection.Closed -= value; } 
		}
		public bool AutoClose
		{
			get { return this.connection.AutoClose; }
			set { this.connection.AutoClose = value; }
		}
		public Method Method { get; private set; }
		public Uri.Path Path { get; private set; }
		public string Protocol { get; private set; }
		Collection.IDictionary<string, string> headers = new Collection.Dictionary<string, string>();
		public string this[string key] 
		{ 
			get { return this.headers[key]; } 
			set 
			{ 
				if (key.NotEmpty())
					this.headers[key] = value; 
			}
		}
		Tcp.Connection connection;
		public IO.IByteDevice Device { get { return this.connection; } }
		public IO.ICharacterReader Reader { get; private set; }
		public IO.ICharacterWriter Writer { get; private set; }
		Server(Tcp.Connection connection)
		{
			this.connection = connection;
			IO.ICharacterDevice characterDevice = IO.CharacterDevice.Open(this.Device);
			this.Reader = IO.CharacterReader.Open(characterDevice);
			this.Writer = IO.CharacterWriter.Open(characterDevice);
			this.Writer.NewLine = new char[] { '\r', '\n' };
			this.ParseRequestHeader(this.Device);
		}
		~Server()
		{
			Error.Log.Call(() => this.Close());
		}
		void IDisposable.Dispose()
		{
			this.Close();
		}
		bool ParseRequestHeader(IO.IByteInDevice device)
		{
			bool result = false;
			string[] firstLine = this.ReadLine(device).Decode().Join().Split(' ');
			if (firstLine.Length == 3)
			{
				this.Method = firstLine[0].Parse<Method>();
				this.Path = firstLine[1];
				this.Protocol = firstLine[2];
				string line;
				result = true;
				while ((line = this.ReadLine(device).Decode().Join()).NotEmpty())
				{
					string[] parts = line.Split(new char[] { ':' }, 2);
					this[parts[0].Trim()] = parts[1].Trim();
				}
			}
			return result;
		}
		Generic.IEnumerable<byte> ReadLine(IO.IByteInDevice device)
		{
			foreach (byte b in device.Read(13, 10))
				if (b != 13 && b!= 10)
					yield return b;
			//byte? next = device.Peek();
			//if (next.HasValue && next.Value == 32 || next.Value == 9) // lines broken into several lines must start with space (SP) or horizontal tab (HT)
			//	foreach (byte b in this.ReadLine(device))
			//		yield return b;
		}
		public bool Respond(Http.Status status, params KeyValue<string, string>[] headers)
		{
			return this.Respond(this.Protocol, status, headers);
		}
		public bool Respond(string protocol, Http.Status status, params KeyValue<string, string>[] headers)
		{
			this.Writer.WriteLine(protocol + " " + status);
			foreach (var header in headers)
				this.Writer.WriteLine(header.Key + ": " + header.Value);
			this.Writer.WriteLine();
			return true;
		}
		public bool Close()
		{
			bool result = false;
			if (this.Device.NotNull())
			{
				result = this.connection.Close();
				this.connection = null;
			}
			return result;
		}
		#region Static Create & CreateThreaded
		public static Tcp.Server Create(Action<Server> process)
		{
			return new Tcp.Server(connection => process(new Server(connection)));
		}
		public static Tcp.ThreadedServer CreateThreaded(Action<Server> process)
		{
			return new Tcp.ThreadedServer("HttpServer", connection => process(new Server(connection)));
		}
		#endregion
		#region static Start & Run
		public static Tcp.Server Start(Action<Server> process, Uri.Endpoint endPoint)
		{
			return Tcp.Server.Start(connection => process(new Server(connection)), endPoint);
		}
		public static Tcp.Server Start(Action<Server> process, uint port)
		{
			return Tcp.Server.Start(connection => process(new Server(connection)), port);
		}
		public static Tcp.ThreadedServer StartThreaded(Action<Server> process, Uri.Endpoint endPoint)
		{
			return Tcp.ThreadedServer.Start(connection => process(new Server(connection)), endPoint);
		}
		public static Tcp.ThreadedServer StartThreaded(Action<Server> process, uint port)
		{
			return Tcp.ThreadedServer.Start(connection => process(new Server(connection)), port);
		}
		public static bool Run(Action<Server> process, Uri.Endpoint endPoint)
		{
			return Tcp.Server.Run(connection => process(new Server(connection)), endPoint);
		}
		public static bool Run(Action<Server> process, uint port)
		{
			return Tcp.Server.Run(connection => process(new Server(connection)), port);
		}
		public static bool RunThreaded(Action<Server> process, Uri.Endpoint endPoint)
		{
			return Tcp.ThreadedServer.Run(connection => process(new Server(connection)), endPoint);
		}
		public static bool RunThreaded(Action<Server> process, uint port)
		{
			return Tcp.ThreadedServer.Run(connection => process(new Server(connection)), port);
		}
		#endregion
	}
}

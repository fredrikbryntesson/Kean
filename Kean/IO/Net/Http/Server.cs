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
using Kean.Collection.Extension;
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
		readonly Collection.IDictionary<string, string> headers = new Collection.Dictionary<string, string>();
		public string this [string key]
		{ 
			get { return this.headers[key]; } 
			set
			{ 
				if (key.NotEmpty())
				{
					if (key == "X-Real-IP")
						this.peer = value;
					this.headers[key] = value; 
				}
			}
		}
		Tcp.Connection connection;
		public IByteDevice ByteDevice { get { return this.connection.ByteDevice; } }
		public IBlockDevice BlockDevice { get { return this.connection.BlockDevice; } }
		ICharacterReader reader;
		public ICharacterReader Reader
		{
			get
			{
				if (this.reader.IsNull())
					this.reader = CharacterReader.Open(this.connection.CharacterDevice);
				return this.reader;
			}
		}
		ICharacterWriter writer;
		public ICharacterWriter Writer
		{
			get
			{
				if (this.writer.IsNull())
				{
					this.writer = CharacterWriter.Open(this.connection.CharacterDevice);
					this.writer.NewLine = new char[] { '\r', '\n' };
				}
				return this.writer;
			}
		}
		Uri.Domain peer;
		public Uri.Domain Peer { get { return this.peer ?? this.connection.Peer.Host; } }
		Server(Tcp.Connection connection)
		{
			this.connection = connection;
			this.ParseRequestHeader(this.ByteDevice);
		}
		~Server()
		{
			Error.Log.Call(() => this.Close());
		}
		void IDisposable.Dispose()
		{
			this.Close();
		}
		bool ParseRequestHeader(IByteInDevice device)
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
		Generic.IEnumerable<byte> ReadLine(IByteInDevice device)
		{
			foreach (byte b in device.Read(13, 10))
				if (b != 13 && b != 10)
					yield return b;
			//byte? next = device.Peek();
			//if (next.HasValue && next.Value == 32 || next.Value == 9) // lines broken into several lines must start with space (SP) or horizontal tab (HT)
			//	foreach (byte b in this.ReadLine(device))
			//		yield return b;
		}
		public bool Respond(Status status, params KeyValue<string, string>[] headers)
		{
			return this.Respond(this.Protocol, status, headers);
		}
		public bool Respond(string protocol, Status status, params KeyValue<string, string>[] headers)
		{
			this.Writer.WriteLine(protocol + " " + status);
			foreach (var header in headers)
				this.Writer.WriteLine(header.Key + ": " + header.Value);
			this.Writer.WriteLine();
			this.Writer.Flush();
			return true;
		}
		public IBlockOutDevice RespondChuncked(Status status, string type)
		{
			this.Respond(status, 
				KeyValue.Create("Transfer-Encoding", "chunked"),
				KeyValue.Create("Content-Type", type)
			);
			return ChunkedBlockOutDevice.Wrap(this.BlockDevice);
		}
		public void SendFile(Uri.Locator file)
		{
			using (var source = IO.BlockDevice.Open(file))
				if (source.NotNull())
				{
					string type;
					switch (file.Path.Extension)
					{
						case "html":
							type = "text/html; charset=utf8";
							break;
						case "css":
							type = "text/css";
							break;
						case "mp4":
							type = "video/mp4";
							break;
						case "webm":
							type = "video/webm";
							break;
						case "png":
							type = "image/png";
							break;
						case "jpeg":
						case "jpg":
							type = "image/jpeg";
							break;
						case "svg":
							type = "image/svg+xml";
							break;
						case "gif":
							type = "image/gif";
							break;
						case "json":
							type = "application/json";
							break;
						case "js":
							type = "application/javascript";
							break;
						case "pdf":
							type = "application/pdf";
							break;
						case "xml":
							type = "application/xml";
							break;
						case "zip":
							type = "application/zip";
							break;
						default:
							type = null;
							break;
					}
					using (var destination = this.RespondChuncked(Status.OK, type))
						destination.Write(source);
				}
				else
					this.Send(Status.NotFound);
		}
		public void Send(Status status)
		{
			var message = status.AsHtml.AsBinary();
			this.Respond(status, 
				KeyValue.Create("Content-Length", message.Length.ToString()),
				KeyValue.Create("Content-Type", "text/html; charset=utf8")
			);
			this.ByteDevice.Write(message);
		}
		public bool Close()
		{
			bool result = false;
			if (this.connection.NotNull())
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

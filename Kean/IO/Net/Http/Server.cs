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
		public Request Request { get; private set; }
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
		public Uri.Domain Peer { get { return this.Request[""] ?? this.connection.Peer.Host; } }
		Server(Tcp.Connection connection)
		{
			this.connection = connection;
			this.Request = Request.Parse(this.ByteDevice);
		}
		~Server()
		{
			Error.Log.Call(() => this.Close());
		}
		void IDisposable.Dispose()
		{
			this.Close();
		}
		public bool Respond(Status status, params KeyValue<string, string>[] headers)
		{
			return this.Respond(status, (Generic.IEnumerable<KeyValue<string, string>>)headers);
		}
		public bool Respond(Status status, Generic.IEnumerable<KeyValue<string, string>> headers)
		{
			return this.Respond(this.Request.Protocol, status, headers);
		}
		public bool Respond(string protocol, Status status, params KeyValue<string, string>[] headers)
		{
			return this.Respond(protocol, status, (Generic.IEnumerable<KeyValue<string, string>>)headers);
		}
		public bool Respond(string protocol, Status status, Generic.IEnumerable<KeyValue<string, string>> headers)
		{
			this.Writer.WriteLine(protocol + " " + status);
			foreach (var header in headers)
				this.Writer.WriteLine(header.Key + ": " + header.Value);
			this.Writer.WriteLine();
			this.Writer.Flush();
			return true;
		}
		public IBlockOutDevice RespondChuncked(Status status, string type, params KeyValue<string, string>[] headers)
		{
			return this.RespondChuncked(status, type, (Generic.IEnumerable<KeyValue<string, string>>)headers);
		}
		public IBlockOutDevice RespondChuncked(Status status, string type, Generic.IEnumerable<KeyValue<string, string>> headers)
		{
			IBlockOutDevice result = null;
			if (this.Respond(status, headers.Prepend(KeyValue.Create("Transfer-Encoding", "chunked"), KeyValue.Create("Content-Type", type))))
				result = ChunkedBlockOutDevice.Wrap(this.BlockDevice);
			return result;
		}
		public Status SendFile(Uri.Locator file, params KeyValue<string, string>[] headers)
		{
			return this.SendFile(file, (Generic.IEnumerable<KeyValue<string, string>>)headers);
		}
		public Status SendFile(Uri.Locator file, Generic.IEnumerable<KeyValue<string, string>> headers)
		{
			Status result;
			if (this.Request.Path.Folder)
				file += "index.html";
			if (System.IO.Directory.Exists(file.Path.PlatformPath))
			{
				this.MovedPermanently(this.Request.Url + "/");
				result = Http.Status.MovedPermanently;
			}
			else
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
						using (var destination = this.RespondChuncked(result = Status.OK, type, headers))
							destination.Write(source);
					}
					else
						this.Send(result = Status.NotFound);
			}
			return result;
		}
		public bool Send(Status status, params KeyValue<string, string>[] headers)
		{
			return this.Send(status, (Generic.IEnumerable<KeyValue<string, string>>)headers);
		}
		public bool Send(Status status, Generic.IEnumerable<KeyValue<string, string>> headers)
		{
			var message = status.AsHtml.AsBinary();
			return this.Respond(status, headers.Prepend(
				KeyValue.Create("Content-Length", message.Length.ToString()),
				KeyValue.Create("Content-Type", "text/html; charset=utf8"))
			) && this.ByteDevice.Write(message);
		}
		public bool MovedPermanently(Uri.Locator location)
		{
			return this.Send(Status.MovedPermanently, KeyValue.Create("Location", (string)location));
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

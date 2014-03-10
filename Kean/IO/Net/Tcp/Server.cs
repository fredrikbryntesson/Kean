﻿// 
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
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Parallel = Kean.Parallel;
using Uri = Kean.Uri;
using Error = Kean.Error;

namespace Kean.IO.Net.Tcp
{
	public class Server :
		Synchronized,
		IDisposable
	{
		Collection.IList<Tcp.Connection> activeConnections = new Collection.Synchronized.List<Connection>();
		public Action<Tcp.Connection> Connected { get; set; }
		public Uri.Endpoint Endpoint { get; private set; }
		public bool Running
		{
			get { return this.listener.NotNull() && this.listener.Running; }
		}
		Parallel.RepeatThread listener;
		System.Net.Sockets.TcpListener tcpListener; 
		public Parallel.ThreadPool ThreadPool { get; private set; }
		#region Constructors
		public Server() : this("TcpServer") { }
		public Server(string name) :
			this(new Kean.Parallel.ThreadPool(name, 1) { MaximumThreadCount = -1 })
		{ }
		public Server(Action<Connection> connected) :
			this()
		{
			this.Connected = connected;
		}
		public Server(Parallel.ThreadPool threadPool)
		{
			this.ThreadPool = threadPool;
		}
		public Server(Action<IByteDevice> connected, Uri.Endpoint endPoint) :
			this(connected)
		{
			this.Start(endPoint);
		}
		public Server(Action<IByteDevice> connected, uint port) :
			this(connected)
		{
			this.Start(port);
		}
		~Server()
		{
			Error.Log.Wrap((Action)this.Dispose)();
		}
		#endregion
		public bool Start(uint port)
		{
			return this.Start(new Uri.Endpoint("", port));
		}
		void OnConnect(Tcp.Connection connection)
		{
			this.activeConnections.Add(connection);
			connection.Closed += () => this.activeConnections.Remove(c => connection == c);
			this.Connected(connection);
			if (connection.AutoClose)
				connection.Close();
		}
		public bool Start(Uri.Endpoint endPoint)
		{
			bool result;
			if (result = (this.tcpListener = this.Connect(endPoint)).NotNull())
			{
				this.Endpoint = endPoint;
				this.tcpListener.Start();
				this.listener = Parallel.RepeatThread.Start("TcpServer", () => 
				{
					try
					{
						this.ThreadPool.Enqueue(this.OnConnect, Connection.Connect(this.tcpListener.AcceptTcpClient()));
					}
					catch (System.Net.Sockets.SocketException)
					{
						if (this.listener.NotNull())
							this.listener.Abort();
					}
				});
			}
			return result;
		}
		System.Net.Sockets.TcpListener Connect(Uri.Endpoint endPoint)
		{
			System.Net.Sockets.TcpListener result = null;
			if (endPoint.Port.HasValue)
			{
				string host = endPoint.Host;
				if (host.NotEmpty())
				{
					System.Net.IPAddress address;
					if (System.Net.IPAddress.TryParse(host, out address))
						result = new System.Net.Sockets.TcpListener(address, (int)endPoint.Port.Value);
				}
				else
					result = new System.Net.Sockets.TcpListener((int)endPoint.Port.Value);
			}
			return result;
		}
		public bool Stop()
		{
			this.tcpListener.Stop();
			return this.listener.Stop();
		}
		#region IDisposable Members
		public void Dispose()
		{
			if (this.listener.NotNull())
				this.Stop();
			if (this.ThreadPool.NotNull())
			{
				this.ThreadPool.Dispose();
				this.ThreadPool = null;
			}
			if (this.listener.NotNull())
			{
				this.listener.Abort();
				this.listener.Dispose();
				this.listener = null;
				this.tcpListener = null;
			}
			if (this.activeConnections.NotNull())
			{
				this.activeConnections.Apply(c => c.Close());
				this.activeConnections = null;
			}
		}
		#endregion
	}
}

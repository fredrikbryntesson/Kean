// 
//  Connection.cs
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
using Kean.Extension;
using Text = System.Text;
using Uri = Kean.Uri;

namespace Kean.IO.Net.Tcp
{
	public class Connection :
		ByteDevice
	{
		System.Net.Sockets.TcpClient client;
		public override bool Opened { get { return this.client.NotNull() && base.Opened; } }
		#region Constructors
		Connection(System.Net.Sockets.TcpClient client) :
			this(client.GetStream())
		{
			this.client = client;
			this.client.NoDelay = true;
		}
		Connection(System.Net.Sockets.NetworkStream stream) :
			base(stream)
		{ 
		}
		#endregion
		public override bool Close()
		{
			bool result = base.Close();
			if (this.client.NotNull())
			{
				this.client.Close();
				this.client = null;
			}
			return result;
		}
		#region Static Creators
		internal static Connection Connect(System.Net.Sockets.TcpClient client)
		{
			return new Connection(client);
		}
		public static Connection Connect(Uri.Endpoint endPoint)
		{
			return endPoint.Port.HasValue ? new Connection(new System.Net.Sockets.TcpClient(endPoint.Host, (int)endPoint.Port.Value)) : null;
		}
		#endregion
	}
}

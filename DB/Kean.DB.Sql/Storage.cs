// 
//  Storage.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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
//  You should have received data copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Kean.Core;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Uri = Kean.Core.Uri;
using Serialize = Kean.Core.Serialize;
using Data = System.Data;

namespace Kean.DB.Sql
{
	public class Storage :
		DB.Storage,
		IDisposable
	{
		Data.IDbConnection connection;
		Collection.IDictionary<string, Table> tables = new Collection.Dictionary<string, Table>();

		public Uri.Locator Locator { get; private set; }

		Storage(Uri.Locator resource, Data.IDbConnection connection)
		{
			this.Locator = resource;
			this.connection = connection;
		}

		public bool Add (params Table[] tables)
		{
			return tables.Fold((table, result) => result && this.Add(table), true);
		}

		public bool Add (Table table)
		{
			bool result;
			if (result = table.NotNull() && table.Open(this.connection))
				this.tables[table.Name] = table;
			return result;
		}

		public bool Create (Table table)
		{
			bool result;
			if (result = table.NotNull() && table.Open(this.connection) && table.Create())
				this.tables[table.Name] = table;
			return result;
		}
		#region implemented abstract members of Kean.Core.Serialize.Storage
		protected override bool Store (Serialize.Data.Node value, Uri.Locator locator)
		{
			// update: mysql://user@password:host/database/table/primaryKey
			// insert: mysql://user@password:host/database/table
			bool result;
			locator = locator.Resolve(this.Locator);
			switch (locator.Path.Count)
			{
				case 2:
					{
						Table table = this.tables[locator.Path[1]];
						result = table.NotNull() && table.Insert(value);
					}
					break;
				case 3:
					{
						Table table = this.tables[locator.Path[1]];
						result = table.NotNull() && table.Update(locator.Path[2], value);
					}
					break;
				default:
					result = false;
					break;
			}
			return result;
		}

		protected override Serialize.Data.Node Load (Uri.Locator locator)
		{
			// select: mysql://user@password:host/database/table/primaryKey
			// select: mysql://user@password:host/database/table
			// select: mysql://user@password:host/database/table?query=number=42
			Serialize.Data.Node result;
			locator = locator.Resolve(this.Locator);
			switch (locator.Path.Count)
			{
				case 2:
					{
						Table table = this.tables[locator.Path[1]];
						result = table.NotNull() ? table.Select() : null;
					}
					break;
				case 3:
					{
						Table table = this.tables[locator.Path[1]];
						result = table.NotNull() ? table.Select(locator.Path[2]) : null;
					}
					break;
				default:
					result = null;
					break;
			}
			return result;
		}
		#endregion
		public bool Close ()
		{
			bool result;
			if (result = this.connection.NotNull())
			{
				this.connection.Close();
				this.connection = null;
			}
			return result;
		}
		#region IDisposable implementation
		~Storage ()
		{
			(this as IDisposable).Dispose();
		}

		void IDisposable.Dispose ()
		{
			this.Close();
		}
		#endregion
		#region Static Open
		public static Storage Open (Uri.Locator locator)
		{
			string connectionString = null;
			connectionString =
				"Server=" + locator.Authority.Endpoint +
				";Database=" + locator.Path[0] +
				";User ID=" + locator.Authority.User.Name +
				";Password=" + locator.Authority.User.Password;
			Console.WriteLine(connectionString);
			Data.IDbConnection connection = null;
			switch (locator.Scheme)
			{
				case "mysql":
					connection = new global::MySql.Data.MySqlClient.MySqlConnection(connectionString);
					break;
			}
			connection.Open();
			Console.WriteLine(connection.State);
			return connection.NotNull() ? new Storage(locator, connection) : null;
		}
		#endregion
	}
}


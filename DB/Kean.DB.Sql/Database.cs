// 
//  Database.cs
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
using Reflect = Kean.Core.Reflect;
namespace Kean.DB.Sql
{
    public class Database :
		DB.Database
    {
        Data.IDbConnection connection;
        Database(Uri.Locator locator, Data.IDbConnection connection) :
			base(locator, null)
        {
            this.connection = connection;
        }
        public override bool Close()
        {
            bool result;
            if (result = this.connection.NotNull())
            {
                this.connection.Close();
                this.connection = null;
            }
            return result;
        }

        #region implemented abstract members of Database

        protected override DB.Table NewTable()
        {
            return new Table(this.connection);
        }

        #endregion

        #region Static Open

        public static Database Open(Uri.Locator locator)
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
            return connection.NotNull() ? new Database(locator, connection) : null;
        }

        #endregion

    }
}


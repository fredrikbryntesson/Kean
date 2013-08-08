// 
//  Table.cs
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
using Kean.Core.Collection.Extension;
using Uri = Kean.Core.Uri;
using Serialize = Kean.Core.Serialize;
using Reflect = Kean.Core.Reflect;
using Kean.Core.Reflect.Extension;
using Data = System.Data;
using IO = Kean.IO;

namespace Kean.DB.Sql
{
	public class Table :
		DB.Table
	{
		Data.IDbConnection connection;

		internal Table(Data.IDbConnection connection, string name, Reflect.Type type, KeyValue<string, Reflect.Type> key, KeyValue<string, Reflect.Type>[] indexFields, KeyValue<string, Reflect.Type>[] nonIndexFields, KeyValue<string, Reflect.Type>[] fields) :
			base(name, type, key, indexFields, nonIndexFields, fields)
		{
			this.connection = connection;
		}

		string SqlType (KeyValue<string, Reflect.Type> field)
		{
			string result = "`" + field.Key + "` ";
			switch (field.Value)
			{
				case "bool":
					result += "boolean";
					break;
				case "byte":
					result += "tinyint UNSIGNED";
					break;
				case "sbyte":
					result += "tinyint";
					break;
				case "byte[]":
					result += "binary";
					break;
				case "System.DateTime":
					result += "datetime";
					break;
				case "decimal":
					result += "decimal";
					break;
				case "double":
					result += "double";
					break;
				case "System.Guid":
					result += "char(36)";
					break;
				case "short":
					result += "smallint";
					break;
				case "ushort":
					result += "smallint UNSIGNED";
					break;
				case "int":
					result += "int";
					break;
				case "uint":
					result += "int UNSIGNED";
					break;
				case "long":
					result += "bigint(20)";
					break;
				case "ulong":
					result += "bigint UNSIGNED";
					break;
				case "float":
					result += "float";
					break;
				case "string":
					result += "text";
					break;
				case "System.TimeSpan":
					result += "time";
					break;

			}
			return result;
		}

		public override bool Create ()
		{
			bool result;
			if (result = this.connection.NotNull())
			{
				IO.Text.Builder query = new IO.Text.Builder("CREATE TABLE ");
				query += "`" + this.Name + "` (" + this.SqlType(this.key) + " NOT NULL, ";
				foreach (var field in this.indexFields)
					query += this.SqlType(field) + ", ";
				query += "`_type` varchar(255) DEFAULT '" + this.type + "', ";
				if (this.nonIndexFields.NotEmpty())
					query += "`_data` longtext, ";
				query += "PRIMARY KEY (`" + this.key.Key + "`),";
				query += "UNIQUE KEY `" + this.key.Key + "` (`" + this.key.Key + "`)";
				query += ") DEFAULT CHARSET=utf8";
				Console.WriteLine(query);
				using (Data.IDbCommand command = this.connection.CreateCommand())
				{
					command.CommandText = query;
					result = command.ExecuteNonQuery() == 0;
				}
			}
			return result;
		}

		#region Select

		public override System.Collections.Generic.IEnumerable<Serialize.Data.Node> Select (string where, string order, int limit, int offset)
		{
			IO.Text.Builder query = (IO.Text.Builder)"SELECT " + this.fieldString + " FROM " + this.Name;
			if (where.NotEmpty())
				query += " WHERE " + where;
			if (order.NotEmpty())
				query += " ORDER BY " + order;
			if (limit > 0)
			{
				query += " LIMIT " + limit;
				if (offset > 0)
					query += " OFFSET " + offset;
			}
			Console.WriteLine(query);
			using (Data.IDbCommand command = this.connection.CreateCommand())
			{
				command.CommandText = query;
				using (Data.IDataReader reader = command.ExecuteReader())
					while (reader.Read())
						yield return this.Read(reader);
			}
		}

		Serialize.Data.Node Read (Data.IDataReader reader)
		{
			Serialize.Data.Branch result = new Serialize.Data.Branch();
			int ordinal = 0;
			result.Nodes.Add(this.Read(reader, ordinal++, this.key));
			foreach (KeyValue<string, Reflect.Type> field in this.indexFields)
				result.Nodes.Add(this.Read(reader, ordinal++, field));
			if (this.nonIndexFields.NotEmpty())
				result.Merge(Json.Serialize.Storage.Convert((Json.Dom.Object)reader.GetString(ordinal++)));
			string type = reader.GetString(ordinal++);
			result.Type = type;
			return result;
		}

		Serialize.Data.Leaf Read (Data.IDataReader reader, int ordinal, KeyValue<string, Reflect.Type> field)
		{
			Serialize.Data.Leaf result = null;
			if (field.Value == typeof(bool))
				result = new Serialize.Data.Boolean(reader.GetBoolean(ordinal));
			else if (field.Value == typeof(byte))
				result = new Serialize.Data.Byte(reader.GetByte(ordinal));
			else if (field.Value == typeof(char))
				result = new Serialize.Data.Character(reader.GetChar(ordinal));
			else if (field.Value == typeof(DateTime))
				result = new Serialize.Data.DateTime(reader.GetDateTime(ordinal));
			else if (field.Value == typeof(decimal))
				result = new Serialize.Data.Decimal(reader.GetDecimal(ordinal));
			else if (field.Value == typeof(double))
				result = new Serialize.Data.Double(reader.GetDouble(ordinal));
			//else if (field.Value == typeof(Guid))
			//case typeof(Guid): result = new Serialize.Data.Guid(reader.GetGuid(ordinal)); break;
			else if (field.Value == typeof(short))
				result = new Serialize.Data.Short(reader.GetInt16(ordinal));
			else if (field.Value == typeof(int))
				result = new Serialize.Data.Integer(reader.GetInt32(ordinal));
			else if (field.Value == typeof(long))
				result = new Serialize.Data.Long(reader.GetInt64(ordinal));
			else if (field.Value == typeof(sbyte))
				result = new Serialize.Data.SignedByte(unchecked((sbyte)reader.GetByte(ordinal)));
			else if (field.Value == typeof(float))
				result = new Serialize.Data.Single(reader.GetFloat(ordinal));
			else if (field.Value == typeof(string))
				result = new Serialize.Data.String(reader.GetString(ordinal));
			else if (field.Value == typeof(TimeSpan))
				result = new Serialize.Data.TimeSpan(reader.GetDateTime(ordinal) - new DateTime());
			else if (field.Value == typeof(ushort))
				result = new Serialize.Data.UnsignedShort(unchecked((ushort)reader.GetInt16(ordinal)));
			else if (field.Value == typeof(uint))
				result = new Serialize.Data.UnsignedInteger(unchecked((uint)reader.GetInt32(ordinal)));
			else if (field.Value == typeof(ulong))
				result = new Serialize.Data.UnsignedLong(unchecked((ulong)reader.GetInt64(ordinal)));
			if (result.NotNull())
				result.Name = field.Key;
			return result;
		}

		#endregion

		#region Insert

		public override bool Insert (Serialize.Data.Branch data)
		{
			bool result;
			IO.Text.Builder query = "INSERT INTO ";
			Collection.IList<string> fields = new Collection.List<string>();
			Collection.IList<string> values = new Collection.List<string>();
			Json.Dom.Object nonIndexData = new Json.Dom.Object();
			foreach (Serialize.Data.Node node in data.Nodes)
			{
				if (node is Serialize.Data.Leaf)
				{
					if (node.Attribute is IndexAttribute || node.Attribute is PrimaryKeyAttribute)
					{
						fields.Add(node.Name);
						values.Add("\"" + (node as Serialize.Data.Leaf).Text + "\"");
					}
					else
						nonIndexData.Add(node.Name, Json.Serialize.Storage.Convert(node as Serialize.Data.Leaf));
				}
			}
			fields.Add("_data");
			values.Add("\"" + ((string)nonIndexData).Replace("\"", "\\\"") + "\"");
			if (data.Type.NotNull())
			{
				fields.Add("_type");
				values.Add("\"" + data.Type + "\"");
			}
			query += this.Name + "(" + fields.Join(", ") + ") VALUES(" + values.Join(", ") + ")";
			Console.WriteLine(query);
			using (Data.IDbCommand command = this.connection.CreateCommand())
			{
				command.CommandText = query;
				result = command.ExecuteNonQuery() > 0;
			}
			return result;
		}

		#endregion

		#region Update

		public override bool Update (string key, Serialize.Data.Node data)
		{
			return false;
		}

		#endregion

		#region Object overrides

		public override string ToString ()
		{
			return string.Format("[Table: Name={0}]", this.Name);
		}

		#endregion

	}
}


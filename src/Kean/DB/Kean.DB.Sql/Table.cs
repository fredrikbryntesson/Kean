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

namespace Kean.DB.Sql
{
	public class Table
	{
		Data.IDbConnection connection;
		string name;
		KeyValue<string, Reflect.Type> key;
		KeyValue<string, Reflect.Type>[] indexFields;
		KeyValue<string, Reflect.Type>[] nonIndexFields;
		KeyValue<string, Reflect.Type>[] fields;
		string fieldString;

		Table(string name, KeyValue<string, Reflect.Type> key, KeyValue<string, Reflect.Type>[] indexFields, KeyValue<string, Reflect.Type>[] nonIndexFields, KeyValue<string, Reflect.Type>[] fields)
		{
			this.name = name;
			this.key = key;
			this.indexFields = indexFields;
			this.nonIndexFields = nonIndexFields;
			this.fields = fields;
			this.fieldString = fields.Fold((f, s) => s.NotNull() ? s + ", " + f.Key : f.Key, (string)null);
		}
		internal void Open(Data.IDbConnection connection)
		{
			this.connection = connection;
		}
		public Serialize.Data.Node Select(string key)
		{
			if (this.key.Value == typeof(string))
				key = "'" + key + "'";
			Serialize.Data.Node[] result = this.Select(this.key.Key + " = " + key , null, 0, 0);
			return result.NotEmpty() ? result[0] : null;
		}
		public Serialize.Data.Node[] Select(string where, string orderBy, int limit, int offset)
		{
			string query = "SELECT " + this.fieldString + " FROM " + this.name;
			if (limit > 0)
				query = " LIMIT " + limit;
			if (offset > 0)
				query = " OFFSET " + limit;
			if (where.NotEmpty())
				query = " WHERE " + where;
			if (orderBy.NotEmpty())
				query = " ORDER BY " + orderBy;
			Data.IDbCommand command = this.connection.CreateCommand();
			command.CommandText = query;
			Data.IDataReader reader = command.ExecuteReader();
			Collection.IList<Serialize.Data.Node> result = new Collection.Linked.List<Serialize.Data.Node>();
			while (reader.Read())
				result.Add(this.Read(reader));
			reader.Close();
			command.Dispose();
			return result.ToArray();
		}
		Serialize.Data.Node Read(Data.IDataReader reader)
		{
			Serialize.Data.Branch result = new Serialize.Data.Branch();
			int ordinal = 0;
			result.Nodes.Add(this.Read(reader, ordinal++, this.key));
			foreach (KeyValue<string, Reflect.Type> field in this.indexFields)
				result.Nodes.Add(this.Read(reader, ordinal++, field));
			//result = new Serialize.Data.UnsignedLong(unchecked((ulong)reader.GetInt64(ordinal)));
			return result;
		}
		Serialize.Data.Leaf Read(Data.IDataReader reader, int ordinal, KeyValue<string, Reflect.Type> field)
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
		public static Table New<T>(string name)
		{
			return Table.New(name, typeof(T));
		}
		public static Table New(string name, Reflect.Type type)
		{
			KeyValue<string, Reflect.Type> key = null;
			Collection.List<KeyValue<string, Reflect.Type>> fields = new Collection.List<KeyValue<string, Reflect.Type>>();
			Collection.List<KeyValue<string, Reflect.Type>> indexFields = new Collection.List<KeyValue<string, Reflect.Type>>();
			Collection.List<KeyValue<string, Reflect.Type>> nonIndexFields = new Collection.List<KeyValue<string, Reflect.Type>>();
			switch (type.Category)
			{
			case Reflect.TypeCategory.Class:
				foreach (Reflect.PropertyInformation property in type.Properties)
				{
					Serialize.ParameterAttribute[] attributes = property.GetAttributes<Serialize.ParameterAttribute>();
					if (attributes.Length == 1)
					{
						KeyValue<string, Reflect.Type> f = KeyValue.Create(attributes[0].Name ?? property.Name, property.Type);
						if (attributes[1] is PrimaryKeyAttribute)
							key = f;
						else if (attributes[1] is IndexAttribute)
							indexFields.Add(f);
						else
							nonIndexFields.Add(f);
						fields.Add(f);
					}
				}
				break;
			case Reflect.TypeCategory.Structure:
				foreach (Reflect.FieldInformation field in type.Fields)
				{
					Serialize.ParameterAttribute[] attributes = field.GetAttributes<Serialize.ParameterAttribute>();
					if (attributes.Length == 1)
					{
						KeyValue<string, Reflect.Type> f = KeyValue.Create(attributes[0].Name ?? field.Name, field.Type);
						if (attributes[1] is PrimaryKeyAttribute)
							key = f;
						else if (attributes[1] is IndexAttribute)
							indexFields.Add(f);
						else
							nonIndexFields.Add(f);
						fields.Add(f);
					}
				}
				break;
			}
			return new Table(name, key, indexFields.ToArray(),  nonIndexFields.ToArray(), fields.ToArray());
		}
	}
}


// 
//  Database.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
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
using Generic = System.Collections.Generic;
using Reflect = Kean.Core.Reflect;
namespace Kean.DB
{
    public abstract class Database :
		IDisposable
    {
        Collection.IDictionary<string, Table> tables = new Collection.Dictionary<string, Table>();
        Serialize.Resolver resolver;
        Serialize.ISerializer serializer;
        Serialize.IRebuilder rebuilder;
        internal Table this [string table]
        { 
            get { return this.tables[table]; }
        }
        public Uri.Locator Locator { get; private set; }
        protected Database(Uri.Locator locator, Serialize.Resolver resolver, Serialize.IRebuilder rebuilder, params Serialize.ISerializer[] serializers)
        {
            this.Locator = locator;
            this.resolver = resolver ?? new Serialize.Resolver();
            this.rebuilder = rebuilder ?? new Serialize.Rebuilder.Identity();
            this.serializer = new Serialize.Serializer.Cache(serializers.NotEmpty() ? new Serialize.Serializer.Group(serializers) : new Serialize.Serializer.Default());
        }
        public T Load<T>(string table, long key)
        {
            return default(T);
        }
        public bool Store<T>(string table, T item)
        {
            return false;
        }

        #region Add & Create Table

        bool Add(params Table[] tables)
        {
            return tables.Fold((table, result) => result && this.Add(table), true);
        }
        bool Add(Table table)
        {
            bool result;
            if (result = table.NotNull())
                this.tables[table.Name] = table;
            return result;
        }
        bool Create(Table table)
        {
            return table.NotNull() && table.Create() && this.Add(table);
        }
        public bool AddTable<T>(string name)
        {
            return this.AddTable(name, typeof(T));
        }
        public bool AddTable(string name, Reflect.Type type)
        {
            return this.Add(this.NewTable(name, type));
        }
        public bool CreateTable<T>(string name)
        {
            return this.CreateTable(name, typeof(T));
        }
        public bool CreateTable(string name, Reflect.Type type)
        {
            return this.Create(this.NewTable(name, type));
        }
        Table NewTable(string name, Reflect.Type type)
        {
            KeyValue<string, Reflect.Type>? key = null;
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
                            if (attributes[0] is PrimaryKeyAttribute)
                                key = f;
                            else if (attributes[0] is IndexAttribute)
                                indexFields.Add(f);
                            else
                                nonIndexFields.Add(f);
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
                        }
                    }
                    break;
            }
            Table result = this.NewTable();
            if (result.NotNull())
            {
                result.Name = name;
                result.Type = type;
                if (key.HasValue)
                    result.Key = key.Value;
                result.IndexFields = indexFields.ToArray();
                result.NonIndexFields = nonIndexFields.ToArray();
            }
            return result;
        }
        protected abstract Table NewTable();

        #endregion

        public virtual bool Close()
        {
            bool result;
            if (result = this.tables.NotNull())
            {
                this.tables.Apply(table => table.Value.Close());
                this.tables = null;
            }
            return result;
        }

        #region IDisposable implementation

        ~Database ()
        {
            (this as IDisposable).Dispose();
        }
        void IDisposable.Dispose()
        {
            this.Close();
        }

        #endregion

    }
}


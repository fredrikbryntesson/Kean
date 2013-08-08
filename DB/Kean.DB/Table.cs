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
using IO = Kean.IO;

namespace Kean.DB
{
	public abstract class Table :
		IDisposable
	{
		public string Name { get; private set; }

		protected Reflect.Type type;
		protected KeyValue<string, Reflect.Type> key;
		protected KeyValue<string, Reflect.Type>[] indexFields;
		protected KeyValue<string, Reflect.Type>[] nonIndexFields;
		protected KeyValue<string, Reflect.Type>[] fields;
		protected string fieldString;

		protected Table(string name, Reflect.Type type, KeyValue<string, Reflect.Type> key, KeyValue<string, Reflect.Type>[] indexFields, KeyValue<string, Reflect.Type>[] nonIndexFields, KeyValue<string, Reflect.Type>[] fields)
		{
			this.Name = name;
			this.type = type;
			this.key = key;
			this.indexFields = indexFields;
			this.nonIndexFields = nonIndexFields;
			this.fields = fields;
			this.fieldString = this.indexFields.Fold((f, s) => s + ", " + f.Key, (IO.Text.Builder)this.key.Key) + (this.nonIndexFields.NotEmpty() ? ", _data" : "") + ", _type";
		}

		#region Select

		public System.Collections.Generic.IEnumerable<Serialize.Data.Node> Select ()
		{
			return this.Select(null, null, 0, 0);
		}

		public System.Collections.Generic.IEnumerable<Serialize.Data.Node> Select (string key)
		{
			if (this.key.Value == typeof(string))
				key = "'" + key + "'";
			return this.Select(this.key.Key + " = " + key, null, 0, 0);
		}

		public abstract System.Collections.Generic.IEnumerable<Serialize.Data.Node> Select (string where, string order, int limit, int offset);

		#endregion

		#region Insert

		public abstract bool Insert (Serialize.Data.Branch data);

		#endregion

		#region Update

		public abstract bool Update (string key, Serialize.Data.Node data);

		#endregion

		#region Create

		public abstract bool Create ();

		#endregion

		#region Object overrides

		public override string ToString ()
		{
			return string.Format("[Table: Name={0}]", this.Name);
		}

		#endregion

		#region IDisposable implementation

		public virtual bool Close ()
		{
			return true;
		}

		~Table ()
		{
			this.Close();
		}

		void IDisposable.Dispose ()
		{
			this.Close();
		}

		#endregion

	}
}


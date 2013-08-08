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
		public string Name { get; internal set; }

		internal protected Reflect.Type Type { get; internal set; }

		internal protected KeyValue<string, Reflect.Type> Key { get; internal set; }

		internal protected KeyValue<string, Reflect.Type>[] IndexFields { get; internal set; }

		internal protected KeyValue<string, Reflect.Type>[] NonIndexFields { get; internal set; }

		protected Table()
		{
		}

		#region Select

		internal protected System.Collections.Generic.IEnumerable<Serialize.Data.Node> Select (string key)
		{
			if (this.Key.Value == typeof(string))
				key = "'" + key + "'";
			return this.Select(this.Key.Key + " = " + key, null, 0, 0);
		}

		internal protected abstract System.Collections.Generic.IEnumerable<Serialize.Data.Node> Select (string where, string order, int limit, int offset);

		#endregion

		#region Insert

		internal protected abstract bool Insert (Serialize.Data.Branch data);

		#endregion

		#region Update

		internal protected abstract bool Update (string key, Serialize.Data.Node data);

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


using System;
using Kean.Core;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Uri = Kean.Core.Uri;
using Serialize = Kean.Core.Serialize;
using Data = System.Data;

namespace Kean.DB.Sql
{
	public abstract class Storage :
		DB.Storage,
		IDisposable
	{
		Data.IDbConnection connection;
		protected abstract Uri.Scheme Scheme { get; }
		public virtual Uri.Locator Resource 
		{
			get 
			{
				Collection.IReadOnlyDictionary<string, string> properties = new Collection.ReadOnlyDictionary<string, string>( this.connection.ConnectionString.Split(';').Map(property => 
				{
					string[] splitted = property.Split(new char[] { ';' }, 2);
					return KeyValue.Create(splitted.NotEmpty() ? splitted[0] : null, splitted.NotNull() && splitted.Length > 1 ? splitted[1] : null);
				}));
				return new Uri.Locator(this.Scheme, new Uri.Authority(new Uri.User(properties["User ID"], properties["Password"]), properties["Server"]), properties["Database"]);
			}
			set
			{
				this.connection.ConnectionString =
					"Server=" + value.Authority.Endpoint +
					";Database=" + value.Path.Last +
					";User ID=" + value.Authority.User.Name +
					";Password=" + value.Authority.User.Password;
			}
		}

		protected Storage (Data.IDbConnection connection)
		{
			this.connection = connection;
			connection.Open();
		}
		#region implemented abstract members of Kean.Core.Serialize.Storage
		protected override bool Store (Serialize.Data.Node value, Uri.Locator locator)
		{
			// update: mysql://user@password:localhost/database/table/primaryKey
			// insert: mysql://user@password:localhost/database/table
			return false;
		}

		protected override Serialize.Data.Node Load (Uri.Locator locator)
		{
			// select: mysql://user@password:localhost/database/table/primaryKey
			// select: mysql://user@password:localhost/database/table
			// select: mysql://user@password:localhost/database/table?query=number=42
			Serialize.Data.Node result = null;
			return result;
		}
		#endregion
		public bool Close()
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

	}
}


using System;
using Kean.Core;
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;
using Serialize = Kean.Core.Serialize;

namespace Kean.DB
{
	public class Storage :
		Serialize.Storage
	{
		Database database;

		public Storage(Database database) :
			base(null, null, null)
		{
			this.database = database;
		}

		#region implemented abstract members of Kean.Core.Serialize.Storage

		protected override bool Store (Serialize.Data.Node value, Uri.Locator locator)
		{
			// update: mysql://user@password:host/database/table/primaryKey
			// insert: mysql://user@password:host/database/table
			bool result;
			if (result = value is Serialize.Data.Branch)
			{
				locator = locator.Resolve(this.database.Locator);
				switch (locator.Path.Count)
				{
					case 2:
						{
							Table table = this.database[locator.Path[1]];
							result = table.NotNull() && table.Insert(value as Serialize.Data.Branch);
						}
						break;
					case 3:
						{
							Table table = this.database[locator.Path[1]];
							result = table.NotNull() && table.Update(locator.Path[2], value as Serialize.Data.Branch);
						}
						break;
					default:
						result = false;
						break;
				}
			}
			return result;
		}

		protected override Serialize.Data.Node Load (Uri.Locator locator)
		{
			// select: mysql://user@password:host/database/table/primaryKey
			// select: mysql://user@password:host/database/table
			// select: mysql://user@password:host/database/table?where=number=42
			Serialize.Data.Node result;
			locator = locator.Resolve(this.database.Locator);
			switch (locator.Path.Count)
			{
				case 2:
					{
						Table table = this.database[locator.Path[1]];
						result = table.NotNull() ? new Serialize.Data.Collection(table.Select(locator.Query["where"], locator.Query["order"], locator.Query.Get("limit", 0), locator.Query.Get("offset", 0))) : null;
					}
					break;
				case 3:
					{
						Table table = this.database[locator.Path[1]];
						result = table.NotNull() ? table.Select(locator.Path[2]).First() : null;
					}
					break;
				default:
					result = null;
					break;
			}
			return result;
		}

		#endregion

	}
}


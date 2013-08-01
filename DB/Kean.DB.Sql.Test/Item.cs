using System;

namespace Kean.DB.Sql.Test
{
	public class Item
	{
		[DB.PrimaryKey]
		public long Identifier { get; set; }

		[DB.Index]
		public string Name { get; set; }

		[DB.Parameter]
		public string Description { get; set; }

		public Item()
		{
		}
	}
}


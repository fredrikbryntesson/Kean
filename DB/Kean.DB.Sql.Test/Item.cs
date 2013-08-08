using System;

namespace Kean.DB.Sql.Test
{
	public class Item
	{
		[DB.PrimaryKey]
		public long Identifier { get; set; }

		[DB.Index]
		public string Name { get; set; }

		[DB.Data]
		public string Description { get; set; }

		public Item()
		{
		}

		public override string ToString ()
		{
			return string.Format("[Item: Identifier={0}, Name={1}, Description={2}]", this.Identifier, this.Name, this.Description);
		}
	}
}


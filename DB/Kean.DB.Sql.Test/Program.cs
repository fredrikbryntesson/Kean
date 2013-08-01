using System;

namespace Kean.DB.Sql.Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine("Hello World!");
			using (Storage storage = Storage.Open("mysql://kean:password@localhost/keanTest"))
			{
				//storage.Create(Table.New<Item>("items"));
				storage.Store(new Item() { Name = "Test Name", Description = "This is a description." }, "./items");
			}
		}
	}
}

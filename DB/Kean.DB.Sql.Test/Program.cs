using System;

namespace Kean.DB.Sql.Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine("Hello World!");
			using (Database database = Database.Open("mysql://kean:password@localhost/keanTest/"))
			{
				//database.CreateTable<Item>("items");
				database.AddTable<Item>("items");
				Storage storage = new Storage(database);
//				storage.Store(new Item() { Identifier = 0, Name = "Test Name A", Description = "This is a description of A." }, "./items");
//				storage.Store(new Item() { Identifier = 1, Name = "Test Name B", Description = "This is a description of B." }, "./items");
//				storage.Store(new Item() { Identifier = 2, Name = "Test Name C", Description = "This is a description of C." }, "./items");
//				storage.Store(new Item() { Identifier = 3, Name = "Test Name D", Description = "This is a description of D." }, "./items");
//				storage.Store(new Item() { Identifier = 4, Name = "Test Name E", Description = "This is a description of E." }, "./items");
//				storage.Store(new Item() { Identifier = 5, Name = "Test Name F", Description = "This is a description of F." }, "./items");
				Item item0 = storage.Load<Item>("./items/0");
				Console.WriteLine(item0);
				Item[] items = storage.Load<Item[]>("./items?where=Identifier!=4;order=Identifier DESC;limit=3;offset=2");
				foreach (Item item in items)
					Console.WriteLine(item);
			}
		}
	}
}

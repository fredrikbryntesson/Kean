using System;

namespace Kean.DB.Sql.Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine("Hello World!");
			using (Storage storage = Storage.Open("mysql://kean:password@localhost/keanTest/"))
			{
				//storage.Create(Table.New<Item>("items"));
				storage.Add(Table.New<Item>("items"));
//				storage.Store(new Item() { Identifier = 0, Name = "Test Name A", Description = "This is a description of A." }, "./items");
//				storage.Store(new Item() { Identifier = 1, Name = "Test Name B", Description = "This is a description of B." }, "./items");
//				storage.Store(new Item() { Identifier = 2, Name = "Test Name C", Description = "This is a description of C." }, "./items");
//				storage.Store(new Item() { Identifier = 3, Name = "Test Name D", Description = "This is a description of D." }, "./items");
//				storage.Store(new Item() { Identifier = 4, Name = "Test Name E", Description = "This is a description of E." }, "./items");
//				storage.Store(new Item() { Identifier = 5, Name = "Test Name F", Description = "This is a description of F." }, "./items");
				Item item0 = storage.Load<Item>("./items/0");
				Console.WriteLine(item0);
				Item[] items = storage.Load<Item[]>("./items");
				foreach (Item item in items)
					Console.WriteLine(item);
			}
		}
	}
}

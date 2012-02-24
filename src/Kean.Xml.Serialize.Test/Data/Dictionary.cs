// 
//  Dictionary.cs
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
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Kean.Core;
using Collection = Kean.Core.Collection;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Xml.Serialize.Test.Data
{
	public class Dictionary :
		IData
	{
		string[] keys = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
		[Core.Serialize.Parameter]
		public Collection.Dictionary<int, Structure> Structures { get; set; }
		[Core.Serialize.Parameter]
        public Collection.Dictionary<int, Class> Classes { get; set; }
		[Core.Serialize.Parameter]
        public Collection.Dictionary<string, object> Objects { get; set; }
		[Core.Serialize.Parameter(Name = "Number")]
        public Collection.Dictionary<string, int> Numbers { get; set; }
		[Core.Serialize.Parameter]
        public Collection.Dictionary<string, int> Empty { get; set; }

		#region IData
		public  void Initilize(IFactory factory)
		{
			this.Structures = new Collection.Dictionary<int, Structure>(Core.KeyValue.Create(0, factory.Create<Structure>()), Core.KeyValue.Create(1, factory.Create<Structure>()));
			this.Classes = new Collection.Dictionary<int, Class>(Core.KeyValue.Create(0, (Class)factory.Create<ComplexClass>()), Core.KeyValue.Create(1, factory.Create<Class>()));
			this.Objects = new Collection.Dictionary<string, object>(Core.KeyValue.Create("complex", (object)factory.Create<ComplexClass>()), Core.KeyValue.Create("class", (object)factory.Create<Class>()), Core.KeyValue.Create("boolean", (object)factory.Create<bool>()), Core.KeyValue.Create("time", (object)factory.Create<DateTime>()));
			this.Numbers = new Collection.Dictionary<string, int>();
			for (int i = 0; i < 10; i++)
				this.Numbers[this.keys[i]] = i;
			this.Empty = new Collection.Dictionary<string, int>();
		}
		public void Verify(IFactory factory, string message, params object[] arguments)
		{
			factory.Verify(this.Structures[0], message, arguments);
			factory.Verify(this.Structures[1], message, arguments);

			factory.Verify(this.Classes[0] as ComplexClass, message, arguments);
			factory.Verify(this.Classes[1], message, arguments);

			factory.Verify(this.Objects["complex"] as ComplexClass, message, arguments);
			factory.Verify(this.Objects["class"] as Class, message, arguments);
			factory.Verify((bool)this.Objects["boolean"], message, arguments);
			factory.Verify((DateTime)this.Objects["time"], message, arguments);

			for (int i = 0; i < 10; i++)
				factory.Verify(this.Numbers[this.keys[i]], Is.EqualTo(i), message, arguments);

			factory.Verify(this.Empty, Is.Null, message, arguments);
		}
		#endregion
	}
}

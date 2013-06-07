// 
//  CollectionTypes.cs
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
using NUnit.Framework;
using Kean.Core.Extension;

namespace Kean.Xml.Serialize.Test
{
	public class CollectionTypes :
		Factory<CollectionTypes>
	{
		protected override void Run()
		{
			this.Run(
				this.Dictionary,
				this.List,
				this.ListExisting,
				this.ListInterface,
				this.Array
				);
		}

		[Test]
		public void Array() { this.Test(typeof(Data.Array)); }
        [Test]
        public void List() { this.Test(typeof(Data.List)); }
		[Test]
		public void ListExisting() { this.Test(typeof(Data.ListExisting)); }
		[Test]
		public void ListInterface() { this.Test(typeof(Data.ListInterface)); }
		[Test]
		public void Dictionary() { this.Test(typeof(Data.Dictionary)); }
	}
}

// 
//  Named.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
using Uri = Kean.Core.Uri;
using Reflect = Kean.Core.Reflect;

namespace Kean.Xml.Serialize.Test
{
	public class Named :
		Factory<Named>
	{
		protected override void Test(Reflect.Type type)
		{
			Uri.Locator filename = this.Filename(type);
			Uri.Locator resource = this.ResourceName(type);
			this.Storage.Store(this.Create(type), filename, "Name");
			Named.VerifyAsResource(filename.Path.PlattformPath, resource.Path, "Serializing test \"{0}\" failed.", this.Name(type));
			this.Verify(this.Storage.Load<object>(resource), "Deserialization text \"{0}\" failed.", this.Name(type));
		}
		protected override string Name(Reflect.Type type)
		{
			return base.Name(type) + "Named";
		}
		protected override void Run()
		{
			this.Run(
				this.Byte,
				this.Structure,
				this.Class,
				this.ComplexClass
				);
		}

		[Test]
		public void Byte() { this.Test(typeof(byte)); }
		[Test]
		public void Structure() { this.Test(typeof(Data.Structure)); }
		[Test]
		public void Class() { this.Test(typeof(Data.Class)); }
		[Test]
		public void ComplexClass() { this.Test(typeof(Data.ComplexClass)); }
	}
}

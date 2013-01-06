// 
//  Preprocessor.cs
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
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;
using Reflect = Kean.Core.Reflect;
using NUnit.Framework;
using NUnit.Framework;

namespace Kean.Xml.Serialize.Test
{
	public class Preprocessor :
		Factory<Preprocessor>
	{
		protected override void Test(Reflect.Type type)
		{
			Uri.Locator resource = this.ResourceName(type);
			this.Verify(this.Storage.Load<object>(resource), "Deserialization text \"{0}\" failed.", this.Name(type));
		}
		protected override Kean.Core.Uri.Locator ResourceName(Kean.Core.Reflect.Type type)
		{
			return "file:///./Xml/" + this.Name(type) + ".xml";
		}
		public override void Setup()
		{
			base.Setup();
		}

		protected override void Run()
		{
			this.Run(
				this.Include
				);
		}

		[Test]
		public void Include() { this.Test(typeof(Data.Include)); }
	}
}

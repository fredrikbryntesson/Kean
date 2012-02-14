// 
//  Linked.cs
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
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Xml.Serialize.Test.Data
{
	public class Linked :
		Class
	{
		[Core.Serialize.Parameter]
		public Structure Structure0 { get; set; }
		[Core.Serialize.Parameter]
		public Structure Structure1 { get; set; }
		[Core.Serialize.Parameter]
		public Class Class0 { get; set; }
		[Core.Serialize.Parameter]
		public Class Class1 { get; set; }

		#region IData
		public override void Initilize(IFactory factory)
		{
			base.Initilize(factory);
			this.Structure0 = this.Structure1 = factory.Create<Structure>();
			this.Class0 = this.Class1 = factory.Create<Class>();
		}
		public override void Verify(IFactory factory, string message, params object[] arguments)
		{
			base.Verify(factory, message, arguments);
			factory.Verify(this.Structure0, Is.Not.SameAs(this.Structure1), message, arguments);
			factory.Verify(this.Structure0, message, arguments);
			factory.Verify(this.Structure1, message, arguments);
			factory.Verify(this.Class0, Is.SameAs(this.Class1), message, arguments);
			factory.Verify(this.Class0, message, arguments);
			factory.Verify(this.Class1, message, arguments);
		}
		#endregion
	}
}

// 
//  Array.cs
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

namespace Kean.Xml.Serialize.Test.Data
{
	public class Include :
		Array
	{
		[Core.Serialize.Parameter]
		public DateTime DateTime { get; set; }

		#region IData
		public override void Initilize(IFactory factory)
		{
			this.DateTime = factory.Create<DateTime>();
			base.Initilize(factory);
		}
		public override void Verify(IFactory factory, string message, params object[] arguments)
		{
			factory.Verify(this.DateTime, message, arguments);
			base.Verify(factory, message, arguments);
		}
		#endregion
	}
}

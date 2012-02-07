// 
//  ValueTypes.cs
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

namespace Kean.Xml.Serialize.Test
{
	public class ValueTypes :
		Factory<ValueTypes>
	{
		protected override void  Run()
		{
			this.Run(
				this.Integer,
				this.Single,
				this.String,
				this.Enumerator
				);
		}

		[Test]
		public void Integer()
		{
			this.Test(typeof(int));
		}
		[Test]
		public void Single()
		{
			this.Test(typeof(float));
		}
		[Test]
		public void String()
		{
			this.Test(typeof(string));
		}
		[Test]
		public void Enumerator()
		{
			this.Test(typeof(Data.Enumerator));
		}
	}
}

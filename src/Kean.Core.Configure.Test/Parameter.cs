// 
//  Parameter.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Core.Configure;
namespace Kean.Core.Configure.Test
{
	[TestFixture]
	public class Parameter :
		AssertionHelper
	{
		string prefix = "Kean.Core.Configure.Test.";
		[Test]
		public void GetSet ()
		{
			Data.Class c = new Data.Class();
			Expect(c.String, EqualTo(null), this.prefix + "0");
			Target.Parameter p = new Target.Parameter(c, "String");
			Expect (p.Value, EqualTo (null), this.prefix + "1");
			p.Value = "test";
			Expect (p.Value, EqualTo ("test"), this.prefix + "2");
			Expect (c.String, EqualTo ("test"), this.prefix + "3");
			c.String = "Q";
			Expect (p.Value, EqualTo ("Q"), this.prefix + "4");
			Expect (c.String, EqualTo ("Q"), this.prefix + "5");
		}
	}
}


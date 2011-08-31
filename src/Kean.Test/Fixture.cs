// 
//  Fixture.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using Kean.Core;
using Kean.Core.Extension;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
namespace Kean.Test
{
	[TestFixture]
	public abstract class Fixture<T> :
		AssertionHelper
		where T : Fixture<T>, new()
	{
		[TestFixtureSetUp]
		public virtual void Setup()
		{
		}
		[TestFixtureTearDown]
		public virtual void TearDown()
		{
		}
		[SetUp]
		public virtual void PreTest()
		{
		}
		[TearDown]
		public virtual void PostTest()
		{
		}
		protected abstract void Run();
		protected void Run(Test test)
		{
			test.Run(this.PreTest, this.PostTest);
		}
		protected void Run(params Test[] tests)
		{
			foreach (Test test in tests)
				this.Run(test);
		}
		public static void Test()
		{
			T fixture = new T();
			fixture.Setup();
			fixture.Run();
			fixture.TearDown();
		}
	}
}

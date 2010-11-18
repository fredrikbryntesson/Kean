// 
//  Array.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009 Simon Mika
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
using Target = Kean.Core.Collection;

namespace Kean.Test.Core.Collection.Base
{
	public abstract class Vector<A> :
		AssertionHelper
		where A : Target.IVector<int>
	{
		public A ZeroToNine { get; set; }
		public string Prefix { get; set; }
		public abstract A Create(int count);


		[Test]
		public void Count()
		{
			Expect(this.ZeroToNine.Count, EqualTo(10), this.Prefix + "Count.0");
		}
		[Test]
		public void Get()
		{
			for (int i = 0; i < 10; i++)
				Expect(this.ZeroToNine[i], EqualTo(i), this.Prefix + "Get." + i);
		}
		[Test]
		[ExpectedException(typeof(Target.Exception.InvalidIndex))]
		public void GetIndexToBig()
		{
#pragma warning disable 219
			int a = this.ZeroToNine[10];
#pragma warning restore 219
		}
		[Test]
		[ExpectedException(typeof(Target.Exception.InvalidIndex))]
		public void GetNegativeIndex()
		{
#pragma warning disable 219
			int a = this.ZeroToNine[-1];
#pragma warning restore 219
		}
		[Test]
		public void Set()
		{
			A test = this.Create(10);
			test[0] = 9;
			test[5] = 4;
			test[3] = 6;
			test[4] = 5;
			test[1] = 8;
			test[7] = 2;
			test[8] = 1;
			test[9] = 0;
			test[6] = 3;
			test[2] = 7;
			for (int i = 0; i < 10; i++)
				Expect(test[i], EqualTo(9 - i), this.Prefix + "Set." + i);
		}
		[Test]
		[ExpectedException(typeof(Target.Exception.InvalidIndex))]
		public void SetIndexToBig()
		{
			this.ZeroToNine[10] = 23;
		}
		[Test]
		[ExpectedException(typeof(Target.Exception.InvalidIndex))]
		public void SetNegativeIndex()
		{
			this.ZeroToNine[-8] = 32;
		}
		public virtual void Run()
		{
			this.Count();
			this.Get();
			try
			{
				this.GetIndexToBig();
				throw new AssertionException(this.Prefix + "this.GetIndexToBig.0");
			}
			catch (Target.Exception.InvalidIndex)
			{
			}
			try
			{
				this.GetNegativeIndex();
				throw new AssertionException(this.Prefix + "this.GetNegativeIndex.0");
			}
			catch (Target.Exception.InvalidIndex)
			{
			}
			this.Set();
			try
			{
				this.SetIndexToBig();
				throw new AssertionException(this.Prefix + "this.SetIndexToBig.0");
			}
			catch (Target.Exception.InvalidIndex)
			{
			}
			try
			{
				this.SetNegativeIndex();
				throw new AssertionException(this.Prefix + "this.SetNegativeIndex.0");
			}
			catch (Target.Exception.InvalidIndex)
			{
			}
		}
	}
}

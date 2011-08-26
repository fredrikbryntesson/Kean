// 
//  Stack.cs
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
using Target = Kean.Core.Collection;

namespace Kean.Core.Collection.Test.Base
{
	public abstract class Stack<S> :
		AssertionHelper
		where S : Target.IStack<int>, new()
	{
		public string Prefix { get; set; }

		public virtual void Run()
		{
			this.EmptyStack();
			try
			{
				this.PopEmpty();
				throw new AssertionException(this.Prefix + "PopEmpty.0");
			}
			catch (Target.Exception.Empty) { }
			try
			{
				this.PeekEmpty();
				throw new AssertionException(this.Prefix + "PeekEmpty.0");
			}
			catch (Target.Exception.Empty) { }
			this.PushPeekPop();
			this.PushPeekPopTen();
			this.PushPopPush();
		}
		[Test]
		public void EmptyStack()
		{
			S target = new S();
			Expect(target.Empty, this.Prefix + "EmptyQueue.0");
		}
		[Test]
		[ExpectedException(typeof(Target.Exception.Empty))]
		public void PopEmpty()
		{
			S target = new S();
			Expect(target.Empty, this.Prefix + "PopEmpty.0");
			target.Pop();
		}
		[Test]
		[ExpectedException(typeof(Target.Exception.Empty))]
		public void PeekEmpty()
		{
			S target = new S();
			Expect(target.Empty, this.Prefix + "PopEmpty.0");
			target.Peek();
		}
		[Test]
		public void PushPeekPop()
		{
			S target = new S();
			Expect(target.Empty, this.Prefix + "PushPeekPop.0");
			target.Push(42);
			Expect(target.Peek(), EqualTo(42), this.Prefix + "PushPeekPop.1");
			Expect(target.Pop(), EqualTo(42), this.Prefix + "PushPeekPop.2");
			Expect(target.Empty, this.Prefix + "PushPeekPop.3");
		}
		[Test]
		public void PushPeekPopTen()
		{
			S target = new S();
			Expect(target.Empty, this.Prefix + "PushPeekPopTen.0");
			for (int i = 0; i < 10; i++)
			{
				target.Push(i);
				Expect(target.Peek(), EqualTo(i), this.Prefix + "PushPeekPopTen." + (1 + i));
			}
			for (int i = 0; i < 10; i++)
			{
				Expect(target.Peek(), EqualTo(9 - i), this.Prefix + "PushPeekPopTen." + (12 + i * 2));
				Expect(target.Pop(), EqualTo(9 - i), this.Prefix + "PushPeekPopTen." + (13 + i * 2));
			}
			Expect(target.Empty, this.Prefix + "PushPeekPop.24");
		}
		[Test]
		public void PushPopPush()
		{
			S target = new S();
			Expect(target.Empty, this.Prefix + "PushPeekPopTen.0");
			for (int i = 0; i < 10; i++)
			{
				target.Push(i);
				Expect(target.Peek(), EqualTo(i), this.Prefix + "PushPeekPopTen." + (1 + i));
			}
			for (int i = 0; i < 5; i++)
			{
				Expect(target.Peek(), EqualTo(9 - i), this.Prefix + "PushPeekPopTen." + (16 + i * 2));
				Expect(target.Pop(), EqualTo(9 - i), this.Prefix + "PushPeekPopTen." + (17 + i * 2));
			}
			for (int i = 0; i < 5; i++)
			{
				target.Push(i + 5);
				Expect(target.Peek(), EqualTo(i + 5), this.Prefix + "PushPeekPopTen." + (28 + i));
			}
			for (int i = 0; i < 10; i++)
			{
				Expect(target.Peek(), EqualTo(9 - i), this.Prefix + "PushPeekPopTen." + (34 + i * 2));
				Expect(target.Pop(), EqualTo(9 - i), this.Prefix + "PushPeekPopTen." + (35 + i * 2));
			}
			Expect(target.Empty, this.Prefix + "PushPeekPop.54");
		}
	}
}


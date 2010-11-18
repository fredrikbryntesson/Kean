// 
//  Queue.cs
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

namespace Kean.Test.Core.Collection.Base
{
	public abstract class Queue<Q> :
		AssertionHelper
		where Q : Target.IQueue<int>, new()
	{
		public string Prefix { get; set; }

		public virtual void Run()
		{
			this.EmptyQueue();
			try
			{
				this.DequeueEmpty();
				throw new AssertionException(this.Prefix + "DequeueEmpty.0");
			}
			catch (Target.Exception.Empty) { }
			try
			{
				this.PeekEmpty();
				throw new AssertionException(this.Prefix + "PeekEmpty.0");
			}
			catch (Target.Exception.Empty) { }
			this.EnqueuePeekDequeue();
			this.EnqueuePeekDequeueTen();
			this.EnqueueDequeueEnqueue();
		}
		[Test]
		public void EmptyQueue()
		{
			Q target = new Q();
			Expect(target.Empty, this.Prefix + "EmptyQueue.0");
		}
		[Test]
		[ExpectedException(typeof(Target.Exception.Empty))]
		public void DequeueEmpty()
		{
			Q target = new Q();
			Expect(target.Empty, this.Prefix + "DequeueEmpty.0");
			target.Dequeue();
		}
		[Test]
		[ExpectedException(typeof(Target.Exception.Empty))]
		public void PeekEmpty()
		{
			Q target = new Q();
			Expect(target.Empty, this.Prefix + "DequeueEmpty.0");
			target.Peek();
		}
		[Test]
		public void EnqueuePeekDequeue()
		{
			Q target = new Q();
			Expect(target.Empty, this.Prefix + "EnqueuePeekDequeue.0");
			target.Enqueue(42);
			Expect(target.Peek(), EqualTo(42), this.Prefix + "EnqueuePeekDequeue.1");
			Expect(target.Dequeue(), EqualTo(42), this.Prefix + "EnqueuePeekDequeue.2");
			Expect(target.Empty, this.Prefix + "EnqueuePeekDequeue.3");
		}
		[Test]
		public void EnqueuePeekDequeueTen()
		{
			Q target = new Q();
			Expect(target.Empty, this.Prefix + "EnqueuePeekDequeueTen.0");
			for (int i = 0; i < 10; i++)
			{
				target.Enqueue(i);
				Expect(target.Peek(), EqualTo(0), this.Prefix + "EnqueuePeekDequeueTen." + (1 + i));
			}
			for (int i = 0; i < 10; i++)
			{
				Expect(target.Peek(), EqualTo(i), this.Prefix + "EnqueuePeekDequeueTen." + (12 + i * 2));
				Expect(target.Dequeue(), EqualTo(i), this.Prefix + "EnqueuePeekDequeueTen." + (13 + i * 2));
			}
			Expect(target.Empty, this.Prefix + "EnqueuePeekDequeue.24");
		}
		[Test]
		public void EnqueueDequeueEnqueue()
		{
			Q target = new Q();
			Expect(target.Empty, this.Prefix + "EnqueuePeekDequeueTen.0");
			for (int i = 0; i < 10; i++)
			{
				target.Enqueue(i);
				Expect(target.Peek(), EqualTo(0), this.Prefix + "EnqueuePeekDequeueTen." + (1 + i));
			}
			for (int i = 0; i < 5; i++)
			{
				Expect(target.Peek(), EqualTo(i), this.Prefix + "EnqueuePeekDequeueTen." + (16 + i * 2));
				Expect(target.Dequeue(), EqualTo(i), this.Prefix + "EnqueuePeekDequeueTen." + (17 + i * 2));
			}
			for (int i = 0; i < 5; i++)
			{
				target.Enqueue(i + 10);
				Expect(target.Peek(), EqualTo(5), this.Prefix + "EnqueuePeekDequeueTen." + (28 + i));
			}
			for (int i = 0; i < 10; i++)
			{
				Expect(target.Peek(), EqualTo(i + 5), this.Prefix + "EnqueuePeekDequeueTen." + (34 + i * 2));
				Expect(target.Dequeue(), EqualTo(i + 5), this.Prefix + "EnqueuePeekDequeueTen." + (35 + i * 2));
			}
			Expect(target.Empty, this.Prefix + "EnqueuePeekDequeue.54");
		}
	}
}


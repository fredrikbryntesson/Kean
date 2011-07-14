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
using Kean.Core.Collection.Extension;
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
            this.AddAfterEmptyQueue();
		}
        [Test]
        public void AddAfterEmptyQueue()
        {
            Q target = new Q();
            for (int i = 0; i < 10; i++)
                target.Enqueue(i);
            target.Clear();
            for (int i = 0; i < 10; i++)
                target.Enqueue(i);
            Expect(target.Count, EqualTo(10), this.Prefix + "AddAfterEmptyQueue.0");
        }
        [Test]
		public void EmptyQueue()
		{
			Q target = new Q();
			Expect(target.Empty, this.Prefix + "EmptyQueue.0");
            Expect(target.Count, EqualTo(0), this.Prefix + "EmptyQueue.1");
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
            Expect(target.Count, EqualTo(0), this.Prefix + "EnqueuePeekDequeue.1");
            target.Enqueue(42);
            Expect(target.Count, EqualTo(1), this.Prefix + "EnqueuePeekDequeue.2");
            Expect(target.Peek(), EqualTo(42), this.Prefix + "EnqueuePeekDequeue.3");
            Expect(target.Count, EqualTo(1), this.Prefix + "EnqueuePeekDequeue.4");
            Expect(target.Dequeue(), EqualTo(42), this.Prefix + "EnqueuePeekDequeue.5");
            Expect(target.Count, EqualTo(0), this.Prefix + "EnqueuePeekDequeue.6");
            Expect(target.Empty, this.Prefix + "EnqueuePeekDequeue.7");
		}
		[Test]
		public void EnqueuePeekDequeueTen()
		{
			Q target = new Q();
			Expect(target.Empty, this.Prefix + "EnqueuePeekDequeueTen.0");
            Expect(target.Count, EqualTo(0), this.Prefix + "EnqueuePeekDequeueTen.1");
            for (int i = 0; i < 10; i++)
			{
				target.Enqueue(i);
				Expect(target.Peek(), EqualTo(0), this.Prefix + "EnqueuePeekDequeueTen." + (2 * i + 0 + 2));
                Expect(target.Count, EqualTo(i+1), this.Prefix + "EnqueuePeekDequeueTen." + (2 * i + 1 + 2));
            }
			for (int i = 0; i < 10; i++)
			{
                Expect(target.Peek(), EqualTo(i), this.Prefix + "EnqueuePeekDequeueTen." + (4 * i + 0 + 22));
                Expect(target.Count, EqualTo(10 - i), this.Prefix + "EnqueuePeekDequeueTen." + (4 * i + 1 + 22));
                Expect(target.Dequeue(), EqualTo(i), this.Prefix + "EnqueuePeekDequeueTen." + (4 * i + 2 + 22));
                Expect(target.Count, EqualTo(10 - i - 1), this.Prefix + "EnqueuePeekDequeueTen." + (4 * i + 3 + 2));
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
				Expect(target.Peek(), EqualTo(0), this.Prefix + "EnqueuePeekDequeueTen." + (2 * i + 0 + 1));
                Expect(target.Count, EqualTo(i + 1), this.Prefix + "EnqueuePeekDequeueTen." + (2 * i + 1 + 1));
            }
			for (int i = 0; i < 5; i++)
			{
                Expect(target.Count, EqualTo(10-i), this.Prefix + "EnqueuePeekDequeueTen." + (5 * i + 0 + 21));
                Expect(target.Peek(), EqualTo(i), this.Prefix + "EnqueuePeekDequeueTen." + (5 * i + 1 + 21));
                Expect(target.Count, EqualTo(10 - i), this.Prefix + "EnqueuePeekDequeueTen." + (5 * i + 2 + 21));
                Expect(target.Dequeue(), EqualTo(i), this.Prefix + "EnqueuePeekDequeueTen." + (5 * i + 3 + 21));
                Expect(target.Count, EqualTo(10 - i - 1), this.Prefix + "EnqueuePeekDequeueTen." + (5 * i + 4 + 21));
            }
			for (int i = 0; i < 5; i++)
			{
				target.Enqueue(i + 10);
                Expect(target.Count, EqualTo(5 + 1 + i), this.Prefix + "EnqueuePeekDequeueTen." + (3 * i + 0 + 46));
                Expect(target.Peek(), EqualTo(5), this.Prefix + "EnqueuePeekDequeueTen." + (3 * i + 1 + 46));
                Expect(target.Count, EqualTo(5 + 1 + i), this.Prefix + "EnqueuePeekDequeueTen." + (3 * i + 2 + 46));
            }
			for (int i = 0; i < 10; i++)
			{
                Expect(target.Count, EqualTo(10 - i), this.Prefix + "EnqueuePeekDequeueTen." + (5 * i + 0 + 61));
                Expect(target.Peek(), EqualTo(i + 5), this.Prefix + "EnqueuePeekDequeueTen." + (5 * i + 1 + 61));
                Expect(target.Count, EqualTo(10 - i), this.Prefix + "EnqueuePeekDequeueTen." + (5 * i + 2 + 61));
                Expect(target.Dequeue(), EqualTo(i + 5), this.Prefix + "EnqueuePeekDequeueTen." + (5 * i + 3 + 61));
                Expect(target.Count, EqualTo(10 - i - 1), this.Prefix + "EnqueuePeekDequeueTen." + (5 * i + 4 + 61));
            }
			Expect(target.Empty, this.Prefix + "EnqueuePeekDequeue.112");
            Expect(target.Count, EqualTo(0), this.Prefix + "EnqueuePeekDequeue.113");
        }
	}
}


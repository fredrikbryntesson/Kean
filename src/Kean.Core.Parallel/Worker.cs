// 
//  Worker.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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

namespace Kean.Core.Parallel
{
	internal class Worker
	{
		System.Threading.Thread thread;
		ThreadPool pool;
		object wakeUp;
		object semaphore;
		Collection.Queue<ITask> tasks;
		bool occupied;
		string name;
		public bool Waiting { get { return thread.ThreadState == System.Threading.ThreadState.WaitSleepJoin; } }
		public bool Occupied
		{
			get
			{
				lock (this.semaphore)
					return this.occupied;
			}
			protected set
			{
				lock (this.semaphore)
					this.occupied = value;
			}
		}
		bool end;

		internal Worker(ThreadPool pool, string poolName, int index)
		{
			this.pool = pool;
			this.wakeUp = new object();
			this.semaphore = new object();
			this.tasks = new Collection.Queue<ITask>();
			this.thread = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			{
				while (!this.end)
				{
					lock (this.wakeUp) System.Threading.Monitor.Wait(this.wakeUp);
					ITask task = null;
					do
					{
						if (task.NotNull())
						{
							try
							{
								this.Occupied = true;
								task.Run();
							}
							//catch (Exception e)
							//{
							//    this.pool.Log(this, e);
							//}
							finally
							{
								task = null;
								this.Occupied = false;
							}
						}
						lock (this.semaphore) task = this.tasks.Empty ? this.pool.Dequeue() : this.tasks.Dequeue();
					} while (task.NotNull());
				}
			}));
			this.thread.Name = poolName + ":" + index;
			this.thread.Start();
		}
		~Worker()
		{
			this.Dispose();
		}
		public void Dispose()
		{
			if (this.thread.NotNull())
			{
				this.End();
				if (this.Occupied)
				{
					this.thread.Interrupt();
					this.thread.Abort();
				}
				this.thread = null;
			}
		}
		public void Start()
		{
			lock (this.wakeUp) System.Threading.Monitor.Pulse(this.wakeUp);
		}
		public void Enque(ITask task)
		{
			lock (this.semaphore)
				this.tasks.Enqueue(task);
			this.Start();
		}
		public void End()
		{
			lock (this.semaphore)
				this.end = true;
			this.Start();
		}
	}
}

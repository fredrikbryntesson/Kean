// 
//  Worker.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2012 Simon Mika
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
    public class ThreadPool :
        IDisposable
    {
		public int ThreadCount { get { return this.workers.Length; } }
        Worker[] workers;
        Collection.Queue<ITask> tasks;
		public ThreadPool(string name) : this(name, System.Environment.ProcessorCount + 2) { }
        public ThreadPool(string name, int workers)
        {
            this.tasks = new Collection.Queue<ITask>();
            this.workers = new Worker[workers];
            for (int i = 0; i < workers; i++)
                this.workers[i] = new Worker(this, name, i);
        }
        ~ThreadPool()
        {
            this.Dispose();
        }
        public void Dispose()
        {
            if (this.workers.NotNull())
            {
				foreach (Worker worker in this.workers)
					if (!worker.Occupied)
						worker.Dispose();
				foreach (Worker worker in this.workers)
					if (worker.Occupied)
						worker.Dispose();
				this.workers = null;
            }
        }
        public void Enqueue(Action task) 
        { 
            if(task.NotNull()) 
                this.Enqueue(new Task(task)); 
        }
        public void Enqueue<T>(Action<T> task, T argument) 
        { 
            if(task.NotNull())
                this.Enqueue(new Task<T>(task, argument)); 
        }
		public void Enqueue<T1, T2>(Action<T1, T2> task, T1 argument1, T2 argument2) 
        { 
            if(task.NotNull())
                this.Enqueue(new Task<T1, T2>(task, argument1, argument2)); 
        }
		public void Enqueue<T1, T2, T3>(Action<T1, T2, T3> task, T1 argument1, T2 argument2, T3 argument3)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3>(task, argument1, argument2, argument3));
		}
		public void Enqueue<T1, T2, T3, T4>(Action<T1, T2, T3, T4> task, T1 argument1, T2 argument2, T3 argument3, T4 argument4)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3, T4>(task, argument1, argument2, argument3, argument4));
		}
		public void Enqueue<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> task, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3, T4, T5>(task, argument1, argument2, argument3, argument4, argument5));
		}
		public void Enqueue<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> task, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3, T4, T5, T6>(task, argument1, argument2, argument3, argument4, argument5, argument6));
		}
		public void Enqueue<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> task, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3, T4, T5, T6, T7>(task, argument1, argument2, argument3, argument4, argument5, argument6, argument7));
		}
		public void Enqueue<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> task, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3, T4, T5, T6, T7, T8>(task, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8));
		}
		public void Enqueue<T1, T2, T3, T4, T5, T6, T7, T8, TRest>(Action<T1, T2, T3, T4, T5, T6, T7, T8, TRest> task, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, TRest argumentRest)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3, T4, T5, T6, T7, T8, TRest>(task, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argumentRest));
		}
		public void Enqueue(ITask task)
        {
            try
            {
                lock (this.tasks)
                    this.tasks.Enqueue(task);
                lock (this.workers)
                    foreach (Worker worker in this.workers)
                        if (!worker.Occupied)
                        {
                            worker.Start();
                            break;
                        }
            }
            catch (ArgumentNullException e)
            {
                throw new Exception.ThreadPoolDisposed(e);
            }
        }

        internal ITask Dequeue()
        {
            lock (this.tasks)
                return this.tasks.Dequeue();
        }

        internal void Log(Worker worker, System.Exception e)
        {
            Console.WriteLine("Thread endend with exception: " + e.ToString());
            throw e;
        }

        public void ForEachWorker(Action task)
        {
            if(task.NotNull())
                this.ForEachWorker(new Task(task));
        }
        public void ForEachWorker<T>(Action<T> task, T argument)
        {
            if (task.NotNull())
                this.ForEachWorker(new Task<T>(task, argument));
        }
        public void ForEachWorker(ITask task)
        {
            lock (this.workers)
                foreach (Worker worker in this.workers)
                    worker.Enque(task);
        }
		static ThreadPool global;
		public static ThreadPool Global
		{
			set { ThreadPool.global = value; }
			get 
			{
				if (ThreadPool.global.IsNull())
					ThreadPool.global = new ThreadPool("Global");
				return ThreadPool.global;
			}
		}
    }
}

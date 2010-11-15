using System;
using Error = Kean.Core.Error;
using Collection = Kean.Core.Collection;
using Basis = Kean.Core.Basis;
using Kean.Core.Basis.Extension;
using Kean.Core.Collection.Extension;

namespace Kean.Extra.Log
{
	public class Log :
		Basis.Synchronized
	{

		Collection.IQueue<Error.IError> log = new Collection.Array.Queue<Error.IError>();
		Collection.Array.List<Error.IError> cacheList = new Collection.Array.List<Error.IError>(100);
		Collection.IQueue<Error.IError> cache;
		public Error.Level Threshold { get; set; }
		public int CacheSize 
		{
			get { lock(this.Lock) return this.cacheList.Capacity;}
			set 
			{
				lock (this.Lock)
				{
					while (value > this.cacheList.Count)
						this.ReduceCache();
					this.cacheList.Capacity = value;
				}
			} 
		}
		public Log()
		{
			this.cache = new Collection.Wrap.QueueList<Error.IError>(this.cacheList);
		}
		public void Append(DateTime time, Error.Level level, string title, string message, System.Diagnostics.StackTrace trace)
		{
			this.Append(new Entry() { Time = time, Level = level, Title = title, Message = message, Trace = trace });
		}
		public void Append(Error.IError item)
		{
			lock (this.Lock)
			{
				if (item.Level == Kean.Core.Error.Level.Critical)
					this.log.Enqueue(this.cache);
				else if (this.cacheList.Count >= this.CacheSize)
					this.ReduceCache();
				this.cache.Enqueue(item);
			}
		}
		void ReduceCache()
		{
			Error.IError entry = this.cache.Dequeue();
			if (entry.Level > this.Threshold)
				this.log.Enqueue(entry);
		}
		public void Flush(Action<Error.IError> save)
		{
			while (!this.cache.Empty)
				this.ReduceCache();
			while (!this.log.Empty)
				save.Call(this.log.Dequeue());
		}
	}
}


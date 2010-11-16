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
		public Error.Level LogThreshold { get; set; }
		public Error.Level AllThreshold { get; set; }
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
			this.LogThreshold = Kean.Core.Error.Level.Recoverable;
			this.cache = new Collection.Wrap.QueueList<Error.IError>(this.cacheList);
		}
		public void Append(Error.Level level, string title, string message)
		{
			this.Append(new Entry() { Time = DateTime.Now, Level = level, Assembly = System.Reflection.Assembly.GetCallingAssembly(), Title = title, Message = message, Trace = new System.Diagnostics.StackTrace(1, true) });
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
			if (entry.Level > this.LogThreshold)
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


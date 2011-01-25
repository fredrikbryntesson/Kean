//
//  Cache
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
using Error = Kean.Core.Error;
using Collection = Kean.Core.Collection;
using Basis = Kean.Core.Basis;
using Kean.Core.Basis.Extension;
using Kean.Core.Collection.Extension;

namespace Kean.Extra.Log
{
	public class Cache :
		Basis.Synchronized,
		IDisposable
	{
		Collection.IQueue<Error.IError> log = new Collection.Array.Queue<Error.IError>();
		Collection.Array.List<Error.IError> cacheList = new Collection.Array.List<Error.IError>(100);
		Collection.IQueue<Error.IError> cache;
		public Error.Level LogThreshold { get; set; }
		public Error.Level AllThreshold { get; set; }
		public Collection.IList<IWriter> Writers { get; private set; }
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
		public Cache()
		{
			this.LogThreshold = Kean.Core.Error.Level.Message;
			this.AllThreshold = Kean.Core.Error.Level.Critical;
			this.cache = new Collection.Wrap.ListQueue<Error.IError>(this.cacheList);
			this.Writers = new Kean.Core.Collection.List<IWriter>();
			Cache.append += this.Append;
		}
		#region IDisposable Members
		public void Dispose()
		{
			Cache.append -= this.Append;
		}
		#endregion
		public void Append(Error.Level level, string title, string message)
		{
			System.Diagnostics.StackFrame frame = new System.Diagnostics.StackTrace().GetFrame(1);
			System.Reflection.MethodBase method = frame.GetMethod();
			Type type = method.DeclaringType;
			System.Reflection.AssemblyName assembly = type.Assembly.GetName();
			this.Append(new Entry()
			{
				Time = DateTime.Now,
				Level = level,
				Title = title,
				Message = message,
				AssemblyName = assembly.Name,
				AssemblyVersion = assembly.Version.ToString(),
				Type = type.FullName,
				Method = method.Name,
				Filename = frame.GetFileName(),
				Line = frame.GetFileLineNumber(),
				Column = frame.GetFileColumnNumber(),
			});
		}
		public void Append(Error.IError item)
		{
			lock (this.Lock)
			{
				if (item.Level >= this.AllThreshold)
					this.log.Enqueue(this.cache);
				else if (this.cacheList.Count >= this.CacheSize)
					this.ReduceCache();
				this.cache.Enqueue(item);
			}
		}
		void ReduceCache()
		{
			Error.IError entry = this.cache.Dequeue();
			if (entry.Level >= this.LogThreshold)
				this.log.Enqueue(entry);
		}
		public void Flush()
		{
			Action<Error.IError> save = this.Writers.Fold((w, a) => a += w.Open(), (Action<Error.IError>)null);
			while (!this.cache.Empty)
				this.ReduceCache();
			while (!this.log.Empty)
				save.Call(this.log.Dequeue());
			this.Writers.Apply(w => w.Close());
		}
		static event Action<Error.IError> append;
		public static void Log(Error.IError entry)
		{
			Cache.append.Call(entry);
		}
		public static void Log(Error.Level level, string title, string message)
		{
			System.Diagnostics.StackFrame frame = new System.Diagnostics.StackTrace().GetFrame(1);
			System.Reflection.MethodBase method = frame.GetMethod();
			Type type = method.DeclaringType;
			System.Reflection.AssemblyName assembly = type.Assembly.GetName();
			Cache.Log(new Entry()
			{ 
				Time = DateTime.Now, 
				Level = level, 
				Title = title, 
				Message = message, 
				AssemblyName = assembly.Name,
				AssemblyVersion = assembly.Version.ToString(),
				Type = type.FullName,
				Method = method.Name,
				Filename = frame.GetFileName(),
				Line = frame.GetFileLineNumber(),
				Column = frame.GetFileColumnNumber(),
			});
		}
}	
}


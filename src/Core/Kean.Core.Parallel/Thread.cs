// 
//  Thread.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
	public class Thread : 
		Synchronized,
		IDisposable
	{
		protected System.Threading.Thread Backend { get; set; }

		public string Name { get { return this.Backend.Name; } }

		#region Constructors
		protected Thread()
		{ }
		protected Thread(string name, System.Threading.ThreadStart task) :
			this(name, new System.Threading.Thread(task))
		{ }

		protected Thread(string name, System.Threading.Thread backend) :
			this()
		{
			try
			{
				this.Backend = backend;
				this.Backend.Name = name;
				this.Backend.Start();

			}
			catch (System.Exception e)
			{
				
				throw;
			}
		}
		#endregion
		public virtual bool Join(int timeOut)
		{
			return this.Backend.Join(timeOut);
		}
		public virtual void Abort()
		{
			this.Backend.Abort();
		}
		#region Static Creators
		static int counter;
		public static Thread Start(Action task)
		{
			return Thread.Start("Thread" + Thread.counter++, task);
		}
		public static Thread Start(string name, Action task)
		{
			task = Error.Log.Wrap(string.Format("Thread \"{0}\" Failed.", name), task);
			return new Thread(name, () => task.Call());
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			if (this.Backend.NotNull())
			{
				this.Abort();
				this.Backend = null;
			}
		}
		#endregion
	}
}

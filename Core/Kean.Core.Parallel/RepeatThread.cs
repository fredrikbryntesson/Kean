// 
//  RepeatThread.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
	public class RepeatThread :
		Thread
	{
		bool running;
		public bool Running
		{
			get { lock (this.Lock) return this.running; }
			private set { lock (this.Lock) this.running = value; }
		}
		bool end;
		bool End
		{
			get { lock (this.Lock) return this.end; } 
			set { lock (this.Lock) this.end = value; } 
		}
		#region Constructors
		RepeatThread(string name, Action task) :
			base()
		{
			Action wrappedTask = Error.Log.Wrap(string.Format("Thread \"{0}\" Failed.", name), () =>
			{
				try
				{
					task.Call();
				}
				catch (System.Threading.ThreadInterruptedException) { this.End = true; }
				catch (System.Threading.ThreadAbortException) { this.End = true; }
			});
			this.Backend = new System.Threading.Thread(() => 
			{ 
				this.Running = true;
				while (!this.End)
					wrappedTask();
				this.Running = false;
			});
			this.Backend.Name = name;
			this.Backend.Start();
		}
		#endregion

		public bool Stop()
		{
			return this.Join(100);
		}
		public override bool Join(int timeOut)
		{
			this.End = true;
			return base.Join(timeOut);
		}
		public override void Abort()
		{
			this.End = true;
			base.Abort();
		}
		#region Static Creators
		static int counter;
		public static new RepeatThread Start(Action task)
		{
			return RepeatThread.Start("RepeatThread" + RepeatThread.counter++, task);
		}
		public static new RepeatThread Start(string name, Action task)
		{
			return new RepeatThread(name, task);
		}
		#endregion
	}
}

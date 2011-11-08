//
//  Abstract
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 - 2011 Simon Mika
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
using Argument = Kean.Cli.Argument;

namespace Kean.Platform
{
	public abstract class Module :
		IDisposable
	{
		#region Properties
		internal Mode Mode { get; private set; }
		public string Name { get; private set; }
		public Application Application { get; internal set; }
		#endregion
		#region Events
		public event Action<Module, Module> Replaced;
		#endregion
		#region Constructors
		protected Module(string name)
		{
			this.Name = name;
		}
		#endregion
		#region Implemetors Interface
		internal protected virtual void AddArguments(Argument.Parser parser)
		{
		}
		internal protected virtual void Initialize() 
		{
			this.Mode = Mode.Initialized;
		}
		internal protected virtual void Start()
		{
			this.Mode = Mode.Started;
		}
		internal protected virtual void Stop()
		{
			this.Mode = Mode.Stopped;
		}
		internal protected virtual void Dispose()
		{
			this.Mode = Mode.Disposed;
		}
		#endregion
		internal Module Merge(Module other)
		{
			this.Replaced += other.Replaced;
			return this;
		}
		#region IDisposable Members
		~Module()
		{
			(this as IDisposable).Dispose();
		}
		void IDisposable.Dispose()
		{
			if (this.Mode != Mode.Disposed)
				this.Dispose();
		}
		#endregion
	}
}

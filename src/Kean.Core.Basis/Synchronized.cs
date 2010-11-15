using System;

namespace Kean.Core.Basis
{
	public abstract class Synchronized
	{
		protected object Lock { get; private set; }
		protected Synchronized() :
			this(new object())
		{ }
		protected Synchronized(object @lock)
		{
			this.Lock = @lock;
		}
	}
}

using System;
namespace Kean.Core.Collection.Hooked
{
	public interface IQueue<T> : 
		Collection.IQueue<T>
	{
		event Action<T> Dequeued;
		event Action<T> Enqueued;
		event Func<T, bool> OnDequeue;
		event Func<T, bool> OnEnqueue;
	}
}

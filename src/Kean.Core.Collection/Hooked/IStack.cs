using System;
namespace Kean.Core.Collection.Hooked
{
	public interface IStack<T> :
		Collection.IStack<T>
	{
		event Func<T, bool> OnPop;
		event Func<T, bool> OnPush;
		event Action<T> Poped;
		event Action<T> Pushed;
	}
}

using System;
namespace Kean.Core.Collection.Hooked
{
	public interface IList<T> : 
		Collection.IList<T>
	{
		event Action<int, T> Added;
		event Func<int, T, bool> OnAdd;
		event Func<int, T, bool> OnRemove;
		event Action<int, T> Removed;
	}
}

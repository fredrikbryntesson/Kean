using System;
namespace Kean.Core.Collection.Hooked
{
	public interface IVector<T> :
		Collection.IVector<T>
	{
		event Func<int, T, T, bool> OnReplace;
		event Action<int, T, T> Replaced;
	}
}

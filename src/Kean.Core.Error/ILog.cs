using System;
namespace Kean.Core.Error
{
	public interface ILog
	{
		void Add(IError error);
	}
}


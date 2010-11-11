using System;
using Error = Kean.Core.Error;
using Collection = Kean.Core.Collection;
namespace Kean.Extra.Log
{
	public class Log :
		Collection.List<Error.IError>,
		Error.ILog
	{
		public Log ()
		{
		}
	}
}


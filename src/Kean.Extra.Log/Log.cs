using System;
using Error = Kean.Core.Error;
using Collection = Kean.Core.Collection;
namespace Kean.Extra.Log
{
	public class Log
	{
		Collection.IList<Error.IError> log = new Collection.List<Error.IError>();
		Collection.IQueue<Error.IError> queue;
		public Error.Level Threshold { get; set; }
		public Log ()
		{
		}
	}
}


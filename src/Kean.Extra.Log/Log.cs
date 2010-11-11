using System;
using Error = Kean.Core.Error;
using Collection = Kean.Core.Collection;
namespace Kean.Extra.Log
{
	public class Log
	{
		public Error.Level Threshold { get; set; }
		public Log ()
		{
		}
	}
}


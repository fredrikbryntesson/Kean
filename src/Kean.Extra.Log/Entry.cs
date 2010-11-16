using System;
using Error = Kean.Core.Error;
using Collection = Kean.Core.Collection;

namespace Kean.Extra.Log
{
	public class Entry :
		Error.IError
	{
		#region IError Members
		public DateTime Time { get; internal set; }
		public Error.Level Level { get; internal set; }
		public System.Reflection.Assembly Assembly { get; internal set; }
		public string Title { get; internal set; }
		public string Message { get; internal set; }
		public System.Diagnostics.StackTrace Trace { get; internal set; }
		#endregion
	}
}

using System;
using Kean.Core.Extension;

namespace Kean.Platform.Settings
{
	public class RemoteConfiguration
	{
		public Asynchronous Asynchronous { get; set; }
		public event Action<bool, string> OnDebug;

		public void Debug(bool direction, string message)
		{
			this.OnDebug.Call(direction, message);
		}
	}
}

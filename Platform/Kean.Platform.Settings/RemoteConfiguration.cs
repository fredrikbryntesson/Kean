using System;
using Kean.Core.Extension;

namespace Kean.Platform.Settings
{
	public class RemoteConfiguration
	{
		private Asynchronous asynchronous;
		public Asynchronous Asynchronous 
		{
			get { return this.asynchronous; } 
			set 
			{ 
				if (this.asynchronous != value) 
					this.AsynchronousChanged.Call(this.asynchronous = value); 
			} 
		}
		public event Action<Asynchronous> AsynchronousChanged;

		public event Action<bool, string> OnDebug;

		public void Debug(bool direction, string message)
		{
			this.OnDebug.Call(direction, message);
		}
	}
}

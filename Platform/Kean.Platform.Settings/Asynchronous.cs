using System;

namespace Kean.Platform.Settings
{
	[Flags]
	public enum Asynchronous
	{
		None = 0x00,
		Set = 0x01,
		Notify = 0x02,
		SetNotify = 0x03,
		Call = 0x04,
		SetCall = 0x05,
		NotifyCall = 0x06,
		All = 0x07,
	}
}

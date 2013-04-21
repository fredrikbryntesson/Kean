using System;

namespace Kean.Platform.Settings
{
	[Flags]
	public enum Asynchronous
	{
		None = 0x00,
		PropretySet = 0x01,
		Notify = 0x02,
		MethodCall = 0x04,
		All = 0x07,
	}
}

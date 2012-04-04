using System;

namespace Kean.Platform.Settings
{
	[Flags]
	enum PropertyMode
	{
		None = 0x00,
		Read = 0x01,
		Write = 0x02,
		Notify = 0x04,
	}
}

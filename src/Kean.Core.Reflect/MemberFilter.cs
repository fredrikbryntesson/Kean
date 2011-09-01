using System;

namespace Kean.Core.Reflect
{
	[Flags]
	public enum MemberFilter
	{
		Default = 0x34,
		Static = 0x01,
		NonPublic = 0x02,
		Public = 0x04,
		Field = 0x08,
		Property = 0x10,
		Method = 0x20,
		Constructor = 0x30,
	}
}

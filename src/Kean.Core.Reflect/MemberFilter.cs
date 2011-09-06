using System;

namespace Kean.Core.Reflect
{
	[Flags]
	public enum MemberFilter
	{
		Default = 0x69,
		Instance = 0x01,
		Static = 0x02,
		NonPublic = 0x04,
		Public = 0x08,
		Field = 0x10,
		Property = 0x20,
		Method = 0x40,
		Constructor = 0x80,
		All = 0xff,
	}
}

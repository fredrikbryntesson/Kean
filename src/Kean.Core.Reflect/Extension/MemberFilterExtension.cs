using System;

namespace Kean.Core.Reflect.Extension
{
	public static class MemberFilterExtension
	{
		public static bool Contains(this MemberFilter me, MemberFilter value)
		{
			return (me & value) == value;
		}
		public static System.Reflection.BindingFlags AsBindingFlags(this MemberFilter me)
		{
			System.Reflection.BindingFlags result = System.Reflection.BindingFlags.Default;
			if (me.Contains(MemberFilter.Static))
				result |= System.Reflection.BindingFlags.Static;
			if (me.Contains(MemberFilter.NonPublic))
				result |= System.Reflection.BindingFlags.NonPublic;
			return result;
		}
	}
}

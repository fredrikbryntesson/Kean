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
			System.Reflection.BindingFlags result = System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.FlattenHierarchy;
			if (me.Contains(MemberFilter.Instance))
				result |= System.Reflection.BindingFlags.Instance;
			if (me.Contains(MemberFilter.Static))
				result |= System.Reflection.BindingFlags.Static;
			if (me.Contains(MemberFilter.Public))
				result |= System.Reflection.BindingFlags.Public;
			if (me.Contains(MemberFilter.NonPublic))
				result |= System.Reflection.BindingFlags.NonPublic;
			return result;
		}
	}
}

using System;

namespace Kean.Core.Extension
{
	public static class ComparableExtension
	{
		public static Order CompareWith(this IComparable me, IComparable other)
		{
			int order = me.CompareTo(other);
			return order > 0 ? Order.GreaterThan : order == 0 ? Order.Equal : Order.LessThan;
		}
	}
}

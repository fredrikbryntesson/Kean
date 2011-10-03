using System;

namespace Attraction.Gui.Base.Target.Extension
{
	public static class ClipboardExtension
	{
		public static bool Contains(this ClipType me, ClipType value)
		{
			return (me & value) == value;
		}
	}
}

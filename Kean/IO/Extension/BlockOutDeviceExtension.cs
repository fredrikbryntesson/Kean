using System;
using Collection = Kean.Collection;

namespace Kean.IO.Extension
{
	public static class BlockOutDeviceExtension
	{
		public static bool Write(this IBlockOutDevice me, params byte[] buffer)
		{
			return me.Write(new Collection.Vector<byte>(buffer));
		}
	}
}

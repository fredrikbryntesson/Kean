using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.Core.Test
{
	public static class All
    {
		public static void Test()
		{
			Core.Test.NonNullable.Test();
			Core.Test.StringExtension.Test();
		}
	}
}

using System;
using Kean.Core;
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;
using Serialize = Kean.Core.Serialize;

namespace Kean.DB
{
	public abstract class Storage :
		Serialize.Storage
	{
		public Storage()
		{
		}
	}
}


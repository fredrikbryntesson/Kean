using System;

namespace Kean.Xml
{
	public class Region
	{
		public string Resource { get; private set; }
		public Position Start { get; private set; }
		public Position End { get; private set; }
		public Region(string resource, Position start, Position end)
		{
			this.Resource = resource;
			this.Start = start;
			this.End = end;
		}
	}
}

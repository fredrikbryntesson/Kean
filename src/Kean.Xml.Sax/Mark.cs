using System;

namespace Kean.Xml.Sax
{
	class Mark
	{
		IParser parser;
		string resource;
		Position start;
		Position end;
		public Mark(IParser parser)
		{
			this.parser = parser;
			this.resource = parser.Resource;
			this.start = this.parser.Position;
		}
		public void End()
		{
			this.end = this.parser.Position;
		}
		public static implicit operator Region(Mark mark)
		{
			return new Region(mark.resource, mark.start, mark.end ?? mark.parser.Position);
		}
	}
}

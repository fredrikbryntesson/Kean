using System;

namespace Kean.Xml.Sax.Reader
{
	class Stream :
		Text
	{
		protected System.IO.Stream ActualStream { get; set; }

		public Stream(System.IO.Stream stream) :
			base(new System.IO.StreamReader(stream))
		{
			this.ActualStream = stream;
		}
	}
}

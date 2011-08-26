using System;

namespace Kean.Xml.Dom
{
	public class Comment :
		Node
	{
		public string Value { get; set; }
		public Comment() { }
		public Comment(string value)
		{
			this.Value = value;
		}
	}
}

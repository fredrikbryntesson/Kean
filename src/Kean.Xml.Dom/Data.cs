using System;

namespace Kean.Xml.Dom
{
	public class Data :
		Node
	{
		public string Value { get; set; }
		public Data() { }
		public Data(string value)
		{
			this.Value = value;
		}
	}
}

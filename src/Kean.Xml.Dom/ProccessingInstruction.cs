using System;

namespace Kean.Xml.Dom
{
	public class ProccessingInstruction :
		Node
	{
		public string Target { get; set; }
		public string Value { get; set; }
	}
}

using System;

namespace Kean.Xml.Dom
{
	public class ProcessingInstruction :
		Node
	{
		public string Target { get; set; }
		public string Value { get; set; }
		public ProcessingInstruction() { }
		public ProcessingInstruction(string target) :
			this()
		{
			this.Target = target;
		}
		public ProcessingInstruction(string target, string value) :
			this(target)
		{
			this.Value = value;
		}
	}
}

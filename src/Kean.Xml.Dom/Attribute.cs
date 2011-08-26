using System;

namespace Kean.Xml.Dom
{
	public class Attribute :
		Object
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public Attribute() { }
		public Attribute(string name) :
			this()
		{
			this.Name = name;
		}
		public Attribute(string name, string value) :
			this(name)
		{
			this.Value = value;
		}
	}
}

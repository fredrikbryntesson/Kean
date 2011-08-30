using System;

namespace Kean.Xml.Sax.Reader
{
	class String :
		Abstract
	{
		string data;
		int position;
		public String(string data)
		{
			this.data = data;
		}
		protected override int Read()
		{
			return this.position < this.data.Length ? (int)this.data[this.position++] : -1;
		}
		protected override int Peek()
		{
			return this.position  < this.data.Length ? (int)this.data[this.position] : -1;
		}
	}
}

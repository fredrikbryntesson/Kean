using System;
using Kean.Core.Basis;
using Kean.Core.Basis.Extension;
using Text = System.Text;

namespace Kean.Xml.Sax.Parser
{
	public class String :
		Abstract<string>
	{
		string data;
		int index;
		char current = '\0';
		protected override char Current { get { return this.current; } }
		bool endOfFile = true;
		protected override bool EndOfFile {	get { return this.endOfFile; } }
		protected override bool Next()
		{
			this.current = (this.endOfFile = this.data.IsEmpty() || ++this.index >= this.data.Length) ? '\0' : this.data[this.index];
			return this.endOfFile;
		}
		public override bool Parse(string data)
		{
			this.data = data;
			this.index = -1;
			this.Next();
			return this.Parse();
		}
	}
}

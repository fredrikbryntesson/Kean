using System;
using Kean.Core.Extension;

namespace Kean.Xml.Sax.Reader
{
	class Text :
		Abstract
	{
		System.IO.TextReader reader;

		public event Action Closing;

		public Text(System.IO.TextReader reader)
		{
			this.reader = reader;
		}
		protected override int Read()
		{
			return this.reader.Read();
		}
		protected override int Peek()
		{
			return this.reader.Peek();
		}
		public virtual void Close()
		{
			if (this.reader != null)
			{
				if (this.Closing.NotNull())
					this.Closing();
				this.reader.Close();
				this.reader = null;
			}
		}
		public virtual string[] Resolve(string argument)
		{
			return new string[] { argument };
		}

		public virtual Text Include(string argument)
		{
			return null;
		}
	}
}

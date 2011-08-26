using System;

namespace Kean.Xml.Sax.Reader
{
	abstract class Abstract
	{
		public string Resource { get; protected set; }
		
		int row = 1;
		int column = 0;
		public Position Position { get { return new Position(this.row, this.column); } }

		public bool EndOfFile { get; private set; }
		public char Current { get; private set; }

		protected abstract int Read();
		protected abstract int Peek();
		public bool Next()
		{
			if (this.Current == '\n')
			{
				this.row++;
				this.column = 1;
			}
			else
				this.column++;
			int next = this.Read();
			if (this.EndOfFile = next < 0)
				this.Current = '\0';
			else if (next == '\n' || next == '\r')
			{
				if (this.Peek() == '\n')
					this.Read();
				this.Current = '\n';
			}
			else
				this.Current = (char)next;
			return !this.EndOfFile;
		}
	}
}

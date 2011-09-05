using System;

namespace Kean.IO
{
	public class Reader
	{
		Stream backend;
		
		public int Row { get; private set; }
		public int Column { get; private set; }

		public bool Ended { get; private set; }
		public char Current { get; private set; }

		public Reader(Stream backend)
		{
			this.backend = backend;
			this.Row = 1;
		}

		public bool Next()
		{
			if (this.Current == '\n')
			{
				this.Row++;
				this.Column = 1;
			}
			else
				this.Column++;
			int next = this.backend.Read();
			if (this.Ended = next < 0)
				this.Current = '\0';
			else if (next == '\n' || next == '\r')
			{
				if (this.backend.Peek() == '\n')
					this.backend.Read();
				this.Current = '\n';
			}
			else
				this.Current = (char)next;
			return !this.Ended;
		}
	}
}

using System;

namespace Kean.IO
{
	public class ConsoleStream :
		Stream
	{
		System.IO.Stream output;
		public bool LocalEcho { get; set; }
		public override bool Opened { get { return true; } }
		public override bool Ended { get { return false; } }
		public ConsoleStream()
		{
			this.LocalEcho = true;
			this.output = Console.OpenStandardOutput();
		}
		public override int Read()
		{
			return Console.ReadKey(!this.LocalEcho).KeyChar;
		}
		public override int Peek()
		{
			return -1;
		}
		public override void Write(byte value)
		{
			this.output.WriteByte(value);
		}
		public override void Close()
		{
			this.output.Close();
		}
	}
}

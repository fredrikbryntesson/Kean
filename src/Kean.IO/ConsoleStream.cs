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
			ConsoleKeyInfo key = Console.ReadKey(!this.LocalEcho);
			char result = (char)0;
			switch (key.Key)
			{
				case ConsoleKey.Home: result = (char)1; break;
				case ConsoleKey.LeftArrow: result = (char)2; break;
					// 3 Copy to clipboard
				case ConsoleKey.Delete: result = (char)4; break;
				case ConsoleKey.End: result = (char)5; break;
				case ConsoleKey.RightArrow: result = (char)6; break;
					// 7
				//case ConsoleKey.Backspace: result = (char)8; break;
				//case ConsoleKey.Tab: result = (char)9; break;
				//case ConsoleKey.Enter: result = (char)10; break;
					// 11
					// 12
				case ConsoleKey.DownArrow: result = (char)13; break;
					// 14
				case ConsoleKey.UpArrow: result = (char)15; break;
					// 16
					// 17
					// 18
					// 19
					// 20
					// 21 Insert from clipboard
					// 22
					// 23 Redo
					// 24 Cut to clipboard
					// 25 Undo
				default: result = key.KeyChar; break;
			}
			return result;
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

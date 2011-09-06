using System;
using IO = Kean.IO;

namespace Kean.Cli.LineBuffer.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Editor editor = new Editor(new IO.ConsoleStream() { LocalEcho = false }, Console.Out);
			editor.Read();
		}
	}
}

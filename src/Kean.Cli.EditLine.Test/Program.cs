using System;
using IO = Kean.IO;

namespace Kean.Cli.EditLine.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Editor editor = new Editor(new Command.Application(), new IO.ConsoleStream() { LocalEcho = false }, Console.Out);
			editor.Read();
		}
	}
}

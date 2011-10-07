﻿// 
//  Program.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using IO = Kean.IO;

namespace Kean.Cli.Processor.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Action close = null;
			Command.Application application = new Command.Application(close);
			IO.Net.Tcp.Server server = new IO.Net.Tcp.Server(connection =>
			{
				close += () => { connection.Close(); connection.Dispose(); };
				new Editor(application, new VT100(new IO.CharacterDevice(new IO.Net.Telnet.Server(connection)))).Read();
			}
			, 23);
			close += () => { server.Stop(); server.Dispose(); };
			//Editor editor = null;
			//editor = new Editor(application, new ConsoleTerminal());
			//editor.Read();
		}
	}
}

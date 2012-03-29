// 
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
using Kean.Core.Extension;
using IO = Kean.IO;
using Uri = Kean.Core.Uri;

namespace Kean.Platform.Settings.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Action close = null;
				Command.Application application = new Command.Application(() => close.Call());
				IDisposable telnet = Editor.Listen(application, "telnet://:23");
				IDisposable tcp = Editor.Listen(application, "tcp://:20");
				IDisposable console = Editor.Listen(application, "console:///");
				close += () =>
				{
					telnet.Dispose();
					tcp.Dispose();
					console.Dispose();
				};
			}
			else
			{
				Application application = new Application();
				Settings.Module remote = new Settings.Module() { OpenConsole = true };
				remote.Load("loader", new Loader(remote));
				remote.Load("old.object", new Object());
				application.Load(remote);
				application.Execute();
			}
		}
	}
}

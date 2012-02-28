// 
//  Object.cs
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
using Kean.Core;
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;
using Serialize = Kean.Core.Serialize;
using Argument = Kean.Cli.Argument;

namespace Kean.Platform.Remote
{
	public class Module :
		Platform.Module
	{
		System.IDisposable editor;
		Uri.Locator locator;

		Root root;

		[Serialize.Parameter]
		public bool OpenConsole { get; set; }

		public object this[string name]
		{
			get { return this.root[name]; }
			set { this.root[name] = value; }
		}

		public Module() :
			base("Remote")
		{
			this.root = new Root(this);
		}

		public void Load(string name, object value)
		{
			this.Load(name, null, value);
		}
		public void Load(string name, string description, object value)
		{
			this.Load(name, description, null, value);
		}
		public void Load(string name, string description, string usage, object value)
		{
			this.root.Load(name, description, usage, value);
		}
		public void Unload(string name)
		{
			this.root.Unload(name);
		}

		protected override void AddArguments(Argument.Parser parser)
		{
			parser.Add('r', "remote", argument => this.locator = argument);
			base.AddArguments(parser);
		}
		protected override void Start()
		{
			if (this.locator.NotNull())
			{
				try
				{
					if (this.OpenConsole && this.locator.Scheme == "console" && Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows)
						Module.AllocateConsole();
					this.editor = Settings.Editor.Listen(this.root, this.locator);
				}
				catch { }
			}
			base.Start();
		}
		protected override void Stop()
		{
			base.Stop();
		}
		protected override void Dispose()
		{
			if (editor.NotNull())
				editor.Dispose();
			base.Dispose();
		}
		[System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto, CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
		static extern int AllocateConsole();
	}
}

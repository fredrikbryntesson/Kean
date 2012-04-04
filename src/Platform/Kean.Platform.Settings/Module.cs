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
using Collection = Kean.Core.Collection;

namespace Kean.Platform.Settings
{
	public class Module :
		Platform.Module
	{
		Collection.IList<IDisposable> editors = new Collection.List<IDisposable>();
		Collection.IList<Uri.Locator> configurations = new Collection.List<Kean.Core.Uri.Locator>();
		Collection.IList<Uri.Locator> remotes = new Collection.List<Kean.Core.Uri.Locator>();

		Root root;

		[Serialize.Parameter]
		public bool OpenConsole { get; set; }

		public object this[string name]
		{
			get { return this.root[name]; }
			set { this.root[name] = value; }
		}

		public Module() :
			base("Settings")
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
			parser.Add('c', "config", argument => this.remotes.Add(argument));
			parser.Add('r', "remote", argument => this.remotes.Add(argument));
			base.AddArguments(parser);
		}
		protected override void Start()
		{
			try { Settings.Editor.Listen(this.root, Uri.Locator.FromPlattformPath(this.Application.ExecutablePath + "/" + System.IO.Path.GetFileNameWithoutExtension(this.Application.Executable).Replace(".vshost", "") + ".conf")).Dispose(); }
			catch { }
			try
			{
				foreach (string file in System.IO.Directory.GetFiles(this.Application.ExecutablePath + "/Settings/", "*.conf", System.IO.SearchOption.AllDirectories))
					try { Settings.Editor.Listen(this.root, Uri.Locator.FromPlattformPath(file)).Dispose(); } catch { } 
			}
			catch { }
			try { Settings.Editor.Listen(this.root, Uri.Locator.FromPlattformPath(this.Application.ExecutablePath + "/settings.conf")).Dispose(); } catch { }
			try
			{
				foreach (Uri.Locator locator in this.configurations)
					try { Settings.Editor.Listen(this.root, locator).Dispose(); }
					catch { }
			}
			catch { }
			if (this.remotes.NotNull())
				foreach (Uri.Locator locator in this.remotes)
				{
					try
					{
						if (this.OpenConsole && locator.Scheme == "console" && (Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows))
							Module.AllocateConsole();
						IDisposable editor = Settings.Editor.Listen(this.root, locator);
						if (editor is IO.Net.Tcp.Server)
						{
							(editor as IO.Net.Tcp.Server).ThreadPool.CatchErrors = this.Application.CatchErrors;
							this.Application["Log"].WhenLoaded<Log.Module>(l => (editor as IO.Net.Tcp.Server).ThreadPool.Log += l.Append);
						}
						this.editors.Add(editor);
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
			if (this.editors.NotNull())
			{
				this.editors.Apply(editor => editor.Dispose());
				this.editors = null;
			}
			base.Dispose();
		}
		[System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto, CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
		static extern int AllocateConsole();
	}
}

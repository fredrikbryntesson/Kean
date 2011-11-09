//
//  Application
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 - 2011 Simon Mika
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
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Error = Kean.Core.Error;
using Argument = Kean.Cli.Argument;
using Serialize = Kean.Core.Serialize;

namespace Kean.Platform
{
	public class Application :
		IDisposable
	{
		#region Modules
		[Serialize.Parameter("Module")]
		protected Collection.Hooked.IList<Module> Modules { get; private set; }
		public Module this[string name]
		{
			get
			{
				Module result = this.Modules.Find(item => item.Name == name);
				if (result.IsNull())
					this.Modules.Add(result = new Placeholder(name));
				return result;
			}
			set
			{
				if (value.NotNull())
					this.Modules.Add(value.Merge(this.Modules.Find(item => item.Name == name)));
				else
					this.Modules.Remove(module => module.Name == name);
			}
		}
		#endregion
		#region Properties
		public string Product { get; private set; }
		public string Version { get; private set; }
		public string Company { get; private set; }
		public string Copyright { get; private set; }
		public string Description { get; private set; }
		public System.Drawing.Icon Icon { get; private set; }

		public string[] CommandLine { get { return System.Environment.GetCommandLineArgs(); } }
		public string Executable { get { return System.IO.Path.GetFullPath(this.CommandLine[0]); } }
		public string ExecutablePath { get { return System.IO.Path.GetDirectoryName(this.Executable); } }

		[Serialize.Parameter]
		public bool CatchErrors { get; set; }
		#endregion
		#region Events
		public Action Run { private get; set; }
		object onIdleLock = new object();
		Action onIdle;
		public event Action OnIdle
		{
			add { lock (this.onIdleLock) this.onIdle += value; }
			remove { lock (this.onIdleLock) this.onIdle -= value; }
		}
		object onNextIdleLock = new object();
		Action onNextIdle;
		public event Action OnNextIdle
		{
			add { lock (this.onNextIdleLock) this.onNextIdle += value; }
			remove { lock (this.onNextIdleLock) this.onNextIdle -= value; }
		}
		object onClosedLock = new object();
		Action onClosed;
		public event Action OnClosed
		{
			add { lock (this.onClosedLock) this.onClosed += value; }
			remove { lock (this.onClosedLock) this.onClosed -= value; }
		}
		object onErrorLock = new object();
		Action<Exception> onError;
		public event Action<Exception> OnError
		{
			add { lock (this.onErrorLock) this.onError += value; }
			remove { lock (this.onErrorLock) this.onError -= value; }
		}
		#endregion
		#region Constructors
		protected Application()
		{
			#region Initialize Properties
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly() ?? System.Reflection.Assembly.GetCallingAssembly();
            // Product
            System.Reflection.AssemblyProductAttribute[] productAttribute = (System.Reflection.AssemblyProductAttribute[])assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyProductAttribute), true);
            if (productAttribute != null && productAttribute.Length > 0)
                this.Product = productAttribute[0].Product;
            // Version
            System.Reflection.AssemblyFileVersionAttribute version = Attribute.GetCustomAttribute(assembly, typeof(System.Reflection.AssemblyFileVersionAttribute)) as System.Reflection.AssemblyFileVersionAttribute;
            if (version != null)
                this.Version = version.Version;
            // Company
            System.Reflection.AssemblyCompanyAttribute[] companyAttribute = (System.Reflection.AssemblyCompanyAttribute[]) assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyCompanyAttribute), true);
            if (companyAttribute != null && companyAttribute.Length > 0)
                this.Company = companyAttribute[0].Company;
            // Copyright
            System.Reflection.AssemblyCopyrightAttribute copyright = Attribute.GetCustomAttribute(assembly, typeof(System.Reflection.AssemblyCopyrightAttribute)) as System.Reflection.AssemblyCopyrightAttribute;
            if (copyright != null)
                this.Copyright = copyright.Copyright;
            // Description
            System.Reflection.AssemblyDescriptionAttribute description = Attribute.GetCustomAttribute(assembly, typeof(System.Reflection.AssemblyDescriptionAttribute)) as System.Reflection.AssemblyDescriptionAttribute;
            if (description.NotNull())
                this.Description = description.Description;
            // Icon
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(this.Executable);
#if !DEBUG
			this.CatchErrors = true;
#endif
			#endregion

		}
		#endregion
		#region Public Methods
		public virtual bool Close()
		{
			return true;
		}
		void Executer()
		{
			Argument.Parser parser = new Argument.Parser();
			parser.Add('v', "version", () =>
			{
				Console.WriteLine("Product: " + this.Product);
				Console.WriteLine("Version: " + this.Version);
				Console.WriteLine("Company: " + this.Company);
				Console.WriteLine("Copyright: " + this.Copyright);
				Console.WriteLine("Description: " + this.Description);
			});
			parser.Add('d', "debug", () => this.CatchErrors = false);
			foreach (Module module in this.Modules)
				module.AddArguments(parser);
			parser.Parse(this.CommandLine);
			foreach (Module module in this.Modules)
				if (module.Mode == Mode.Created)
					module.Initialize();
			foreach (Module module in this.Modules)
				if (module.Mode == Mode.Initialized)
					module.Start();
			this.Run.Call();
			foreach (Module module in this.Modules)
				if (module.Mode == Mode.Started)
					module.Stop();
			foreach (Module module in this.Modules)
				if (module.Mode == Mode.Stopped)
					module.Dispose();
		}
		public void Execute()
		{
			if (this.CatchErrors)
			{
				try
				{
					this.Executer();
				}
				catch(Exception e)
				{
					this.onError.Call(e);
				}
				finally
				{
					this.onClosed.Call();
				}
			}
			else
			{
				this.Executer();
				this.onClosed.Call();
			}
		}
		public void Idle()
		{
			Action onNextIdle;
			lock (this.onNextIdleLock)
			{
				onNextIdle = this.onNextIdle;
				this.onNextIdle = null;
			}
			onNextIdle.Call();
			lock (this.onIdleLock)
				this.onIdle.Call();
		}
		#endregion
		#region IDisposable Members
		~Application()
		{
			(this as IDisposable).Dispose();
		}
		void  IDisposable.Dispose()
		{
			throw new NotImplementedException();
		}
		#endregion
}
}

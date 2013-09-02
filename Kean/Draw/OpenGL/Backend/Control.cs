// 
//  Control.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
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
using Geometry2D = Kean.Math.Geometry2D;
using GL = OpenTK.Graphics.OpenGL.GL;
using Parallel = Kean.Core.Parallel;
using Error = Kean.Core.Error;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Control :
		OpenTK.GLControl
	{
		public event Action Initialized;
		public event Action Draw;

		public Context Context { get; private set; }

		ThreadPool threadPool;
		public Parallel.ThreadPool ThreadPool { get { return this.threadPool; } }

		bool designMode;
		protected new bool DesignMode 
		{ 
			get { return this.designMode || base.DesignMode || System.Diagnostics.Process.GetCurrentProcess().ProcessName.StartsWith("devenv") || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime; }
			set { this.designMode = value; }
		}
		#region AutoStart
		[System.ComponentModel.DefaultValue(true)]
		public bool AutoStart { get; set; }
		#endregion

		bool loaded;
		protected Control(Context context) :
			base()
		{
			this.Context = context;
		}
		protected Control()
		{
			this.AutoStart = true;
			this.SuspendLayout();
			// 
			// Viewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.Name = "Viewer";
			this.ResumeLayout(false);
			this.DesignMode = this.DesignMode;
		}
		public bool Initialize()
		{
			bool result;
			if (result = !this.DesignMode && !this.loaded)
			{
				OpenTK.Platform.IWindowInfo window;
				try
				{
					this.loaded = true;
					if (!this.IsHandleCreated)
						this.CreateHandle();
					window = this.WindowInfo;
				}
				catch
				{
					window = null;
					this.loaded = false;
				}
				if (result = this.loaded)
				{
					Backend.Context.Current = this.Context = this.Context ?? Backend.Context.Current ?? this.CreateContext();
					this.threadPool = this.CreateThreadPool("OpenGL", 8);
					this.SetupViewport();
					this.Initialized.Call();
				}
			}
			return result;
		}
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint(e);
			Error.Log.Wrap(() =>
			{
				if (!this.DesignMode)
				{
					if (!this.loaded && this.AutoStart)
						this.Initialize();
					if (this.loaded)
					{
						this.Clear();
						this.Draw.Call();
						this.SwapBuffers();
					}
				}
			})();
		}
		protected abstract Context CreateContext();
		protected abstract ThreadPool CreateThreadPool(string name, int workers);
		protected abstract void SetupViewport();
		protected abstract void Clear();
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Error.Log.Wrap(() =>
			{
				if (this.loaded)
					this.SetupViewport();
			})();
		}
		protected override void Dispose(bool disposing)
		{
			if (this.threadPool.NotNull())
			{
				this.threadPool.Delete();
				this.threadPool = null;
			} 
			base.Dispose(disposing);
		}
		public static Control Create()
		{
			// TODO: select OpenGL implementation
			return new OpenGL21.Control();
		}
	}
}

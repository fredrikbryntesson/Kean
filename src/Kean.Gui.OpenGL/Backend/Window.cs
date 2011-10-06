// 
//  Window.cs
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
using Error = Kean.Core.Error;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Core.Extension;
using GL = OpenTK.Graphics.OpenGL.GL;
using Parallel = Kean.Core.Parallel;
using Gpu = Kean.Draw.Gpu;
using Raster = Kean.Draw.Raster;

namespace Kean.Gui.OpenGL.Backend
{
	abstract class Window :
		OpenTK.NativeWindow
	{
		object @lock = new object();
		System.Threading.EventWaitHandle redrawSignal;
		public Parallel.ThreadPool ThreadPool { get; private set; }
		OpenTK.Graphics.IGraphicsContext mainContext;
		public event Action Render;
		public string Clipboard
		{
			get { return System.Windows.Forms.Clipboard.GetText(); }
			set
			{
				if (value.NotNull() && value.Length > 0)
					System.Windows.Forms.Clipboard.SetText(value);
			}
		}
		bool exit;
		public bool Exit
		{
			get { lock (this.@lock) return this.exit; }
			set { lock (this.@lock) this.exit = true; this.Redraw(); }
		}
		protected Window(Geometry2D.Integer.Size size, string title, OpenTK.GameWindowFlags options, OpenTK.Graphics.GraphicsMode mode, OpenTK.DisplayDevice device) :
			base(size.Width, size.Height, title, options, mode, device)
		{
			this.redrawSignal = new System.Threading.EventWaitHandle(true, System.Threading.EventResetMode.AutoReset);
			this.ThreadPool = new ThreadPool(this.WindowInfo, w =>
			{
				OpenTK.Graphics.GraphicsContext result = this.CreateContext(w);
				if (this.mainContext.IsNull())
					this.mainContext = result;
				return result;
			} , "OpenGL", 8);
			this.InitializeGpu();
		}
		protected abstract OpenTK.Graphics.GraphicsContext CreateContext(OpenTK.Platform.IWindowInfo windowInformation);
		public abstract Gpu.Backend.IImage CreateImage();
		protected virtual void InitializeGpu()
		{
			GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
			GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
			GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.SrcAlpha, OpenTK.Graphics.OpenGL.BlendingFactorDest.OneMinusSrcAlpha);
		}
		public override void Dispose()
		{
			if (this.ThreadPool.NotNull())
			{
				this.ThreadPool.Dispose();
				this.ThreadPool = null;
			}
			base.Dispose();
		}
		public void Run()
		{
			try
			{
				this.Runner();
			}
			finally
			{
				this.Dispose();
			}
		}
		protected virtual void Runner()
		{
			this.Visible = true;
			this.OnResize(System.EventArgs.Empty);
			bool redraw = true;
			while (this.Exists && !this.Exit)
			{
				if (redraw)
				{
					this.Render.Call();
					if (this.mainContext.NotNull())
					{
						this.mainContext.VSync = false;
						this.mainContext.SwapBuffers();
					}
				}
				base.ProcessEvents();
				redraw = this.redrawSignal.WaitOne(20);
			}
		}
		/// <summary>
		/// If called, Draw will be called during next rendering callback.
		/// </summary>
		public void Redraw()
		{
			this.redrawSignal.Set();
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.mainContext.NotNull())
			{
				this.mainContext.Update(this.WindowInfo);
				this.redrawSignal.Set();
			}
		}
		protected override void OnFocusedChanged(EventArgs e)
		{
			base.OnFocusedChanged(e);
			this.redrawSignal.Set();
		}
	}
}

using System;
using Error = Kean.Core.Error;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Core.Extension;
using GL = OpenTK.Graphics.OpenGL.GL;
using Parallel = Kean.Core.Parallel;

namespace Kean.Gui.OpenGL.Backend
{
	abstract class Window :
		OpenTK.NativeWindow
	{
		object @lock = new object();
		System.Threading.EventWaitHandle redrawSignal;
		public Parallel.ThreadPool ThreadPool { get; private set; }
		OpenTK.Graphics.IGraphicsContext mainContext;

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
			this.ThreadPool = new ThreadPool(this.WindowInfo, this.CreateContext, "OpenGL", 8);
		}
		protected abstract OpenTK.Graphics.GraphicsContext CreateContext(OpenTK.Platform.IWindowInfo windowInformation);
		public override void Dispose()
		{
			if (this.ThreadPool != null)
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
			bool redraw = false;
			while (this.Exists && !this.Exit)
			{
				if (redraw)
				{
					this.Draw();
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

		protected virtual void Draw()
		{
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
			this.mainContext.Update(this.WindowInfo);
			this.redrawSignal.Set();
		}
		protected override void OnFocusedChanged(EventArgs e)
		{
			base.OnFocusedChanged(e);
			this.redrawSignal.Set();
		}
	}
}

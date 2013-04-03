using System;
using Error = Kean.Core.Error;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Core.Extension;
using GL = OpenTK.Graphics.OpenGL.GL;
using Parallel = Kean.Core.Parallel;

namespace Kean.Draw.OpenGL
{
	public class ThreadPool :
		Parallel.ThreadPool,
		IDisposable
	{
		OpenTK.Graphics.IGraphicsContext[] contexts;
		public OpenTK.Graphics.IGraphicsContext MainContext
		{
			get { return this.contexts[0]; }
		}
		public ThreadPool(OpenTK.Platform.IWindowInfo windowInformation, string name, int workers) :
			base(name, workers)
		{
			Backend.Context.Current = new Backend.OpenGL21.Context();
			OpenTK.Graphics.GraphicsContext.ShareContexts = true;
			this.contexts = new OpenTK.Graphics.GraphicsContext[workers + 1];
			try
			{
				this.contexts[0] = new OpenTK.Graphics.GraphicsContext(OpenTK.Graphics.GraphicsMode.Default, windowInformation, 2, 1, OpenTK.Graphics.GraphicsContextFlags.Default);
				this.contexts[0].MakeCurrent(windowInformation);
				this.contexts[0].VSync = false;
				(this.contexts[0] as OpenTK.Graphics.IGraphicsContextInternal).LoadAll();

				for (int i = 1; i < this.contexts.Length; i++)
					this.contexts[i] = new OpenTK.Graphics.GraphicsContext(OpenTK.Graphics.GraphicsMode.Default, windowInformation, 2, 1, OpenTK.Graphics.GraphicsContextFlags.Default);
				int j = 1;
				this.ForEachWorker(() =>
				{
					lock (contexts)
						this.contexts[j++].MakeCurrent(windowInformation);
				});
			}
			catch (System.Exception e)
			{
				this.Dispose();
				new Exception.ContextNotCreatable(e).Throw();
			}
		}

		#region IDisposable Members
		public void Dispose()
		{
			if (this.contexts.NotNull())
			{
				foreach (OpenTK.Graphics.GraphicsContext context in this.contexts)
                    if (context.NotNull())
    					context.Dispose();
				this.contexts = null;
				base.Dispose();
			}
		}
		#endregion
	}
}

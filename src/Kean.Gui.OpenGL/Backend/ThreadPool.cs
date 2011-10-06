// 
//  ThreadPool.cs
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

namespace Kean.Gui.OpenGL
{
	class ThreadPool :
		Parallel.ThreadPool,
		IDisposable
	{
		OpenTK.Graphics.IGraphicsContext[] contexts;
		public ThreadPool(OpenTK.Platform.IWindowInfo windowInformation, Func<OpenTK.Platform.IWindowInfo, OpenTK.Graphics.GraphicsContext> createContext, string name, int workers) :
			base(name, workers)
		{
			OpenTK.Graphics.GraphicsContext.ShareContexts = true;
			this.contexts = new OpenTK.Graphics.GraphicsContext[workers + 1];
			try
			{
				this.contexts[0] = createContext(windowInformation);
				this.contexts[0].MakeCurrent(windowInformation);
				this.contexts[0].VSync = false;
				(this.contexts[0] as OpenTK.Graphics.IGraphicsContextInternal).LoadAll();

				for (int i = 1; i < this.contexts.Length; i++)
					this.contexts[i] = createContext(windowInformation);;
				int j = 1;
				this.ForEachWorker(() =>
				{
					lock (contexts)
						this.contexts[j++].MakeCurrent(windowInformation);
				});
			}
			catch (System.Exception e)
			{
				base.Dispose();
				new Exception.ContextNotCreatable(e).Throw();
			}
		}

		#region IDisposable Members
		public void Dispose()
		{
			if (this.contexts.NotNull())
			{
				foreach (OpenTK.Graphics.GraphicsContext context in this.contexts)
					context.Dispose();
				this.contexts = null;
			}
		}
		#endregion
	}
}
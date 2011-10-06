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
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Parallel = Kean.Core.Parallel;
using Kean.Gui.OpenGL.Backend.Extension;
using Gpu = Kean.Draw.Gpu;

namespace Kean.Gui.OpenGL
{
	public class Window :
		Gui.Backend.IWindow
	{
		class Image :
			Gpu.Bgra
		{
			public Image(Gpu.Backend.IImage backend) :
				base(backend)
			{ }
		}
		Backend.Window target;
		Backend.Window Target
		{
			get { return this.target; }
			set 
			{
				if (this.target.IsNull() && value.NotNull())
				{
					this.target = value;

					this.target.IconChanged += (sender, e) => this.IconChanged.Call(this.Icon);
					this.target.TitleChanged += (sender, e) => this.TitleChanged.Call(this.Title);
					this.target.Move += (sender, e) => this.PositionChanged.Call(this.Position);
					this.target.Resize += (sender, e) => this.SizeChanged.Call(this.Size);
					this.target.WindowStateChanged += (sender, e) => this.StateChanged.Call(this.State);
					this.target.WindowBorderChanged += (sender, e) => this.BorderChanged.Call(this.Border);
					this.target.VisibleChanged += (sender, e) => this.VisibleChanged.Call(this.Visible);

					this.target.InputDriver.Keyboard[0].KeyRepeat = true;
					this.Keyboard = new Backend.Input.Keyboard(this.target);
					this.Pointer = new Backend.Input.Pointer(this.target);
					this.target.Render += () => this.Render(this.image.Canvas);
				}
			}
		}
		Image image;
		bool invalidated;
		bool validClose;

		public Window(Geometry2D.Integer.Size size, string title)
		{
			this.Target = new Backend.OpenGL21.Window(size, title);
			Draw.Gpu.Backend.Factory.Implemetation = new Backend.OpenGL21.Factory();
			this.image = new Image(this.Target.CreateImage());
			this.VisibleChanged += v => { if (v) this.Invalidate(); };
		}
		~Window()
		{
			this.Dispose();
		}

		#region IWindow Members
		public Gui.Backend.Pointer Pointer { get; private set; }
		public Gui.Backend.Keyboard Keyboard { get; private set; }
		public Gui.Backend.IClipboard Clipboard { get; private set; }
		public Parallel.ThreadPool ThreadPool { get; private set; }
		#region Icon
		[Notify("IconChanged")]
		public System.Drawing.Icon Icon
		{
			get { return this.Target.Icon; }
			set { this.Target.Icon = value; }
		}
		public event Action<System.Drawing.Icon> IconChanged;
		#endregion
		#region Title
		[Notify("TitleChanged")]
		public string Title
		{
			get { return this.Target.Title; }
			set { this.Target.Title = value; }
		}
		public event Action<string> TitleChanged;
		#endregion
		#region Position
		[Notify("PositionChanged")]
		public Geometry2D.Single.Point Position
		{
			get { return new Geometry2D.Single.Point(this.Target.Location.X, this.Target.Location.Y); }
			set { this.Target.Location = new System.Drawing.Point((int)value.X, (int)value.Y); }
		}
		public event Action<Geometry2D.Single.Point> PositionChanged;
		public void StartMove()
		{
			this.target.StartMove();
		}
		#endregion
		#region Size
		[Notify("SizeChanged")]
		public Geometry2D.Single.Size Size
		{
			get { return new Geometry2D.Single.Size(this.Target.Size.Width, this.Target.Size.Height); }
			set { this.target.Size = new System.Drawing.Size((int)value.Width ,(int)value.Height); }
		}
		public event Action<Geometry2D.Single.Size> SizeChanged;
		public void StartResize(Gui.Backend.ResizeDirection direction)
		{
			this.target.StartResize(direction.AsOpenTK());
		}
		#endregion
		#region State
		public WindowState State
		{
			get { return this.target.WindowState.AsKean(); }
			set { this.target.WindowState = value.AsOpenTK(); }
		}
		public event Action<WindowState> StateChanged;
		#endregion
		#region Border
		public WindowBorder Border
		{
			get { return this.target.WindowBorder.AsKean(); }
			set { this.target.WindowBorder = value.AsOpenTK(); }
		}
		public event Action<WindowBorder> BorderChanged;
		#endregion
		#region Visible
		public bool Visible
		{
			get { return this.target.Visible; }
			set { this.target.Visible = value; }
		}
		public event Action<bool> VisibleChanged;
		#endregion
		public event Action<Draw.Canvas> Render;
		public void Run()
		{
			this.Target.Run();
		}
		public void Invalidate()
		{
			this.invalidated = true;
			this.target.Redraw();
		}
		object idlingLock = new object();
		Action idling;
		public event Action OnIdle
		{
			add { lock (this.idlingLock) this.idling += value; }
			remove { lock (this.idlingLock) this.idling -= value; }
		}
		object nextIdlingLock = new object();
		Action nextIdling;
		public event Action OnNextIdle
		{
			add { lock (this.nextIdlingLock) this.nextIdling += value; }
			remove { lock (this.nextIdlingLock) this.nextIdling -= value; }
		}

		public bool Close()
		{
			bool result = this.Closing.AllTrue();
			if (result)
			{
				this.validClose = true;
				this.target.Exit = true;
			}
			return result;
		}
		public event Action OnClosed;
		public event Func<bool> Closing;
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			if (this.Target.NotNull())
			{
				this.Target.Dispose();
				this.Target = null;
			}
		}
		#endregion
	}
}

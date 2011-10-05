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

namespace Kean.Gui.OpenGL
{
	public class Window :
		Gui.Backend.IWindow
	{
		Backend.Window target;
		Backend.Window Target
		{
			get { return this.target; }
			set 
			{
				this.target = value;
				if (this.target.NotNull())
				{
					this.target.Move += (sender, e) => this.PositionChanged.Call(this.Position);
					this.target.Resize += (sender, e) => this.SizeChanged.Call(this.Size);
				}
			}
		}

		bool invalidated;
		bool validClose;

		public Window(Geometry2D.Single.Size size, string title)
		{
			this.Target = new Backend.OpenGL21.Window(size, title);
			Draw.Gpu.Backend.Factory.Implemetation = new Backend.OpenGL21.Factory();
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

		public System.Drawing.Icon Icon
		{
			get { return this.Target.Icon; }
			set { this.Target.Icon = value; }
		}
		public string Title
		{
			get { return this.Target.Title; }
			set { this.Target.Title = value; }
		}
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
		public Geometry2D.Single.Size Size
		{
			get { return new Geometry2D.Single.Size(this.Target.Size.Width, this.Target.Size.Height); }
			set { this.target.Size = new System.Drawing.Size((int)value.Width ,(int)value.Height); }
		}
		public event Action<Geometry2D.Single.Size> SizeChanged;
		public void StartResize(Gui.Backend.ResizeDirection direction)
		{
			switch (direction)
			{
				case Gui.Backend.ResizeDirection.Left:
					this.target.StartResize(OpenTK.ResizeDirection.Left);
					break;
				case Gui.Backend.ResizeDirection.Right:
					this.target.StartResize(OpenTK.ResizeDirection.Right);
					break;
				case Gui.Backend.ResizeDirection.Top:
					this.target.StartResize(OpenTK.ResizeDirection.Top);
					break;
				case Gui.Backend.ResizeDirection.Bottom:
					this.target.StartResize(OpenTK.ResizeDirection.Bottom);
					break;
				case Gui.Backend.ResizeDirection.LeftTop:
					this.target.StartResize(OpenTK.ResizeDirection.LeftTop);
					break;
				case Gui.Backend.ResizeDirection.LeftBottom:
					this.target.StartResize(OpenTK.ResizeDirection.LeftBottom);
					break;
				case Gui.Backend.ResizeDirection.RightTop:
					this.target.StartResize(OpenTK.ResizeDirection.RightTop);
					break;
				case Gui.Backend.ResizeDirection.RightBottom:
					this.target.StartResize(OpenTK.ResizeDirection.RightBottom);
					break;
			}
		}

		public WindowState State
		{
			get
			{
				Gui.WindowState result;
				switch (this.target.WindowState)
				{
					default:
					case OpenTK.WindowState.Normal:
						result = Gui.WindowState.Normal;
						break;
					case OpenTK.WindowState.Minimized:
						result = Gui.WindowState.Minimized;
						break;
					case OpenTK.WindowState.Maximized:
						result = Gui.WindowState.Maximized;
						break;
					case OpenTK.WindowState.Fullscreen:
						result = Gui.WindowState.Fullscreen;
						break;
				}
				return result;
			}
			set
			{
				if (value != this.State)
				{
					switch (value)
					{
						case Gui.WindowState.Normal:
							this.target.WindowState = OpenTK.WindowState.Normal;
							break;
						case Gui.WindowState.Minimized:
							this.target.WindowState = OpenTK.WindowState.Minimized;
							break;
						case Gui.WindowState.Maximized:
							this.target.WindowState = OpenTK.WindowState.Maximized;
							break;
						case Gui.WindowState.Fullscreen:
							this.target.WindowState = OpenTK.WindowState.Fullscreen;
							break;
					}
					this.StateChanged.Call(value);
				}
			}
		}
		public event Action<WindowState> StateChanged;

		public event Action<Gui.Backend.ICanvas> Render;
		public void Invalidate()
		{
			this.invalidated = true;
			this.target.Redraw();
		}

		public void Run()
		{
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

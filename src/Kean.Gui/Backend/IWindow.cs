// 
//  IWindow.cs
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
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Parallel = Kean.Core.Parallel;
using Kean.Core.Extension;

namespace Kean.Gui.Backend
{
	public interface IWindow :
		IDisposable
	{
		[Notify("IconChanged")]
		System.Drawing.Icon Icon { get; set; }
		event Action<System.Drawing.Icon> IconChanged;
		[Notify("TitleChanged")]
		string Title { get; set; }
		event Action<string> TitleChanged;
		[Notify("PositionChanged")]
		Geometry2D.Single.Point Position { get; set; }
		event Action<Geometry2D.Single.Point> PositionChanged;
		[Notify("SizeChanged")]
		Geometry2D.Single.Size Size { get; set; }
		event Action<Geometry2D.Single.Size> SizeChanged;
		[Notify("StateChanged")]
		WindowState State { get; set; }
		event Action<WindowState> StateChanged;
		[Notify("BorderChanged")]
		WindowBorder Border { get; set; }
		event Action<WindowBorder> BorderChanged;
		[Notify("VisibleChanged")]
		bool Visible { get; set; }
		event Action<bool> VisibleChanged;

		Pointer Pointer { get; }
		Keyboard Keyboard { get; }
		IClipboard Clipboard { get; }
		Parallel.ThreadPool ThreadPool { get; }

		event Action<Draw.Canvas> Render;
		void Invalidate();
		void Run();
		bool Close();

		void StartMove();
		void StartResize(ResizeDirection direction);

		event Action OnClosed;
		event Func<bool> Closing;
		event Action OnNextIdle;
		event Action OnIdle;
	}
}

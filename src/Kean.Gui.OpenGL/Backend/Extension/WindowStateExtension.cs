// 
//  WindowStateExtension.cs
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
using Geometry2D = Kean.Math.Geometry2D;
using GL = OpenTK.Graphics.OpenGL.GL;
using Error = Kean.Core.Error;
using Log = Kean.Extra.Log;
using Draw = Kean.Draw;
using Gpu = Kean.Draw.Gpu;
using Raster = Kean.Draw.Raster;
using Kean.Gui.OpenGL.Backend.Extension;

namespace Kean.Gui.OpenGL.Backend.Extension
{
	public static class WindowStateExtension
	{
		public static Gui.WindowState AsKean(this OpenTK.WindowState me)
		{
			Gui.WindowState result;
			switch (me)
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
		public static OpenTK.WindowState AsOpenTK(this Gui.WindowState me)
		{
			OpenTK.WindowState result;
			switch (me)
			{
				default:
				case Gui.WindowState.Normal:
					result = OpenTK.WindowState.Normal;
					break;
				case Gui.WindowState.Minimized:
					result = OpenTK.WindowState.Minimized;
					break;
				case Gui.WindowState.Maximized:
					result = OpenTK.WindowState.Maximized;
					break;
				case Gui.WindowState.Fullscreen:
					result = OpenTK.WindowState.Fullscreen;
					break;
			}
			return result;
		}
	}
}

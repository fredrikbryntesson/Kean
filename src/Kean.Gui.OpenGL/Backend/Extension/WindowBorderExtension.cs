﻿// 
//  WindowBorderExtension.cs
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
	public static class WindowBorderExtension
	{
		public static Gui.WindowBorder AsKean(this OpenTK.WindowBorder me)
		{
			Gui.WindowBorder result;
			switch (me)
			{
				default:
				case OpenTK.WindowBorder.Resizable:
					result = Gui.WindowBorder.Resizable;
					break;
				case OpenTK.WindowBorder.Fixed:
					result = Gui.WindowBorder.Fixed;
					break;
				case OpenTK.WindowBorder.Hidden:
					result = Gui.WindowBorder.Hidden;
					break;
			}
			return result;
		}
		public static OpenTK.WindowBorder AsOpenTK(this Gui.WindowBorder me)
		{
			OpenTK.WindowBorder result;
			switch (me)
			{
				default:
				case Gui.WindowBorder.Resizable:
					result = OpenTK.WindowBorder.Resizable;
					break;
				case Gui.WindowBorder.Fixed:
					result = OpenTK.WindowBorder.Fixed;
					break;
				case Gui.WindowBorder.Hidden:
					result = OpenTK.WindowBorder.Hidden;
					break;
			}
			return result;
		}
	}
}

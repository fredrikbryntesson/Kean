// 
//  Map.cs
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
using Gpu = Kean.Draw.Gpu;

namespace Kean.Gui.OpenGL
{
	public abstract class Map<T, Result> :
		Gpu.Map<T, Result>
		where T : Gpu.Image
		where Result : Gpu.Image
	{ }
	public abstract class Map<T1, T2, Result> :
		Gpu.Map<T1, T2, Result>
		where T1 : Gpu.Image
		where T2 : Gpu.Image
		where Result : Gpu.Image
	{ }
	public abstract class Map<T1, T2, T3, Result> :
		Gpu.Map<T1, T2, T3, Result>
		where T1 : Gpu.Image
		where T2 : Gpu.Image
		where T3 : Gpu.Image
		where Result : Gpu.Image
	{ }
}

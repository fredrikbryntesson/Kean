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

namespace Kean.Draw.Gpu
{
	public abstract class Map<T, Result> :
		Draw.Map<T, Result>
		where T : Image
		where Result : Image
	{ }
	public abstract class Map<T1, T2, Result> :
		Draw.Map<T1, T2, Result>
		where T1 : Image
		where T2 : Image
		where Result : Image
	{ }
	public abstract class Map<T1, T2, T3, Result> :
		Draw.Map<T1, T2, T3, Result>
		where T1 : Image
		where T2 : Image
		where T3 : Image
		where Result : Image
	{ }
}

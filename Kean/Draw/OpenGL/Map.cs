// 
//  Map.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
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
using Kean;
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.OpenGL
{
	public class Map :
		Draw.Map
	{
		protected internal Backend.Program Backend { get; private set; }
		public Map(Backend.Program backend)
		{
			this.Backend = backend;
		}
		public override void Dispose()
		{
			if (this.Backend.NotNull())
			{
				this.Backend.Dispose();
				this.Backend = null;
			}
			base.Dispose();
		}
		public static Map MonochromeToBgr { get { return new Map(OpenGL.Backend.Context.Current.GetProgram(OpenGL.Backend.Programs.MonochromeToBgr)); } }
		public static Map BgrToMonochrome { get { return new Map(OpenGL.Backend.Context.Current.GetProgram(OpenGL.Backend.Programs.BgrToMonochrome)); } }
		public static Map BgrToU { get { return new Map(OpenGL.Backend.Context.Current.GetProgram(OpenGL.Backend.Programs.BgrToU)); } }
		public static Map BgrToV { get { return new Map(OpenGL.Backend.Context.Current.GetProgram(OpenGL.Backend.Programs.BgrToV)); } }
		public static Map Yuv420ToBgr { get { return new Map(OpenGL.Backend.Context.Current.GetProgram(OpenGL.Backend.Programs.Yuv420ToBgr)); } }

		public static Map Create(string fragment)
		{
			return Map.Create(null, fragment);
		}
		public static Map Create(string vertex, string fragment)
		{
			return new Map(OpenGL.Backend.Context.Current.CreateProgram(vertex, fragment));
		}
	}
}

// 
//  Depth.cs
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
using Log = Kean.Platform.Log;
using Draw = Kean.Draw;
using Gpu = Kean.Draw.Gpu;
using Raster = Kean.Draw.Raster;
using Kean.Gui.OpenGL.Backend.Extension;

namespace Kean.Gui.OpenGL.Backend.OpenGL21
{
	internal class Depth :
		Texture
	{

		internal Depth(Factory factory, Geometry2D.Integer.Size size) :
			base(factory, Gpu.Backend.TextureType.Bgra, size, Draw.CoordinateSystem.Default)
		{ }
		protected override void Load(Gpu.Backend.TextureType type, Geometry2D.Integer.Size size, IntPtr data)
		{
			GL.TexImage2D(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0, (OpenTK.Graphics.OpenGL.PixelInternalFormat)OpenTK.Graphics.OpenGL.All.DepthComponent32, size.Width, size.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, OpenTK.Graphics.OpenGL.PixelType.UnsignedInt, IntPtr.Zero);
		}
	}
}

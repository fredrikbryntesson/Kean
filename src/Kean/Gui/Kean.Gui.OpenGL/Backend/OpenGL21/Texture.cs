// 
//  Image.cs
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
	public class Texture :
		OpenGL.Backend.Texture
	{

		internal Texture(Factory factory, Gpu.Backend.TextureType type, Geometry2D.Integer.Size size, Draw.CoordinateSystem coordinateSystem) :
			base(factory, type, size, coordinateSystem)
		{ }
		internal Texture(Factory factory, Raster.Image image) :
			base(factory, image.GetImageType(), image.Size, image.CoordinateSystem, image.Pointer)
		{ }
		internal Texture(Backend.Texture original, Raster.Image image) :
			base(original, image.CoordinateSystem, image.Pointer)
		{ }
		internal Texture(Backend.Texture original, Draw.CoordinateSystem coordinateSystem) :
			base(original, coordinateSystem)
		{ }

		protected override Backend.FrameBuffer CreateCanvas()
		{
			return new FrameBuffer(this);
		}
		protected override Backend.Texture Create()
		{
			return new Texture(this.Factory as Factory, this.Type, this.Size, this.CoordinateSystem);
		}
	}
}

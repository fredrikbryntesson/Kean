// 
//  Factory.cs
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

namespace Kean.Draw.Gpu.Backend
{
	public static class Factory
	{
		public static IFactory Implemetation { get; set; }
		public static ITexture CreateImage(TextureType type, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem)
		{
			return Factory.Implemetation.CreateImage(type, size, coordinateSystem);
		}
		public static ITexture CreateImage(Draw.Raster.Image image)
		{
			return Factory.Implemetation.CreateImage(image);
		}
        public static IShader ConvertMonochromeToBgr { get { return Factory.Implemetation.ConvertMonochromeToBgr; } }
        public static IShader ConvertBgrToMonochrome { get { return Factory.Implemetation.ConvertBgrToMonochrome; } }
		public static IShader ConvertBgrToU { get { return Factory.Implemetation.ConvertBgrToU; } }
		public static IShader ConvertBgrToV { get { return Factory.Implemetation.ConvertBgrToV; } }
		public static IShader ConvertBgrToYuv420 { get { return Factory.Implemetation.ConvertBgrToYuv420; } }
		public static IShader ConvertYuv420ToBgr { get { return Factory.Implemetation.ConvertYuv420ToBgr; } }
	}
}

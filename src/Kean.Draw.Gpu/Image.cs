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
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Gpu
{
	public abstract class Image :
		Draw.Image
	{
		#region Constructors
		protected Image(Planar original) :
			base(original) { }
		protected Image(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(size, coordinateSystem)
		{ }
		#endregion
		#region Draw.Image Overrides
		public override T Convert<T>()
		{
			T result = null;
			Type type = typeof(T);
			if (type == this.GetType())
				result = this.Copy() as T;
			else
				if (type.IsSubclassOf(typeof(Packed)))
				{
					if (type == typeof(Bgra))
						result = new Bgra(this) as T;
					else if (type == typeof(Monochrome))
						result = new Monochrome(this) as T;
				}
				else if (type == typeof(Yuv420))
					result = new Yuv420(this) as T;
			return result;
		}
		#endregion
		#region Static Creators
		public static Image Create(Draw.Image image)
		{
			Image result = null;
			if (image is Raster.Image)
			{
				if (image is Raster.Bgra)
					result = new Bgra(image as Raster.Bgra);
				else if (image is Raster.Monochrome)
					result = new Monochrome(image as Raster.Monochrome);
				else
					result = new Bgra(image.Convert<Raster.Bgra>());
			}
			else if (image is Gpu.Image)
				result = image.Copy() as Gpu.Image;
			else
				result = new Bgra(image.Convert<Raster.Bgra>());
			return result;
		}
		#endregion
	}
}

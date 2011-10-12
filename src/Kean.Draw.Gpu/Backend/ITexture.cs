// 
//  IImage.cs
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
namespace Kean.Draw.Gpu.Backend
{
	public interface ITexture :
		IDisposable
	{
		IFrameBuffer FrameBuffer { get; }
		IFactory Factory { get; }

		bool Wrap { get; set; }
		CoordinateSystem CoordinateSystem { get; set; }
		Geometry2D.Integer.Size Size { get; }
		TextureType Type { get; }
		
		void Load(Geometry2D.Integer.Point offset, Raster.Image image);
		Raster.Image Read();

		void Use();
		void Use(int channel);
		void Unuse();
		void Unuse(int channel);

		void Render(Geometry2D.Single.Box source, Geometry2D.Single.Box destination);
	}
}

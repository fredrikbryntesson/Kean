// 
//  Convert.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Draw.Gpu.Test
{
	public class Convert : 
		Abstract<Convert>
	{
		protected override void Run()
		{
			this.Run(
				this.BgrToYuv420,
				this.Yuv420ToBgr,
				this.Yuv420ToBgra,
				this.BgraToYuv420,
				this.BgraToMonochrome,
				this.BgraToBgr,
				this.MonochromeToYuv420,
				this.MonochromeToBgr,
				this.MonochromeToBgra,
				this.Yuv420ToMonochrome,
				this.BgrToMonochrome,
				this.BgrToBgra
				);
		}
		[Test]
		public void BgraToMonochrome()
		{
			Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>());
			Gpu.Monochrome destination = source.Convert<Gpu.Monochrome>();
			destination.Convert<Raster.Bgra>().Save("test.png");
		}
		[Test]
		public void BgraToBgr()
		{
			Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>());
			Gpu.Bgr destination = source.Convert<Gpu.Bgr>();
			destination.Convert<Raster.Bgra>().Save("test.png");
		}
		
		[Test]
		public void BgraToYuv420()
		{
			Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>());
			Gpu.Yuv420 destination = source.Convert<Gpu.Yuv420>();
			destination.Convert<Raster.Bgra>().Save("test.png");
		}
		[Test]
		public void BgrToMonochrome()
		{
			Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgr>());
			Gpu.Monochrome destination = source.Convert<Gpu.Monochrome>();
			destination.Convert<Raster.Bgra>().Save("test.png");
		}
		[Test]
		public void BgrToBgra()
		{
			Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgr>());
			Gpu.Bgra destination = source.Convert<Gpu.Bgra>();
			destination.Convert<Raster.Bgra>().Save("test.png");
		}
		[Test]
		public void BgrToYuv420()
		{
			Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgr>());
			Gpu.Yuv420 destination = source.Convert<Gpu.Yuv420>();
			destination.Convert<Raster.Bgra>().Save("test.png");
		}
		[Test]
		public void MonochromeToBgra()
		{
			Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>());
			Gpu.Bgra destination = source.Convert<Gpu.Bgra>();
			destination.Convert<Raster.Bgra>().Save("test.png");
		}
		[Test]
		public void MonochromeToBgr()
		{
			Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>());
			Gpu.Bgr destination = source.Convert<Gpu.Bgr>();
			destination.Convert<Raster.Bgra>().Save("test.png");
		}
		[Test]
		public void MonochromeToYuv420()
		{
			Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>());
			Gpu.Yuv420 destination = source.Convert<Gpu.Yuv420>();
			destination.Convert<Raster.Bgra>().Save("test.png");
		}
		[Test]
		public void Yuv420ToBgra()
		{
			Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Yuv420>());
			Gpu.Bgra destination = source.Convert<Gpu.Bgra>();
			destination.Convert<Raster.Bgra>().Save("test.png");
		}
		[Test]
		public void Yuv420ToBgr()
		{
			Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Yuv420>());
			Gpu.Bgr destination = source.Convert<Gpu.Bgr>();
			destination.Convert<Raster.Bgra>().Save("test.png");
		}
		[Test]
		public void Yuv420ToMonochrome()
		{
			Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Yuv420>());
			Gpu.Monochrome destination = source.Convert<Gpu.Monochrome>();
			destination.Convert<Raster.Bgra>().Save("test.png");
		}
	}
}

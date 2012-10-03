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
		Fixture<Convert>
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
			using (Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			using (Gpu.Monochrome destination = source.Convert<Gpu.Monochrome>())
				//destination.Convert<Raster.Bgra>().Save("BgraToMonochrome.png");
				Verify(destination, "Correct.Convert.BgraToMonochrome.png");
		}
		[Test]
		public void BgraToBgr()
		{
			using (Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			using (Gpu.Bgr destination = source.Convert<Gpu.Bgr>())
				//destination.Convert<Raster.Bgra>().Save("BgraToBgr.png");
				Verify(destination, "Correct.Convert.BgraToBgr.png");
		}
		
		[Test]
		public void BgraToYuv420()
		{
			using (Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			using (Gpu.Yuv420 destination = source.Convert<Gpu.Yuv420>())
				//destination.Convert<Raster.Bgra>().Save("BgraToYuv420.png");
				Verify(destination, "Correct.Convert.BgraToYuv420.png");
		}
		[Test]
		public void BgrToMonochrome()
		{
			using (Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgr>()))
			using (Gpu.Monochrome destination = source.Convert<Gpu.Monochrome>())
				//destination.Convert<Raster.Bgra>().Save("BgrToMonochrome.png");
				Verify(destination, "Correct.Convert.BgrToMonochrome.png");
		}
		[Test]
		public void BgrToBgra()
		{
			using (Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgr>()))
			using (Gpu.Bgra destination = source.Convert<Gpu.Bgra>())
				//destination.Convert<Raster.Bgra>().Save("BgrToBgra.png");
				Verify(destination, "Correct.Convert.BgrToBgra.png");
		}
		[Test]
		public void BgrToYuv420()
		{
			using (Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgr>()))
			using (Gpu.Yuv420 destination = source.Convert<Gpu.Yuv420>())
				//destination.Convert<Raster.Bgra>().Save("BgrToYuv420.png");
				Verify(destination, "Correct.Convert.BgrToYuv420.png");
		}
		[Test]
		public void MonochromeToBgra()
		{
			using (Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>()))
			using (Gpu.Bgra destination = source.Convert<Gpu.Bgra>())
				//destination.Convert<Raster.Bgra>().Save("MonochromeToBgra.png");
				Verify(destination, "Correct.Convert.MonochromeToBgra.png");
		}
		[Test]
		public void MonochromeToBgr()
		{
			using (Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>()))
			using (Gpu.Bgr destination = source.Convert<Gpu.Bgr>())
				//destination.Convert<Raster.Bgra>().Save("MonochromeToBgr.png");
				Verify(destination, "Correct.Convert.MonochromeToBgr.png");
		}
		[Test]
		public void MonochromeToYuv420()
		{
			using (Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>()))
			using (Gpu.Yuv420 destination = source.Convert<Gpu.Yuv420>())
				//destination.Convert<Raster.Bgra>().Save("MonochromeToYuv420.png");
				Verify(destination, "Correct.Convert.MonochromeToYuv420.png");
		}
		[Test]
		public void Yuv420ToBgra()
		{
			using (Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Yuv420>()))
			using (Gpu.Bgra destination = source.Convert<Gpu.Bgra>())
				//destination.Convert<Raster.Bgra>().Save("Yuv420ToBgra.png");
				Verify(destination, "Correct.Convert.Yuv420ToBgra.png");
		}
		[Test]
		public void Yuv420ToBgr()
		{
			using (Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Yuv420>()))
			using (Gpu.Bgr destination = source.Convert<Gpu.Bgr>())
				//destination.Convert<Raster.Bgra>().Save("Yuv420ToBgr.png");
				Verify(destination, "Correct.Convert.Yuv420ToBgr.png");
		}
		[Test]
		public void Yuv420ToMonochrome()
		{
			using (Gpu.Image source = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Yuv420>()))
			using (Gpu.Monochrome destination = source.Convert<Gpu.Monochrome>())
				//destination.Convert<Raster.Bgra>().Save("Yuv420ToMonochrome.png");
				Verify(destination, "Correct.Convert.Yuv420ToMonochrome.png");
		}
	}
}

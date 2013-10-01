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

namespace Kean.Draw.OpenGL.Test
{
	public class Convert : 
		Fixture<Convert>
	{
		protected override void Run()
		{
			this.Run(
				//this.BgraToMonochrome,
				this.BgraToBgr,
				this.BgraToYuv420,
				this.BgrToYuv420,
				//this.BgrToMonochrome,
				this.BgrToBgra,
				this.MonochromeToBgra,
				this.MonochromeToBgr,
				this.MonochromeToYuv420,
				this.Yuv420ToMonochrome,
				this.Yuv420ToBgra,
				this.Yuv420ToBgr
				);
		}
		[Test]
		public void BgraToMonochrome()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg").Convert<Raster.Bgra>()))
			using (OpenGL.Monochrome destination = source.Convert<OpenGL.Monochrome>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.BgraToMonochrome.png");
		}
		[Test]
		public void BgraToBgr()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg").Convert<Raster.Bgra>()))
			using (OpenGL.Bgr destination = source.Convert<OpenGL.Bgr>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.BgraToBgr.png");
		}
		
		[Test]
		public void BgraToYuv420()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg").Convert<Raster.Bgra>()))
			using (OpenGL.Yuv420 destination = source.Convert<OpenGL.Yuv420>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.BgraToYuv420.png");
		}
		[Test]
		public void BgrToMonochrome()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg").Convert<Raster.Bgr>()))
			using (OpenGL.Monochrome destination = source.Convert<OpenGL.Monochrome>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.BgrToMonochrome.png");
		}
		[Test]
		public void BgrToBgra()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg").Convert<Raster.Bgr>()))
			using (OpenGL.Bgra destination = source.Convert<OpenGL.Bgra>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.BgrToBgra.png");
		}
		[Test]
		public void BgrToYuv420()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg").Convert<Raster.Bgr>()))
			using (OpenGL.Yuv420 destination = source.Convert<OpenGL.Yuv420>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.BgrToYuv420.png");
		}
		[Test]
		public void MonochromeToBgra()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg").Convert<Raster.Monochrome>()))
			using (OpenGL.Bgra destination = source.Convert<OpenGL.Bgra>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.MonochromeToBgra.png");
		}
		[Test]
		public void MonochromeToBgr()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg").Convert<Raster.Monochrome>()))
			using (OpenGL.Bgr destination = source.Convert<OpenGL.Bgr>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.MonochromeToBgr.png");
		}
		[Test]
		public void MonochromeToYuv420()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg").Convert<Raster.Monochrome>()))
			using (OpenGL.Yuv420 destination = source.Convert<OpenGL.Yuv420>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.MonochromeToYuv420.png");
		}
		[Test]
		public void Yuv420ToBgra()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg").Convert<Raster.Yuv420>()))
			using (OpenGL.Bgra destination = source.Convert<OpenGL.Bgra>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.Yuv420ToBgra.png");
		}
		[Test]
		public void Yuv420ToBgr()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg").Convert<Raster.Yuv420>()))
			using (OpenGL.Bgr destination = source.Convert<OpenGL.Bgr>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.Yuv420ToBgr.png");
		}
		[Test]
		public void Yuv420ToMonochrome()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg").Convert<Raster.Yuv420>()))
			using (OpenGL.Monochrome destination = source.Convert<OpenGL.Monochrome>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.Yuv420ToMonochrome.png");
		}
	}
}

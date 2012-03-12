// 
//  Monochrome.cs
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
	public class Monochrome :
		Abstract<Monochrome>
	{
		protected override void Run()
		{
			this.Run(
				this.Create,
				this.Equality,
				this.CreateFromRaster,
				this.Copy,
				this.ResizeTo,
				this.ResizeWithin,
				this.DrawColor,
				this.DrawColorRegion,
				this.ClearArea,
				this.DrawImageOnRegion,
				this.DrawImageOnBgra
				);
		}
		[Test]
		public void Create()
		{
			using (Gpu.Monochrome image = new Gpu.Monochrome(new Geometry2D.Integer.Size(128, 256)))
			{
				Expect(image, Is.Not.Null);
				Expect(image.Size, Is.EqualTo(new Geometry2D.Integer.Size(128, 256)));
			}
		}
		[Test]
		public void Equality()
		{
			using (Gpu.Monochrome a = new Gpu.Monochrome(new Geometry2D.Integer.Size(128, 256)))
			using (Gpu.Monochrome b = new Gpu.Monochrome(new Geometry2D.Integer.Size(256, 256)))
			{
				Expect(a, Is.EqualTo(a));
				Expect(a, Is.Not.EqualTo(b));
			}
		}
		/*
		[Test]
		public void ConvertToRaster()
		{
			using (Gpu.Image image = new Gpu.Monochrome(new Geometry2D.Integer.Size(128, 256)))
			{
				Expect(image.Convert<Raster.Monochrome>().Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Monochrome.ConvertToRaster.png")));
			}
		}*/
		[Test]
		public void CreateFromRaster()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>()))
			Expect(image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Monochrome.CreateFromRaster.png")));
		}
		[Test]
		public void Copy()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>()))
				Expect(image.Copy().Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Monochrome.CreateFromRaster.png")));
		}
		[Test]
		public void ResizeTo()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>()))
				Expect(image.ResizeTo(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Monochrome>().Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Monochrome.ResizeTo.png")));
		}
		[Test]
		public void ResizeWithin()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>()))
				Expect(image.ResizeWithin(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Monochrome>().Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Monochrome.ResizeWithin.png")));
		}
		#region Canvas
		[Test]
		public void ClearArea()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>()))
			{
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Clear(new Geometry2D.Single.Box(200, 300, 200, 100));
				Expect(canvas.Image.Convert<Raster.Monochrome>().Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Monochrome.ClearArea.png")));
			}
		}
		[Test]
		public void DrawColor()
		{
			using (Draw.Image monochrome = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>()))
			{
				Draw.Canvas canvas = monochrome.Canvas;
				canvas.Draw(new Kean.Draw.Color.Bgra(50, 100, 200, 255));
				Expect(canvas.Image.Convert<Raster.Monochrome>().Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Monochrome.DrawColor.png")));
			}
		}
		[Test]
		public void DrawColorRegion()
		{
			using (Draw.Image monochrome = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>()))
			{
				Draw.Canvas canvas = monochrome.Canvas;
				canvas.Draw(new Kean.Draw.Color.Bgra(0, 0, 255, 255), new Geometry2D.Single.Box(100, 100, 200, 200));
				Expect(canvas.Image.Convert<Raster.Monochrome>().Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Monochrome.DrawColorRegion.png")));
			}
		}
		[Test]
		public void DrawImageOnRegion()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>()))
			{
				Kean.Draw.Image part = Raster.Image.OpenResource("Input.Flower.jpg").ResizeTo(new Kean.Math.Geometry2D.Integer.Size(100, 100)).Convert<Raster.Monochrome>();
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Draw(part, new Geometry2D.Single.Box(0, 0, 50, 100), new Geometry2D.Single.Box(200, 200, 100, 100));
				Expect(canvas.Image.Convert<Raster.Monochrome>().Convert<Raster.Bgra>().Distance(Raster.Bgra.OpenResource("Correct.Monochrome.DrawImageOnRegion.png")), Is.LessThan(5));
			}
		}
		[Test]
		public void DrawImageOnBgra()
		{
			using (Draw.Image monochrome = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Monochrome>()))
			using (Draw.Image bgra = Gpu.Image.Create(Raster.Image.OpenResource("Input.ElephantSeal.jpg").Convert<Raster.Bgra>()))
			{
				Draw.Image resized = monochrome.ResizeWithin(new Geometry2D.Integer.Size(100, 100));
				Draw.Canvas bgraCanvas = bgra.Canvas;
				bgraCanvas.Draw(resized);
				Expect(bgraCanvas.Image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Monochrome.DrawImageOnBgra.png").Convert<Raster.Bgra>()));
			}
		}
		#endregion
	}
}

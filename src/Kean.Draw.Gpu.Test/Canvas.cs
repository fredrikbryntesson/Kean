// 
//  Bgra.cs
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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Draw.Gpu.Test
{
	public class Canvas :
		Abstract<Canvas>
	{
		protected override void Run()
		{
			this.Run(
				this.Create,
				this.CreateFromRaster,
				this.DrawColor,
				this.DrawColorRegion,
				this.Clear,
				this.ClearArea,
				this.DrawImage,
				this.Blend
				);
		}
		[Test]
		public void Create()
		{
			using (Gpu.Bgra image = new Gpu.Bgra(new Geometry2D.Integer.Size(128, 256)))
			{
				Draw.Canvas canvas = image.Canvas;
				Expect(canvas, Is.Not.Null);
				Expect(canvas.Size, Is.EqualTo(new Geometry2D.Integer.Size(128, 256)));
			}
		}
		[Test]
		public void CreateFromRaster()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Canvas canvas = image.Canvas;
				Expect(canvas.Image, Is.EqualTo(image));
			}
		}
		[Test]
		public void DrawColor()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Draw(new Kean.Draw.Color.Bgra(255, 0, 0, 255));
				Expect(canvas.Image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.DrawColor.png")));
			}
		}
		[Test]
		public void DrawColorRegion()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Draw(new Kean.Draw.Color.Bgra(255, 0, 0, 255), new Kean.Math.Geometry2D.Single.Box(100, 100, 200, 200));
				Expect(canvas.Image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.DrawColorRegion.png")));
			}
		}
		[Test]
		public void Clear()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Clear();
				canvas.Image.Convert<Raster.Bgra>().Save("C:\\document\\Anders development folder\\Kean\\src\\Kean.Draw.Gpu.Test\\Correct\\Bgra\\Clear.png");
				Expect(canvas.Image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.Clear.png")));
			}
		}
		[Test]
		public void ClearArea()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Clear(new Geometry2D.Single.Box(0, 0, 100, 100));
				canvas.Image.Convert<Raster.Bgra>().Save("C:\\document\\Anders development folder\\Kean\\src\\Kean.Draw.Gpu.Test\\Correct\\Bgra\\ClearArea.png");
				Expect(canvas.Image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.ClearArea.png")));
			}
		}
		[Test]
		public void DrawImage()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Image part = Raster.Image.OpenResource("Input.Flower.jpg").ResizeTo(new Kean.Math.Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>();
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Draw(part, new Kean.Math.Geometry2D.Single.Point(500, 500));
				canvas.Image.Convert<Raster.Bgra>().Save("test.png");
			}
		}
		[Test]
		public void Blend()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
				Expect(image.ResizeWithin(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.ResizeWithin.png")));
		}

	}
}

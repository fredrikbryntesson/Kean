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
	public class Bgra :
		Abstract<Bgra>
	{
		protected override void Run()
		{
			this.Run(
				this.Create,
				this.Equality,
				this.CreateFromRaster,
				this.Copy,
				this.ResizeTo,
				this.ResizeWithin
				);
		}
		[Test]
		public void Create()
		{
			using (Gpu.Bgra image = new Gpu.Bgra(new Geometry2D.Integer.Size(128, 256)))
			{
				Expect(image, Is.Not.Null);
				Expect(image.Size, Is.EqualTo(new Geometry2D.Integer.Size(128, 256)));
			}
		}
		[Test]
		public void Equality()
		{
			using (Gpu.Bgra a = new Gpu.Bgra(new Geometry2D.Integer.Size(128, 256)))
			using (Gpu.Bgra b = new Gpu.Bgra(new Geometry2D.Integer.Size(256, 256)))
			{
				Expect(a, Is.EqualTo(a));
				Expect(a, Is.Not.EqualTo(b));
			}
		}
		/*
		[Test]
		public void ConvertToRaster()
		{
			using (Gpu.Bgra image = new Gpu.Bgra(new Geometry2D.Integer.Size(128, 256)))
				Expect(image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.ConvertToRaster.png")));
		}*/
		[Test]
		public void CreateFromRaster()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
				Expect(image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.CreateFromRaster.png")));
		}
		[Test]
		public void Copy()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
				Expect(image.Copy().Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.CreateFromRaster.png")));
		}
		[Test]
		public void ResizeTo()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
				Expect(image.ResizeTo(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.ResizeTo.png")));
		}
		[Test]
		public void ResizeWithin()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
				Expect(image.ResizeWithin(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.ResizeWithin.png")));
		}
	}
}

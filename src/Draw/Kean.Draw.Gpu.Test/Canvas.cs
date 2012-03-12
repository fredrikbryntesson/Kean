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
				this.DrawImageOnPosition,
				this.Create,
				this.CreateFromRaster,
				this.DrawColor,
				this.DrawColorRegion,
				this.Clear,
				this.ClearArea,
				this.DrawImageOnPosition,
				this.DrawImageOnRegion,
				this.Blend,
				this.DrawColorRegionWithClipping,
				this.DrawImageOnRegionWithClipping,
				this.BlendWithClipping,
				this.DrawColorRegionWithTransformAndClipping
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
				canvas.Draw(new Kean.Draw.Color.Bgra(255, 0, 0, 255), new Geometry2D.Single.Box(100, 100, 200, 200));
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
				Expect(canvas.Image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.Clear.png")));
			}
		}
		[Test]
		public void ClearArea()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Clear(new Geometry2D.Single.Box(200, 300, 200, 100));
				Expect(canvas.Image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.ClearArea.png")));
			}
		}
		[Test]
		public void DrawImageOnPosition()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Image part = Raster.Image.OpenResource("Input.Flower.jpg").ResizeTo(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>();
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Draw(part, new Geometry2D.Single.Point(500, 200));
				Expect(canvas.Image.Convert<Raster.Bgra>().Distance(Raster.Bgra.OpenResource("Correct.Bgra.DrawImageOnPosition.png")), Is.LessThan(5));
			}
		}
		[Test]
		public void DrawImageOnRegion()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Image part = Raster.Image.OpenResource("Input.Flower.jpg").ResizeTo(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>();
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Draw(part, new Geometry2D.Single.Box(0,0,50,100), new Geometry2D.Single.Box(200,200,100,100));
				Expect(canvas.Image.Convert<Raster.Bgra>().Distance(Raster.Bgra.OpenResource("Correct.Bgra.DrawImageOnRegion.png")), Is.LessThan(5));
			}
		}
		[Test]
		public void Blend()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Image part = Raster.Image.OpenResource("Input.Flower.jpg").ResizeTo(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>();
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Blend(0.5f);
				Expect(canvas.Image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.Blend.png")));
			}
		}
		[Test]
		public void DrawColorRegionWithClipping()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Draw(new Kean.Draw.Color.Bgra(0, 0, 255, 255), new Geometry2D.Single.Box(50, 100, 400, 100));
				canvas.Push(new Geometry2D.Single.Box(100, 50, 320, 200));
				canvas.Draw(new Kean.Draw.Color.Bgra(255, 0, 0, 255), new Geometry2D.Single.Box(50, 100, 400, 100));
				canvas.Pop();
				Expect(canvas.Image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.DrawColorRegionWithClipping.png")));
			}
		}
		[Test]
		public void DrawImageOnRegionWithClipping()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Image part = Raster.Image.OpenResource("Input.Flower.jpg").ResizeTo(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>();
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Push(new Geometry2D.Single.Box(100, 50, 320, 200));
				canvas.Draw(part, new Geometry2D.Single.Box(0, 0, 50, 100), new Geometry2D.Single.Box(200, 200, 100, 100));
				canvas.Pop();
				Expect(canvas.Image.Convert<Raster.Bgra>().Distance(Raster.Bgra.OpenResource("Correct.Bgra.DrawImageOnRegionWithClipping.png")), Is.LessThan(5));
			}
		}
		[Test]
		public void BlendWithClipping()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Image part = Raster.Image.OpenResource("Input.Flower.jpg").ResizeTo(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>();
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Push(new Geometry2D.Single.Box(100, 50, 320, 200)); 
				canvas.Blend(0.5f);
				canvas.Pop();
				Expect(canvas.Image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.BlendWithClipping.png")));
			}
		}
		//TODO Need a way to set both clip and transform independently.
		[Test]
		public void DrawColorRegionWithTransformAndClipping()
		{
			using (Gpu.Image image = Gpu.Image.Create(Raster.Image.OpenResource("Input.Flower.jpg").Convert<Raster.Bgra>()))
			{
				Kean.Draw.Canvas canvas = image.Canvas;
				canvas.Draw(new Color.Bgra(0, 0, 255, 255), new Geometry2D.Single.Box(50, image.Size.Height / 2 - 50, image.Size.Width - 100, 100));
				canvas.Push(new Geometry2D.Single.Box(0,0, image.Size.Width / 2, image.Size.Height), Geometry2D.Single.Transform.CreateTranslation(100,0) * Geometry2D.Single.Transform.CreateRotation(Kean.Math.Single.ToRadians(45)));
				canvas.Draw(new Color.Bgra(255, 0, 0, 255), new Geometry2D.Single.Box(50, image.Size.Height / 2 - 50, image.Size.Width - 100, 100));
				canvas.Pop();
				Expect(canvas.Image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Correct.Bgra.DrawColorRegionWithTransformAndClipping.png")));
			}
		}		
	}
}

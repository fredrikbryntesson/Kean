// 
//  Bgra.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
		Fixture<Canvas>
	{
		Gpu.Image image;
		public override void Setup()
		{
			base.Setup();
			using (Raster.Bgra raster = Raster.Bgra.OpenResource("Input.Flower.jpg"))
				this.image = Gpu.Image.Create(raster);
		}
		public override void TearDown()
		{
			base.TearDown();
			this.image.Dispose();
		}
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
				Verify(image.Canvas, Is.Not.Null);
				Verify(image.Canvas.Size, Is.EqualTo(new Geometry2D.Integer.Size(128, 256)));
			}
		}
		[Test]
		public void CreateFromRaster()
		{
			using (Raster.Bgra raster = Raster.Bgra.OpenResource("Input.Flower.jpg"))
				Verify(this.image.Canvas.Image, raster);
		}
		[Test]
		public void Copy()
		{
			using (Raster.Bgra raster = Raster.Bgra.OpenResource("Input.Flower.jpg"))
			using (Draw.Image copy = this.image.Copy())
				Verify(copy, raster);
		}
		[Test]
		public void DrawColor()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Draw(new Kean.Draw.Color.Bgra(255, 0, 0, 255));
				Verify(image, "Correct.Bgra.DrawColor.png");
			}
		}
		[Test]
		public void DrawColorRegion()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Draw(new Kean.Draw.Color.Bgra(255, 0, 0, 255), new Geometry2D.Single.Box(100, 100, 200, 200));
				Verify(image, "Correct.Bgra.DrawColorRegion.png");
			}
		}
		[Test]
		public void Clear()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Clear();
				Verify(image, "Correct.Bgra.Clear.png");
			}
		}
		[Test]
		public void ClearArea()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Clear(new Geometry2D.Single.Box(200, 300, 200, 100));
				Verify(image, "Correct.Bgra.ClearArea.png");
			}
		}
		[Test]
		public void DrawImageOnPosition()
		{
			using (Draw.Image image = this.image.Copy())
			using (Draw.Image part = Raster.Bgra.OpenResource("Input.Flower.jpg"))
			using (Draw.Image resized = part.ResizeTo(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>())
			{
				image.Canvas.Draw(resized, new Geometry2D.Single.Point(500, 200));
				Verify(image, "Correct.Bgra.DrawImageOnPosition.png");
			}
		}
		[Test]
		public void DrawImageOnRegion()
		{
			using (Draw.Image image = this.image.Copy())
			using (Draw.Image part = Raster.Bgra.OpenResource("Input.Flower.jpg"))
			using (Draw.Image resized = part.ResizeTo(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>())
			{
				image.Canvas.Draw(resized, new Geometry2D.Single.Box(0, 0, 50, 100), new Geometry2D.Single.Box(200, 200, 100, 100));
				Verify(image, "Correct.Bgra.DrawImageOnRegion.png");
			}
		}
		[Test]
		public void Blend()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Blend(0.5f);
				Verify(image, "Correct.Bgra.Blend.png");
			}
		}
		[Test]
		public void DrawColorRegionWithClipping()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Draw(new Kean.Draw.Color.Bgra(0, 0, 255, 255), new Geometry2D.Single.Box(50, 100, 400, 100));
				image.Canvas.Push(new Geometry2D.Single.Box(100, 50, 320, 200));
				image.Canvas.Draw(new Kean.Draw.Color.Bgra(255, 0, 0, 255), new Geometry2D.Single.Box(50, 100, 400, 100));
				image.Canvas.Pop();
				Verify(image, "Correct.Bgra.DrawColorRegionWithClipping.png");
			}
		}
		[Test]
		public void DrawImageOnRegionWithClipping()
		{
			using (Draw.Image image = this.image.Copy())
			using (Draw.Image part = Raster.Bgra.OpenResource("Input.Flower.jpg"))
			using (Draw.Image resized = part.ResizeTo(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>())
			{
				image.Canvas.Push(new Geometry2D.Single.Box(100, 50, 320, 200));
				image.Canvas.Draw(part, new Geometry2D.Single.Box(0, 0, 50, 100), new Geometry2D.Single.Box(200, 200, 100, 100));
				image.Canvas.Pop();
				Verify(image, "Correct.Bgra.DrawImageOnRegionWithClipping.png");
			}
		}
		[Test]
		public void BlendWithClipping()
		{
			using (Draw.Image image = this.image.Copy())
			using (Draw.Image part = Raster.Bgra.OpenResource("Input.Flower.jpg"))
			using (Draw.Image resized = part.ResizeTo(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>())
			{
				image.Canvas.Push(new Geometry2D.Single.Box(100, 50, 320, 200)); 
				image.Canvas.Blend(0.5f);
				image.Canvas.Pop();
				Verify(image, "Correct.Bgra.BlendWithClipping.png");
			}
		}
		//TODO Need a way to set both clip and transform independently.
		[Test]
		public void DrawColorRegionWithTransformAndClipping()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Draw(new Color.Bgra(0, 0, 255, 255), new Geometry2D.Single.Box(50, image.Size.Height / 2 - 50, image.Size.Width - 100, 100));
				image.Canvas.Push(new Geometry2D.Single.Box(0,0, image.Size.Width / 2, image.Size.Height), Geometry2D.Single.Transform.CreateTranslation(100,0) * Geometry2D.Single.Transform.CreateRotation(Kean.Math.Single.ToRadians(45)));
				image.Canvas.Draw(new Color.Bgra(255, 0, 0, 255), new Geometry2D.Single.Box(50, image.Size.Height / 2 - 50, image.Size.Width - 100, 100));
				image.Canvas.Pop();
				Verify(image, "Correct.Bgra.DrawColorRegionWithTransformAndClipping.png");
			}
		}		
	}
}

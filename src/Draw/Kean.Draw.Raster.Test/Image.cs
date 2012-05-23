// 
//  Image.cs
//  
//  Author:
//      Simon Mika <smika@hx.se
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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Geometry2D = Kean.Math.Geometry2D;
namespace Kean.Draw.Raster.Test
{
	public abstract class Image<T> :
		Fixture<T>
		where T : Image<T>, new()
	{
		protected Image(float tolerance) :
			base(tolerance)
		{ }
		protected Image() :
			base()
		{ }
		protected override void Run()
		{
			this.Run(
				this.Rectangle,
				this.RoundedRectangle,
				this.Circle,
				this.Ellipse,
				this.Bitmap
				);
		}
		protected abstract Draw.Image CreateImage(Geometry2D.Integer.Size size);
		[Test]
		public void Rectangle()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Color.Bgra.Blue, new Stroke(Color.Bgra.Red, 2), Path.Rectangle(new Geometry2D.Single.Box(10, 10, 100, 100)));
				Verify(image, "Correct.Image.Rectangle.png");
			}
		}
		[Test]
		public void RoundedRectangle()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Color.Bgra.Blue, new Stroke(Color.Bgra.Red, 2), Path.Rectangle(new Kean.Math.Geometry2D.Single.Box(10, 10, 100, 100), new Math.Geometry2D.Single.Size(10, 20)));
				Verify(image, "Correct.Image.RoundedRectangle.png");
			}
		}
		[Test]
		public void Circle()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Color.Bgra.Blue, new Stroke(Color.Bgra.Red, 4), Path.Circle(new Geometry2D.Single.Point(64, 128), 48));
				Verify(image, "Correct.Image.Circle.png");
			}
		}
		[Test]
		public void Ellipse()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Color.Bgra.Blue, new Stroke(Color.Bgra.Red, 8), Path.Ellipse(new Geometry2D.Single.Point(64, 128), new Geometry2D.Single.Size(48, 72)));
				Verify(image, "Correct.Image.Ellipse.png");
			}
		}
		[Test]
		public void Bitmap()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(256, 512)))
			using (Image ellipse = Image.OpenResource("Correct.Image.Ellipse.png"))
			{
				image.Canvas.Draw(ellipse, new Geometry2D.Single.Point(128, 256));
				Verify(image, "Correct.Image.Bitmap.png");
			}
		}
	}
}

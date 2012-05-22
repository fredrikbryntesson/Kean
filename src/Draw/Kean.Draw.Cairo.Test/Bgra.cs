// 
//  Bgra.cs
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
namespace Kean.Draw.Cairo.Test
{
	[TestFixture]
	public class Bgra :
		Kean.Test.Fixture<Bgra>
	{
		string prefix = "Kean.Draw.Cairo.Test.Bgra.";
		float tolerance = 0.01f;
		protected override void Run()
		{
			this.Run(
				this.Create,
				this.Rectangle,
				this.RoundedRectangle,
				this.Circle,
				this.Ellipse,
				this.Bitmap
				);
		}
		[Test]
		public void Create()
		{
			using (Cairo.Raster image = new Cairo.Bgra(new Geometry2D.Integer.Size(128, 256)))
			using (Draw.Raster.Bgra raster = new Draw.Raster.Bgra(image.Buffer, image.Size, image.CoordinateSystem))
			using (Draw.Raster.Image correct = Draw.Raster.Image.OpenResource("Correct.Bgra.Create.png"))
				Expect(raster.Distance(correct), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "Create.0");
		}
		[Test]
		public void Rectangle()
		{
			using (Cairo.Raster image = new Cairo.Bgra(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Color.Bgra.Blue, new Stroke(Color.Bgra.Red, 2), Path.Rectangle(new Kean.Math.Geometry2D.Single.Box(10, 10, 100, 100)));
				using (Draw.Raster.Bgra raster = new Draw.Raster.Bgra(image.Buffer, image.Size, image.CoordinateSystem))
				using (Draw.Raster.Image correct = Draw.Raster.Image.OpenResource("Correct.Bgra.Rectangle.png"))
					//raster.Save("Rectangle.png"); 
					Expect(raster.Distance(correct), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "Rectangle.0");
			}
		}
		[Test]
		public void RoundedRectangle()
		{
			using (Cairo.Raster image = new Cairo.Bgra(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Color.Bgra.Blue, new Stroke(Color.Bgra.Red, 2), Path.Rectangle(new Kean.Math.Geometry2D.Single.Box(10, 10, 100, 100), new Math.Geometry2D.Single.Size(10, 20)));
				using (Draw.Raster.Bgra raster = new Draw.Raster.Bgra(image.Buffer, image.Size, image.CoordinateSystem))
				using (Draw.Raster.Image correct = Draw.Raster.Image.OpenResource("Correct.Bgra.RoundedRectangle.png"))
					//raster.Save("RoundedRectangle.png");
					Expect(raster.Distance(correct), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "RoundedRectangle.0");
			}
		}
		[Test]
		public void Circle()
		{
			using (Cairo.Raster image = new Cairo.Bgra(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Color.Bgra.Blue, new Stroke(Color.Bgra.Red, 4), Path.Circle(new Geometry2D.Single.Point(64, 128), 48));
				using (Draw.Raster.Bgra raster = new Draw.Raster.Bgra(image.Buffer, image.Size, image.CoordinateSystem))
				using (Draw.Raster.Image correct = Draw.Raster.Image.OpenResource("Correct.Bgra.Circle.png"))
					//raster.Save("Circle.png");
					Expect(raster.Distance(correct), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "Circle.0");
			}
		}
		[Test]
		public void Ellipse()
		{
			using (Cairo.Raster image = new Cairo.Bgra(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Color.Bgra.Blue, new Stroke(Color.Bgra.Red, 8), Path.Ellipse(new Geometry2D.Single.Point(64, 128), new Geometry2D.Single.Size(48, 72)));
				using (Draw.Raster.Bgra raster = new Draw.Raster.Bgra(image.Buffer, image.Size, image.CoordinateSystem))
				using (Draw.Raster.Image correct = Draw.Raster.Image.OpenResource("Correct.Bgra.Ellipse.png"))
					//raster.Save("Ellipse.png");
					Expect(raster.Distance(correct), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "Ellipse.0");
			}
		}
		[Test]
		public void Bitmap()
		{
			using (Cairo.Raster image = new Cairo.Bgra(new Geometry2D.Integer.Size(256, 512)))
			using (Draw.Raster.Image ellipse = Draw.Raster.Image.OpenResource("Correct.Bgra.Ellipse.png"))
			{
				image.Canvas.Draw(ellipse, new Geometry2D.Single.Point(128, 256));
				using (Draw.Raster.Bgra raster = new Draw.Raster.Bgra(image.Buffer, image.Size, image.CoordinateSystem))
				using (Draw.Raster.Image correct = Draw.Raster.Image.OpenResource("Correct.Bgra.Bitmap.png"))
					//raster.Save("Bitmap.png");
					Expect(raster.Distance(correct), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "Bitmap.0");
			}
		}
	}
}

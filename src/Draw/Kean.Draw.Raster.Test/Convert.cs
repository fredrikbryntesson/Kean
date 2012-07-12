// 
//  Convert.cs
//  
//  Author:
//      Anders Frisk <andersfrisk77@gmail.com>
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
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Target = Kean.Draw.Raster;
using Geometry2D = Kean.Math.Geometry2D;
namespace Kean.Draw.Raster.Test
{
	[TestFixture]
	public class Convert :
		Fixture<Convert>
	{
		public Convert() :
			base(5f)
		{ }
		protected override void Run()
		{
			this.Run(
				this.Open,
				this.Equality,
				this.Save,
				this.Copy,
				this.Bgr,
				this.Bgra,
				this.Monochrome,
				this.Yuv420,
				this.Yvu420,
				this.Yuyv
				);
		}
		#region Basic Operations
		[Test]
		public void Open()
		{
			using (Target.Image bitmap = Target.Image.OpenResource("Correct/Convert/original.png"))
			{
				Verify(bitmap.Size, Is.EqualTo(new Geometry2D.Integer.Size(800, 348)));
				Verify(bitmap.Length, Is.EqualTo(new Geometry2D.Integer.Size(800, 348).Area * 3));
			}
			using (Target.Image bitmap = Target.Image.OpenResource("Correct/Convert/monochrome.png"))
			{
				Verify(bitmap.Size, Is.EqualTo(new Geometry2D.Integer.Size(800, 348)));
				Verify(bitmap.Length, Is.EqualTo(new Geometry2D.Integer.Size(800, 348).Area * 4));
			}

		}
		[Test]
		public void Equality()
		{
			using (Target.Image first = Target.Image.OpenResource("Correct/Convert/original.png"))
				Verify(first, "Correct/Convert/original.png");
		}
		[Test]
		public void Save()
		{
			using (Target.Image original = Target.Image.OpenResource("Correct/Convert/original.png"))
			{
				original.Save("original.png");
				original.Save("original.bmp");
				using (Raster.Image opened = Target.Image.Open("original.png"))
					Verify(opened, original);
				using (Raster.Image opened = Target.Image.Open("original.bmp"))
					Verify(opened, original);
			}
		}
		[Test]
		public void Copy()
		{
			using (Target.Image original = Target.Image.OpenResource("Correct/Convert/original.png"))
			using (Target.Image copy = original.Copy() as Target.Image)
				Verify(original.Pointer, Is.Not.EqualTo(copy.Pointer));
		}
		#endregion
		#region Convert
		[Test]
		public void Bgr()
		{
			using (Target.Bgr original = Target.Bgr.OpenResource("Correct/Convert/original.png"))
			{
				Verify(original.Convert<Target.Bgra>().Convert<Target.Bgr>(), original);
				Verify(original.Convert<Target.Yuv420>().Convert<Target.Bgr>(), original);
				Verify(original.Convert<Target.Yuyv>().Convert<Target.Bgr>(), original);
				Verify(original.Convert<Target.Yvu420>().Convert<Target.Bgr>(), original);
			}
		}
		[Test]
		public void Bgra()
		{
			using (Target.Bgra original = Target.Bgra.OpenResource("Correct/Convert/original.png"))
			{
				Verify(original.Convert<Target.Bgra>().Convert<Target.Bgra>(), original);
				Verify(original.Convert<Target.Yuv420>().Convert<Target.Bgra>(), original);
				Verify(original.Convert<Target.Yuyv>().Convert<Target.Bgra>(), original);
				Verify(original.Convert<Target.Yvu420>().Convert<Target.Bgra>(), original);
			}
		}
		[Test]
		public void Monochrome()
		{
			using (Target.Monochrome original = Target.Monochrome.OpenResource("Correct/Convert/original.png"))
			{
				Verify(original.Convert<Target.Bgra>().Convert<Target.Monochrome>(), original);
				Verify(original.Convert<Target.Yuv420>().Convert<Target.Monochrome>().Distance(original), Is.LessThanOrEqualTo(0));
				Verify(original.Convert<Target.Yuyv>().Convert<Target.Monochrome>().Distance(original), Is.LessThanOrEqualTo(0));
				Verify(original.Convert<Target.Yvu420>().Convert<Target.Monochrome>().Distance(original), Is.LessThanOrEqualTo(0));
				using (Target.Monochrome monochrome = Target.Monochrome.OpenResource("Correct/Convert/monochrome.png"))
					Verify(original, "Correct/Convert/monochrome.png");
			}
		}
		[Test]
		public void Yuv420()
		{
			using (Target.Yuv420 original = Target.Yuv420.OpenResource("Correct/Convert/original.png"))
			{
				Verify(original.Convert<Target.Bgra>().Convert<Target.Yuv420>(), original);
				Verify(original.Convert<Target.Yuv420>().Convert<Target.Yuv420>(), original);
				Verify(original.Convert<Target.Yuyv>().Convert<Target.Yuv420>(), original);
				Verify(original.Convert<Target.Yvu420>().Convert<Target.Yuv420>(), original);
			}
		}
		[Test]
		public void Yvu420()
		{
			using (Target.Yvu420 original = Target.Yvu420.OpenResource("Correct/Convert/original.png"))
			{
				Verify(original.Convert<Target.Bgra>().Convert<Target.Yvu420>(), original);
				Verify(original.Convert<Target.Yuv420>().Convert<Target.Yvu420>(), original);
				Verify(original.Convert<Target.Yuyv>().Convert<Target.Yvu420>(), original);
				Verify(original.Convert<Target.Yvu420>().Convert<Target.Yvu420>(), original);
			}
		}
		[Test]
		public void Yuyv()
		{
			using (Target.Yuyv original = Target.Yuyv.OpenResource("Correct/Convert/original.png"))
			{
				Verify(original.Convert<Target.Bgra>().Convert<Target.Yuyv>(), original);
				Verify(original.Convert<Target.Monochrome>().Convert<Target.Yuyv>(), original);
				Verify(original.Convert<Target.Yuv420>().Convert<Target.Yuyv>(), original);
				Verify(original.Convert<Target.Yuyv>().Convert<Target.Yuyv>(), original);
				Verify(original.Convert<Target.Yvu420>().Convert<Target.Yuyv>(), original);
			}
		}
		#endregion
	}
}

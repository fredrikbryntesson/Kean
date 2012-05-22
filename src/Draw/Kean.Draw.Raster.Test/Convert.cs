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
			base(0.01f)
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
                this.Yuv422
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
            Target.Image first = Target.Image.OpenResource("Correct/Convert/original.png");
            Target.Image second = Target.Image.OpenResource("Correct/Convert/original.png");
            Expect(first.Distance(second), Is.LessThanOrEqualTo(this.Tolerance), this.Prefix + "Equality.0");
        }
        [Test]
        public void Save()
        {
            Target.Image original = Target.Image.OpenResource("Correct/Convert/original.png");
            original.Save("original.png");
            original.Save("original.bmp");
            Expect(original.Distance(Target.Image.Open("original.png")), Is.LessThanOrEqualTo(this.Tolerance), this.Prefix + "Save.0");
            Expect(original.Distance(Target.Image.Open("original.bmp")), Is.LessThanOrEqualTo(this.Tolerance), this.Prefix + "Save.1");
        }
        [Test]
        public void Copy()
        {
            Target.Image original = Target.Image.OpenResource("Correct/Convert/original.png");
            Target.Image copy = original.Copy() as Target.Image;
            Expect(original.Pointer,Is.Not.EqualTo(copy.Pointer), this.Prefix + "Copy.0");
        }
        #endregion
        #region Convert
        [Test]
        public void Bgr()
        {
            Target.Bgr original = Target.Image.OpenResource("Correct/Convert/original.png").Convert<Target.Bgr>();
            Expect(original.Convert<Target.Bgra>().Convert<Target.Bgr>().Distance(original), Is.LessThanOrEqualTo(this.Tolerance), this.Prefix + "Bgr.0");
            Expect(original.Convert<Target.Yuv420>().Convert<Target.Bgr>().Distance(original), Is.LessThanOrEqualTo(5), this.Prefix + "Bgr.2");
            Expect(original.Convert<Target.Yuv422>().Convert<Target.Bgr>().Distance(original), Is.LessThanOrEqualTo(5), this.Prefix + "Bgr.3");
            Expect(original.Convert<Target.Yvu420>().Convert<Target.Bgr>().Distance(original), Is.LessThanOrEqualTo(5), this.Prefix + "Bgr.4");
        }
        [Test]
        public void Bgra()
        {
            Target.Bgra original = Target.Image.OpenResource("Correct/Convert/original.png").Convert<Target.Bgra>();
            Expect(original.Convert<Target.Bgra>().Convert<Target.Bgra>().Distance(original), Is.LessThanOrEqualTo(this.Tolerance), this.Prefix + "Bgra.0");
            Expect(original.Convert<Target.Yuv420>().Convert<Target.Bgra>().Distance(original), Is.LessThanOrEqualTo(3), this.Prefix + "Bgra.1");
            Expect(original.Convert<Target.Yuv422>().Convert<Target.Bgra>().Distance(original), Is.LessThanOrEqualTo(3), this.Prefix + "Bgra.2");
            Expect(original.Convert<Target.Yvu420>().Convert<Target.Bgra>().Distance(original), Is.LessThanOrEqualTo(3), this.Prefix + "Bgra.3");
        }
        [Test]
        public void Monochrome()
        {
            Target.Monochrome original = Target.Image.OpenResource("Correct/Convert/original.png").Convert<Target.Monochrome>();
            Expect(original.Convert<Target.Bgra>().Convert<Target.Monochrome>().Distance(original), Is.LessThanOrEqualTo(1), this.Prefix + "Monochrome.0");
            Expect(original.Convert<Target.Yuv420>().Convert<Target.Monochrome>().Distance(original), Is.LessThanOrEqualTo(0), this.Prefix + "Monochrome.1");
            Expect(original.Convert<Target.Yuv422>().Convert<Target.Monochrome>().Distance(original), Is.LessThanOrEqualTo(0), this.Prefix + "Monochrome.2");
            Expect(original.Convert<Target.Yvu420>().Convert<Target.Monochrome>().Distance(original), Is.LessThanOrEqualTo(0), this.Prefix + "Monochrome.3");
            Target.Monochrome monochrome = Target.Image.OpenResource("Correct/Convert/monochrome.png").Convert<Target.Monochrome>();
            Expect(original.Distance(monochrome), Is.LessThanOrEqualTo(2), this.Prefix + "Monochrome.4");
        }
        [Test]
        public void Yuv420()
        {
            Target.Yuv420 original = Target.Image.OpenResource("Correct/Convert/original.png").Convert<Target.Yuv420>();
            Expect(original.Convert<Target.Bgra>().Convert<Target.Yuv420>().Distance(original), Is.LessThanOrEqualTo(1), this.Prefix + "Yuv420.0");
            Expect(original.Convert<Target.Yuv420>().Convert<Target.Yuv420>().Distance(original), Is.LessThanOrEqualTo(0), this.Prefix + "Yuv420.1");
            Expect(original.Convert<Target.Yuv422>().Convert<Target.Yuv420>().Distance(original), Is.LessThanOrEqualTo(0), this.Prefix + "Yuv420.2");
            Expect(original.Convert<Target.Yvu420>().Convert<Target.Yuv420>().Distance(original), Is.LessThanOrEqualTo(0), this.Prefix + "Yuv420.3");
        }
        [Test]
        public void Yvu420()
        {
            Target.Yvu420 original = Target.Image.OpenResource("Correct/Convert/original.png").Convert<Target.Yvu420>();
            Expect(original.Convert<Target.Bgra>().Convert<Target.Yvu420>().Distance(original), Is.LessThanOrEqualTo(1), this.Prefix + "Yvu420.0");
            Expect(original.Convert<Target.Yuv420>().Convert<Target.Yvu420>().Distance(original), Is.LessThanOrEqualTo(0), this.Prefix + "Yvu420.1");
            Expect(original.Convert<Target.Yuv422>().Convert<Target.Yvu420>().Distance(original), Is.LessThanOrEqualTo(2), this.Prefix + "Yvu420.2");
            Expect(original.Convert<Target.Yvu420>().Convert<Target.Yvu420>().Distance(original), Is.LessThanOrEqualTo(0), this.Prefix + "Yvu420.4");
        }
        [Test]
        public void Yuv422()
        {
            Target.Yuv422 original = Target.Image.OpenResource("Correct/Convert/original.png").Convert<Target.Yuv422>();
            Expect(original.Convert<Target.Bgra>().Convert<Target.Yuv422>().Distance(original), Is.LessThanOrEqualTo(1), this.Prefix + "Yuv422.0");
            Expect(original.Convert<Target.Monochrome>().Convert<Target.Yuv422>().Distance(original), Is.LessThanOrEqualTo(15), this.Prefix + "Yuv422.1");
            Expect(original.Convert<Target.Yuv420>().Convert<Target.Yuv422>().Distance(original), Is.LessThanOrEqualTo(2), this.Prefix + "Yuv422.2");
            Expect(original.Convert<Target.Yuv422>().Convert<Target.Yuv422>().Distance(original), Is.LessThanOrEqualTo(0), this.Prefix + "Yuv422.3");
            Expect(original.Convert<Target.Yvu420>().Convert<Target.Yuv422>().Distance(original), Is.LessThanOrEqualTo(2), this.Prefix + "Yuv422.4");
        }
        #endregion
    }
}

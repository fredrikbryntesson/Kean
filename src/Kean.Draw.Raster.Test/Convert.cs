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
        Kean.Test.Fixture<Convert>
    {
        string prefix = "Kean.Draw.Raster.Test.Convert.";
        float tolerance = 0.01f; 
        protected override void Run()
        {
            this.Run(
                this.Open,
                this.Equality,
                this.Save,
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
            Target.Image bitmap = Target.Image.OpenResource("Bitmaps/Convert/original.jpg");
            Expect(bitmap.Resolution, Is.EqualTo(new Geometry2D.Integer.Size(800, 348)), this.prefix + "Open.0");
            Expect(bitmap.Length, Is.EqualTo(new Geometry2D.Integer.Size(800, 348).Area * 3), this.prefix + "Open.0");
        }
        [Test]
        public void Equality()
        {
            Target.Image first = Target.Image.OpenResource("Bitmaps/Convert/original.jpg");
            Target.Image second = Target.Image.OpenResource("Bitmaps/Convert/original.jpg");
            Expect(first.Distance(second), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "Equality.0");
        }
        [Test]
        public void Save()
        {
            Target.Image original = Target.Image.OpenResource("Bitmaps/Convert/original.jpg");
            original.Save("original.png");
            original.Save("original.bmp");
            Expect(original.Distance(Target.Image.Open("original.png")), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "Save.0");
            Expect(original.Distance(Target.Image.Open("original.bmp")), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "Save.1");
        }
        #endregion
        #region Convert
        [Test]
        public void Bgr()
        {
            Target.Bgr original = Target.Image.OpenResource("Bitmaps/Convert/original.jpg").Convert<Target.Bgr>();
            Expect(original.Convert<Target.Bgra>().Convert<Target.Bgr>().Distance(original), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "Bgr.0");
            Expect(original.Convert<Target.Yuv420>().Convert<Target.Bgr>().Distance(original), Is.LessThanOrEqualTo(5), this.prefix + "Bgr.2");
            Expect(original.Convert<Target.Yuv422>().Convert<Target.Bgr>().Distance(original), Is.LessThanOrEqualTo(5), this.prefix + "Bgr.3");
            Expect(original.Convert<Target.Yvu420>().Convert<Target.Bgr>().Distance(original), Is.LessThanOrEqualTo(5), this.prefix + "Bgr.4");
        }
        [Test]
        public void Bgra()
        {
            Target.Bgra original = Target.Image.OpenResource("Bitmaps/Convert/original.jpg").Convert<Target.Bgra>();
            Expect(original.Convert<Target.Bgra>().Convert<Target.Bgra>().Distance(original), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "Bgra.0");
            Expect(original.Convert<Target.Yuv420>().Convert<Target.Bgra>().Distance(original), Is.LessThanOrEqualTo(3), this.prefix + "Bgra.1");
            Expect(original.Convert<Target.Yuv422>().Convert<Target.Bgra>().Distance(original), Is.LessThanOrEqualTo(3), this.prefix + "Bgra.2");
            Expect(original.Convert<Target.Yvu420>().Convert<Target.Bgra>().Distance(original), Is.LessThanOrEqualTo(3), this.prefix + "Bgra.3");
        }
        [Test]
        public void Monochrome()
        {
            Target.Monochrome original = Target.Image.OpenResource("Bitmaps/Convert/original.jpg").Convert<Target.Monochrome>();
            Expect(original.Convert<Target.Bgra>().Convert<Target.Monochrome>().Distance(original), Is.LessThanOrEqualTo(1), this.prefix + "Monochrome.0");
            Expect(original.Convert<Target.Yuv420>().Convert<Target.Monochrome>().Distance(original), Is.LessThanOrEqualTo(0), this.prefix + "Monochrome.1");
            Expect(original.Convert<Target.Yuv422>().Convert<Target.Monochrome>().Distance(original), Is.LessThanOrEqualTo(0), this.prefix + "Monochrome.2");
            Expect(original.Convert<Target.Yvu420>().Convert<Target.Monochrome>().Distance(original), Is.LessThanOrEqualTo(0), this.prefix + "Monochrome.3");
        }
        [Test]
        public void Yuv420()
        {
            Target.Yuv420 original = Target.Image.OpenResource("Bitmaps/Convert/original.jpg").Convert<Target.Yuv420>();
            Expect(original.Convert<Target.Bgra>().Convert<Target.Yuv420>().Distance(original), Is.LessThanOrEqualTo(1), this.prefix + "Yuv420.0");
            Expect(original.Convert<Target.Yuv420>().Convert<Target.Yuv420>().Distance(original), Is.LessThanOrEqualTo(0), this.prefix + "Yuv420.1");
            Expect(original.Convert<Target.Yuv422>().Convert<Target.Yuv420>().Distance(original), Is.LessThanOrEqualTo(0), this.prefix + "Yuv420.2");
            Expect(original.Convert<Target.Yvu420>().Convert<Target.Yuv420>().Distance(original), Is.LessThanOrEqualTo(0), this.prefix + "Yuv420.3");
        }
        [Test]
        public void Yvu420()
        {
            Target.Yvu420 original = Target.Image.OpenResource("Bitmaps/Convert/original.jpg").Convert<Target.Yvu420>();
            Expect(original.Convert<Target.Bgra>().Convert<Target.Yvu420>().Distance(original), Is.LessThanOrEqualTo(1), this.prefix + "Yvu420.0");
            Expect(original.Convert<Target.Yuv420>().Convert<Target.Yvu420>().Distance(original), Is.LessThanOrEqualTo(0), this.prefix + "Yvu420.1");
            Expect(original.Convert<Target.Yuv422>().Convert<Target.Yvu420>().Distance(original), Is.LessThanOrEqualTo(2), this.prefix + "Yvu420.2");
            Expect(original.Convert<Target.Yvu420>().Convert<Target.Yvu420>().Distance(original), Is.LessThanOrEqualTo(0), this.prefix + "Yvu420.4");
        }
        [Test]
        public void Yuv422()
        {
            Target.Yuv422 original = Target.Image.OpenResource("Bitmaps/Convert/original.jpg").Convert<Target.Yuv422>();
            Expect(original.Convert<Target.Bgra>().Convert<Target.Yuv422>().Distance(original), Is.LessThanOrEqualTo(1), this.prefix + "Yuv422.0");
            Expect(original.Convert<Target.Monochrome>().Convert<Target.Yuv422>().Distance(original), Is.LessThanOrEqualTo(15), this.prefix + "Yuv422.1");
            Expect(original.Convert<Target.Yuv420>().Convert<Target.Yuv422>().Distance(original), Is.LessThanOrEqualTo(2), this.prefix + "Yuv422.2");
            Expect(original.Convert<Target.Yuv422>().Convert<Target.Yuv422>().Distance(original), Is.LessThanOrEqualTo(0), this.prefix + "Yuv422.3");
            Expect(original.Convert<Target.Yvu420>().Convert<Target.Yuv422>().Distance(original), Is.LessThanOrEqualTo(2), this.prefix + "Yuv422.4");
        }
        #endregion
    }
}

// 
//  Transform.cs
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
    public class Transform : 
         Kean.Test.Fixture<Transform>
    {
        string prefix = "Kean.Draw.Raster.Test.Transform.";
        float tolerance = 0.01f;
        protected override void Run()
        {
            this.Run(
				this.Different,
                this.CutRotate,
                this.CutScale,
                this.CutTranslate,
                this.CoordinateSystems,
				this.ResizeWithin,
                this.ResizeTo
                );
        }
		[Test]
		public void Different()
		{
			 Target.Image a = Target.Image.OpenResource("Bitmaps/Transform/scaled.png");
			 Target.Image b = Target.Image.OpenResource("Bitmaps/Transform/translated.png");
			 float distance = a.Distance(b);
			 Expect(distance, Is.GreaterThan(50), this.prefix + "Different.0");
		}
        #region  Cut, resize and copy
        [Test]
        public void CutRotate()
        {
            Target.Image image = Target.Image.OpenResource("Bitmaps/Transform/original.png");
            Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 100);
            Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.CreateRotation(Kean.Math.Single.ToRadians(75));
            Target.Image copy = image.Copy(size, transform) as Target.Image;
            Expect(copy.Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Bitmaps/Transform/rotated.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(1.5f), this.prefix + "CutRotate.0");
        }
        [Test]
        public void CutScale()
        {
            Target.Image image = Target.Image.OpenResource("Bitmaps/Transform/original.png");
            Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 240);
            Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.CreateScaling(1.5f);
            Target.Image copy = image.Copy(size, transform) as Target.Image;
			copy.Save("copy.png");
			image.Save("image.png");
			float distance = copy.Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Bitmaps/Transform/scaled.png").Convert<Target.Bgra>());
            Expect(distance, Is.LessThanOrEqualTo(20), this.prefix + "CutScale.0");
        }
        [Test]
        public void CutTranslate()
        {
            Target.Image image = Target.Image.OpenResource("Bitmaps/Transform/original.png");
            Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 240);
            Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.CreateTranslation(100, 100);
            Target.Image copy = image.Copy(size, transform) as Target.Image;
            Expect(copy.Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Bitmaps/Transform/translated.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "CutTranslate.0");
        }
        [Test]
        public void CoordinateSystems()
        {
            Target.Image source = Target.Image.OpenResource("Bitmaps/Transform/original.png");
            Target.Image image = new Target.Bgr(new Kean.Core.Buffer.Sized(source.Pointer, source.Size.Area * 3), source.Size, CoordinateSystem.XLeftward | CoordinateSystem.YUpward);
            Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 100);
            Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.CreateRotation(Kean.Math.Single.ToRadians(45));
            Target.Image copy = image.Copy(size, transform) as Target.Image;
            Expect(copy.Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Bitmaps/Transform/coordinateSystem.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(2), this.prefix + "CoordinateSystems.0");
        }
        [Test]
        public void ResizeWithin()
        {
            Target.Image first = Target.Image.OpenResource("Bitmaps/Transform/original.png");
            Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 100);
            Expect(first.ResizeWithin(size).Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Bitmaps/Transform/resized1.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(20), this.prefix + "Resize1.0");
        }
        [Test]
        public void ResizeTo()
        {
            Target.Image first = Target.Image.OpenResource("Bitmaps/Transform/original.png");
            Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(100, 100);
            Expect(first.ResizeTo(size).Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Bitmaps/Transform/resized2.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(20), this.prefix + "Resize2.0");
        }
        #endregion
    }
}

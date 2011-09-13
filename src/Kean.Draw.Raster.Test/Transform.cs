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
                this.CutRotate,
                this.CutScale,
                this.CutTranslate,
                this.CoordinateSystems,
                this.Resize
                );
        }
        #region  Cut, resize and copy
        [Test]
        public void CutRotate()
        {
            Target.Image image = Target.Image.OpenResource("Bitmaps/Transform/original.png");
            Geometry2D.Single.Size size = new Geometry2D.Single.Size(320, 100);
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
            Expect(copy.Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Bitmaps/Transform/scaled.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "CutScale.0");
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
            Target.Image image = new Target.Bgr(new Kean.Core.Buffer.Sized(source.Pointer, source.Resolution.Area * 3), source.Resolution, CoordinateSystem.XLeftward | CoordinateSystem.YUpward);
            Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 100);
            Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.CreateRotation(Kean.Math.Single.ToRadians(45));
            Target.Image copy = image.Copy(size, transform) as Target.Image;
            Expect(copy.Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Bitmaps/Transform/coordinateSystem.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "CoordinateSystems.0");
        }
        [Test]
        public void Resize()
        {
            Target.Image first = Target.Image.OpenResource("Bitmaps/Transform/original.png");
            Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 100);
            Target.Image copy = first.Resize(size) as Target.Image;
            Expect(first.Resize(size).Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Bitmaps/Transform/resized.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(this.tolerance), this.prefix + "Resize.0");
        }
        #endregion
    }
}

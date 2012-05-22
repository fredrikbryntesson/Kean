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
         Fixture<Transform>
    {
		public Transform() :
			base(0.05f)
		{ }
        protected override void Run()
        {
            this.Run(
                this.AspectRatio,
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
			 using (Target.Image a = Target.Image.OpenResource("Correct/Transform/scaled.png"))
			 using (Target.Image b = Target.Image.OpenResource("Correct/Transform/translated.png"))
				Verify(a.Distance(b), Is.GreaterThan(50));
		}
        #region  Cut, resize and copy
        [Test]
        public void CutRotate()
        {
			using (Target.Image image = Target.Image.OpenResource("Correct/Transform/original.png"))
			{
				Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 100);
				Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.CreateRotation(Kean.Math.Single.ToRadians(75));
				using (Target.Image copy = image.Copy(size, transform) as Target.Image)
					Verify(copy, "Correct/Transform/rotated.png");
			}
        }
        [Test]
        public void CutScale()
        {
			using (Target.Image image = Target.Image.OpenResource("Correct/Transform/original.png"))
			{
				Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 240);
				Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.CreateScaling(1.5f);
				using (Target.Image copy = image.Copy(size, transform) as Target.Image)
				{
					//copy.Save("copy.png");
					//image.Save("image.png");

					float distance = copy.Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Correct/Transform/scaled.png").Convert<Target.Bgra>());
					Verify(distance, Is.LessThanOrEqualTo(20));
				}
			}
        }
        [Test]
        public void CutTranslate()
        {
            Target.Image image = Target.Image.OpenResource("Correct/Transform/original.png");
            Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 240);
            Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.CreateTranslation(100, 100);
            Target.Image copy = image.Copy(size, transform) as Target.Image;
            Expect(copy.Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Correct/Transform/translated.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(this.Tolerance), this.Prefix + "CutTranslate.0");
        }
        [Test]
        public void CoordinateSystems()
        {
            Target.Image source = Target.Image.OpenResource("Correct/Transform/original.png");
            Target.Image image = new Target.Bgr(new Kean.Core.Buffer.Sized(source.Pointer, source.Size.Area * 3), source.Size, CoordinateSystem.XLeftward | CoordinateSystem.YUpward);
            Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 100);
            Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.CreateRotation(Kean.Math.Single.ToRadians(45));
            Target.Image copy = image.Copy(size, transform) as Target.Image;
            Expect(copy.Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Correct/Transform/coordinateSystem.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(2), this.Prefix + "CoordinateSystems.0");
        }
        [Test]
        public void ResizeWithin()
        {
            Target.Image first = Target.Image.OpenResource("Correct/Transform/original.png");
            Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 100);
            Expect(first.ResizeWithin(size).Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Correct/Transform/resized1.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(20), this.Prefix + "Resize1.0");
        }
        [Test]
        public void ResizeTo()
        {
            Target.Image first = Target.Image.OpenResource("Correct/Transform/original.png");
            Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(100, 100);
            Expect(first.ResizeTo(size).Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Correct/Transform/resized2.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(20), this.Prefix + "Resize2.0");
        }
        [Test]
        public void AspectRatio()
        {
            Target.Image first = Target.Image.OpenResource("Correct/Transform/original.png");
            float ratio = 3f;
            Target.Image second = first.Copy(new Geometry2D.Integer.Size(Kean.Math.Integer.Round((ratio != 0 ? ratio : 1) * first.Size.Height), first.Size.Height), Geometry2D.Single.Transform.CreateScaling(first.Size.Width / (float)first.Size.Height / (ratio != 0  ? ratio : 1), 1)) as Target.Image;
            Expect(second.Convert<Target.Bgra>().Distance(Target.Image.OpenResource("Correct/Transform/aspect.png").Convert<Target.Bgra>()), Is.LessThanOrEqualTo(20), this.Prefix + "Resize2.0");
        }
        
        #endregion
    }
}

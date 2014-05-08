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

using Target = Kean.Draw.Raster;
using Geometry2D = Kean.Math.Geometry2D;
using Geometry3D = Kean.Math.Geometry3D;
namespace Kean.Draw.Raster.Test
{
	[TestFixture]
	public class Transform : 
		 Fixture<Transform>
	{
		public Transform() :
			base(1f)
		{ }
		protected override void Run()
		{
			this.Run(
				this.ShiftMonochrome,
				this.ShiftYuyv,
				this.ShiftYuv420,
				this.ShiftYvu420,
				this.ShiftBgra,
				this.ShiftBgr,
				this.AspectRatio,
				this.Different,
				this.CutRotate,
				this.CutScale,
				this.CutTranslate,
				this.CoordinateSystems,
				this.ResizeWithin,
				this.ResizeTo,
				this.Draw3DTransformIdentity,
				this.Draw3DTransform,
				this.Draw3DTransformAndCorrect
				);
		}
		[Test]
		public void Different()
		{
			using (Target.Image a = Target.Image.OpenResource("Draw/Raster/Correct/Transform/scaled.png"))
			using (Target.Image b = Target.Image.OpenResource("Draw/Raster/Correct/Transform/translated.png"))
				Verify(a.Distance(b), Is.GreaterThan(50));
		}
		#region Shift
		[Test]
		public void ShiftMonochrome()
		{
			using (Target.Monochrome image = Target.Monochrome.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			using (Target.Monochrome shifted = image.Shift(new Kean.Math.Geometry2D.Integer.Size(51, -131)) as Target.Monochrome)
				Verify(shifted, "Draw/Raster/Correct/Transform/shiftedMonochrome.png");
		}
		[Test]
		public void ShiftBgr()
		{
			using (Target.Bgr image = Target.Bgr.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			using (Target.Bgr shifted = image.Shift(new Kean.Math.Geometry2D.Integer.Size(51, -131)) as Target.Bgr)
				Verify(shifted, "Draw/Raster/Correct/Transform/shiftedBgr.png");
		}
		[Test]
		public void ShiftBgra()
		{
			using (Target.Bgra image = Target.Bgra.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			using (Target.Bgra shifted = image.Shift(new Kean.Math.Geometry2D.Integer.Size(51, -131)) as Target.Bgra)
				Verify(shifted, "Draw/Raster/Correct/Transform/shiftedBgra.png");
		}
		[Test]
		public void ShiftYuyv()
		{
			using (Target.Yuyv image = Target.Yuyv.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			using (Target.Yuyv shifted = image.Shift(new Kean.Math.Geometry2D.Integer.Size(51, -131)) as Target.Yuyv)
				Verify(shifted, "Draw/Raster/Correct/Transform/shiftedYuyv.png");
		}
		[Test]
		public void ShiftYuv420()
		{
			using (Target.Yuv420 image = Target.Yuv420.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			using (Target.Yuv420 shifted = image.Shift(new Kean.Math.Geometry2D.Integer.Size(51, -131)) as Target.Yuv420)
				Verify(shifted, "Draw/Raster/Correct/Transform/shiftedYuv420.png");
		}
		[Test]
		public void ShiftYvu420()
		{

			using (Target.Yvu420 image = Target.Yvu420.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			using (Target.Yvu420 shifted = image.Shift(new Kean.Math.Geometry2D.Integer.Size(51, -131)) as Target.Yvu420)
				Verify(shifted, "Draw/Raster/Correct/Transform/shiftedYvu420.png");
		}
		[Test]
		public void Draw3DTransformIdentity()
		{
			using (Raster.Image first = Raster.Image.OpenResource("Draw/Raster/Correct/Transform/600x600b.png"))
			using (var image = new Raster.Bgra(first.Size))
			{
				first.ProjectOn(image, Geometry3D.Single.Transform.CreateTranslation(0, 0, 0), new Geometry2D.Single.Size(Math.Single.ToRadians(45f), Math.Single.ToRadians(45f)));
				Verify(image, "Draw/Raster/Correct/Transform/600x600b.png");
			}
		}
		[Test]
		public void Draw3DTransform()
		{
			using (Raster.Image first = Raster.Image.OpenResource("Draw/Raster/Correct/Transform/600x600b.png"))
			using (var image = new Raster.Bgra(first.Size))
			{
				first.ProjectOn(image, Geometry3D.Single.Transform.CreateRotationX(0.2f) * Geometry3D.Single.Transform.CreateRotationY(0.2f), new Geometry2D.Single.Size(Math.Single.ToRadians(45f), Math.Single.ToRadians(45f)));
				Verify(image, "Draw/Raster/Correct/Transform/600x600transformed.png");
			}
		}
		[Test]
		public void Draw3DTransformAndCorrect()
		{
			using (Raster.Image first = Raster.Image.OpenResource("Draw/Raster/Correct/Transform/600x600b.png"))
			using (var image = new Raster.Bgra(first.Size))
			using (var image2 = new Raster.Bgra(first.Size))
			{
				var transform = Geometry3D.Single.Transform.CreateRotationX(0.2f) * Geometry3D.Single.Transform.CreateRotationY(0.2f);
				first.ProjectOn(image, transform, new Geometry2D.Single.Size(Math.Single.ToRadians(45f), Math.Single.ToRadians(45f)));
				image.ProjectOn(image2, transform.Inverse, new Geometry2D.Single.Size(Math.Single.ToRadians(45f), Math.Single.ToRadians(45f))); 
				Verify(image2, "Draw/Raster/Correct/Transform/600x600corrected.png");
			}
		}

		#endregion
		#region  Cut, resize and copy
		[Test]
		public void CutRotate()
		{
			using (Target.Image image = Target.Image.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			{
				Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 100);
				Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.CreateRotation(Kean.Math.Single.ToRadians(75));
				using (Target.Image copy = image.Copy(size, transform) as Target.Image)
					Verify(copy, "Draw/Raster/Correct/Transform/rotated.png");
			}
		}
		[Test]
		public void CutScale()
		{
			using (Target.Image image = Target.Image.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			{
				Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 240);
				Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.CreateScaling(1.5f);
				using (Target.Bgra copy = image.Copy(size, transform).Convert<Target.Bgra>())
					Verify(copy, "Draw/Raster/Correct/Transform/scaled.png");
			}
		}
		[Test]
		public void CutTranslate()
		{
			using (Target.Image image = Target.Image.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			{
				Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 240);
				using(Target.Image copy = image.Copy(size, Geometry2D.Single.Transform.CreateTranslation(100, 100)) as Target.Image)
					Verify(copy, "Draw/Raster/Correct/Transform/translated.png");
			}
		}
		[Test]
		public void CoordinateSystems()
		{
			using (Target.Image source = Target.Image.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			using (Target.Image image = new Target.Bgr(new Kean.Buffer.Sized(source.Pointer, source.Size.Area * 3), source.Size, CoordinateSystem.XLeftward | CoordinateSystem.YUpward))
			{
				Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 100);
				Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.CreateRotation(Kean.Math.Single.ToRadians(45));
				using(Target.Image copy = image.Copy(size, transform) as Target.Image)
					Verify(copy, "Draw/Raster/Correct/Transform/coordinateSystem.png");
			}
		}
		[Test]
		public void ResizeWithin()
		{
			using (Target.Image first = Target.Image.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			{
				Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(320, 100);
				Verify(first.ResizeWithin(size).Convert<Raster.Bgra>(), "Draw/Raster/Correct/Transform/resized1.png");
			}
		}
		[Test]
		public void ResizeTo()
		{
			using (Target.Image first = Target.Image.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			{
				Geometry2D.Integer.Size size = new Geometry2D.Integer.Size(100, 100);
				Verify(first.ResizeTo(size).Convert<Raster.Bgra>(), "Draw/Raster/Correct/Transform/resized2.png");
			}
		}
		[Test]
		public void AspectRatio()
		{
			using (Target.Image first = Target.Image.OpenResource("Draw/Raster/Correct/Transform/original.png"))
			{
				float ratio = 3f;
				Target.Image second = first.Copy(new Geometry2D.Integer.Size(Kean.Math.Integer.Round((ratio != 0 ? ratio : 1) * first.Size.Height), first.Size.Height), Geometry2D.Single.Transform.CreateScaling(first.Size.Width / (float)first.Size.Height / (ratio != 0 ? ratio : 1), 1)) as Target.Image;
				Verify(second, "Draw/Raster/Correct/Transform/aspect.png");
			}
		}
		
		#endregion
	}
}

// 
//  Fixture.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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

using Kean.Core;
using Kean.Core.Extension;
using NUnit.Framework;

using Reflect = Kean.Core.Reflect;

namespace Kean.Draw.Raster.Test
{
	public abstract class Fixture<T> :
		Kean.Test.Fixture<T>
		where T : Fixture<T>, new()
	{
		public float Tolerance { get; private set; }
		protected Fixture() :
			this(0.01f)
		{ }
		protected Fixture(float tolerance)
		{
			this.Tolerance = tolerance;
		}
		public void Verify(Draw.Image image, string resource)
		{
			using (Image correct = Image.OpenResource(System.Reflection.Assembly.GetCallingAssembly(), resource))
				this.Verify(image, correct);
		}
		public void Verify(Draw.Image image, Raster.Image correct)
		{
			try
			{
				if (correct is Raster.Bgr)
				{
					if (image is Raster.Bgr)
						this.Verify(image.Distance(correct), Is.LessThanOrEqualTo(this.Tolerance));
					else
						using (Raster.Bgr i = image.Convert<Raster.Bgr>())
							this.Verify(i.Distance(correct), Is.LessThanOrEqualTo(this.Tolerance));
				}
				else if (correct is Raster.Bgra)
				{
					if (image is Raster.Bgra)
						this.Verify(image.Distance(correct), Is.LessThanOrEqualTo(this.Tolerance));
					else
						using (Raster.Bgra i = image.Convert<Raster.Bgra>())
							this.Verify(i.Distance(correct), Is.LessThanOrEqualTo(this.Tolerance));
				}
			}
			catch
			{
				using (Raster.Image raster = image.Convert<Raster.Image>())
					raster.Save(this.CurrentTestStep + ".png");
				throw;
			} 
		}
	}
}

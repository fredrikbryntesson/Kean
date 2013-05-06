// 
//  Convert.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
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

using Target = Kean.Draw.Color;

namespace Kean.Draw.Test.Color
{
	public class Convert :
		Kean.Test.Fixture<Convert>
	{
		string prefix = "Kean.Draw.Color.Test.";

		protected override void Run ()
		{
			this.Run (
				this.FromBgr, 
				this.FromBgra,
				this.FromY,
				this.FromYuv
				);
		}

		[Test]
		public void FromBgr ()
		{
			Target.Bgr value = new Target.Bgr (120, 100, 80);
			Verify(value.Convert<Target.Y>(), Is.EqualTo(new Target.Y(95)), this.prefix + "FromBgr.0");
			Verify(value.Convert<Target.Yuv>(), Is.EqualTo(new Target.Yuv(95, 141, 116)), this.prefix + "FromBgr.1");
			Verify(value.Convert<Target.Bgr>(), Is.EqualTo(new Target.Bgr(120, 100, 80)), this.prefix + "FromBgr.2");
			Verify(value.Convert<Target.Bgra>(), Is.EqualTo(new Target.Bgra(120, 100, 80, 255)), this.prefix + "FromBgr.3");
		}

		[Test]
		public void FromBgra ()
		{
			Target.Bgra value = new Target.Bgra (120, 100, 80, 60);
			Verify(value.Convert<Target.Y>(), Is.EqualTo(new Target.Y(95)), this.prefix + "FromBgra.0");
			Verify(value.Convert<Target.Yuv>(), Is.EqualTo(new Target.Yuv(95, 141, 116)), this.prefix + "FromBgra.1");
			Verify(value.Convert<Target.Bgr>(), Is.EqualTo(new Target.Bgr(120, 100, 80)), this.prefix + "FromBgra.2");
			Verify(value.Convert<Target.Bgra>(), Is.EqualTo(new Target.Bgra(120, 100, 80, 60)), this.prefix + "FromBgra.3");
		}

		[Test]
		public void FromY ()
		{
			Target.Y value = new Target.Y (95);
			Verify(value.Convert<Target.Y>(), Is.EqualTo(new Target.Y(95)), this.prefix + "FromY.0");
			Verify(value.Convert<Target.Yuv>(), Is.EqualTo(new Target.Yuv(95, 128, 128)), this.prefix + "FromY.1");
			Verify(value.Convert<Target.Bgr>(), Is.EqualTo(new Target.Bgr(95, 95, 95)), this.prefix + "FromY.2");
			Verify(value.Convert<Target.Bgra>(), Is.EqualTo(new Target.Bgra(95, 95, 95, 255)), this.prefix + "FromY.3");
		}

		[Test]
		public void FromYuv ()
		{
			Target.Yuv value = new Target.Yuv (95, 141, 116);
			Verify(value.Convert<Target.Y>(), Is.EqualTo(new Target.Y(95)), this.prefix + "FromYuv.0");
			Verify(value.Convert<Target.Yuv>(), Is.EqualTo(new Target.Yuv(95, 141, 116)), this.prefix + "FromYuv.1");
			Verify(value.Convert<Target.Bgr>(), Is.EqualTo(new Target.Bgr(118, 99, 78)), this.prefix + "FromYuv.2");
			Verify(value.Convert<Target.Bgra>(), Is.EqualTo(new Target.Bgra(118, 99, 78, 255)), this.prefix + "FromYuv.3");
		}
	}
}

// 
//  Bgra.cs
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
using Geometry2D = Kean.Math.Geometry2D;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Kean.Draw.Gpu.Test
{
	public class Bgra :
		Abstract<Bgra>
	{
		[Test]
		public void Create()
		{
			using (Gpu.Bgra image = new Gpu.Bgra(new Geometry2D.Integer.Size(128, 256)))
			{
				Expect(image, Is.Not.Null);
				Expect(image.Size, Is.EqualTo(new Geometry2D.Single.Size(128, 256)));
			}

		}
		protected override void Run()
		{
			this.Run(
				this.Create
				);
		}
	}
}

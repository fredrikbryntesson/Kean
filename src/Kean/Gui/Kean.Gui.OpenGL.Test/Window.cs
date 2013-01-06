// 
//  Window.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;

using System;
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using NUnit.Framework;
using NUnit.Framework;

namespace Kean.Gui.OpenGL.Test
{
	public class Window :
		Kean.Test.Fixture<Window>
	{
		[Test]
		public void Create()
		{
			using (Gui.Backend.IWindow window = new Gui.OpenGL.Window(new Geometry2D.Integer.Size(1024, 768), "Kean Test"))
			{
				window.Visible = true;
				window.Render += canvas =>
				{
					//canvas.Clear();
					canvas.Draw(new Draw.Color.Bgra(255, 128, 0, 128), new Geometry2D.Single.Box(20, 50, 300, 100));
					Console.Write("T");
				};
				//System.Timers.Timer timer = new System.Timers.Timer(40);
				//timer.Elapsed += (sender, e) => window.Invalidate();
				//timer.Start();
				window.Run();
				//timer.Stop();
			}
		}

		protected override void Run()
		{
			//this.Run(this.Create);
		}
	}
}

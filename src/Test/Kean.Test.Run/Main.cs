// 
//  Main.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009 Simon Mika
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
//  You should have received data copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;

namespace Kean.Test.Run
{
	class MainClass
	{
		public static void Main(string[] args)
		{
            //Kean.Draw.Jpeg.Test.All.Test();
            Xml.Serialize.Test.All.Test();
            Kean.Draw.Net.Test.All.Test();
            Math.Test.All.Test();
            

            Core.Reflect.Test.All.Test();

			Core.Test.All.Test();
			Core.Collection.Test.All.Test();
			Core.Reflect.Test.All.Test();
			Core.Uri.Test.All.Test();
			Xml.Dom.Test.All.Test();
			Draw.Raster.Test.Transform.Test();
            Draw.Raster.Test.Convert.Test();
			Draw.Gpu.Test.All.Test();
			Gui.OpenGL.Test.Window.Test();
			Math.Test.All.Test();
            Math.Complex.Test.All.Test();
            Math.Geometry2D.Test.All.Test();
            Math.Geometry3D.Test.All.Test();
            Math.Matrix.Test.All.Test();
            Math.Random.Test.All.Test();
            Math.Regression.Test.All.Test();
            //Core.Error.Test.Error.Test();

		}
	}
}
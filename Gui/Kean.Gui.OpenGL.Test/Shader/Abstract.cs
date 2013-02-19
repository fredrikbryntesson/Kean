// 
//  Abstract.cs
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
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Gui.OpenGL.Test.Shader
{
	public abstract class Abstract<T> :
		Kean.Test.Fixture<T>
		where T : Abstract<T>, new()
	{
		Gui.Backend.IWindow window;
		public override void Setup()
		{
			base.Setup();
			this.window = new Gui.OpenGL.Window(new Geometry2D.Integer.Size(100, 100), "");
		}
		public override void TearDown()
		{
			this.window.Dispose();
			base.TearDown();
		}
	}
}

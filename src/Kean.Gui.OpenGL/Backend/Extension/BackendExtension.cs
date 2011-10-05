// 
//  BackendExtension.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
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
using GL = OpenTK.Graphics.OpenGL.GL;

namespace Kean.Gui.OpenGL.Backend.Extension
{

	public static class BackendExtension
	{
		#region Vector
		public static OpenTK.Vector2 Vector2(this Geometry2D.Single.Size size)
		{
			return new OpenTK.Vector2(size.Width, size.Height);
		}
		public static OpenTK.Vector2 Vector2(this Geometry2D.Single.Point point)
		{
			return new OpenTK.Vector2(point.X, point.Y);
		}
		#endregion
		#region Transform
		public static OpenTK.Matrix4 Matrix4(this Geometry2D.Single.Transform transform)
		{
			return new OpenTK.Matrix4()
			{
				Row0 = new OpenTK.Vector4(transform.A, transform.B, 0, 0),
				Row1 = new OpenTK.Vector4(transform.C, transform.D, 0, 0),
				Row2 = new OpenTK.Vector4(0, 0, 1, 0),
				Row3 = new OpenTK.Vector4(transform.E, transform.F, 0, 1),
			};
		}
		public static void Multiply(this Geometry2D.Single.Transform transform)
		{
			OpenTK.Matrix4 matrix = transform.Matrix4();
			GL.MultMatrix(ref matrix);
		}
		public static void Load(this Geometry2D.Single.Transform transform)
		{
			OpenTK.Matrix4 matrix = transform.Matrix4();
			GL.LoadMatrix(ref matrix);
		}
		#endregion
	}
}

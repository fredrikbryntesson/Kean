//
//  Context.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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


using Kean.Core.Collection.Extension;
using System;
using Collection = Kean.Core.Collection;
using Error = Kean.Core.Error;
using GL = OpenTK.Graphics.OpenGL.GL;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Context
	{
		public abstract Texture CreateTexture();
		public abstract Composition CreateComposition();
		public abstract Program CreateProgram();
		public abstract Shader CreateShader(ShaderType type);
		public abstract Geometry2D.Integer.Size ClampTextureSize(Geometry2D.Integer.Size size);

		static Context context = new Backend.OpenGL21.Context();
		public static Context Current 
		{
			get { return Context.context; }
			protected set { Context.context = value ?? new Backend.OpenGL21.Context(); }
		}
	}
}

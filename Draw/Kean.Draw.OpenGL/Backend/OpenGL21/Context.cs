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

namespace Kean.Draw.OpenGL.Backend.OpenGL21
{
	public class Context :
        Backend.Context
	{
		int maximumTextureSize;
		int MaximumTextureSize
		{
			get 
			{
 				if (this.maximumTextureSize == 0)
					GL.GetInteger(OpenTK.Graphics.OpenGL.GetPName.MaxTextureSize, out this.maximumTextureSize);
				return maximumTextureSize; 
			}
		}

		public Context()
		{
			GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
		}
        protected override Backend.Texture AllocateTexture()
        {
            return new Texture(this);
        }
		protected override Backend.Composition AllocateComposition()
        {
            return new Composition(this);
        }
        public override Backend.Program CreateProgram()
        {
            return new Program(this);
        }
        public override Backend.Shader CreateShader(ShaderType type)
        {
            return new Shader(this, type);
        }
		protected internal override Geometry2D.Integer.Size ClampTextureSize(Geometry2D.Integer.Size size)
		{
			if (size.Width > this.MaximumTextureSize)
			{
				Error.Log.Append(Error.Level.Warning, "Texture Width Exceeds Limit", string.Format("The requested texture size \"{0}\" is bigger than the maximum texture size \"{1}, {1}\". The textures width will be clamped.", size, this.MaximumTextureSize));
				size = new Geometry2D.Integer.Size(this.MaximumTextureSize, size.Height);
			}
			if (size.Height > this.MaximumTextureSize)
			{
				Error.Log.Append(Error.Level.Warning, "Texture Height Exceeds Limit", string.Format("The requested texture size \"{0}\" is bigger than the maximum texture size \"{1}, {1}\". The textures height will be clamped.", size, this.MaximumTextureSize));
				size = new Geometry2D.Integer.Size(size.Width, this.MaximumTextureSize);
			}
			return size;
		}
	}
}

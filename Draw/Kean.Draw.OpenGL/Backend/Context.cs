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
		RecycleBin<Texture> textureBin;
		RecycleBin<Composition> compositionBin;
		WasteBin<Program> programBin;
		WasteBin<Shader> shaderBin;

		protected Context()
		{
			//this.textureBin = new RecycleBin<Texture>(this.Delete);
			//this.compositionBin = new RecycleBin<Composition>(this.Delete);
			//this.programBin = new WasteBin<Program>(this.Delete);
			//this.shaderBin = new WasteBin<Shader>(this.Delete);
		}

		public Texture CreateTexture(TextureType type, Geometry2D.Integer.Size size)
		{
			Texture result = this.NewTexture();
			result.Create(type, size);
			return result;
		}
		public Texture CreateTexture(Raster.Image image)
		{
			Texture result = this.NewTexture();
			result.Create(image);
			return result;
		}
		public Composition CreateComposition(TextureType type, Geometry2D.Integer.Size size)
		{
			Composition result = this.NewComposition();
			result.Texture.Create(type, size);
			result.Create();
			return result;
		}
		public Composition CreateComposition(Raster.Image image)
		{
			Composition result = this.NewComposition();
			result.Texture.Create(image);
			result.Create();
			return result;
		}
		public abstract Program NewProgram();
		public abstract Shader NewShader(ShaderType type);
		public abstract Geometry2D.Integer.Size ClampTextureSize(Geometry2D.Integer.Size size);

		

		protected abstract Texture NewTexture();
		protected abstract Composition NewComposition();

		//protected abstract void Delete(Texture texture);
		//protected abstract void Delete(Composition composition);
		//protected abstract void Delete(Program program);
		//protected abstract void Delete(Shader shader);

		#region Static
		static Context current;
		public static Context Current 
		{
			get 
			{
 				if (Context.current.IsNull())
					Context.current =  new Backend.OpenGL21.Context();
				return Context.current; 
			}
			protected set { Context.current = value; }
		}
		public static void Free()
		{
			FrameBuffer.Free();
			Shader.Free();
			Program.Free();
			Texture.Free();
		}
		#endregion
	}
}

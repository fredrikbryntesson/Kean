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
	public abstract class Context :
		IDisposable
	{
		RecycleBin<Texture> textureBin;
		RecycleBin<Composition> compositionBin;
		WasteBin<Depth> depthBin;
		WasteBin<FrameBuffer> frameBufferBin;
		WasteBin<Program> programBin;
		WasteBin<Shader> shaderBin;

		protected Context()
		{
			this.compositionBin = new RecycleBin<Composition>(item => item.Delete());
			this.textureBin = new RecycleBin<Texture>(item => item.Delete());
			this.depthBin = new WasteBin<Depth>(item => item.Delete());
			this.frameBufferBin = new WasteBin<FrameBuffer>(item => item.Delete());
			this.programBin = new WasteBin<Program>(item => item.Delete());
			this.shaderBin = new WasteBin<Shader>(item => item.Delete());
		}
		public Texture CreateTexture(TextureType type, Geometry2D.Integer.Size size)
		{
			this.Free();
			Texture result = this.AllocateTexture();
			result.Create(type, size);
			return result;
		}
		public Texture CreateTexture(Raster.Image image)
		{
			this.Free();
			Texture result = this.AllocateTexture();
			result.Create(image);
			return result;
		}
		public Composition CreateComposition(TextureType type, Geometry2D.Integer.Size size)
		{
			this.Free();
			Composition result = this.AllocateComposition();
			result.Texture.Create(type, size);
			result.Create();
			return result;
		}
		public Composition CreateComposition(Raster.Image image)
		{
			this.Free();
			Composition result = this.AllocateComposition();
			result.Texture.Create(image);
			result.Create();
			return result;
		}

		public abstract Program CreateProgram();
		public abstract Shader CreateShader(ShaderType type);
		protected abstract Texture AllocateTexture();
		protected abstract Composition AllocateComposition();

		internal protected abstract Geometry2D.Integer.Size ClampTextureSize(Geometry2D.Integer.Size size);

		internal void Recycle(Composition composition)
		{
			this.compositionBin.Add(composition);
		}
		internal void Recycle(Texture texture)
		{
			this.textureBin.Add(texture);
		}
		internal void Recycle(Depth depth)
		{
			this.depthBin.Add(depth);
		}
		internal void Recycle(FrameBuffer frameBuffer)
		{
			this.frameBufferBin.Add(frameBuffer);
		}
		internal void Recycle(Program program)
		{
			this.programBin.Add(program);
		}
		internal void Recycle(Shader shader)
		{
			this.shaderBin.Add(shader);
		}

		void Free()
		{
			this.compositionBin.Free();
			this.textureBin.Free();
			this.depthBin.Free();
			this.frameBufferBin.Free();
			this.programBin.Free();
			this.shaderBin.Free();
		}
		public void Dispose()
		{
			this.compositionBin.On = this.textureBin.On = false;
			this.Free();
			this.compositionBin.On = this.textureBin.On = true;


			//if (this.compositionBin.NotNull())
			//{
			//	this.compositionBin.Dispose();
			//	this.compositionBin = null;
			//}
			//if (this.textureBin.NotNull())
			//{
			//	this.textureBin.Dispose();
			//	this.textureBin = null;
			//}
			//if (this.depthBin.NotNull())
			//{
			//	this.depthBin.Dispose();
			//	this.depthBin = null;
			//}
			//if (this.frameBufferBin.NotNull())
			//{
			//	this.frameBufferBin.Dispose();
			//	this.frameBufferBin = null;
			//}
			//if (this.programBin.NotNull())
			//{
			//	this.programBin.Dispose();
			//	this.programBin = null;
			//}
			//if (this.shaderBin.NotNull())
			//{
			//	this.shaderBin.Dispose();
			//	this.shaderBin = null;
			//}
		}


		#region Static
		static Context current;
		public static Context Current 
		{
			get { return Context.current; }
			internal set { Context.current = value; }
		}
		#endregion
	}
}

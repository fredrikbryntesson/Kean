// 
//  Factory.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using GL = OpenTK.Graphics.OpenGL.GL;
using Error = Kean.Core.Error;
using Draw = Kean.Draw;
using Gpu = Kean.Draw.Gpu;
using Raster = Kean.Draw.Raster;
using Kean.Gui.OpenGL.Backend.Extension;
using Recycle = Kean.Core.Recycle;

namespace Kean.Gui.OpenGL.Backend
{
	public abstract class Factory :
		Gpu.Backend.IFactory
	{
		Recycle.IBin<Texture, Geometry2D.Integer.Size>[] textures;
		#region Constructors
		protected Factory()
		{
			Func<int, int> index = area => area < 10000 ? 0 : area < 100000 ? 1 : 2;
			this.textures = new Recycle.IBin<Texture, Geometry2D.Integer.Size>[Enum.GetValues(typeof(Gpu.Backend.TextureType)).Length].
				Initialize(() => new Recycle.Bins<Texture, Geometry2D.Integer.Size>(10, 3,
					(texture, size) => texture.Size == size,
					texture => texture.Delete(),
					item => index(item.Size.Area),
					size => index(size.Area)));
		}
		#endregion

		#region Recycle
		protected internal void Recycle(Texture texture)
		{
			if (texture.NotNull())
				this.textures[(int)texture.Type].Recycle(texture);
		}
		protected internal void Recycle(FrameBuffer frameBuffer)
		{
			if (frameBuffer.NotNull())
			{
			}

		}
		#endregion

		#region Inheritors Interface
		protected abstract Shader.Vertex DefaultVertex { get; }
        protected abstract Shader.Fragment MonochromeToBgrFragment { get; }
		protected abstract Shader.Fragment BgrToMonochromeFragment { get; }
		protected abstract Shader.Fragment BgrToUFragment { get; }
		protected abstract Shader.Fragment BgrToVFragment { get; }
		protected abstract Shader.Fragment BgrToYuv420Fragment { get; }
		protected abstract Shader.Fragment Yuv420ToBgrFragment { get; }
		protected abstract Texture AllocateTexture(Gpu.Backend.TextureType type, Geometry2D.Integer.Size size, Draw.CoordinateSystem coordinateSystem);
		protected abstract Texture AllocateTexture(Raster.Image image);
		protected abstract Texture AllocateDepth(Geometry2D.Integer.Size size);
		protected abstract Texture ReuseTexture(Texture original, Draw.CoordinateSystem coordinateSystem);
		protected abstract Texture ReuseTexture(Texture original, Raster.Image image);
		protected abstract Texture ReuseDepth(Backend.Texture original);
		#endregion
		#region IFactory Members
		public Gpu.Backend.ITexture CreateTexture(Gpu.Backend.TextureType type, Geometry2D.Integer.Size size, Draw.CoordinateSystem coordinateSystem)
		{
			Texture result = this.textures[(int)type].Find(size);
			return result.NotNull() ? this.ReuseTexture(result, coordinateSystem) : this.AllocateTexture(type, size, coordinateSystem);
		}
		public Gpu.Backend.ITexture CreateTexture(Raster.Image image)
		{
			Texture result = this.textures[(int)image.GetImageType()].Find(image.Size);
			return result.NotNull() ? this.ReuseTexture(result, image) : this.AllocateTexture(image);
		}
		internal Gpu.Backend.ITexture CreateDepth(Geometry2D.Integer.Size size)
		{
			Texture result = this.textures[(int)Gpu.Backend.TextureType.Bgra].Find(size);
			return result.NotNull() ? this.ReuseDepth(result) : this.AllocateDepth(size);
		}
		public abstract Gpu.Backend.IFrameBuffer CreateFrameBuffer(params Gpu.Backend.ITexture[] textures);

		public void FreeRecycled()
		{
			this.textures.Apply(bin => bin.Free());
		}

        public Gpu.Backend.IShader ConvertMonochromeToBgr { get { return new Shader.Program(this.DefaultVertex, this.MonochromeToBgrFragment); } }
        public Gpu.Backend.IShader ConvertBgrToMonochrome { get { return new Shader.Program(this.DefaultVertex, this.BgrToMonochromeFragment); } }
		public Gpu.Backend.IShader ConvertBgrToU { get { return new Shader.Program(this.DefaultVertex, this.BgrToUFragment); } }
		public Gpu.Backend.IShader ConvertBgrToV { get { return new Shader.Program(this.DefaultVertex, this.BgrToVFragment); } }
		public Gpu.Backend.IShader ConvertBgrToYuv420 { get { return new Shader.Program(this.DefaultVertex, this.BgrToYuv420Fragment); } }
		public Gpu.Backend.IShader ConvertYuv420ToBgr { get { return new Shader.Program(this.DefaultVertex, this.Yuv420ToBgrFragment); } }
		#endregion
	}
}

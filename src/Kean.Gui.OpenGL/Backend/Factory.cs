// 
//  Factory.cs
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
using GL = OpenTK.Graphics.OpenGL.GL;
using Error = Kean.Core.Error;
using Log = Kean.Extra.Log;
using Draw = Kean.Draw;
using Gpu = Kean.Draw.Gpu;
using Raster = Kean.Draw.Raster;
using Kean.Gui.OpenGL.Backend.Extension;

namespace Kean.Gui.OpenGL.Backend
{
	public abstract class Factory :
		Gpu.Backend.IFactory
	{
		#region Constructors
		protected Factory()
		{

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
		#endregion
		#region IFactory Members
		public abstract Gpu.Backend.ITexture CreateImage(Gpu.Backend.TextureType type, Geometry2D.Integer.Size size, Draw.CoordinateSystem coordinateSystem);
		public abstract Gpu.Backend.ITexture CreateImage(Raster.Image image);
		public abstract Gpu.Backend.IFrameBuffer CreateFrameBuffer(params Gpu.Backend.ITexture[] textures);

        public Gpu.Backend.IShader ConvertMonochromeToBgr { get { return new Shader.Program(this.DefaultVertex, this.MonochromeToBgrFragment); } }
        public Gpu.Backend.IShader ConvertBgrToMonochrome { get { return new Shader.Program(this.DefaultVertex, this.BgrToMonochromeFragment); } }
		public Gpu.Backend.IShader ConvertBgrToU { get { return new Shader.Program(this.DefaultVertex, this.BgrToUFragment); } }
		public Gpu.Backend.IShader ConvertBgrToV { get { return new Shader.Program(this.DefaultVertex, this.BgrToVFragment); } }
		public Gpu.Backend.IShader ConvertBgrToYuv420 { get { return new Shader.Program(this.DefaultVertex, this.BgrToYuv420Fragment); } }
		public Gpu.Backend.IShader ConvertYuv420ToBgr { get { return new Shader.Program(this.DefaultVertex, this.Yuv420ToBgrFragment); } }
		#endregion
	}
}

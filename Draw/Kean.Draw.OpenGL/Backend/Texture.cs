//
//  Texture.cs
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
using Geometry2D = Kean.Math.Geometry2D;
using Raster = Kean.Draw.Raster;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Texture :
		Resource,
        IData, Kean.Draw.OpenGL.Backend.ITexture
	{
		internal int Identifier { get; private set; }
		public Geometry2D.Integer.Size Size { get; protected set; }
		public TextureType Type { get; protected set; }
		public abstract bool Wrap { get; set; }
		protected Texture(Context context) :
			base(context)
		{
			Texture.Free();
			this.Identifier = this.CreateIdentifier();
		}
		protected override void Dispose(bool disposing)
		{
			if (this.Identifier != 0)
			{
				lock (Texture.garbage)
					Texture.garbage.Add(this.Identifier);
				this.Identifier = 0;
			}
			base.Dispose(disposing);
		}
		public void Create(TextureType type, Geometry2D.Integer.Size size)
		{
			this.SetFormat(type, size);
			this.Use();
			this.Create(IntPtr.Zero);
			this.UnUse();
		}
		public void Create(Raster.Image image)
		{
			this.SetFormat(image);
			this.Use();
			this.Create(image.Pointer);
			this.UnUse();
		}
		public void Load(Raster.Image image, Geometry2D.Integer.Point offset)
		{
			TextureType type;
			if (image is Raster.Bgra)
				type = TextureType.Argb;
			else if (image is Raster.Bgr)
				type = TextureType.Rgb;
			else
				type = TextureType.Monochrome;
			this.Use();
			this.Load(image.Pointer, new Geometry2D.Integer.Box(offset, image.Size), type);
			this.UnUse();
		}
		public Raster.Image Read()
		{
			Raster.Image result = null;
			switch (this.Type)
			{
				default:
				case TextureType.Argb:
					result = new Raster.Bgra(this.Size);
					break;
				case TextureType.Rgb:
					result = new Raster.Bgr(this.Size);
					break;
				case TextureType.Monochrome:
					result = new Raster.Monochrome(this.Size);
					break;
			}
			this.Read(result.Pointer, new Geometry2D.Integer.Box(0, 0, this.Size.Width, this.Size.Height));
			return result;
		}
		public abstract void Render(Geometry2D.Single.PointValue leftTop, Geometry2D.Single.PointValue rightTop, Geometry2D.Single.PointValue leftBottom, Geometry2D.Single.PointValue rightBottom, Geometry2D.Single.Box rectangle);
		public override string ToString()
		{
			return this.Identifier.ToString();
		}

		void SetFormat(Raster.Image image)
		{
			TextureType type;
			if (image is Raster.Bgra)
				type = TextureType.Argb;
			else if (image is Raster.Bgr)
				type = TextureType.Rgb;
			else
				type = TextureType.Monochrome;
			this.SetFormat(type, image.Size);
		}

		#region Implementors Interface
		protected abstract int CreateIdentifier();
		public abstract void Use();
		public abstract void UnUse();
		public abstract void Configure();
		protected abstract void SetFormat(TextureType type, Geometry2D.Integer.Size size);
		protected abstract void Create(IntPtr data);
		protected abstract void Load(IntPtr data, Geometry2D.Integer.Box region, TextureType type);
		protected abstract void Read(IntPtr data, Geometry2D.Integer.Box region);
		#endregion
		#region Garbage
		static Collection.IList<int> garbage = new Collection.List<int>();
		internal static void Free()
		{
			lock (Texture.garbage)
			{
				int[] garbage = Texture.garbage.ToArray();
				if (garbage.Length > 0)
					GL.DeleteTextures(garbage.Length, garbage);
				Texture.garbage.Clear();
			}
		}
		#endregion
	}
}

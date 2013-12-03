//
//  Composition.cs
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

using Kean.Collection.Extension;
using System;
using Collection = Kean.Collection;
using Error = Kean.Error;
using GL = OpenTK.Graphics.OpenGL.GL;
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Color = Kean.Draw.Color;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Composition :
		Resource,
		ITexture,
		IDisposable
	{
		public Geometry2D.Integer.Size Size { get { return this.Texture.Size; } }
		public TextureType Type { get { return this.Texture.Type; } }
		public Texture Texture { get; private set; }
		public Depth Depth { get; private set; }
		public FrameBuffer FrameBuffer { get; private set; }
		protected Composition(Context context, Texture texture, Depth depth, FrameBuffer frameBuffer) :
			base(context)
		{
			this.Texture = texture;
			this.Depth = depth;
			this.FrameBuffer = frameBuffer;
			this.Create();
			this.Texture.Composition = this;
		}
		protected Composition(Composition original) :
			base(original)
		{
			this.Texture = original.Texture.Refurbish();
			this.Texture.Composition = this;
			original.Texture.Composition = null;
			original.Texture = null;
			this.Depth = original.Depth.Refurbish();
			original.Depth = null;
			this.FrameBuffer = original.FrameBuffer.Refurbish();
			original.FrameBuffer = null;
		}
		protected override void Dispose(bool disposing)
		{
			if (this.Texture.NotNull() && this.Texture.Identifier != 0 && this.Depth.NotNull() && this.Depth.Identifier != 0 && this.FrameBuffer.NotNull() && this.FrameBuffer.Identifier != 0)
				this.Context.Recycle(this.Refurbish());
			else
			{
				if (this.Texture.NotNull())
				{
					this.Texture.Composition = null;
					this.Context.Delete(this.Texture);
					this.Texture = null;
				}
				if (this.Depth.NotNull())
				{
					this.Context.Delete(this.Depth);
					this.Depth = null;
				}
				if (this.FrameBuffer.NotNull())
				{
					this.Context.Delete(this.FrameBuffer);
					this.FrameBuffer = null;
				}
			}
		}
		public void Create()
		{
			this.Depth.Use();
			this.Depth.Create(this.Size);
			this.FrameBuffer.Use();
			this.FrameBuffer.Create(this.Texture, this.Depth);
			this.FrameBuffer.UnUse();
		}
		#region Implementors Interface
		public abstract void Setup();
		public abstract void Teardown();
		public abstract void SetClip(Geometry2D.Single.Box region);
		public abstract void UnSetClip();
		public abstract void SetTransform(Geometry2D.Single.Transform transform);
		public abstract void SetIdentityTransform();
		public abstract void CopyToTexture();
		public abstract void CopyToTexture(Geometry2D.Integer.Size offset);
		public abstract void Read(IntPtr pointer, Geometry2D.Integer.Box region);

		public abstract void Clear();
		public abstract void Clear(Geometry2D.Single.Box region);
		public abstract void Blend(float factor);
		public abstract void Draw(IColor color, Geometry2D.Single.Box region);
		protected abstract Composition Refurbish();
		protected internal override void Delete()
		{
			if (this.Texture.NotNull())
			{
				this.Texture.Composition = null;
				this.Texture.Delete();
				this.Texture = null;
			}
			if (this.Depth.NotNull())
			{
				this.Depth.Delete();
				this.Depth = null;
			}
			if (this.FrameBuffer.NotNull())
			{
				this.FrameBuffer.Delete();
				this.FrameBuffer = null;
			}
		}
		#endregion
		public override string ToString()
		{
			return "color: " + this.Texture.Identifier + " depth:" + this.Depth.Identifier + " frame buffer:" + this.FrameBuffer.Identifier + " (s: " + this.Size + ", t: " + this.Type + ")";
		}
	}
}

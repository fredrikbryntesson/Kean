// 
//  Canvas.cs
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
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw
{
	public abstract class Canvas
	{

		public Image Image { get; private set; }
		public Geometry2D.Integer.Size Size { get { return this.Image.Size; } }

		protected Canvas(Image image)
		{
			this.Image = image;
			this.clipStack = new ClipStack(this.Image.Size, (transform, clip) => { this.Transform = this.OnTransformChange(transform); this.Clip = this.OnClipChange(clip); });
		}
		#region Text Antialias
		bool textAntialias;
		[Notify("TextAntialiasChanged")]
		public bool TextAntialias 
		{
			get { return this.textAntialias; }
			set
			{
				if (this.textAntialias != value)
				{
					this.textAntialias = value;
					this.TextAntialiasChanged(value);
				}
			}
		}
		public event Action<bool> TextAntialiasChanged; 
		#endregion

		#region Clip, Transform, Push & Pop
		ClipStack clipStack;
		public Geometry2D.Single.Box Clip { get; private set; }
		public Geometry2D.Single.Transform Transform { get; private set; }
		protected virtual Geometry2D.Single.Box OnClipChange(Geometry2D.Single.Box clip)
		{
			return clip;
		}
		protected virtual Geometry2D.Single.Transform OnTransformChange(Geometry2D.Single.Transform transform)
		{
			return transform;
		}
		public void Push(Geometry2D.Single.Box clip, Geometry2D.Single.Transform transform)
		{
			this.clipStack.Push(clip, transform);
		}
		public void Push(Geometry2D.Single.Box clip)
		{
			this.Push(clip, Geometry2D.Single.Transform.Identity);
		}
		public void Push(Geometry2D.Single.Transform transform)
		{
			this.clipStack.Push(new Geometry2D.Single.Box(0,0, this.Image.Size.Width, this.Image.Size.Height), transform);
		}
		public void PushAndTranslate(Geometry2D.Single.Box clip)
		{
			this.Push(clip.Intersection(this.Clip) - clip.LeftTop, Geometry2D.Single.Transform.CreateTranslation(clip.LeftTop));
		}
		public void Pop()
		{
			this.clipStack.Pop();
		}
		#endregion
		#region Create
		public abstract Canvas CreateSubcanvas(Geometry2D.Single.Box bounds);
		#endregion
		#region Draw, Blend, Clear
		#region Draw Image
		public virtual void Draw(Draw.Image image)
		{
			this.Draw(null, image);
		}
		public virtual void Draw(Map map, Image image)
		{
			this.Draw(map, image, new Geometry2D.Single.Point());
		}
		public virtual void Draw(Draw.Image image, Geometry2D.Single.Point position)
		{
			this.Draw(null, image, new Geometry2D.Single.Point());
		}
		public virtual void Draw(Map map, Draw.Image image, Geometry2D.Single.Point position)
		{
			Geometry2D.Single.Box region = image.Crop.Decrease((Kean.Math.Geometry2D.Abstract.ISize<float>)(Geometry2D.Single.Size)image.Size);
			this.Draw(map, image, region, new Geometry2D.Single.Box(position, region.Size));
		}
		public virtual void Draw(Draw.Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			this.Draw(null, image, source, destination);
		}
		public abstract void Draw(Draw.Map map, Draw.Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination);
		#endregion
		#region Draw Box
		public virtual void Draw(IColor color)
		{
			this.Draw(color, (Geometry2D.Single.Size)this.Size);
		}
		public virtual void Draw(IColor color, Geometry2D.Single.Box region)
		{
			this.Draw(color, Path.Rectangle(region));
		}
		#endregion
		#region Draw Path
		public void Draw(Stroke stroke, Path path)
		{
			this.Draw(null, stroke, path);
		}
		public void Draw(IPaint fill, Path path)
		{
			this.Draw(fill, null, path);
		}
		public abstract void Draw(IPaint fill, Stroke stroke, Path path);
		#endregion
		#region Draw Text
		public void Draw(Stroke stroke, Text text, Geometry2D.Single.Point position)
		{
			this.Draw(null, stroke, text, position);
		}
		public void Draw(IPaint fill, Text text, Geometry2D.Single.Point position)
		{
			this.Draw(fill, null, text, position);
		}
		public abstract void Draw(IPaint fill, Stroke stroke, Text text, Geometry2D.Single.Point position);
		#endregion
		#region Blend
		public abstract void Blend(float factor);
		#endregion
		#region Clear
		public virtual void Clear()
		{
			this.Clear((Geometry2D.Single.Size)this.Size);
		}
		public abstract void Clear(Geometry2D.Single.Box region);
		#endregion
		#endregion
	}
}

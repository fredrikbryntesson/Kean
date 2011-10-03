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

		public abstract bool TextAntiAlias { get; set; }

		protected Canvas(Image image)
		{
			this.Image = image;
			this.clipStack = new ClipStack(this.Image.Size, (transform, clip) => { this.Transform = this.OnTransformChange(transform); this.Clip = this.OnClipChange(clip); });
		}

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
		public void PushAndTranslate(Geometry2D.Single.Box clip)
		{
			this.Push(clip.Intersection(this.Clip) - clip.LeftTop, Geometry2D.Single.Transform.CreateTranslation(clip.LeftTop));
		}
		public void Pop()
		{
			this.clipStack.Pop();
		}
		#endregion

		public abstract Canvas Create(Geometry2D.Single.Size size);
		public abstract Canvas Subcanvas(Geometry2D.Single.Box bounds);
		public abstract void Draw(Draw.Image image);
		public abstract void Draw(Draw.Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination);

		public abstract void Clear(Geometry2D.Single.Box area);
	}
}

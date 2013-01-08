// 
//  ClipStack.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;

namespace Kean.Draw
{
	class ClipStack
	{
		Geometry2D.Single.Size newSize;
		Collection.IStack<Tuple<Geometry2D.Single.Box, Geometry2D.Single.Transform>> clips;
		System.Action<Geometry2D.Single.Transform, Geometry2D.Single.Box> update;
		int counter = 0;
		public Geometry2D.Single.Box Clip { get { return this.clips.Peek().Item1; } }

		public ClipStack(Geometry2D.Single.Size size, System.Action<Geometry2D.Single.Transform, Geometry2D.Single.Box> update)
		{
			if (update.IsNull())
				throw new ArgumentNullException("update");
			if (size.IsNull())
				throw new ArgumentNullException("size");
			this.update = update;
			this.clips = new Collection.Stack<Tuple<Geometry2D.Single.Box, Geometry2D.Single.Transform>>();
			this.clips.Push(Tuple.Create(new Geometry2D.Single.Box(0, 0, size.Width, size.Height), Geometry2D.Single.Transform.Identity));
		}
		public void Push(Geometry2D.Single.Box region, Geometry2D.Single.Transform transform)
		{
			Tuple<Geometry2D.Single.Box, Geometry2D.Single.Transform> clip = Tuple.Create(region, this.clips.Peek().Item2 * transform);
			this.clips.Push(clip);
			this.UpdateClip(clip);
			this.counter++;
		}
		public void Pop()
		{
			this.clips.Pop();
			this.counter--;
			Tuple<Geometry2D.Single.Box, Geometry2D.Single.Transform> clip = this.clips.Pop();
			if (this.newSize.NotNull() && this.clips.Empty)
			{
				clip = Tuple.Create(new Geometry2D.Single.Box(0, 0, this.newSize.Width, this.newSize.Height), Geometry2D.Single.Transform.Identity);
				this.newSize = null;
			}
			this.clips.Push(clip);
			this.UpdateClip(clip);
		}
		void UpdateClip(Tuple<Geometry2D.Single.Box, Geometry2D.Single.Transform> clip)
		{
			this.update(clip.Item2, clip.Item1 + (Geometry2D.Single.Point)clip.Item2.Translation);
		}

		internal void Resize(Geometry2D.Integer.Size size)
		{
			this.newSize = size;
		}
	}
}

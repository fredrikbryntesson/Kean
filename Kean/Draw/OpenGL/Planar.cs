﻿// 
//  Planar.cs
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

using Buffer = Kean.Buffer;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Extension;

namespace Kean.Draw.OpenGL
{
	public abstract class Planar :
		Image
	{
		protected Packed[] Channels { get; private set; }
		protected Planar(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, params Packed[] channels) :
			base(size, coordinateSystem) 
		{
			this.Channels = channels;
		}
		protected Planar(Planar original) :
			base(original) 
		{
			this.Channels = original.Channels.Map(c => c.Copy() as Packed);
		}
		internal override void Render(Map map, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			if (map.NotNull())
			{
				for (int i = 0; i < this.Channels.Length; i++)
					map.Backend.SetTexture("texture", i, this.Channels[i].Backend);
				map.Backend.Use();
			}
			this.Render(source, destination);
			if (map.NotNull())
				map.Backend.UnUse();
		}
		internal override void Render(Geometry2D.Single.Point leftTop, Geometry2D.Single.Point rightTop, Geometry2D.Single.Point leftBottom, Geometry2D.Single.Point rightBottom, Geometry2D.Single.Box destination)
		{
			this.Channels[0].Render(leftTop, rightTop, leftBottom, rightBottom, destination);
		}
		#region Draw.Image Overrides
		Canvas canvas;
		public override Draw.Canvas Canvas
		{
			get
			{
				if (this.canvas.IsNull())
					this.canvas = new Canvas(this, this.Channels);
				return this.canvas;
			}
		}
		public override void Dispose()
		{
			if (this.Channels.NotNull())
			{
				this.Channels.Apply(c => c.Dispose());
				this.Channels = null;
			}
		}
		#endregion
	}
}
// 
//  Planar.cs
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

using Buffer = Kean.Core.Buffer;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Core.Extension;

namespace Kean.Draw.Gpu
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
			this.Channels = original.Channels.Map(c => c.Copy() as Gpu.Packed);
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

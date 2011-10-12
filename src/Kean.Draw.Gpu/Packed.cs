// 
//  Packed.cs
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

namespace Kean.Draw.Gpu
{
	public abstract class Packed :
		Image
	{
		protected internal Backend.ITexture Backend { get; private set; }

		#region Constructors
		protected Packed(Planar original) :
			base(original) { }
		protected Packed(Backend.ITexture backend) :
			base(backend.Size, backend.CoordinateSystem)
		{
			this.Backend = backend;
		}
		#endregion
		internal override void Render(Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			this.Backend.Render(source, destination);
		}
		#region Draw.Image Overrides
		public override Draw.Canvas Canvas
		{
			get { return new Canvas(this); }
		}
		public override T Convert<T>()
		{
			return null;
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return null;
		}
		public override Draw.Image Copy(Geometry2D.Integer.Size size, Geometry2D.Single.Transform transform)
		{
			return null;
		}
		public override void Shift(Geometry2D.Integer.Size offset)
		{
		}
		public override float Distance(Draw.Image other)
		{
			float result;
			if (other.NotNull() && this.Size == other.Size)
				result = this.Convert<Raster.Bgra>().Distance(other.Convert<Raster.Bgra>());
			else
				result = float.MaxValue;
			return result;
		}
		public override void Dispose()
		{
			if (this.Backend.NotNull())
			{
				this.Backend.Dispose();
				this.Backend = null;
			}
		}
		#endregion
	}
}

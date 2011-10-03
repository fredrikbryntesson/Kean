// 
//  Image.cs
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
	public abstract class Image :
		Draw.Image,
		IDisposable
	{
		protected internal Backend.IImage Backend;

		public override Kean.Draw.Canvas Canvas
		{
			get { return new Canvas(this); }
		}
		#region Constructors
		protected Image(Backend.IImage backend) :
			base(backend.Size, backend.CoordinateSystem)
		{
			this.Backend = backend;
		}
		#endregion
		public override T Convert<T>()
		{
			throw new NotImplementedException();
		}
		public override Draw.Image Copy()
		{
			throw new NotImplementedException();
		}
		public override Draw.Image Copy(Geometry2D.Single.Size size, Geometry2D.Single.Transform transform)
		{
			throw new NotImplementedException();
		}
		public override Draw.Image Resize(Geometry2D.Single.Size restriction)
		{
			throw new NotImplementedException();
		}
		public override float Distance(Draw.Image other)
		{
			throw new NotImplementedException();
		}

		#region IDisposable Members

		public void Dispose()
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

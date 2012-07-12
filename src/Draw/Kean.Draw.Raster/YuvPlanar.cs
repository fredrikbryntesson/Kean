// 
//  YuvPlanar.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
using Buffer = Kean.Core.Buffer;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Raster
{
	[System.Runtime.InteropServices.ComVisible(true)]
	public abstract class YuvPlanar :
		Planar
	{
		public Monochrome Y { get; private set; }
		public Monochrome U { get; private set; }
		public Monochrome V { get; private set; }

		protected YuvPlanar(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(buffer, size, coordinateSystem)
		{
			this.Y = this.CreateY();
			this.U = this.CreateU();
			this.V = this.CreateV();
		}
		protected YuvPlanar(YuvPlanar original) :
			base(original)
		{
			this.Y = this.CreateY();
			this.U = this.CreateU();
			this.V = this.CreateV();
		}
		protected abstract Monochrome CreateY();
		protected abstract Monochrome CreateU();
		protected abstract Monochrome CreateV();

		public override void Apply(Action<Color.Bgr> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
		public override void Apply(Action<Color.Y> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
		public override float Distance(Draw.Image other)
		{
			return other is YuvPlanar ? base.Distance(other) : float.MaxValue;
		}
		public override bool Equals(Draw.Image other)
		{
			return other is YuvPlanar && base.Equals(other);
		}
		#region Static Open
		public static new YuvPlanar OpenResource(System.Reflection.Assembly assembly, string name)
		{
			return Image.OpenResource<YuvPlanar>(assembly, name);
		}
		public static new YuvPlanar OpenResource(string name)
		{
			return Image.OpenResource<YuvPlanar>(System.Reflection.Assembly.GetCallingAssembly(), name);
		}
		public static new YuvPlanar Open(string filename)
		{
			return Image.Open<YuvPlanar>(filename);
		}
		public static new YuvPlanar Open(System.IO.Stream stream)
		{
			return Image.Open<YuvPlanar>(stream);
		}
		#endregion
	}
}

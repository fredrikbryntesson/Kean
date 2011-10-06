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

namespace Kean.Draw
{
	public abstract class Image :
		IEquatable<Image>,
		IDisposable
	{
		public abstract Canvas Canvas { get; }

		public Geometry2D.Integer.Size Size { get; private set; }
		Geometry2D.Integer.Transform transform = Geometry2D.Integer.Transform.Identity;
		protected Geometry2D.Integer.Transform Transform { get { return this.transform; } }
		#region CoordinateSystem
		CoordinateSystem coordinateSystem;
		[Notify("CoordinateSystemChanged")]
		public CoordinateSystem CoordinateSystem
		{
			get { return this.coordinateSystem; }
			set
			{
				if (this.coordinateSystem != value)
				{
					this.coordinateSystem = value;
					this.transform = Geometry2D.Integer.Transform.CreateScaling(
						(value & CoordinateSystem.XLeftward) == CoordinateSystem.XLeftward ? -1 : 1,
						(value & CoordinateSystem.YUpward) == CoordinateSystem.YUpward ? -1 : 1);
					this.CoordinateSystemChanged.Call(this.coordinateSystem);
				}
			}
		}
		public event Action<CoordinateSystem> CoordinateSystemChanged;
		#endregion
		#region Crop
		Geometry2D.Single.Shell crop;
		[Notify("CropChanged")]
		public Geometry2D.Single.Shell Crop 
		{
			get { return this.crop ?? new Geometry2D.Single.Shell(); }
			set
			{
				if (this.crop != value)
				{
					this.crop = value;
					this.CropChanged.Call(this.crop);
				}
			}
		}
		public event Action<Geometry2D.Single.Shell> CropChanged;
		#endregion
		#region Wrap
		bool wrap;
		[Notify("WrapChanged")]
		public bool Wrap
		{
			get { return this.wrap; }
			set
			{
				if (this.wrap != value)
				{
					this.wrap = value;
					this.WrapChanged.Call(this.wrap);
				}
			}
		}
		public event Action<bool> WrapChanged;
		#endregion
		//public abstract IColor this[int x, int y] { get; }
		//public abstract IColor this[float x, float y] { get; }
		protected Image()
		{ }
		protected Image(Image original) :
			this(original.Size, original.CoordinateSystem, original.Crop, original.Wrap) 
		{ }
		protected Image(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem)
		{
			this.Size = size;
			this.CoordinateSystem = coordinateSystem;
		}
		protected Image(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Single.Shell crop, bool wrap) :
			this(size, coordinateSystem)
		{
			this.Crop = crop;
			this.Wrap = wrap;
		}
		public abstract T Convert<T>() where T : Image;
		public virtual Draw.Image ResizeWithin(Geometry2D.Integer.Size restriction)
		{
			return this.ResizeTo((Geometry2D.Integer.Size)((Geometry2D.Single.Size)this.Size * Math.Single.Minimum((float)restriction.Width / (float)this.Size.Width, (float)restriction.Height / (float)this.Size.Height)));
		}
		public abstract Image ResizeTo(Geometry2D.Integer.Size size);
		public abstract Image Create(Geometry2D.Integer.Size size);
		public abstract Image Copy();
		public abstract Image Copy(Geometry2D.Integer.Size size, Geometry2D.Single.Transform transform);
		public abstract void Shift(Geometry2D.Integer.Size offset);

		public abstract float Distance(Image other);
		public override bool Equals(object other)
		{
			return other is Image && this.Equals(other as Image);
		}
		public virtual bool Equals(Image other)
		{
			return other.NotNull() && this.Size == other.Size && this.Distance(other) < 10 * float.Epsilon;
		}
		~Image()
		{
			this.Dispose();
		}
		#region IDisposable Members
		public virtual void Dispose()
		{ }
		#endregion
	}
}

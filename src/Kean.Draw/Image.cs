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
//  You should have received a copy of the GNU Lesser General Public Licenseusing System;
using System;
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw
{
	public abstract class Image :
		IEquatable<Image>
	{
		public Geometry2D.Single.Size Size { get; private set; }
		protected Geometry2D.Integer.Transform Transform { get; private set; }

		CoordinateSystem coordinateSystem;
		public CoordinateSystem CoordinateSystem
		{
			get { return this.coordinateSystem; }
			set
			{
				this.coordinateSystem = value;
				this.Transform = Geometry2D.Integer.Transform.CreateScaling(
					(value & CoordinateSystem.XLeftward) == CoordinateSystem.XLeftward ? -1 : 1,
					(value & CoordinateSystem.YUpward) == CoordinateSystem.YUpward ? -1 : 1);
			}
		}
		//public abstract IColor this[int x, int y] { get; }
		//public abstract IColor this[float x, float y] { get; }
		protected Image()
		{ }
		protected Image(Image original) :
			this(original.Size, original.CoordinateSystem) { }
		protected Image(Geometry2D.Single.Size size, CoordinateSystem coordinateSystem)
		{
			this.Size = size;
			this.CoordinateSystem = coordinateSystem;
		}
		public abstract T Convert<T>() where T : Image;
		public abstract Image Resize(Geometry2D.Single.Size restriction);
		public abstract Image Copy();
		public abstract Image Copy(Geometry2D.Single.Size size, Geometry2D.Single.Transform transform);

		public abstract float Distance(Image other);
		public override bool Equals(object other)
		{
			return other is Image && this.Equals(other as Image);
		}
		public virtual bool Equals(Image other)
		{
			return other.NotNull() && this.Size == other.Size;
		}
	}
}

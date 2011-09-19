// 
//  Size.cs
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
using Kean.Core.Extension;
namespace Kean.Math.Geometry2D.Abstract
{
    public abstract class Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
		Vector<SizeType, SizeValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>,
        ISize<V>,
		IEquatable<SizeType>
		where TransformType : Transform<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, ITransform<V>, new()
        where TransformValue : struct, ITransform<V>
		where ShellType : Shell<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, IShell<V>, new()
		where ShellValue : struct, IShell<V>
		where BoxType : Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, IBox<PointValue, SizeValue, V>, new()
        where BoxValue : struct, IBox<PointValue, SizeValue, V>
		where PointType : Point<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, IPoint<V>, new()
        where PointValue : struct, IPoint<V>, IVector<V>
		where SizeType : Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, ISize<V>, new()
        where SizeValue : struct, ISize<V>, IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
	{
		public V Width { get { return base.X; } }
		public V Height { get { return base.Y; } }
		#region ISize<V> Members
		V ISize<V>.Width { get { return this.Width; } }
		V ISize<V>.Height { get { return this.Height; } }
		#endregion
		public V Area { get { return base.X.Multiply(base.Y); } }
        public V Length { get { return (((R)base.X).Squared() + ((R)base.Y).Squared()).SquareRoot(); } }
        public bool Empty { get { return ((R)this.Width) == Kean.Math.Abstract<R, V>.Zero || ((R)this.Height) == Kean.Math.Abstract<R, V>.Zero; } }
        protected Size() { }
	    protected Size(R width, R height) :
			base(width, height)
		{ }
		#region IEquatable<SizeType> Members
		public bool Equals(SizeType other)
		{
			return other.NotNull() && this.Width.Equals(other.Width) && this.Height.Equals(other.Height);
		}
		#endregion
		#region Arithmetic Operators
        public static SizeType operator +(Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return new SizeType().Create(left.Width + (R)right.Width, left.Height + (R)right.Height);
        }
        public static SizeType operator +(Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, ISize<V> right)
        {
            return new SizeType().Create(left.Width + (R)right.Width, left.Height + (R)right.Height);
        }
        public static SizeType operator +(ISize<V> left, Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return new SizeType().Create(left.Width + (R)right.Width, left.Height + (R)right.Height);
        }
        public static SizeType operator -(Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return new SizeType().Create(left.Width - (R)right.Width, left.Height - (R)right.Height);
        }
        public static SizeType operator -(Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, ISize<V> right)
        {
            return new SizeType().Create(left.Width - (R)right.Width, left.Height - (R)right.Height);
        }
        public static SizeType operator -(ISize<V> left, Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return new SizeType().Create(left.Width - (R)right.Width, left.Height - (R)right.Height);
        }
        public static SizeType operator *(Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return new SizeType().Create(left.Width * (R)right.Width, left.Height * (R)right.Height);
        }
        public static SizeType operator *(Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, ISize<V> right)
        {
            return new SizeType().Create(left.Width * (R)right.Width, left.Height * (R)right.Height);
        }
        public static SizeType operator *(ISize<V> left, Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return new SizeType().Create(left.Width * (R)right.Width, left.Height * (R)right.Height);
        }
        public static SizeType operator /(Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return new SizeType().Create(left.Width / (R)right.Width, left.Height / (R)right.Height);
        }
        public static SizeType operator /(Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, ISize<V> right)
        {
            return new SizeType().Create(left.Width / (R)right.Width, left.Height / (R)right.Height);
        }
        public static SizeType operator /(ISize<V> left, Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return new SizeType().Create(left.Width / (R)right.Width, left.Height / (R)right.Height);
        }
        public static SizeType operator *(TransformType left, Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
			return new SizeType().Create(left.A * (R)right.Width + left.C * (R)right.Height, left.B * (R)right.Width + left.D * (R)right.Height);
        }
        #endregion
        /*
        public static SizeType Create(V width, V height)
        {
			return Vector<SizeType, SizeValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(width, height);
        }*/
		#region Casts
		public static explicit operator PointType(Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> size)
		{
			return Vector<PointType, PointValue, TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(size.Width, size.Height);
		}
		#endregion
	}
}


// 
//  Box.cs
//  
//  Author:
//       Anders Frisk <@>
//  
//  Copyright (c) 2011 Anders Frisk
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

namespace Kean.Math.Geometry2D.Abstract
{
    public abstract class Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
        IBox<PointValue, SizeValue, V>,
		IEquatable<BoxType>
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
        #region IBox<PointValue,SizeValue,V> Members
        PointType leftTop;
        public PointType LeftTop { get { return this.leftTop; } }
        SizeType size;
        public SizeType Size { get { return this.size; } }
        #endregion

        #region Sizes
        public V Width { get { return this.Size.Width; } }
        public V Height { get { return this.Size.Height; } }
        #endregion

        #region All sides
        public V Left { get { return (this.LeftTop as IPoint<V>).X; } }
        public V Right { get { return (R)((this.LeftTop as IPoint<V>).X) + this.Size.Width; } }
        public V Top { get { return (this.LeftTop as IPoint<V>).Y; } }
        public V Bottom { get { return (R)((this.LeftTop as IPoint<V>).Y) + this.Size.Height; } }
        #endregion

		#region All other corners
		public PointType RightTop { get { return Point<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(this.Right, this.Top); } }
		public PointType LeftBottom { get { return Point<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(this.Left, this.Bottom); } }
		public PointType RightBottom { get { return this.LeftTop + this.Size; } }
		#endregion
        public PointType Center { get { return this.LeftTop + this.Size / Kean.Math.Abstract<R, V>.Two; } }
        #region IBox<PointValue, SizeValue, V>
        PointValue IBox<PointValue, SizeValue, V>.LeftTop { get { return this.LeftTop.Value; } }
        SizeValue IBox<PointValue, SizeValue, V>.Size { get { return this.Size.Value; } }
        #endregion
        public bool Empty { get { return this.Size.Empty; } }
        public abstract BoxValue Value { get; }

        #region Constructors
        protected Box()
        {
            this.leftTop = new PointType();
            this.size = new SizeType();
        }
        protected Box(PointType leftTop, SizeType size)
        {
            this.leftTop = leftTop;
            this.size = size;
        }
        #endregion
        #region Methods
        public BoxType Swap()
        {
            return new BoxType() { leftTop = this.leftTop.Swap(), size = this.size.Swap() };
        }
        public BoxType Pad(V pad)
        {
            return this.Pad(pad, pad, pad, pad);
        }
        public BoxType Pad(SizeType padding)
        {
            return this.Pad(padding.Width, padding.Width, padding.Height, padding.Height);
        }
        public abstract BoxType Pad(V left, V right, V top, V bottom);
        public bool Contains(PointType point)
        {
            return point.X >= (R)this.Left && point.X <= (R)this.Right && point.Y >= (R)this.Top && point.Y <= (R)this.Bottom;
        }
        public bool Contains(BoxType box)
        {
            return this.Intersection(box) == box;
        }
        public abstract BoxType Intersection(BoxType other);
        public abstract BoxType Union(BoxType other);
       	public BoxType Round()
		{
			return new BoxType() { leftTop = this.LeftTop.Round(), size = this.Size.Round() };
		}
		public BoxType Ceiling()
		{
			PointType leftTop = this.LeftTop.Floor();
			return new BoxType() { leftTop = leftTop, size = (SizeType)(Point<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>)(this.RightBottom.Ceiling()) - (IVector<V>)leftTop, };
		}
		public BoxType Floor()
		{
			return new BoxType() { leftTop = this.LeftTop.Round(), size = this.Size.Round() };
		}
		#endregion
        #region Arithmetic operators
        public static BoxType operator +(Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, BoxType right)
        {
            BoxType result;
            if (left.Empty)
                result = right;
            else if (right.Empty)
                result = (BoxType)left;
            else
                result = new BoxType()
                {
                    leftTop = Kean.Math.Geometry2D.Abstract.Point<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(Kean.Math.Abstract<R, V>.Minimum((R)left.Left, (R)right.Left), Kean.Math.Abstract<R, V>.Minimum((R)left.Top, (R)right.Top)),
                    size = Kean.Math.Geometry2D.Abstract.Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(Kean.Math.Abstract<R, V>.Maximum((R)left.Right, (R)right.Right) - Kean.Math.Abstract<R, V>.Minimum((R)left.Left, (R)right.Left), Kean.Math.Abstract<R, V>.Maximum((R)left.Bottom, (R)right.Bottom) - Kean.Math.Abstract<R, V>.Minimum((R)left.Top, (R)right.Top))
                };
            return result;
        }
        public static BoxType operator -(Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            BoxType result;
            if (!left.Empty && !right.Empty)
            {
                R l = Kean.Math.Abstract<R, V>.Maximum((R)left.Left, (R)right.Left);
                R r = Kean.Math.Abstract<R, V>.Minimum((R)left.Right, (R)right.Right);
                R t = Kean.Math.Abstract<R, V>.Maximum((R)left.Top, (R)right.Top);
                R b = Kean.Math.Abstract<R, V>.Minimum((R)left.Bottom, (R)right.Bottom);
                if (l.LessThan(r) && t.LessThan(b))
                {
                    result = new BoxType()
                    {
                        leftTop = Kean.Math.Geometry2D.Abstract.Point<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(l, t),
                        size = Kean.Math.Geometry2D.Abstract.Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(r - l, b - t)
                    };
                }
                else
                    result = new BoxType();
            }
            else
                result = new BoxType();
            return result;
        }
        public static BoxType operator +(Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, PointType right)
        {
            return new BoxType() { leftTop = left.leftTop + (IVector<V>)right, size = left.size };
        }
        public static BoxType operator -(Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, PointType right)
        {
            return new BoxType() { leftTop = left.leftTop - right, size = left.size };
        }
        public static BoxType operator +(Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, SizeType right)
        {
            return new BoxType() { leftTop = left.leftTop, size = left.size + right };
        }
        public static BoxType operator -(Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, SizeType right)
        {
            return new BoxType() { leftTop = left.leftTop, size = left.size - right };
        }
        public static BoxType operator *(TransformType left, Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return new BoxType() { leftTop = left * right.leftTop, size = left * right.size };
        }
        #endregion
        #region Comparison Operators
        public static bool operator ==(Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IBox<PointValue, SizeValue, V> right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) &&
                left.leftTop == right.LeftTop &&
                left.size == right.Size;
        }
        public static bool operator !=(Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IBox<PointValue, SizeValue, V> right)
        {
            return !(left == right);
        }
        #endregion
        #region Casts
        public static explicit operator Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>(BoxValue value)
        {
            return new BoxType() { leftTop = (PointType)value.LeftTop, size = (SizeType)value.Size };
        }
        #endregion
        #region Object Overrides
        public override bool Equals(object other)
        {
            return (other is BoxType) && this.Equals(other as BoxType);
        }
        public bool Equals(BoxType other)
        {
            return this == other;
        }
        public override string ToString()
        {
            return this.leftTop.ToString() + " " + this.size.ToString();
        }
        public override int GetHashCode()
        {
            return this.leftTop.GetHashCode() ^ this.size.GetHashCode();
        }
        #endregion
        #region Static Methods
        public static BoxType Create(PointType leftTop, SizeType size)
        {
            BoxType result = new BoxType();
            result.leftTop = leftTop;
            result.size = size;
            return result;
        }
        public static BoxType Bounds(V left, V right, V top, V bottom)
        {
			return Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(Point<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(left, top), Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create((R)right - left, (R)bottom - top));
        }
        public static BoxType Bounds(params PointType[] points)
        {
            BoxType result = null;
            if (points.Length > 0)
            {
                R xMinimum = Kean.Math.Abstract<R, V>.Zero;
                R xMaximum = xMinimum;
                R yMinimum = xMinimum;
                R yMaximum = xMinimum;
                bool initilized = false;
                foreach (PointType point in points)
                {
                    if (point != null)
                    {
                        if (!initilized)
                        {
                            initilized = true;
                            xMinimum = (R)point.X;
                            xMaximum = (R)point.X;
                            yMinimum = (R)point.Y;
                            yMaximum = (R)point.Y;
                        }
                        else
                        {
                            if (point.X < xMinimum)
                                xMinimum = (R)point.X;
                            else if (point.X > xMaximum)
                                xMaximum = (R)point.X;
                            if (point.Y < yMinimum)
                                yMinimum = (R)point.Y;
                            else if (point.Y > yMaximum)
                                yMaximum = (R)point.Y;
                        }
                    }
                }
				result = Box<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(Point<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(xMinimum, yMinimum), Size<TransformType, TransformValue, ShellType, ShellValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>.Create(xMaximum - xMinimum, yMaximum - yMinimum));
            }
            return result;
        }
        #endregion
    }
}

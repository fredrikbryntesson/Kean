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
    public abstract class Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
        IBox<PointValue, SizeValue, V>,
        IEquatable<BoxType>
        where TransformType : Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, ITransform<V>, new()
        where TransformValue : struct, ITransform<V>
        where BoxType : Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, IBox<PointValue, SizeValue, V>, new()
        where BoxValue : struct, IBox<PointValue, SizeValue, V>
        where PointType : Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, IPoint<V>, new()
        where PointValue : struct, IPoint<V>, IVector<V>
        where SizeType : Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, ISize<V>, new()
        where SizeValue : struct, ISize<V>, IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        #region IBox<PointValue,SizeValue,V> Members
        PointType leftTop;
        public PointValue LeftTop { get { return this.LeftTop; } }
        SizeType size;
        public SizeValue Size { get { return this.Size; } }
        #endregion

        #region Sizes
        public V Width {get{return this.Size.Width;}}
        public V Height { get { return this.Size.Height; } }
        #endregion

        #region All sides
        public V Left { get { return (this.LeftTop as IPoint<V>).X; } }
        public V Right { get { return (R)((this.LeftTop as IPoint<V>).X) + this.Size.Width; } }
        public V Top { get { return (this.LeftTop as IPoint<V>).Y; } }
        public V Bottom { get { return (R)((this.LeftTop as IPoint<V>).Y) + this.Size.Height; } }
        #endregion

        #region IBox<PointValue, SizeValue, V>
        PointValue IBox<PointValue, SizeValue, V>.LeftTop { get { return this.LeftTop; } }
        SizeValue IBox<PointValue, SizeValue, V>.Size { get { return this.Size; } }
        #endregion
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
        #endregion
        #region Arithmetic operators
        public static BoxType operator +(Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, PointType right)
        {
            return new BoxType() { leftTop = left.leftTop + right, size = left.size };
        }
        public static BoxType operator -(Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, PointType right)
        {
            return new BoxType() { leftTop = left.leftTop - right, size = left.size };
        }
        public static BoxType operator +(Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, SizeType right)
        {
            return new BoxType() { leftTop = left.leftTop, size = left.size + right };
        }
        public static BoxType operator -(Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, SizeType right)
        {
            return new BoxType() { leftTop = left.leftTop, size = left.size - right };
        }
        public static BoxType operator *(TransformType left, Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return new BoxType() { leftTop = left * right.leftTop, size = left * right.size };
        }
        #endregion
        #region Comparison Operators
        public static bool operator ==(Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IBox<PointValue, SizeValue, V> right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) &&
                left.leftTop == right.LeftTop &&
                left.size == right.Size;
        }
        public static bool operator !=(Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IBox<PointValue, SizeValue, V> right)
        {
            return !(left == right);
        }
        #endregion
        #region Casts
        public static explicit operator Box<TransformType, TransformValue, BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>(BoxValue value)
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
        #endregion
    }
}

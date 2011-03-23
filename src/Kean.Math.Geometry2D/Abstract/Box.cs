using System;

namespace Kean.Math.Geometry2D.Abstract
{
    public abstract class Box<BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
        IBox<PointValue, SizeValue, V>
        where BoxType : Box<BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, IBox<PointValue, SizeValue, V>, new()
        where BoxValue : struct, IBox<PointValue, SizeValue, V>
        where PointType : Point<PointType, PointValue, R, V>, IPoint<V>, new()
        where PointValue : struct, IPoint<V>, IVector<V>
        where SizeType : Size<SizeType, SizeValue, R, V>, ISize<V>, new()
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
        public BoxType Copy()
        {
            return new BoxType()
            {
                leftTop = this.leftTop.Copy(),
                size = this.size.Copy()
            };
        }
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
        public static BoxType operator +(Box<BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> box, PointType point)
        {
            return new BoxType() { leftTop = box.leftTop + point, size = box.size };
        }
        public static BoxType operator -(Box<BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> box, PointType point)
        {
            return new BoxType() { leftTop = box.leftTop - point, size = box.size };
        }
        public static BoxType operator +(Box<BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> box, SizeType size)
        {
            return new BoxType() { leftTop = box.leftTop, size = box.size + size };
        }
        public static BoxType operator -(Box<BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> box, SizeType size)
        {
            return new BoxType() { leftTop = box.leftTop, size = box.size - size };
        }
        #endregion
        #region Comparison Operators
        public static bool operator ==(Box<BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IBox<PointValue, SizeValue, V> right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) &&
                left.leftTop == right.LeftTop &&
                left.size == right.Size;
        }
        public static bool operator !=(Box<BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> left, IBox<PointValue, SizeValue, V> right)
        {
            return !(left == right);
        }
        #endregion
        #region Casts
        public static explicit operator Box<BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>(BoxValue value)
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

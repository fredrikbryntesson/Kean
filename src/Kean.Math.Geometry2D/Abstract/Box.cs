using System;

namespace Kean.Math.Geometry2D.Abstract
{
    public abstract class Box<BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V> :
        IBox<PointValue, SizeValue, V>
        where BoxType : Box<BoxType, BoxValue, PointType, PointValue, SizeType, SizeValue, R, V>, IBox<PointValue, SizeValue, V>, new()
        where BoxValue : struct, IBox<PointValue, SizeValue, V>
        where PointType : Point<PointType, R, V>, IPoint<V>, new()
        where PointValue : struct, IPoint<V>, IVector<V>
        where SizeType : Size<SizeType, R, V>, ISize<V>, new()
        where SizeValue : struct, ISize<V>, IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        #region IBox<PointValue,SizeValue,V> Members
        PointValue leftTop;
        public PointValue LeftTop { get { return this.leftTop; } }
        SizeValue size;
        public SizeValue Size { get { return this.size; } }
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
        #region Constructors
        protected Box()
        {
            this.leftTop = new PointValue();
            this.size = new SizeValue();
        }
        protected Box(PointValue leftTop, SizeValue size)
        {
            this.leftTop = leftTop;
            this.size = size;
        }
        #endregion
        #region Methods
        /*
        public BoxType Copy()
        {
            return new BoxType()
            {
                leftTop = this.LeftTop.Copy();
                size = this.Size.Copy()
            };
        }
        */
        #endregion
    
    }
}

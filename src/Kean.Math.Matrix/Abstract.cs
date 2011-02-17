using System;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Math.Matrix
{
    public class Abstract<R, V> 
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        public Geometry2D.Integer.Size Size { get; private set; }
        // Matrix elements are supposed to be in column major order 
        public V[] Elements { get; private set; }
        #region Constructors
        protected Abstract() { }
        protected Abstract(Integer order) : this(order, order) { }
        protected Abstract(Integer width, Integer height) : this(new Geometry2D.Integer.Size(width, height)) { }
        protected Abstract(Geometry2D.Integer.Size size)
		{
            this.Size = size;
            this.Elements = new V[size.Area]; 
        }
        #endregion

        #region Arithmetic Methods
        
        #endregion
        
        #region Static Methods

        #endregion

    }
}

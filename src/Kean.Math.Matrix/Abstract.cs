using System;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Math.Matrix
{
    public class Abstract<MatrixType, R, V>
        where MatrixType : Abstract<MatrixType, R, V>, new()
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        public Geometry2D.Integer.Size Dimension { get; private set; }
        // Matrix elements are supposed to be in column major order 
        private V[] elements;
        #region Constructors
        protected Abstract() { }
        protected Abstract(Integer order) : this(order, order) { }
        protected Abstract(Integer width, Integer height) : this(new Geometry2D.Integer.Size(width, height)) { }
        protected Abstract(Geometry2D.Integer.Size dimension)
		{
            this.Dimension = dimension;
            this.elements = new V[dimension.Area]; 
        }
        #endregion
        public V this[int x, int y]
        {
            get { return this.elements[this.Index(x, y)]; }
            set { this.elements[this.Index(x, y)] = value; }
        }
        private int Index(int x, int y)
        {
            return x * this.Dimension.Height + y; // Column major order 
            // Use y * this.Dimension.Width + x for row major order
        }
        public MatrixType Copy()
        {
            MatrixType result = new MatrixType() { Dimension = this.Dimension, elements = new V[this.elements.Length] };
            Array.Copy(this.elements, result.elements, this.elements.Length);
            return result;
        }
        #region Arithmetic Matrix - Matrix Operators
        public static MatrixType operator +(Abstract<MatrixType, R, V> left, MatrixType right)
        {
            if (left.Dimension != right.Dimension)
                new Exception.InvalidDimensions();
            MatrixType result = new MatrixType() { Dimension = left.Dimension };
            for (int i = 0; i < result.elements.Length; i++)
                result.elements[i] = (R)left.elements[i] + right.elements[i];
            return result;
        }
        public static MatrixType operator *(Abstract<MatrixType, R, V> left, MatrixType right)
        {
            if (left.Dimension.Width != right.Dimension.Height)
                new Exception.InvalidDimensions();
            MatrixType result = new MatrixType() { Dimension = new Geometry2D.Integer.Size(right.Dimension.Width, left.Dimension.Height) };
            for (int x = 0; x < right.Dimension.Width; x++)
                for (int y = 0; y < left.Dimension.Height; y++)
                    for(int z = 0; z < left.Dimension.Width; z++)
                result[x,y] = (R)result[x,y] + (R)left[z,y] * right[x,z];
            return result;
        }
        public static MatrixType operator -(Abstract<MatrixType, R, V> left, MatrixType right)
        {
            return left + (-right);
        }
        #endregion
        #region Arithmetic Scalar - Matrix Operators
        public static MatrixType operator +(V left, Abstract<MatrixType, R, V> right)
        {
            MatrixType result = new MatrixType() { Dimension = right.Dimension };
            for (int i = 0; i < result.elements.Length; i++)
                result.elements[i] = (R)left + right.elements[i];
            return result;
        }
        public static MatrixType operator +(Abstract<MatrixType, R, V> left, V right)
        {
            return right * left;
        }
        public static MatrixType operator *(V left, Abstract<MatrixType, R, V> right)
        {
            MatrixType result = new MatrixType() { Dimension = right.Dimension};
            for (int i = 0; i < result.elements.Length; i++)
                result.elements[i] = (R)left * right.elements[i];
            return result;
        }
        public static MatrixType operator *(Abstract<MatrixType, R, V> left, V right)
        {
            return right * left;
        }
        public static MatrixType operator -(Abstract<MatrixType, R, V> value)
        {
            return new R().One.Negate()  * value ;
        }
        #endregion
        #region Static Operators
        public static MatrixType Identity(int order)
        {
            MatrixType result = new MatrixType()
            {
                Dimension = new Kean.Math.Geometry2D.Integer.Size(order, order),
                elements = new V[order * order]
            };
            for (int i = 0; i < order; i++)
                result[i, i] = new R().One;
            return result;
        }
        #endregion 
        #region Casts
        public static implicit operator Abstract<MatrixType, R, V> (V[] value)
        {
            MatrixType result = new MatrixType()
            {
                Dimension = new Kean.Math.Geometry2D.Integer.Size(1, value.Length),
                elements = new V[value.Length]
            };
            Array.Copy(value, result.elements, value.Length);
            return result;
        }
        public static implicit operator V[](Abstract<MatrixType, R, V> value)
        {
            V[] result = new V[value.elements.Length];
            Array.Copy(value.elements, result, result.Length);
            return result;
        }
        #endregion
    }
}

// 
//  Vector.cs
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
namespace Kean.Math.Geometry2D.Abstract
{
    public class Vector<VectorType, R, V> :
        IVector<V>,
        IEquatable<Vector<VectorType, R, V>>
        where VectorType : Vector<VectorType, R, V>, new()
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        protected R X { get; private set; }
        protected R Y { get; private set; }
        #region IVector<V> Members
        V IVector<V>.X { get { return this.X; } }
        V IVector<V>.Y { get { return this.Y; } }
        #endregion
        #region Constructors
        protected Vector()
        {
            this.X = Kean.Math.Abstract<R, V>.Zero;
            this.Y = Kean.Math.Abstract<R, V>.Zero;
        }
        protected Vector(R x, R y)
        {
            this.X = x;
            this.Y = y;
        }
        #endregion
        #region Methods
        public VectorType Copy()
        {
            return new VectorType() { X = this.X, Y = this.Y };
        }
        public VectorType Swap()
        {
            return new VectorType() { X = this.Y, Y = this.X };
        }
        #endregion
        #region Arithmetic Vector - Vector Operators
        public static VectorType operator +(Vector<VectorType, R, V> left, VectorType right)
        {
            VectorType result = new VectorType()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
            };
            return result;
        }
        public static VectorType operator -(Vector<VectorType, R, V> vector)
        {
            VectorType result = new VectorType()
            {
                X = -vector.X,
                Y = -vector.Y,
            };
            return result;
        }
        public static VectorType operator -(Vector<VectorType, R, V> left, VectorType right)
        {
            return left + (-right);
        }
        #endregion
        #region Arithmetic Vector and Scalar
        public static VectorType operator *(Vector<VectorType, R, V> left, R right)
        {
            VectorType result = new VectorType()
            {
                X = left.X * right,
                Y = left.Y * right,
            };
            return result;
        }
        public static VectorType operator *(R left, Vector<VectorType, R, V> right)
        {
            return right * left;
        }
        #endregion
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="left">Point left of operator.</param>
        /// <param name="right">Point right of operator.</param>
        /// <returns>True if <paramref name="left"/> equals <paramref name="right"/> else false.</returns>
        public static bool operator ==(Vector<VectorType, R, V> left, IVector<V> right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) && left.X == right.X && left.Y == right.Y;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="left">Point left of operator.</param>
        /// <param name="right">Point right of operator.</param>
        /// <returns>False if <paramref name="left"/> equals <paramref name="right"/> else true.</returns>
        public static bool operator !=(Vector<VectorType, R, V> left, IVector<V> right)
        {
            return !(left == right);
        }
        #endregion
        #region Object overides and IEquatable<VectorType>
        public override bool Equals(object other)
        {
            return (other is Vector<VectorType, R, V>) && this.Equals(other as Vector<VectorType, R, V>);
        }
        // other is not null here.
        public bool Equals(Vector<VectorType, R, V> other)
        {
            return this.X == other.X && this.Y == other.Y;
        }
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
        public override string ToString()
        {
            return this.X.ToString() + " " + this.Y.ToString();
        }
        #endregion
        #region Casts.
        public static explicit operator V[](Vector<VectorType, R, V> value)
        {
            return new V[] { value.X, value.Y};
        }
        public static explicit operator Vector<VectorType, R, V>(V[] value)
        {
            return new VectorType() { X = (R)value[0], Y = (R)value[1]};
        }
        #endregion
    }
}


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
namespace Kean.Math.Geometry3D.Abstract
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
        protected R Z { get; private set; }
        public R Norm { get { return (this.X * this.X + this.Y * this.Y + this.Z + this.Z).SquareRoot(); } }
        #region IVector<V> Members
        V IVector<V>.X { get { return this.X; } }
        V IVector<V>.Y { get { return this.Y; } }
        V IVector<V>.Z { get { return this.Z; } }
        #endregion
        #region Static Constants
        public static VectorType Basis1 { get { return new VectorType() { X = Kean.Math.Abstract<R, V>.One, Y = new R(), Z = new R() }; } }
        public static VectorType Basis2 { get { return new VectorType() { X = new R(), Y = Kean.Math.Abstract<R, V>.One, Z = new R() }; } }
        public static VectorType Basis3 { get { return new VectorType() { X = new R(), Y = new R(), Z = Kean.Math.Abstract<R, V>.One }; } }
        #endregion
        #region Constructors
        protected Vector()
        {
            this.X = Kean.Math.Abstract<R, V>.Zero;
            this.Y = Kean.Math.Abstract<R, V>.Zero;
            this.Z = Kean.Math.Abstract<R, V>.Zero;
        }
        protected Vector(R x, R y, R z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        #endregion
        #region Methods
        public VectorType Copy()
        {
            return new VectorType() { X = this.X, Y = this.Y, Z = this.Z };
        }
        public R ScalarProduct(VectorType other)
        {
            return this.X * other.X + this.Y * other.Y + this.Z * other.Z;
        }
        #endregion
        #region Arithmetic Vector - Vector Operators
        public static VectorType operator *(Vector<VectorType, R, V> left, VectorType right)
        {
            VectorType result = new VectorType()
            {
                X = left.Y * right.Z - right.Y * left.Z,
                Y = -(left.X * right.Z - right.X * left.Z),
                Z = left.X * right.Y - right.X * left.Y
            };
            return result;
        }
        public static VectorType operator +(Vector<VectorType, R, V> left, VectorType right)
        {
            VectorType result = new VectorType()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z,
            };
            return result;
        }
        public static VectorType operator -(Vector<VectorType, R, V> vector)
        {
            VectorType result = new VectorType()
            {
                X = -vector.X,
                Y = -vector.Y,
                Z = -vector.Z,
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
                Z = left.Z * right,
            };
            return result;
        }
        public static VectorType operator *(R left, Vector<VectorType, R, V> right)
        {
            return right * left;
        }
        public static VectorType operator /(Vector<VectorType, R, V> left, R right)
        {
            VectorType result = new VectorType()
            {
                X = left.X / right,
                Y = left.Y / right,
                Z = left.Z / right,
            };
            return result;
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
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }
        public override string ToString()
        {
            return this.X.ToString() + " " + this.Y.ToString() + " " + this.Z.ToString();
        }
        #endregion
    }
}


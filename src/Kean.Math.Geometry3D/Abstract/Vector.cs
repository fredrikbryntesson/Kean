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
    public abstract class Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue, R, V> :
        IVector<V>,
        IEquatable<Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue, R, V>>
        where VectorType : Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue, R, V>, IVector<V>, new()
        where VectorValue : struct, IVector<V>
        where TransformType : Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, ITransform<V>, new()
        where TransformValue : struct, ITransform<V>
        where SizeType : Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, IVector<V>, new()
        where SizeValue : struct, ISize<V>, IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        protected R X { get; private set; }
        protected R Y { get; private set; }
        protected R Z { get; private set; }
        public abstract VectorValue Value { get; }
        /// <summary>
        /// Length of vector. 
        /// </summary>
        public R Norm { get { return (this.X.Squared() + this.Y.Squared() + this.Z.Squared()).SquareRoot(); } }
        /// <summary>
        /// Angle in radians describing the projection in the xy-plane of the vector. [-pi, pi]. See http://mathworld.wolfram.com/SphericalCoordinates.html.
        /// Example: point (1,0,0) has azimuth 0, and point (0,1,0) has azimuth pi/2.
        /// </summary>
        public R Azimuth { get { return this.Y.ArcusTangensExtended(this.X); } }
        /// <summary>
        /// Angle of elevation beteen the z-axis and the vector. [0, pi]. Elevation = 0 refers to a point on the z-axis.
        /// Example: point (1,0,0) has elevation pi/2, and point (0,0,1) has elevation 0.
        /// </summary>
        public R Elevation 
        {
            get 
            {   R result = new R();
                R r = this.Norm;
                if (r != result)
                    result = (this.Z / r).Clamp(Kean.Math.Abstract<R, V>.One.Negate(), Kean.Math.Abstract<R, V>.One).ArcusCosinus();
                return result;
            } 
        }
        #region IVector<V> Members
        V IVector<V>.X { get { return this.X; } }
        V IVector<V>.Y { get { return this.Y; } }
        V IVector<V>.Z { get { return this.Z; } }
        #endregion
        #region Static Constants
        public static VectorType BasisX { get { return new VectorType() { X = Kean.Math.Abstract<R, V>.One, Y = new R(), Z = new R() }; } }
        public static VectorType BasisY { get { return new VectorType() { X = new R(), Y = Kean.Math.Abstract<R, V>.One, Z = new R() }; } }
        public static VectorType BasisZ { get { return new VectorType() { X = new R(), Y = new R(), Z = Kean.Math.Abstract<R, V>.One }; } }
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
        protected VectorType Create(R x, R y, R z)
        {
            return new VectorType() { X = x, Y = y, Z = z };
        }
        public R ScalarProduct(VectorType other)
        {
            return this.X * other.X + this.Y * other.Y + this.Z * other.Z;
        }
        public VectorType VectorProduct(VectorType other)
        {
            return new VectorType()
            {
                X = this.Y * other.Z - other.Y * this.Z,
                Y = -(this.X * other.Z - other.X * this.Z),
                Z = this.X * other.Y - other.X * this.Y
            };
        }
        public R Distance(VectorType other)
        {
            return (this - other).Norm;
        }
        #endregion
        #region Arithmetic Vector - Vector Operators
        public static VectorType operator *(Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue, R, V> left, VectorType right)
        {
            return new VectorType()
            {
                X = left.X * right.X,
                Y = left.Y * right.Y,
                Z = left.Z * right.Z
            };
        }
        public static VectorType operator +(Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue, R, V> left, VectorType right)
        {
            VectorType result = new VectorType()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z,
            };
            return result;
        }
        public static VectorType operator -(Vector<TransformType, TransformValue, VectorType, VectorValue,SizeType, SizeValue, R, V> vector)
        {
            VectorType result = new VectorType()
            {
                X = -vector.X,
                Y = -vector.Y,
                Z = -vector.Z,
            };
            return result;
        }
        public static VectorType operator -(Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue,R, V> left, VectorType right)
        {
            return left + (-right);
        }
        #endregion
        #region Arithmetic Vector and Scalar
        public static VectorType operator *(Vector<TransformType, TransformValue, VectorType, VectorValue,SizeType, SizeValue, R, V> left, R right)
        {
            VectorType result = new VectorType()
            {
                X = left.X * right,
                Y = left.Y * right,
                Z = left.Z * right,
            };
            return result;
        }
        public static VectorType operator *(R left, Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue,R, V> right)
        {
            return right * left;
        }
        public static VectorType operator /(Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue, R, V> left, R right)
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
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="left">Point left of operator.</param>
        /// <param name="right">Point right of operator.</param>
        /// <returns>True if <paramref name="left"/> equals <paramref name="right"/> else false.</returns>
        public static bool operator ==(Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue,R, V> left, IVector<V> right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) && left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="left">Point left of operator.</param>
        /// <param name="right">Point right of operator.</param>
        /// <returns>False if <paramref name="left"/> equals <paramref name="right"/> else true.</returns>
        public static bool operator !=(Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue,R, V> left, IVector<V> right)
        {
            return !(left == right);
        }
        #endregion
        #region Object overides and IEquatable<VectorType>
        public override bool Equals(object other)
        {
            return (other is Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue, R, V>) && this.Equals(other as Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue, R, V>);
        }
        // other is not null here.
        public bool Equals(Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue,R, V> other)
        {
            return this == other;
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
        #region Casts
        public static explicit operator V[](Vector<TransformType, TransformValue, VectorType, VectorValue,SizeType, SizeValue, R, V> value)
        {
            return new V[] { value.X, value.Y, value.Z };
        }
        public static explicit operator Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue,R, V>(V[] value)
        {
            return new VectorType() { X = (R)value[0], Y = (R)value[1], Z = (R)value[2] };
        }
        public static explicit operator Vector<TransformType, TransformValue, VectorType, VectorValue, SizeType, SizeValue, R, V>(VectorValue value)
        {
            return new VectorType() { X = (R)value.X, Y = (R)value.Y, Z = (R)value.Z };
        }
        #endregion
        public static VectorType Create(V x, V y, V z)
        {
            return new VectorType() { X = (R)x, Y = (R)y, Z = (R)z };
        }
    }
}


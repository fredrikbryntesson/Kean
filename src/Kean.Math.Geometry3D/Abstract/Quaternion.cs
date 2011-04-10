// 
//  Point.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
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
namespace Kean.Math.Geometry3D.Abstract
{
    public abstract class Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> :
        IEquatable<QuaternionType>
        where QuaternionType : Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointType : Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>, new()
        where PointValue : struct, IPoint<V>, IVector<V>
        where TransformType : Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, ITransform<V>, new()
        where TransformValue : struct, ITransform<V>
        where SizeType : Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, IVector<V>, new()
        where SizeValue : struct, ISize<V>, IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        public R Real { get; private set; }
        public PointType Imaginary { get; private set; }
        public R Norm { get { return (this.Real.Squared() + this.Imaginary.Norm.Squared()).SquareRoot(); } }
        /// <summary>
        /// Clockwise rotation around the direction vector of the corresponding rotation.
        /// </summary>
        public R Rotation { get { return Kean.Math.Abstract<R, V>.Two * (this.Logarithm().Imaginary).Norm; } }
        /// <summary>
        /// Direction vector of corresponding rotation. 
        /// </summary>
        public PointType Direction { get { return this.Logarithm().Imaginary / this.Logarithm().Imaginary.Norm;}}
        public QuaternionType Inverse { get { return this.Conjugate / this.Norm.Squared(); } }
        public QuaternionType Conjugate { get { return new QuaternionType() { Real = this.Real, Imaginary = -this.Imaginary }; } }
        #region Representations
        public R RotationX { get { return (Kean.Math.Abstract<R, V>.Two * (this.Real * this.Imaginary.X + this.Imaginary.Y * this.Imaginary.Z)).ArcusTangensExtended(Kean.Math.Abstract<R, V>.One - Kean.Math.Abstract<R, V>.Two * (this.Imaginary.X.Squared() + this.Imaginary.Y.Squared())); } }
        public R RotationY { get { return (Kean.Math.Abstract<R, V>.Two * (this.Real * this.Imaginary.Y - this.Imaginary.Z * this.Imaginary.X)).Clamp(Kean.Math.Abstract<R, V>.One.Negate(), Kean.Math.Abstract<R, V>.One).ArcusSinus(); } }
        public R RotationZ { get { return (Kean.Math.Abstract<R, V>.Two * (this.Real * this.Imaginary.Z + this.Imaginary.X * this.Imaginary.Y)).ArcusTangensExtended(Kean.Math.Abstract<R, V>.One - Kean.Math.Abstract<R, V>.Two * (this.Imaginary.Y.Squared() + this.Imaginary.Z.Squared())); } }
        #endregion
        #region Static Constants
        public static QuaternionType BasisReal { get { return new QuaternionType() { Real = Kean.Math.Abstract<R,V>.One, Imaginary = new PointType() }; } }
        public static QuaternionType BasisImaginaryX { get { return new QuaternionType() { Real = new R(), Imaginary = Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>.BasisX }; } }
        public static QuaternionType BasisImaginaryY { get { return new QuaternionType() { Real = new R(), Imaginary = Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>.BasisY }; } }
        public static QuaternionType BasisImaginaryZ { get { return new QuaternionType() { Real = new R(), Imaginary = Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>.BasisZ }; } }
        #endregion
        #region Constructors
        protected Quaternion()
        {
            this.Real = new R();
            this.Imaginary = new PointType();
        }
        protected Quaternion(R real, PointType imaginary)
        {
            this.Real = real;
            this.Imaginary = imaginary;
        }
        #endregion
        #region Methods
        public QuaternionType Copy()
        {
            return new QuaternionType() { Real = this.Real, Imaginary = this.Imaginary };
        }
        #region Transcendental Functions
        public QuaternionType Exponential()
        {
            QuaternionType result = new QuaternionType();
            R norm = this.Imaginary.Norm;
            R exponentialReal = this.Real.Exponential();
            if(norm != new R())
            {
                result.Real = exponentialReal * norm.Cosinus();
                result.Imaginary = exponentialReal * (this.Imaginary / norm) * norm.Sinus(); 
            }
            else
                result = (QuaternionType)(exponentialReal);
            return result;
        }
        public QuaternionType Logarithm()
        {
            QuaternionType result = new QuaternionType();
            R norm = this.Imaginary.Norm;
            if (norm != new R())
            {
                result.Real = this.Norm.Logarithm();
                result.Imaginary = (this.Imaginary / norm) * (this.Real / this.Norm).ArcusCosinus();
            }
            else
                result = (QuaternionType)(this.Norm);
            return result;
        }
        #endregion
        #endregion
        #region Arithmetic Point - Point Operators
        public static PointType operator *(Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> left, PointType right)
        {
            return (left * new QuaternionType() { Real = new R(), Imaginary = right } * left.Inverse).Imaginary;
        }
        public static QuaternionType operator *(Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> left, QuaternionType right)
        {
            QuaternionType result = new QuaternionType()
            {
                Real = left.Real * right.Real - left.Imaginary.ScalarProduct(right.Imaginary),
                Imaginary = left.Real * right.Imaginary + left.Imaginary * right.Real + left.Imaginary * right.Imaginary
            };
            return result;
        }
        public static QuaternionType operator +(Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> left, QuaternionType right)
        {
            QuaternionType result = new QuaternionType()
            {
                Real = left.Real + right.Real,
                Imaginary = left.Imaginary + right.Imaginary,
            };
            return result;
        }
        public static QuaternionType operator -(Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> quaternion)
        {
            QuaternionType result = new QuaternionType()
            {
                Real = -quaternion.Real,
                Imaginary = -quaternion.Imaginary,
            };
            return result;
        }
        public static QuaternionType operator -(Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> left, QuaternionType right)
        {
            return left + (-right);
        }
        #endregion
        #region Arithmetic Point and Scalar
        public static QuaternionType operator *(Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> left, R right)
        {
            QuaternionType result = new QuaternionType()
            {
                Real = left.Real * right,
                Imaginary = left.Imaginary * right,
            };
            return result;
        }
        public static QuaternionType operator *(R left, Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return right * left;
        }
        public static QuaternionType operator /(Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> left, R right)
        {
            QuaternionType result = new QuaternionType()
            {
                Real = left.Real / right,
                Imaginary = left.Imaginary / right,
            };
            return result;
        }
        #endregion
        #region Static Functions
        /// <summary>
        /// Rotation around the real-axis
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static QuaternionType CreateRotationX(R angle)
        {
            return Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.CreateRotation(angle, Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>.BasisX);
        }
        /// <summary>
        /// Rotation around the imaginary-axis
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static QuaternionType CreateRotationY(R angle)
        {
            return Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.CreateRotation(angle, Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>.BasisY);
        }
        /// <summary>
        /// Rotation around the z-axis
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static QuaternionType CreateRotationZ(R angle)
        {
            return Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>.CreateRotation(angle, Point<TransformType, TransformValue, PointType, PointValue, SizeType, SizeValue, R, V>.BasisZ);
        }
        /// <summary>
        /// Rotation around the given axis vector 
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static QuaternionType CreateRotation(R angle, PointType direction)
        {
            R halfAngle = angle / Kean.Math.Abstract<R, V>.Two;
            R norm = direction.Norm;
            if(norm != new R())
                direction = direction / direction.Norm;
            return ((Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>)(halfAngle * direction)).Exponential();
        }
        #endregion
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="left">Point left of operator.</param>
        /// <param name="right">Point right of operator.</param>
        /// <returns>True if <paramref name="left"/> equals <paramref name="right"/> else false.</returns>
        public static bool operator ==(Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> left, Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) && left.Real == right.Real && left.Imaginary == right.Imaginary;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="left">Point left of operator.</param>
        /// <param name="right">Point right of operator.</param>
        /// <returns>False if <paramref name="left"/> equals <paramref name="right"/> else true.</returns>
        public static bool operator !=(Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> left, Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> right)
        {
            return !(left == right);
        }
        #endregion
        #region Object overides and IEquatable<QuaternionType>
        public override bool Equals(object other)
        {
            return (other is QuaternionType) && this.Equals(other as QuaternionType);
        }
        // other is not null here.
        public bool Equals(QuaternionType other)
        {
            return this == other;
        }
        public override int GetHashCode()
        {
            return this.Real.GetHashCode() ^ this.Imaginary.GetHashCode();
        }
        public override string ToString()
        {
            return this.Real.ToString() + " " + this.Imaginary.ToString();
        }
        #endregion
        #region Casts.
        /// <summary>
        /// Cast from Real to a quaternion.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>(R value)
        {
            return new QuaternionType() { Real = value, Imaginary = new PointType() };
        }
        /// <summary>
        /// Cast from Vector to a quaternion.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V>(PointType value)
        {
            return new QuaternionType() { Real = new R(), Imaginary = value };
        }
        public static explicit operator V[](Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> value)
        {
            return new V[] { value.Real, value.Imaginary.X, value.Imaginary.Y, value.Imaginary.Z };
        }
        public static explicit operator V[,](Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> value)
        {
            V[,] result = new V[3, 3];
            QuaternionType normalized = value / value.Norm;
            R q0 = normalized.Real;
            R q1 = normalized.Imaginary.X;
            R q2 = normalized.Imaginary.Y;
            R q3 = normalized.Imaginary.Z;
            result[0, 0] = q0.Squared() + q1.Squared() - q2.Squared() - q3.Squared();
            result[1, 0] = Kean.Math.Abstract<R, V>.Two * (q1 * q2 - q0 * q3);
            result[2, 0] = Kean.Math.Abstract<R, V>.Two * (q0 * q2 + q1 * q3);
            result[0, 1] = Kean.Math.Abstract<R, V>.Two * (q1 * q2 + q0 * q3);
            result[1, 1] = q0.Squared() - q1.Squared() + q2.Squared() - q3.Squared();
            result[2, 1] = Kean.Math.Abstract<R, V>.Two * (q2 * q3 - q0 * q1);
            result[0, 2] = Kean.Math.Abstract<R, V>.Two * (q1 * q3 - q0 * q2);
            result[1, 2] = Kean.Math.Abstract<R, V>.Two * (q0 * q1 + q2 * q3);
            result[2, 2] = q0.Squared() - q1.Squared() - q2.Squared() + q3.Squared();
            return result;
        }
        public static explicit operator Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>(Quaternion<TransformType, TransformValue, QuaternionType, PointType, PointValue, SizeType, SizeValue, R, V> value)
        {
            V[,] values = (V[,])value;
            return Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.Create(values[0, 0], values[0, 1], values[0, 2], values[1, 0], values[1, 1], values[1, 2], values[2, 0], values[2, 1], values[2, 2], new R(), new R(), new R());
        }
        #endregion
    }
}

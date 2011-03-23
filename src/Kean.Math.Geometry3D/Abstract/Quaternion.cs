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
    public abstract class Quaternion<QuaternionType, PointType, R, V> :
        IEquatable<Quaternion<QuaternionType, PointType, R, V>>
        where QuaternionType : Quaternion<QuaternionType, PointType, R, V>, new()
        where PointType : Point<PointType, R, V>, new()
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        protected R Real { get; private set; }
        protected PointType Imaginary { get; private set; }
        public R Norm { get { return (this.Real.Squared() + this.Imaginary.Norm.Squared()).SquareRoot(); } }
        public QuaternionType Inverse { get { return this.Conjugate / this.Norm.Squared(); } }
        public QuaternionType Conjugate { get { return new QuaternionType() { Real = this.Real, Imaginary = -this.Imaginary }; } }
        #region Representations
        public R Roll { get { return (Kean.Math.Abstract<R, V>.Two * (this.Real * this.Imaginary.X + this.Imaginary.Y * this.Imaginary.Z)).ArcusTangensExtended(Kean.Math.Abstract<R, V>.One - Kean.Math.Abstract<R, V>.Two * (this.Imaginary.X.Squared() + this.Imaginary.Y.Squared())); } }
        public R Pitch { get { return (Kean.Math.Abstract<R, V>.Two * (this.Real * this.Imaginary.Y - this.Imaginary.Z * this.Imaginary.X)).ArcusSinus(); } }
        public R Yaw { get { return (Kean.Math.Abstract<R, V>.Two * (this.Real * this.Imaginary.Z + this.Imaginary.X * this.Imaginary.Y)).ArcusTangensExtended(Kean.Math.Abstract<R, V>.One - Kean.Math.Abstract<R, V>.Two * (this.Imaginary.Y.Squared() + this.Imaginary.Z.Squared())); } }
        #endregion
        #region Static Constants
        public static QuaternionType Basis1 { get { return new QuaternionType() { Real = new R(), Imaginary = Point<PointType, R, V>.Basis1 }; } }
        public static QuaternionType Basis2 { get { return new QuaternionType() { Real = new R(), Imaginary = Point<PointType, R, V>.Basis2 }; } }
        public static QuaternionType Basis3 { get { return new QuaternionType() { Real = new R(), Imaginary = Point<PointType, R, V>.Basis3 }; } }
        #endregion
        #region Constructors
        protected Quaternion()
        {
            this.Real = new R();
            this.Imaginary = new PointType();
        }
        protected Quaternion(R x, PointType y)
        {
            this.Real = x;
            this.Imaginary = y;
        }
        #endregion
        #region Methods
        public QuaternionType Copy()
        {
            return new QuaternionType() { Real = this.Real, Imaginary = this.Imaginary };
        }
        #endregion
        #region Arithmetic Point - Point Operators
        public static PointType operator *(Quaternion<QuaternionType, PointType, R, V> left, PointType right)
        {
            return (left * new QuaternionType() { Real = new R(), Imaginary = right } * left.Inverse).Imaginary;
        }
        public static QuaternionType operator *(Quaternion<QuaternionType, PointType, R, V> left, QuaternionType right)
        {
            QuaternionType result = new QuaternionType()
            {
                Real = left.Real * right.Real - left.Imaginary.ScalarProduct(right.Imaginary),
                Imaginary = left.Real * right.Imaginary + left.Imaginary * right.Real + left.Imaginary * right.Imaginary
            };
            return result;
        }
        public static QuaternionType operator +(Quaternion<QuaternionType, PointType, R, V> left, QuaternionType right)
        {
            QuaternionType result = new QuaternionType()
            {
                Real = left.Real + right.Real,
                Imaginary = left.Imaginary + right.Imaginary,
            };
            return result;
        }
        public static QuaternionType operator -(Quaternion<QuaternionType, PointType, R, V> quaternion)
        {
            QuaternionType result = new QuaternionType()
            {
                Real = -quaternion.Real,
                Imaginary = -quaternion.Imaginary,
            };
            return result;
        }
        public static QuaternionType operator -(Quaternion<QuaternionType, PointType, R, V> left, QuaternionType right)
        {
            return left + (-right);
        }
        #endregion
        #region Arithmetic Point and Scalar
        public static QuaternionType operator *(Quaternion<QuaternionType, PointType, R, V> left, R right)
        {
            QuaternionType result = new QuaternionType()
            {
                Real = left.Real * right,
                Imaginary = left.Imaginary * right,
            };
            return result;
        }
        public static QuaternionType operator *(R left, Quaternion<QuaternionType, PointType, R, V> right)
        {
            return right * left;
        }
        public static QuaternionType operator /(Quaternion<QuaternionType, PointType, R, V> left, R right)
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
        /// First yaw, then roll, then pitch.
        /// </summary>
        /// <param name="roll"></param>
        /// <param name="pitch"></param>
        /// <param name="yaw"></param>
        /// <returns></returns>
        public static QuaternionType EulerAngles(R roll, R pitch, R yaw)
        {
            R halfRoll = roll / Kean.Math.Abstract<R, V>.Two;
            R halfPitch = pitch / Kean.Math.Abstract<R, V>.Two;
            R halfYaw = yaw / Kean.Math.Abstract<R, V>.Two;
            return
                 ((Quaternion<QuaternionType, PointType, R, V>)halfRoll.Cosinus() + halfRoll.Sinus() * Quaternion<QuaternionType, PointType, R, V>.Basis1) *
                 ((Quaternion<QuaternionType, PointType, R, V>)halfPitch.Cosinus() + halfPitch.Sinus() * Quaternion<QuaternionType, PointType, R, V>.Basis2) *
                 ((Quaternion<QuaternionType, PointType, R, V>)halfYaw.Cosinus() + halfYaw.Sinus() * Quaternion<QuaternionType, PointType, R, V>.Basis3);
        }
        #endregion
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="left">Point left of operator.</param>
        /// <param name="right">Point right of operator.</param>
        /// <returns>True if <paramref name="left"/> equals <paramref name="right"/> else false.</returns>
        public static bool operator ==(Quaternion<QuaternionType, PointType, R, V> left, Quaternion<QuaternionType, PointType, R, V> right)
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
        public static bool operator !=(Quaternion<QuaternionType, PointType, R, V> left, Quaternion<QuaternionType, PointType, R, V> right)
        {
            return !(left == right);
        }
        #endregion
        #region Object overides and IEquatable<QuaternionType>
        public override bool Equals(object other)
        {
            return (other is Quaternion<QuaternionType, PointType, R, V>) && this.Equals(other as Quaternion<QuaternionType, PointType, R, V>);
        }
        // other is not null here.
        public bool Equals(Quaternion<QuaternionType, PointType, R, V> other)
        {
            return this.Real == other.Real && this.Imaginary == other.Imaginary;
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
        public static explicit operator Quaternion<QuaternionType, PointType, R, V>(R value)
        {
            return new QuaternionType() { Real = value, Imaginary = new PointType() };
        }
        public static explicit operator V[](Quaternion<QuaternionType, PointType, R, V> value)
        {
            return new V[] { value.Real, value.Imaginary.X, value.Imaginary.Y, value.Imaginary.Z };
        }
        public static explicit operator V[,](Quaternion<QuaternionType, PointType, R, V> value)
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
        #endregion
    }
}

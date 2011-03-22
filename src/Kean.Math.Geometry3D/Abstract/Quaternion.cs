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
    public abstract class Quaternion<QuaternionType, PointType, R, V>
        where QuaternionType : Quaternion<QuaternionType, PointType, R, V>, new()
        where PointType : Point<PointType, R, V>, new()
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        protected R X { get; private set; }
        protected PointType Y { get; private set; }
        public R Norm { get { return (this.X.Squared() + this.Y.Norm.Squared()).SquareRoot(); } }
        public QuaternionType Conjugate { get { return new QuaternionType() { X = this.X, Y = -this.Y }; } }
        #region Representations
        public R Roll { get { return (Kean.Math.Abstract<R, V>.Two * (this.X * this.Y.X + this.Y.Y * this.Y.Z)).ArcusTangensExtended(Kean.Math.Abstract<R, V>.One - Kean.Math.Abstract<R, V>.Two * (this.Y.X.Squared() + this.Y.Y.Squared())); } }
        public R Pitch { get { return (Kean.Math.Abstract<R, V>.Two * (this.X * this.Y.Y - this.Y.Z * this.Y.X)).ArcusSinus(); } }
        public R Yaw { get { return (Kean.Math.Abstract<R, V>.Two * (this.X * this.Y.Z + this.Y.X * this.Y.Y)).ArcusTangensExtended(Kean.Math.Abstract<R, V>.One - Kean.Math.Abstract<R, V>.Two * (this.Y.Y.Squared() + this.Y.Z.Squared())); } }
        #endregion
        #region Static Constants
        public static QuaternionType Basis1 { get { return new QuaternionType() { X = new R(), Y = Point<PointType, R, V>.Basis1 }; } }
        public static QuaternionType Basis2 { get { return new QuaternionType() { X = new R(), Y = Point<PointType, R, V>.Basis2 }; } }
        public static QuaternionType Basis3 { get { return new QuaternionType() { X = new R(), Y = Point<PointType, R, V>.Basis3 }; } }
        #endregion
        #region Constructors
        protected Quaternion()
        {
            this.X = new R();
            this.Y = new PointType();
        }
        protected Quaternion(R x, PointType y)
        {
            this.X = x;
            this.Y = y;
        }
        #endregion
        #region Methods
        public QuaternionType Copy()
        {
            return new QuaternionType() { X = this.X, Y = this.Y };
        }
        public QuaternionType Reciprocal()
        {
            return this.Conjugate / this.Norm;
        }
        #endregion
        #region Arithmetic Point - Point Operators
        public static PointType operator *(Quaternion<QuaternionType, PointType, R, V> left, PointType right)
        {
            return (left * new QuaternionType() { X = new R(), Y = right } * left.Reciprocal()).Y;
        }
        public static QuaternionType operator *(Quaternion<QuaternionType, PointType, R, V> left, QuaternionType right)
        {
            QuaternionType result = new QuaternionType()
            {
                X = left.X * right.X - left.Y.ScalarProduct(right.Y),
                Y = left.X * right.Y + left.Y * right.X + left.Y * right.Y
            };
            return result;
        }
        public static QuaternionType operator +(Quaternion<QuaternionType, PointType, R, V> left, QuaternionType right)
        {
            QuaternionType result = new QuaternionType()
            {
                X = left.X + right.X,
                Y = left.Y + right.Y,
            };
            return result;
        }
        public static QuaternionType operator -(Quaternion<QuaternionType, PointType, R, V> quaternion)
        {
            QuaternionType result = new QuaternionType()
            {
                X = -quaternion.X,
                Y = -quaternion.Y,
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
                X = left.X * right,
                Y = left.Y * right,
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
                X = left.X / right,
                Y = left.Y / right,
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
        #region Object overides and IEquatable<QuaternionType>
        public override bool Equals(object other)
        {
            return (other is Quaternion<QuaternionType, PointType, R, V>) && this.Equals(other as Quaternion<QuaternionType, PointType, R, V>);
        }
        // other is not null here.
        public bool Equals(Quaternion<QuaternionType, PointType, R, V> other)
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
        public static explicit operator Quaternion<QuaternionType, PointType, R, V>(R value)
        {
            return new QuaternionType() { X = value, Y = new PointType() };
        }
        #endregion
    }
}

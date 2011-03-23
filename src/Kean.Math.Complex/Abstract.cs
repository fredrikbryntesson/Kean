// 
//  Abstract.cs
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

namespace Kean.Math.Complex
{
    public abstract class Abstract<ComplexType, R, V> :
        IEquatable<ComplexType>
        where ComplexType : Abstract<ComplexType, R, V>, new()
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        public V Real { get; private set; }
        public V Imaginary { get; private set; }
        public ComplexType Conjugate {get { return new ComplexType() { Real = this.Real, Imaginary = -(R)this.Imaginary};}}
        public V Norm { get { return ((R)(this * this.Conjugate).Real).SquareRoot(); } }
        public ComplexType Inverse { get { return this.Conjugate / ((R)(this.Norm)).SquareRoot(); } }
        #region Abstract Properties
        protected ComplexType ZeroHelper { get { return this.CreateConstant(0, 0); } }
        protected ComplexType OneHelper { get { return this.CreateConstant(1, 0); } }
        protected ComplexType ImaginaryUnitHelper { get { return this.CreateConstant(0, 1); } }
        #endregion
        #region Static Constants
        public static ComplexType Zero { get { return new ComplexType().ZeroHelper; } }
        public static ComplexType One { get { return new ComplexType().OneHelper; } }
        public static ComplexType ImaginaryUnit { get { return new ComplexType().ImaginaryUnitHelper; } }
        #endregion
        #region Constructors
        protected Abstract(V real, V imaginary)
        {
            this.Real = real;
            this.Imaginary = imaginary;
        }
        #endregion
        public abstract ComplexType CreateConstant(int real, int imaginary);
        public ComplexType Copy()
        {
            return new ComplexType() { Real = this.Real, Imaginary = this.Imaginary };
        }
        #region Artihmetic Operators
        public static ComplexType operator +(Kean.Math.Complex.Abstract<ComplexType, R, V> left, ComplexType right)
        {
            return left.Add(right);
        }
        public static ComplexType operator -(Kean.Math.Complex.Abstract<ComplexType, R, V> left, ComplexType right)
        {
            return left.Substract(right);
        }
        public static ComplexType operator -(Kean.Math.Complex.Abstract<ComplexType, R, V> value)
        {
            return value.Negate();
        }
        public static ComplexType operator *(Kean.Math.Complex.Abstract<ComplexType, R, V> left, ComplexType right)
        {
            return left.Multiply(right);
        }
        public static ComplexType operator *(Kean.Math.Complex.Abstract<ComplexType, R, V> left, R right)
        {
            return left * (ComplexType)(right);
        }
        public static ComplexType operator /(Kean.Math.Complex.Abstract<ComplexType, R, V> left, ComplexType right)
        {
            return left.Divide(right);
        }
        public static ComplexType operator /(Kean.Math.Complex.Abstract<ComplexType, R, V> left, R right)
        {
            return left * (ComplexType)(Kean.Math.Abstract<R,V>.One / right);
        }
        #endregion
        #region Static Functions
        public static V Absolute(ComplexType value)
        {
            return (((Kean.Math.Abstract<R, V>)value.Real).Squared() + ((Kean.Math.Abstract<R, V>)value.Imaginary).Squared()).SquareRoot();
        }
        #endregion
        #region Functions
        #region Arithmetic Functions
        public abstract ComplexType Add(ComplexType other);
        public abstract ComplexType Substract(ComplexType other);
        public abstract ComplexType Multiply(ComplexType other);
        public abstract ComplexType Divide(ComplexType other);
        public abstract ComplexType Negate();
        #endregion
        /*
        #region Trigonometric Functions
        public abstract ComplexType Sinus();
        public abstract ComplexType Cosinus();
        public abstract ComplexType Tangens();
        #endregion
        #region Inverse Trigonometric Functions
        public abstract ComplexType ArcusSinus();
        public abstract ComplexType ArcusCosinus();
        public abstract ComplexType ArcusTangens();
        #endregion
        #region Transcendental Functions
        public abstract ComplexType Exponential();
        public abstract ComplexType Logarithm();
        #endregion
        #region Power Function
        public abstract ComplexType Power(ComplexType value);
        public abstract ComplexType SquareRoot();
        public abstract ComplexType Squared();
        #endregion
        */
        #endregion
        #region Object overides and IEquatable<R>
        public override bool Equals(object other)
        {
            return (other is ComplexType) && this.Equals(other as ComplexType);
        }
        // other is not null here.
        public bool Equals(ComplexType other)
        {
            return (R)this.Real == other.Real && (R)this.Imaginary == other.Imaginary;
        }
        public override int GetHashCode()
        {
            return this.Real.GetHashCode() ^ this.Imaginary.GetHashCode();
        }
        #endregion
        #region Comparison Functions and IComparable<R>
        public static bool operator ==(Kean.Math.Complex.Abstract<ComplexType, R, V> left, ComplexType right)
        {
            return
                object.ReferenceEquals(left, right) || (!object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null)) &&
                left.Equals(right);
        }
        public static bool operator !=(Kean.Math.Complex.Abstract<ComplexType, R, V> left, ComplexType right)
        {
            return !(left == right);
        }
        #endregion
        #region Casts
        public static explicit operator Kean.Math.Complex.Abstract<ComplexType, R, V>(R value)
        {
            return new ComplexType() { Real = value, Imaginary = new V() };
        }
        #endregion


    }
}

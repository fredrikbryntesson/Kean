// 
//  Single.cs
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
using Kean.Core.Basis.Extension;
namespace Kean.Math.Complex
{
    public struct Single
    {
        public float Real;
        public float Imaginary;
        public Single Conjugate { get { return new Single(this.Real, -this.Imaginary); } }
        public float AbsoluteValue { get { return Kean.Math.Single.SquareRoot(this.Real * this.Real + this.Imaginary * this.Imaginary); } }
        public Single(float real, float imaginary)
        {
            this.Real = real;
            this.Imaginary = imaginary;
        }
        #region Static Operators
        public static Single operator +(Single left, Single right)
        {
            return new Single(left.Real + right.Real, left.Imaginary + right.Imaginary);
        }
        public static Single operator -(Single left, Single right)
        {
            return new Single(left.Real - right.Real, left.Imaginary - right.Imaginary);
        }
        public static Single operator -(Single value)
        {
            return new Single(-value.Real, -value.Imaginary);
        }
        public static Single operator *(Single left, Single right)
        {
            return new Single(left.Real * right.Real - left.Imaginary * right.Imaginary, left.Real * right.Imaginary + left.Imaginary * right.Real);
        }
        public static Single operator *(Single left, float right)
        {
            return new Single(left.Real * right, left.Imaginary * right);
        }
        public static Single operator *(float left, Single right)
        {
            return right * left;
        }
        public static Single operator /(Single left, Single right)
        {
            return (left * right.Conjugate) / right.AbsoluteValue;
        }
        public static Single operator /(Single left, float right)
        {
            return new Single(left.Real / right, left.Imaginary / right);
        }
        #endregion
        #region Static Functions
        public static Single Exponential(Single value)
        {
            return Kean.Math.Single.Exponential(value.Real) * new Single(Kean.Math.Single.Cosinus(value.Imaginary), Kean.Math.Single.Sinus(value.Imaginary));
        }
        public static Single Logarithm(Single value)
        {
            return Kean.Math.Single.Logarithm(value.AbsoluteValue) + new Single(0, Kean.Math.Single.ArcusTangensExtended(value.Imaginary, value.Real));
        }
        public static Single RootOfUnity(int n, int k)
        {
            return Single.Exponential(new Single(0, Kean.Math.Single.Pi * 2 * k / (float)n));
        }
        #endregion
        #region Object overides and IEquatable<Single>
        public override bool Equals(object other)
        {
            return (other is Single) && this.Equals((Single)other);
        }
        // other is not null here.
        public bool Equals(Single other)
        {
            return this.Real == other.Real && this.Imaginary == other.Imaginary;
        }
        public override int GetHashCode()
        {
            return this.Real.GetHashCode() ^ this.Imaginary.GetHashCode();
        }
        #endregion
        #region Comparison Functions and IComparable<R>
        public static bool operator ==(Single left, Single right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Single left, Single right)
        {
            return !(left == right);
        }
        #endregion
        #region Casts
        public static implicit operator Single(float value)
        {
            return new Single(value, 0);
        }
        #endregion
    }
}


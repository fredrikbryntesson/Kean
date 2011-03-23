// 
//  Single.cs
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
using Kean.Core.Basis.Extension;
namespace Kean.Math
{
    public class Single :
        Abstract<Single, float>
    {
        #region Abtract Properties
        protected override Single NegativeInfinityHelper { get { return Single.NegativeInfinity; } }
        protected override Single PositiveInfinityHelper { get { return Single.PositiveInfinity; } }
        protected override Single EpsilonHelper { get { return Single.Epsilon; } }
        #endregion
        #region Constructors
        public Single() :
            base(0)
        { }
        public Single(float value) :
            base(value)
        { }
        #endregion
        public override Single CreateConstant(int value)
        {
            return new Single(value);
        }
        #region Functions
        #region Arithmetic Functions
        public override Single Add(float value)
        {
            return new Single(this.Value + value);
        }
        public override Single Substract(float value)
        {
            return new Single(this.Value - value);
        }
        public override Single Multiply(float value)
        {
            return new Single(this.Value * value);
        }
        public override Single Divide(float value)
        {
            return new Single(this.Value / value);
        }
        public override Single Negate()
        {
            return new Single(-this.Value);
        }
        #endregion
        #region Trigonometric Helpers
        public override Single ToRadians()
        {
            return Single.PI / 180 * this;
        }
        public override Single ToDegrees()
        {
            return 180 / Single.PI * this;
        }
        #endregion
        #region Trigonometric Functions
        public override Single Sinus()
        {
            return Single.Sinus(this.Value);
        }
        public override Single Cosinus()
        {
            return Single.Cosinus(this.Value);
        }
        public override Single Tangens()
        {
            return Single.Tangens(this.Value);
        }
        #endregion
        #region Inverse Trigonometric Functions
        public override Single ArcusSinus()
        {
            return Single.ArcusSinus(this.Value);
        }
        public override Single ArcusCosinus()
        {
            return Single.ArcusCosinus(this.Value);
        }
        public override Single ArcusTangens()
        {
            return Single.ArcusTangens(this.Value);
        }
        public override Single ArcusTangensExtended(Single x)
        {
            return Single.ArcusTangensExtended(this.Value, x);
        }
        #endregion
        #region Transcendental Functions
        public override Single Exponential()
        {
            return Single.Exponential(this.Value);
        }
        public override Single Logarithm()
        {
            return Single.Logarithm(this.Value);
        }
        #endregion
        #region Power Function
        public override Single Power(Single exponent)
        {
            return Single.Power(this.Value, exponent);
        }
        public override Single SquareRoot()
        {
            return Single.SquareRoot(this.Value);
        }
        public override Single Squared()
        {
            return this.Value * this.Value;
        }
        #endregion
        #region Comparison Functions
        public override bool LessThan(Single other)
        {
            return this.Value < other.Value;
        }
        public override bool GreaterThan(Single other)
        {
            return this.Value > other.Value;
        }
        #endregion
        #endregion
        #region Cast Operators
        public static implicit operator float(Single value)
        {
            return value.IsNull() ? 0 : value.Value;
        }
        public static implicit operator Single(float value)
        {
            return new Single(value);
        }
        #endregion
        #region Static Functions
        #region Properties
        public static float NegativeInfinity { get { return float.NegativeInfinity; } }
        public static float PositiveInfinity { get { return float.PositiveInfinity; } }
        public static float Epsilon { get { return float.Epsilon; } }
        public static float MinimumValue { get { return float.MinValue; } }
        public static float MaximumValue { get { return float.MaxValue; } }
        public static float PI { get { return (float)System.Math.PI; } }
        #endregion
        #region Trigonometric Functions
        public static float Sinus(float value)
        {
            return (float)System.Math.Sin(value);
        }
        public static float Cosinus(float value)
        {
            return (float)System.Math.Cos(value);
        }
        public static float Tangens(float value)
        {
            return (float)System.Math.Tan(value);
        }
        #endregion
        #region Inverse Trigonometric Functions
        public static float ArcusSinus(float value)
        {
            return (float)System.Math.Asin(value);
        }
        public static float ArcusCosinus(float value)
        {
            return (float)System.Math.Acos(value);
        }
        public static float ArcusTangens(float value)
        {
            return (float)System.Math.Atan(value);
        }
        public static float ArcusTangensExtended(float y, float x)
        {
            return (float)System.Math.Atan2(y, x);
        }
        #endregion
        #region Transcendental Functions
        public static float Exponential(float value)
        {
            return (float)System.Math.Exp(value);
        }
        public static float Logarithm(float value)
        {
            return (float)System.Math.Log(value);
        }
        #endregion
        #region Power Function
        public static float Power(float @base, float exponent)
        {
            return (float)System.Math.Pow(@base, exponent);
        }
        public static float SquareRoot(float value)
        {
            return (float)System.Math.Sqrt(value);
        }
        #endregion
        #endregion
        #region Object overides
        public override string ToString()
        {
            return this.Value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        #endregion
    }
}


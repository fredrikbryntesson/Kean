// 
//  Integer.cs
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
    public class Integer :
        Abstract<Integer, int>
    {
        #region Abtract Properties
        protected override Integer NegativeInfinityHelper { get { return Integer.NegativeInfinity; } }
        protected override Integer PositiveInfinityHelper { get { return Integer.PositiveInfinity; } }
        protected override Integer EpsilonHelper { get { return Integer.Epsilon; } }
        #endregion
        #region Constructors
        public Integer() :
            base(0)
        { }
        public Integer(int value) :
            base(value)
        { }
        #endregion
        public override Integer CreateConstant(int value)
        {
            return new Integer(value);
        }
        #region Functions
        #region Arithmetic Functions
        public override Integer Add(int value)
        {
            return new Integer(this.Value + value);
        }
        public override Integer Substract(int value)
        {
            return new Integer(this.Value - value);
        }
        public override Integer Multiply(int value)
        {
            return new Integer(this.Value * value);
        }
        public override Integer Divide(int value)
        {
            return new Integer(this.Value / value);
        }
        public override Integer Negate()
        {
            return new Integer(-this.Value);
        }
        #endregion
        #region Trigonometric Helpers
        public override Integer ToRadians()
        {
            return Integer.PI / 180 * this;
        }
        public override Integer ToDegrees()
        {
            return 180 / Integer.PI * this;
        }
        #endregion
        
        #region Trigonometric Functions
        public override Integer Sinus()
        {
            return Integer.Sinus(this.Value);
        }
        public override Integer Cosinus()
        {
            return Integer.Cosinus(this.Value);
        }
        public override Integer Tangens()
        {
            return Integer.Tangens(this.Value);
        }
        #endregion
        #region Inverse Trigonometric Functions
        public override Integer ArcusSinus()
        {
            return Integer.ArcusSinus(this.Value);
        }
        public override Integer ArcusCosinus()
        {
            return Integer.ArcusCosinus(this.Value);
        }
        public override Integer ArcusTangens()
        {
            return Integer.ArcusTangens(this.Value);
        }
        public override Integer ArcusTangensExtended(Integer x)
        {
            return Integer.ArcusTangensExtended(this.Value, x);
        }
        #endregion
        #region Transcendental Functions
        public override Integer Exponential()
        {
            return Integer.Exponential(this.Value);
        }
        public override Integer Logarithm()
        {
            return Integer.Logarithm(this.Value);
        }
        #endregion
        #region Power Function
        public override Integer Power(Integer exponent)
        {
            return Integer.Power(this.Value, exponent);
        }
        public override Integer SquareRoot()
        {
            return Integer.SquareRoot(this.Value);
        }
        public override Integer Squared()
        {
            return this.Value * this.Value;
        }
        #endregion
        #region Comparison Functions
        public override bool LessThan(Integer other)
        {
            return this.Value < other.Value;
        }
        public override bool GreaterThan(Integer other)
        {
            return this.Value > other.Value;
        }
        #endregion
        #endregion
        #region Cast Operators
        public static implicit operator int(Integer value)
        {
            return value.IsNull() ? 0 : value.Value;
        }
        public static implicit operator Integer(int value)
        {
            return new Integer(value);
        }
        #endregion
        #region Static Functions
        #region Properties
        public static int NegativeInfinity { get { return int.MaxValue; } }
        public static int PositiveInfinity { get { return int.MinValue; } }
        public static int Epsilon { get { return 1; } }
        public static int MinimumValue { get { return int.MinValue; } }
        public static int MaximumValue { get { return int.MaxValue; } }
        public static int PI { get { return (int)System.Math.PI; } }
        #endregion
        #region Trigonometric Functions
        public static int Sinus(int value)
        {
            return (int)System.Math.Sin(value);
        }
        public static int Cosinus(int value)
        {
            return (int)System.Math.Cos(value);
        }
        public static int Tangens(int value)
        {
            return (int)System.Math.Tan(value);
        }
        #endregion
        #region Inverse Trigonometric Functions
        public static int ArcusSinus(int value)
        {
            return (int)System.Math.Asin(value);
        }
        public static int ArcusCosinus(int value)
        {
            return (int)System.Math.Acos(value);
        }
        public static int ArcusTangens(int value)
        {
            return (int)System.Math.Atan(value);
        }
        public static int ArcusTangensExtended(int y, int x)
        {
            return (int)System.Math.Atan2(y, x);
        }
        #endregion
        #region Transcendental Functions
        public static int Exponential(int value)
        {
            return (int)System.Math.Exp(value);
        }
        public static int Logarithm(int value)
        {
            return (int)System.Math.Log(value);
        }
        #endregion
        #region Power Function
        public static int Power(int @base, int exponent)
        {
            return (int)System.Math.Pow(@base, exponent);
        }
        public static int SquareRoot(int value)
        {
            return (int)System.Math.Sqrt(value);
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



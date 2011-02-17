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
    public class Integer :
        Abstract<Integer, int>
    {
        #region Constants
        public override Integer Zero { get { return 0; } }
        public override Integer One { get { return 1; } }
        public override Integer Two { get { return 2; } }
        #endregion
        #region Constructors
        public Integer() :
            base(0)
        { }
        public Integer(int value) :
            base(value)
        { }
        #endregion
        #region Functions
        #region Arithmetic Functions
        public override Integer Add(Integer value)
        {
            return this + value;
        }
        public override Integer Substract(Integer value)
        {
            return this - value;
        }
        public override Integer Multiply(Integer value)
        {
            return this * value;
        }
        public override Integer Divide(Integer value)
        {
            return this / value;
        }
        #endregion
        #region Trigometric Functions
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
        #region Inverse Trigometric Functions
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
        public static int MinimumValue { get { return int.MinValue; } }
        public static int MaximumValue { get { return int.MaxValue; } }
        public static int PI { get { return (int)System.Math.PI; } }
        #endregion
        #region Trigometric Functions
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
        #region Inverse Trigometric Functions
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
    }
}


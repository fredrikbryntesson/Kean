// 
//  Double.cs
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
	public class Double :
		Abstract<Double, double>
	{
        #region Abtract Properties
        protected override Double NegativeInfinityHelper { get { return Double.NegativeInfinity; } }
        protected override Double PositiveInfinityHelper { get { return Double.PositiveInfinity; } }
        protected override Double EpsilonHelper { get { return Double.Epsilon; } }
        #endregion
        #region Constructors
		public Double() :
			base(0)
		{ }
		public Double(double value) :
			base(value)
		{ }
		#endregion
        public override Double CreateConstant(int value)
        {
            return new Double(value);
        }
   		#region Functions
		#region Arithmetic Functions
		public override Double Add(double value)
		{
			return new Double(this.Value + value);
		}
        public override Double Substract(double value)
		{
			return new Double(this.Value - value);
		}
        public override Double Multiply(double value)
		{
			return new Double(this.Value * value);
		}
        public override Double Divide(double value)
		{
			return new Double(this.Value / value);
		}
        public override Double Negate()
        {
            return new Double(-this.Value);
        }
		#endregion
		#region Trigometric Functions
		public override Double Sinus()
		{
			return Double.Sinus(this.Value);
		}
		public override Double Cosinus()
		{
			return Double.Cosinus(this.Value);
		}
		public override Double Tangens()
		{
			return Double.Tangens(this.Value);
        }
        #endregion
        #region Inverse Trigometric Functions
        public override Double ArcusSinus()
        {
            return Double.ArcusSinus(this.Value);
        }
        public override Double ArcusCosinus()
        {
            return Double.ArcusCosinus(this.Value);
        }
        public override Double ArcusTangens()
        {
            return Double.ArcusTangens(this.Value);
        }
        public override Double ArcusTangensExtended(Double x)
        {
            return Double.ArcusTangensExtended(this.Value, x);
        }
		#endregion
        #region Transcendental Functions
        public override Double Exponential()
        {
            return Double.Exponential(this.Value);
        }
        public override Double Logarithm()
        {
            return Double.Logarithm(this.Value);
        }
        #endregion
        #region Power Function
        public override Double Power(Double exponent)
        {
            return Double.Power(this.Value, exponent);
        }
        public override Double SquareRoot()
        {
            return Double.SquareRoot(this.Value);
        }
        public override Double Squared()
        {
            return this.Value * this.Value;
        }
        #endregion
		#region Comparison Functions
		public override bool LessThan(Double other)
		{
			return this.Value < other.Value;
		}
		public override bool GreaterThan(Double other)
		{
			return this.Value > other.Value;
		}
		#endregion
		#endregion
        #region Cast Operators
        public static implicit operator double(Double value)
		{
			return value.IsNull() ? 0 : value.Value;
		}
		public static implicit operator Double(double value)
		{
			return new Double(value);
		}
		#endregion
		#region Static Functions
		#region Properties
		public static double NegativeInfinity { get { return double.NegativeInfinity; } }
		public static double PositiveInfinity { get { return double.PositiveInfinity; } }
		public static double Epsilon { get { return double.Epsilon; } }
		public static double MinimumValue { get { return double.MinValue; } }
		public static double MaximumValue { get { return double.MaxValue; } }
		public static double PI { get { return (double)System.Math.PI; } }
		#endregion
		#region Trigometric Functions
		public static double Sinus(double value)
		{
			return System.Math.Sin(value);
		}
		public static double Cosinus(double value)
		{
			return System.Math.Cos(value);
		}
		public static double Tangens(double value)
		{
			return System.Math.Tan(value);
		}
	    #endregion
        #region Inverse Trigometric Functions
        public static double ArcusSinus(double value)
        {
            return System.Math.Asin(value);
        }
        public static double ArcusCosinus(double value)
        {
            return System.Math.Acos(value);
        }
        public static double ArcusTangens(double value)
        {
            return System.Math.Atan(value);
        }
        public static double ArcusTangensExtended(double y, double x)
        {
            return System.Math.Atan2(y, x);
        }
	    #endregion
        #region Transcendental Functions
        public static double Exponential(double value)
        {
            return System.Math.Exp(value);
        }
        public static double Logarithm(double value)
        {
            return System.Math.Log(value);
        }
	    #endregion
        #region Power Function
        public static double Power(double @base, double exponent)
        {
            return System.Math.Pow(@base, exponent);
        }
        public static double SquareRoot(double value)
        {
            return System.Math.Sqrt(value);
        }
        #endregion
        #endregion
    }
}


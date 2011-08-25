// 
//  Integer.Function.cs
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
using Kean.Core.Extension;
namespace Kean.Math
{
    public partial class UnsignedLong
    {
        #region Constants
        public static uint NegativeInfinity { get { return uint.MinValue; } }
        public static uint PositiveInfinity { get { return uint.MaxValue; } }
        public static uint Epsilon { get { return 1; } }
        public static uint MinimumValue { get { return uint.MinValue; } }
        public static uint MaximumValue { get { return uint.MaxValue; } }
        public static uint Pi { get { return UnsignedLong.Convert(System.Math.PI); } }
        public static uint E { get { return UnsignedLong.Convert(System.Math.E); } }
        #endregion
        #region Convert Functions
        public static uint Convert(double value)
        {
            return System.Convert.ToUInt32(value);
        }
        public static uint Convert(float value)
        {
            return System.Convert.ToUInt32(value);
        }
        /// <summary>
        /// Parses a string to a uint
        /// </summary>
        /// <exception cref="System.FormatException">When string does not contain a int</exception>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint Parse(string value)
        {
            return uint.Parse(value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        public static string ToString(uint value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        #endregion
        #region Utility Functions
        public static uint Clamp(uint value, uint floor, uint ceiling)
        {
            if (value > ceiling)
                value = ceiling;
            else if (value < floor)
                value = floor;
            return value;
        }
        public static uint Maximum(uint first, uint second)
        {
            return first > second ? first : second;
        }
        public static uint Maximum(uint value, params uint[] values)
        {
            foreach (uint v in values)
                if (value < v)
                    value = v;
            return value;
        }
        public static uint Minimum(uint first, uint second)
        {
            return first < second ? first : second;
        }
        public static uint Minimum(uint value, params uint[] values)
        {
            foreach (uint v in values)
                if (value > v)
                    value = v;
            return value;
        }
        public static uint Modulo(uint dividend, uint divisor)
        {
            if (dividend < 0)
                dividend += UnsignedLong.Ceiling(dividend / (float)divisor) * divisor;
            return dividend % divisor;
        }
        #endregion
        #region Rounding Functions
        public static uint Floor(float value)
        {
            return UnsignedLong.Convert(System.Math.Floor(value));
        }
        public static uint Floor(double value)
        {
            return UnsignedLong.Convert(System.Math.Floor(value));
        }
        public static uint Ceiling(float value)
        {
            return UnsignedLong.Convert(System.Math.Ceiling(value));
        }
        public static uint Ceiling(double value)
        {
            return UnsignedLong.Convert(System.Math.Ceiling(value));
        }
        public static uint Truncate(float value)
        {
            return UnsignedLong.Convert(System.Math.Truncate(value));
        }
        public static uint Truncate(double value)
        {
            return UnsignedLong.Convert(System.Math.Truncate(value));
        }
        public static uint Round(float value)
        {
            return UnsignedLong.Convert(System.Math.Round(value));
        }
        public static uint Round(double value)
        {
            return UnsignedLong.Convert(System.Math.Round(value));
        }
        #endregion
        #region Transcendental and Power Functions
        public static uint Exponential(uint value)
        {
            return UnsignedLong.Convert(System.Math.Exp(value));
        }
        public static uint Logarithm(uint value)
        {
            return UnsignedLong.Convert(System.Math.Log(value));
        }
        public static uint Logarithm(uint value, uint @base)
        {
            return UnsignedLong.Convert(System.Math.Log(value, @base));
        }
        public static uint Power(uint @base, uint exponent)
        {
            return UnsignedLong.Convert(System.Math.Pow(@base, exponent));
        }
        public static uint SquareRoot(uint value)
        {
            return UnsignedLong.Convert(System.Math.Sqrt(value));
        }
        public static uint Squared(uint value)
        {
            return value * value;
        }
        #endregion
    }
}

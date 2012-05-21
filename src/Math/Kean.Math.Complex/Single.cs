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
using Kean.Core.Extension;
namespace Kean.Math.Complex
{
    public struct Single
    {
        public float Real;
        public float Imaginary;
        public Single Conjugate { get { return new Single(this.Real, -this.Imaginary); } }
        public float AbsoluteValue { get { return Kean.Math.Single.SquareRoot(Kean.Math.Single.Squared(this.Real) + Kean.Math.Single.Squared(this.Imaginary)); } }
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
        public static Single operator *(float left, Single right)
        {
            return new Single(left * right.Real, left * right.Imaginary);
        }
        public static Single operator /(Single left, float right)
        {
            return new Single(left.Real / right, left.Imaginary / right);
        }
        public static Single operator /(Single left, Single right)
        {
            return (left * right.Conjugate) / Kean.Math.Single.Squared(right.AbsoluteValue);
        }
        #endregion
        #region Fourier
        /// <summary>
        /// Discrete Fourier transform. Input array of arbitrary size.
        /// </summary>
        /// <param name="input">Input array to be transformed.</param>
        /// <returns>Output Fourier transformed array.</returns>
        public static Single[] DiscreteTransform(Single[] input)
        {
            Single[] result = new Single[input.Length];
            if (input.Length > 0)
            {
                for (int i = 0; i < input.Length; i++)
                    for (int j = 0; j < input.Length; j++)
                        result[i] += input[j] * Single.RootOfUnity(input.Length, -i * j);
            }
            return result;
        }
        /// <summary>
        /// Inverse discrete Fourier transform. Input array of arbitrary size. 
        /// </summary>
        /// <param name="input">Input array to be transformed.</param>
        /// <returns>Output Fourier transformed array.</returns>
        public static Single[] InverseDiscreteTransform(Single[] input)
        {
            return input.Length > 0 ? Single.DiscreteTransform(input.Map(c => c.Conjugate)).Map(c => c.Conjugate / (float)input.Length) : new Single[0];
        }
        /// <summary>
        /// Fast Fourier transform. Input array must have a length which is a power of 2.
        /// </summary>
        /// <param name="input">Input array to be transformed.</param>
        /// <returns>Output Fourier transformed array.</returns>
        public static Single[] FastTransform(Single[] input)
        {
            Single[] result = new Single[input.Length];
            if (input.Length == 1)
                result[0] = input[0];
            else if (input.Length > 1)
            {
                int halfLength = input.Length / 2;
                Single[] evenInput = new Single[halfLength];
                for (int i = 0; i < evenInput.Length; i++)
                    evenInput[i] = input[2 * i];
                Single[] oddInput = new Single[halfLength];
                for (int i = 0; i < oddInput.Length; i++)
                    oddInput[i] = input[2 * i + 1];
                Single[] evenOutput = Single.FastTransform(evenInput);
                Single[] oddOutput = Single.FastTransform(oddInput);
                for (int i = 0; i < halfLength; i++)
                {
                    Single root = Single.RootOfUnity(input.Length, -i);
                    result[i] = evenOutput[i] + root * oddOutput[i];
                    result[halfLength + i] = evenOutput[i] - root * oddOutput[i];
                }
            }
            return result;
        }
        /// <summary>
        /// Inverse fast Fourier transform. nput array must have a length which is a power of 2.
        /// </summary>
        /// <param name="input">Input array to be transformed.</param>
        /// <returns>Output Fourier transformed array.</returns>
        public static Single[] InverseFastTransform(Single[] input)
        {
            return input.Length > 0 ? Single.FastTransform(input.Map(c => c.Conjugate)).Map(c => c.Conjugate / (float)input.Length) : new Single[0];
        }
        #endregion
        #region Static Functions
        public static Single Exponential(Single value)
        {
            return Kean.Math.Single.Exponential(value.Real) * new Single(Kean.Math.Single.Cosinus(value.Imaginary), Kean.Math.Single.Sinus(value.Imaginary));
        }
        public static Single Logarithm( Single value)
        {
            return Kean.Math.Single.Logarithm(value.AbsoluteValue) + new Single(0, Kean.Math.Single.ArcusTangensExtended(value.Imaginary, value.Real));
        }
        public static Single RootOfUnity(int n)
        {
            return Single.RootOfUnity(n, 1);
        }
        public static Single RootOfUnity(int n, int k)
        {
            return Single.Exponential(new Single(0, 2 * k * Kean.Math.Single.Pi / n));
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
        public bool Equals(Single other, double tolerance)
        {
            return Kean.Math.Single.Absolute(this.Real - other.Real) < tolerance && Kean.Math.Single.Absolute(this.Imaginary - other.Imaginary) < tolerance;
        }
        public override int GetHashCode()
        {
            return this.Real.GetHashCode() ^ this.Imaginary.GetHashCode();
        }
        public override string ToString()
        {
            return Kean.Math.Single.ToString(this.Real) + " " + (Kean.Math.Single.Sign(this.Imaginary) >= 0 ? "+" : "") + Kean.Math.Double.ToString(this.Imaginary) + "i";
        }
        #endregion
        #region Comparison Functions and IComparable<Single>
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
        public static implicit operator string(Single value)
        {
            return value.ToString();
        }
        public static implicit operator Single(string value)
        {
            Single result = new Single();
            if (value.NotEmpty())
            {
                try
                {
                    value = value.ToLower().Replace(" ", "").Replace("f", "").Replace("\t", "");
                    if (!value.Contains("i"))
                        result = Kean.Math.Single.Parse(value);
                    else
                    {
                        string[] values = value.Split(new char[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length == 1)
                            result = new Single(0, Kean.Math.Single.Parse(value.Trim('i', '*')));
                        else
                        {
                            string real = null;
                            string imaginary = null;
                            if (values[0].Contains("i"))
                            {
                                int index = value.IndexOf(values[0]);
                                char sign = index >= 1 && value[index - 1] == '-' ? '-' : '+';
                                imaginary = sign + values[0].Trim('i', '*');
                                index = value.IndexOf(values[1]);
                                sign = index >= 1 && value[index - 1] == '-' ? '-' : '+';
                                real = sign + values[1];
                            }
                            else if (values[1].Contains("i"))
                            {
                                int index = value.IndexOf(values[1]);
                                char sign = index >= 1 && value[index - 1] == '-' ? '-' : '+';
                                imaginary = sign + values[1].Trim('i', '*');
                                index = value.IndexOf(values[0]);
                                sign = index >= 1 && value[index - 1] == '-' ? '-' : '+';
                                real = sign + values[0];
                            }
                            if (real.NotEmpty() && imaginary.NotEmpty())
                                result = new Single(Kean.Math.Single.Parse(real), Kean.Math.Single.Parse(imaginary));
                        }
                    }
                }
                catch
                { }
            }
            return result;
        }
        #endregion
    }
}

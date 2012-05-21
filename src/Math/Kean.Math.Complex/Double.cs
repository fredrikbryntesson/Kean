// 
//  Double.cs
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
    public struct Double
    {
        public double Real;
        public double Imaginary;
        public Double Conjugate { get { return new Double(this.Real, -this.Imaginary); } }
        public double AbsoluteValue { get { return Kean.Math.Double.SquareRoot(Kean.Math.Double.Squared(this.Real) + Kean.Math.Double.Squared(this.Imaginary)); } }
        public Double(double real, double imaginary)
        {
            this.Real = real;
            this.Imaginary = imaginary;
        }
        #region Static Operators
        public static Double operator +(Double left, Double right)
        {
            return new Double(left.Real + right.Real, left.Imaginary + right.Imaginary);
        }
        public static Double operator -(Double left, Double right)
        {
            return new Double(left.Real - right.Real, left.Imaginary - right.Imaginary);
        }
        public static Double operator -(Double value)
        {
            return new Double(-value.Real, -value.Imaginary);
        }
        public static Double operator *(Double left, Double right)
        {
            return new Double(left.Real * right.Real - left.Imaginary * right.Imaginary, left.Real * right.Imaginary + left.Imaginary * right.Real);
        }
        public static Double operator *(double left, Double right)
        {
            return new Double(left * right.Real, left * right.Imaginary);
        }
        public static Double operator /(Double left, double right)
        {
            return new Double(left.Real / right, left.Imaginary / right);
        }
        public static Double operator /(Double left, Double right)
        {
            return (left * right.Conjugate) / Kean.Math.Double.Squared(right.AbsoluteValue);
        }
        #endregion
        #region Fourier
        /// <summary>
        /// Discrete Fourier transform. Input array of arbitrary size.
        /// </summary>
        /// <param name="input">Input array to be transformed.</param>
        /// <returns>Output Fourier transformed array.</returns>
        public static Double[] DiscreteTransform(Double[] input)
        {
            Double[] result = new Double[input.Length];
            if (input.Length > 0)
            {
                for (int i = 0; i < input.Length; i++)
                    for (int j = 0; j < input.Length; j++)
                        result[i] += input[j] * Double.RootOfUnity(input.Length, -i * j);
            }
            return result;
        }
        /// <summary>
        /// Inverse discrete Fourier transform. Input array of arbitrary size. 
        /// </summary>
        /// <param name="input">Input array to be transformed.</param>
        /// <returns>Output Fourier transformed array.</returns>
        public static Double[] InverseDiscreteTransform(Double[] input)
        {
            return input.Length > 0 ? Double.DiscreteTransform(input.Map(c => c.Conjugate)).Map(c => c.Conjugate / (double)input.Length) : new Double[0];
        }
        /// <summary>
        /// Fast Fourier transform. Input array must have a length which is a power of 2.
        /// </summary>
        /// <param name="input">Input array to be transformed.</param>
        /// <returns>Output Fourier transformed array.</returns>
        public static Double[] FastTransform(Double[] input)
        {
            Double[] result = new Double[input.Length];
            if (input.Length == 1)
                result[0] = input[0];
            else if (input.Length > 1)
            {
                int halfLength = input.Length / 2;
                Double[] evenInput = new Double[halfLength];
                for (int i = 0; i < evenInput.Length; i++)
                    evenInput[i] = input[2 * i];
                Double[] oddInput = new Double[halfLength];
                for (int i = 0; i < oddInput.Length; i++)
                    oddInput[i] = input[2 * i + 1];
                Double[] evenOutput = Double.FastTransform(evenInput);
                Double[] oddOutput = Double.FastTransform(oddInput);
                for (int i = 0; i < halfLength; i++)
                {
                    Double root = Double.RootOfUnity(input.Length, -i);
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
        public static Double[] InverseFastTransform(Double[] input)
        {
            return input.Length > 0 ? Double.FastTransform(input.Map(c => c.Conjugate)).Map(c => c.Conjugate / (double)input.Length) : new Double[0];
        }
        #endregion
        #region Static Functions
        public static Double Exponential(Double value)
        {
            return Kean.Math.Double.Exponential(value.Real) * new Double(Kean.Math.Double.Cosinus(value.Imaginary), Kean.Math.Double.Sinus(value.Imaginary));
        }
        public static Double Logarithm( Double value)
        {
            return Kean.Math.Double.Logarithm(value.AbsoluteValue) + new Double(0, Kean.Math.Double.ArcusTangensExtended(value.Imaginary, value.Real));
        }
        public static Double RootOfUnity(int n)
        {
            return Double.RootOfUnity(n, 1);
        }
        public static Double RootOfUnity(int n, int k)
        {
            return Double.Exponential(new Double(0, 2 * k * Kean.Math.Double.Pi / n));
        }
        #endregion
        #region Object overides and IEquatable<Double>
        public override bool Equals(object other)
        {
            return (other is Double) && this.Equals((Double)other);
        }
        // other is not null here.
        public bool Equals(Double other)
        {
            return this.Real == other.Real && this.Imaginary == other.Imaginary;
        }
        public bool Equals(Double other, double tolerance)
        {
            return Kean.Math.Double.Absolute(this.Real - other.Real) < tolerance && Kean.Math.Double.Absolute(this.Imaginary - other.Imaginary) < tolerance;
        }
        public override int GetHashCode()
        {
            return this.Real.GetHashCode() ^ this.Imaginary.GetHashCode();
        }
        public override string ToString()
        {
            return Kean.Math.Double.ToString(this.Real) + " " + (Kean.Math.Double.Sign(this.Imaginary) >= 0 ? "+" : "") + Kean.Math.Double.ToString(this.Imaginary) + "i";
        }
        #endregion
        #region Comparison Functions and IComparable<Double>
        public static bool operator ==(Double left, Double right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Double left, Double right)
        {
            return !(left == right);
        }
        #endregion
        #region Casts
        public static implicit operator Double(double value)
        {
            return new Double(value, 0);
        }
        public static implicit operator string(Double value)
        {
            return value.ToString();
        }
        public static implicit operator Double(string value)
        {
            Double result = new Double();
            if (value.NotEmpty())
            {
                try
                {
                    value = value.ToLower().Replace(" ", "").Replace("f", "").Replace("\t", "");
                    if (!value.Contains("i"))
                        result = Kean.Math.Double.Parse(value);
                    else
                    {
                        string[] values = value.Split(new char[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length == 1)
                            result = new Double(0, Kean.Math.Double.Parse(value.Trim('i', '*')));
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
                                result = new Double(Kean.Math.Double.Parse(real), Kean.Math.Double.Parse(imaginary));
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

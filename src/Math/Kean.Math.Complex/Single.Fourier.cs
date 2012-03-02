// 
//  Fourier.cs
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
using Kean.Core.Collection.Extension;

namespace Kean.Math.Complex
{
    public partial struct Single
    {
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
            return input.Length > 0 ? Single.DiscreteTransform(input.Map(c => c.Conjugate)).Map(c => c.Conjugate / input.Length) : new Single[0];
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
            return input.Length > 0 ? Single.FastTransform(input.Map(c => c.Conjugate)).Map(c => c.Conjugate / input.Length) : new Single[0];
        }
    }
}

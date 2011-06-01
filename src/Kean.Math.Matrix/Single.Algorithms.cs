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
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Core.Basis.Extension;

namespace Kean.Math.Matrix
{
    public partial class Single
    {
        /// <summary>
        /// Cholesky factorization of positive symmetric matrix. A = C * C'.  The matrix C is lower triangular.
        /// </summary>
        /// <returns> Cholesky factorization matix.</returns>
        public Single Cholesky()
        {
            if (!this.IsSquare)
                new Exception.InvalidDimensions();
            int order = this.Dimensions.Width;
            Single result = Single.Identity(order);
            Single[] l = new Single[order];
            Single working = this;
            for (int i = 0; i < order - 1; i++)
            {
                float a = working[0, 0];
                float aSquareRoot = Kean.Math.Single.SquareRoot(a);
                Single b = working.Extract(new Geometry2D.Integer.Box(0, 1, 1, working.Dimensions.Height - 1));
                l[i] = Single.Identity(order);
                l[i][i, i] = aSquareRoot;
                l[i].Paste(new Geometry2D.Integer.Point(i, i + 1), (1 / aSquareRoot) * b);
                Single corner = working.Extract(new Geometry2D.Integer.Box(1, 1, working.Dimensions.Width - 1, working.Dimensions.Height - 1));
                working = corner - (1 / a) * b * b.Transpose();
            }
            l[order - 1] = Single.Identity(order);
            l[order - 1][order - 1, order - 1] = Kean.Math.Single.SquareRoot(working[0, 0]);
            for (int i = 0; i < l.Length; i++)
                result *= l[i];
            return result;
        }
        /// <summary>
        /// Forward solver lower * x = y. Current object is y. 
        /// </summary>
        /// <param name="lower">Lower triangual matrix.</param>
        /// <returns>Solution x.</returns>
        public Single ForwardSubstituion(Single lower)
        {
            Single result = new Single(this.Dimensions);
            for (int x = 0; x < this.Dimensions.Width; x++)
            {
                for (int y = 0; y < this.Dimensions.Height; y++)
                {
                    float accumulator = this[x, y];
                    for (int x2 = 0; x2 < y; x2++)
                        accumulator -= lower[x2, y] * result[x, x2];
                    if (lower[y, y] != 0)
                        result[x, y] = accumulator / lower[y, y];
                    else
                        throw new Exception.DivisionByZero();
                }
            }
            return result;
        }
        /// <summary>
        /// Backward solver upper * x = y. Current object is y. 
        /// </summary>
        /// <param name="lower">Upper triangual matrix.</param>
        /// <returns>Solution x.</returns>
        public Single BackwardSubstituion(Single upper)
        {
            Single result = new Single(this.Dimensions);
            for (int x = 0; x < this.Dimensions.Width; x++)
            {
                for (int y = this.Dimensions.Height - 1; y >= 0; y--)
                {
                    float accumulator = this[x, y];
                    for (int x2 = y + 1; x2 < upper.Dimensions.Width; x2++)
                        accumulator -= upper[x2, y] * result[x, x2];
                    if (upper[y, y] != 0)
                        result[x, y] = accumulator / upper[y, y];
                    else
                        throw new Exception.DivisionByZero();
                }
            }
            return result;
        }
        public Single LeastSquare(Single y)
        {
            if (this.Dimensions.Width > this.Dimensions.Height || this.Dimensions.Height != y.Dimensions.Height)
                throw new Exception.InvalidDimensions();
            Single transpose = this.Transpose();
            Single lower = (transpose * this).Cholesky();
            Single z = (transpose * y).ForwardSubstituion(lower);
            return z.BackwardSubstituion(lower.Transpose());
        }
    }
}

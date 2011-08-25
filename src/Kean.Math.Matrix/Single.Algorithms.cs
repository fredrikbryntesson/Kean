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
using Kean.Core.Extension;

namespace Kean.Math.Matrix
{
    public partial class Single
    {
        /// <summary>
        /// Return true if the current matrix is a diagonal matrix within the specified tolerance.
        /// </summary>
        /// <param name="tolerance">Tolerance used to check zero elements outside the diagonal.</param>
        /// <returns>True is the matrix is a diagonal matrix within the given tolerance.</returns>
        public bool IsDiagonal(float tolerance)
        {
            bool result = true;
            for (int x = 0; x < this.Dimensions.Width; x++)
                for (int y = 0; y < this.Dimensions.Height; y++)
                {
                    if (x != y && Kean.Math.Single.Absolute(this[x, y]) > tolerance)
                    {
                        result = false;
                        x = this.Dimensions.Width;
                        break;
                    }
                }
            return result;
        }
        /// <summary>
        /// Trace of the current matrix.
        /// </summary>
        /// <returns>Return the trace of the current matrix.</returns>
        public float Trace()
        {
            float result = 0;
            int order = this.Order;
            for (int i = 0; i < order; i++)
                result += this[i, i];
            return result;
        }
        #region Cholesky Methods
        /// <summary>
        /// Gaxby (Algorithm 4.2.1 p.144) Cholesky factorization of positive symmetric matrix. A = C * C'.  The matrix C is lower triangular.
        /// </summary>
        /// <returns> Cholesky factorization matix.</returns>
        public Single Cholesky()
        {
            if (!this.IsSquare)
                new Exception.InvalidDimensions();
            int order = this.Dimensions.Width;
            Single result = this.Copy();
            for (int j = 0; j < order; j++)
            {
                if (j > 0)
                    result.Set(j, j, result.Extract(j, j + 1, j, order) - result.Extract(0, j, j, order) * result.Extract(0, j, j, j + 1).Transpose());
                float value = result[j, j];
                if (value <= 0)
                    throw new Exception.NonPositive();
                result.Set(j, j, result.Extract(j, j + 1, j, order) / Kean.Math.Single.SquareRoot(value));
            }
            for (int y = 0; y < order; y++)
                for (int x = y + 1; x < order; x++)
                    result[x, y] = 0;
            return result;
        }
        /// <summary>
        /// Cholesky least square solver A * x = y. See http://en.wikipedia.org/wiki/Cholesky_decomposition
        /// The current matrix determines the matrix A above.
        /// </summary>
        /// <param name="y">The right hand column y vector of the equation system.</param>
        /// <returns>Return the least square solution to the system,</returns>
        public Single SolveCholesky(Single y)
        {
            Single result = null;
            try
            {
                if (this.Dimensions.Height < this.Dimensions.Width)
                {
                    // Least norm
                    Single transpose = this.Transpose();
                    Single lower = (this * transpose).Cholesky();
                    Single z = y.ForwardSubstitution(lower);
                    z = z.BackwardSubstitution(lower.Transpose());
                    result = transpose * z;
                }
                else
                {
                    // Standard
                    Single transpose = this.Transpose();
                    Single lower = (transpose * this).Cholesky();
                    Single z = (transpose * y).ForwardSubstitution(lower);
                    result = z.BackwardSubstitution(lower.Transpose());
                }
            }
            catch (Kean.Core.Error.Exception e)
            {
            }
            return result;
        }
        #endregion
       
        #region Lup Methods
        /// <summary>
        /// See http://en.wikipedia.org/wiki/LUP_decomposition.
        /// Lup decomposition of the current matrix. Recall that Lup decomposition is A = LUP, 
        /// where L is lower triangular, U is upper triangular, and P is a permutation matrix.
        /// </summary>
        /// <returns>Returns the Lup decomposition. L = [0], U = [1], P = [2].</returns>
        Single[] LupDecomposition()
        {
            if (!this.IsSquare)
                throw new Exception.InvalidDimensions();
            int order = this.Order;
            Single l = Single.Identity(order);
            Single u = this.Copy();
            Single p = Single.Identity(order);
            int last = order - 1;
            for (int position = 0; position < last; position++)
            {
                int pivotRow = position;
                for (int y = position + 1; y < u.Dimensions.Height; y++)
                    if (Kean.Math.Single.Absolute(u[position, position]) < Kean.Math.Single.Absolute(u[position, y]))
                        pivotRow = y;
                p.SwapRows(position, pivotRow);
                u.SwapRows(position, pivotRow);
                if (u[position, position] != 0)
                {
                    for (int y = position + 1; y < order; y++)
                    {
                        float pivot = u[position, y] / u[position, position];
                        for (int x = position; x < order; x++)
                            u[x, y] -= pivot * u[x, position];
                        u[position, y] = pivot;
                    }
                }
            }
            for (int y = 0; y < order; y++)
                for (int x = 0; x < y; x++)
                {
                    l[x, y] = u[x, y];
                    u[x, y] = 0;
                }
            return new Single[] { l, u, p };
        }
        void SwapRows(int row1, int row2)
        {
            int order = this.Order;
            if (row1 != row2)
            {
                for (int x = 0; x < order; x++)
                {
                    float buffer = this[x, row1];
                    this[x, row1] = this[x, row2];
                    this[x, row2] = buffer;
                }
            }
        }
        /// <summary>
        /// Forward solver lower * x = y. Current object is y. 
        /// </summary>
        /// <param name="lower">Lower triangual matrix.</param>
        /// <returns>Solution x.</returns>
        public Single ForwardSubstitution(Single lower)
        {
            Single result = new Single(this.Dimensions);
            for (int x = 0; x < this.Dimensions.Width; x++)
            {
                for (int y = 0; y < this.Dimensions.Height; y++)
                {
                    float accumulator = this[x, y];
                    for (int x2 = 0; x2 < y; x2++)
                        accumulator -= lower[x2, y] * result[x, x2];
                    float value = lower[y, y];
                    if (value != 0)
                        result[x, y] = accumulator / value;
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
        public Single BackwardSubstitution(Single upper)
        {
            Single result = new Single(this.Dimensions);
            for (int x = 0; x < this.Dimensions.Width; x++)
            {
                for (int y = this.Dimensions.Height - 1; y >= 0; y--)
                {
                    float accumulator = this[x, y];
                    for (int x2 = y + 1; x2 < upper.Dimensions.Width; x2++)
                        accumulator -= upper[x2, y] * result[x, x2];
                    float value = upper[y, y];
                    if (value != 0)
                        result[x, y] = accumulator / value;
                    else
                        throw new Exception.DivisionByZero();
                }
            }
            return result;
        }
        /// <summary>
        /// Lup least square solver A * x = y.
        /// The current matrix determines the matrix A above.
        /// </summary>
        /// <param name="y">The right hand column y vector of the equation system.</param>
        /// <returns>Return the least square solution to the system.</returns>
        public Single Solve(Single y)
        {
            Single result = null;
            if (this.Dimensions.Width > this.Dimensions.Height)
                throw new Exception.InvalidDimensions();
            else
                try
                {
                    if (this.IsSquare)
                    {
                        Single[] lup = this.LupDecomposition();
                        result = (lup[2] * y).ForwardSubstitution(lup[0]).BackwardSubstitution(lup[1]);
                    }
                    else
                    {
                        Single transpose = this.Transpose();
                        Single[] lup = (transpose * this).LupDecomposition();
                        result = (lup[2] * transpose * y).ForwardSubstitution(lup[0]).BackwardSubstitution(lup[1]);
                    }
                }
                catch (Kean.Core.Error.Exception e)
                {
                }
            return result;
        }
        /// <summary>
        /// Computes the inverse of the current matrix using Lup-decomposition.
        /// </summary>
        /// <returns>Inverse of the current matrix.</returns>
        public Single Inverse()
        {
            if (!this.IsSquare)
                throw new Exception.InvalidDimensions();
            Single result = null;
            Single[] lup = this.LupDecomposition();
            try
            {
                result = (lup[2] * Single.Identity(this.Order)).ForwardSubstitution(lup[0]).BackwardSubstitution(lup[1]);
            }
            catch (Kean.Core.Error.Exception e)
            {
            }
            return result;
        }
        /// <summary>
        /// Computes the determinant of the current matrix using the Lup-decomposition.
        /// </summary>
        /// <returns>Determinant of the current matrix.</returns>
        public float Determinant()
        {
            Single[] lup = this.LupDecomposition();
            float result = 1;
            for (int position = 0; position < lup[1].Dimensions.Height; position++)
                result *= lup[1][position, position];
            return result * lup[2].Sign();
        }
        /// <summary>
        /// Sign of a permutation matrix.
        /// </summary>
        /// <returns>Returns the sign of the permutation matrix.</returns>
        float Sign()
        {
            int[] permutation = new int[this.Dimensions.Height];
            for (int y = 0; y < this.Dimensions.Width; y++)
            {
                int x = 0;
                while (this[x, y] == 0 && x < this.Dimensions.Width)
                    x++;
                permutation[y] = x;
            }
            float accumulated = 1;
            for (int i = 0; i < permutation.Length; i++)
                for (int j = i + 1; j < permutation.Length; j++)
                    accumulated *= (float)(permutation[i] - permutation[j]) / (i - j);
            return Kean.Math.Single.Sign(accumulated);
        }
        #endregion
    }
}
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
        #region QR methods
        /// <summary>
        /// QR least square solver A * x = y. See http://en.wikipedia.org/wiki/QR_decomposition
        /// The current matrix determines the matrix A above.
        /// </summary>
        /// <param name="y">The right hand column y vector of the equation system.</param>
        /// <returns>Return the least square solution to the system,</returns>
        public Single SolveQr(Single y)
        {
            Single result = null;
            try
            {
                if (this.Dimensions.Height < this.Dimensions.Width)
                {
                    // Least norm
                    Single transpose = this.Transpose();
                    Single[] qr = transpose.QRFactorization();
                    Single q = qr[0].Extract(0, transpose.Dimensions.Width, 0, transpose.Dimensions.Height);
                    Single r = qr[1].Extract(0, transpose.Dimensions.Width, 0, transpose.Dimensions.Width);
                    Single z = y.ForwardSubstitution(r.Transpose());
                    result = q * z;
                }
                else
                {
                    // Standard
                    Single[] qr = this.QRFactorization();
                    Single q = qr[0].Extract(0, this.Dimensions.Width, 0, this.Dimensions.Height);
                    Single r = qr[1].Extract(0, this.Dimensions.Width, 0, this.Dimensions.Width);
                    result = (q.Transpose() * y).BackwardSubstitution(r);
                }
            }
            catch (Kean.Core.Error.Exception e)
            {
            }
            return result;
        }
        /// <summary>
        /// QR factorizion of the current matrix. See http://en.wikipedia.org/wiki/QR_decomposition
        /// Recall that A = QR.
        /// </summary>
        /// <returns>Return the QR-factorization array with Q = [0] and R = [1]. </returns>
        public Single[] QRFactorization()
        {
            int order = this.Dimensions.Height;
            int iterations = Kean.Math.Integer.Minimum(this.Dimensions.Height - 1, this.Dimensions.Width);
            Single r = this;
            Single q = Single.Identity(order);
            for (int i = 0; i < iterations; i++)
            {
                Single x = r.Extract(i, i + 1, i, r.Dimensions.Height);
                Single y = (Kean.Math.Single.Sign(x[0, 0]) * x.Norm) * Single.Basis(r.Dimensions.Height - i, 0);
                Single qi = Single.Identity(order).Paste(i, i, Single.HouseHolder(x, y));
                r = qi * r;
                q *= qi.Transpose();
            }
            return new Single[] { q, r };
        }
        // Contruct Householder transform from two  column vectors of same length and norm.
        public static Single HouseHolder(Single x, Single y)
        {
            if (x.Dimensions.Width != y.Dimensions.Width && x.Dimensions.Width != 1 && x.Dimensions.Height != y.Dimensions.Height)
                throw new Exception.InvalidDimensions();
            int length = x.Dimensions.Height;
            Single w = x - y;
            float norm = w.Norm;
            Single result = Single.Identity(length);
            if (norm != 0)
            {
                w /= norm;
                result -= 2 * w * w.Transpose();
            }
            return result;
        }
        /// <summary>
        /// Eigenvalue decomposition of a symmetric square matrix.
        /// a = u * d * u.Transpose();
        /// </summary>
        /// <returns>Array of matrices {u, d}.</returns>
        public Single[] Eigenvalues()
        {
            if (!this.IsSquare)
                new Exception.InvalidDimensions();
            int order = this.Dimensions.Width;
            float tolerance = 1e-5f;
            int iterations = 100;
            Single u = this;
            Single q = Single.Identity(order);
            int i = 0;
            float error = float.MaxValue;
            while (error > tolerance && i < iterations)
            {
                Single[] qr = u.QRFactorization();
                u = qr[0].Transpose() * u * qr[0];
                q *= qr[0];
                error = 0;
                for (int j = 0; j < order; j++)
                    for (int k = 0; k < order; k++)
                        error += j != k ? Kean.Math.Single.Absolute(u[j, k]) : 0;
                i++;
            }
            return new Single[] { q, u };
        }
        #endregion
        #region Diagonalization
        /// <summary>
        /// See Algorithm 5.4.2 (Householder Bidiagonalization).
        /// Computation of the Householder bidiagonalization of current matrix. Note height >= width.
        /// The method return {u, y, v}, where u,v are orthogonal matrices such that u' * current * v = y,
        /// where y is a bidiagonal matrix with a posssibly nonzero superdiagonal.
        /// </summary>
        /// <returns>Array of matrices {u,y,v}.</returns>
        public Single[] BiDiagonalization()
        {
            Single[] result;
            Single b = this.Copy();
            int n = b.Dimensions.Width;
            int m = b.Dimensions.Height;
            Single[] leftHouseholder = new Single[n];
            Single[] rightHouseholder = null;
            if (n - 2 >= 1)
                rightHouseholder = new Single[n - 2];

            for (int j = 0; j < n; j++)
            {
                Kean.Core.Basis.Tuple<Single, float> leftHousePair = b.Extract(j, j + 1, j, m).House();
                Single leftHouseMultiplier = Single.Identity(m - j) - leftHousePair.Item2 * leftHousePair.Item1 * leftHousePair.Item1.Transpose();
                leftHouseholder[j] = leftHouseMultiplier;
                b.Set(j, j, leftHouseMultiplier * b.Extract(j, j));
                if (j < n - 2)
                {
                    Kean.Core.Basis.Tuple<Single, float> rightHousePair = b.Extract(j + 1, n, j, j + 1).Transpose().House();
                    Single rightHouseMultiplier = Single.Identity(n - j - 1) - rightHousePair.Item2 * rightHousePair.Item1 * rightHousePair.Item1.Transpose();
                    rightHouseholder[j] = rightHouseMultiplier;
                    b.Set(j + 1, j, b.Extract(j + 1, j) * rightHouseMultiplier);
                }
            }
            Single u = Single.Identity(m);
            for (int j = n - 1; j >= 0; j--)
                u.Set(j, j, leftHouseholder[j] * u.Extract(j, j));
            Single v = Single.Identity(n);
            for (int j = n - 3; j >= 0; j--)
                v.Set(j + 1, j + 1, rightHouseholder[j] * v.Extract(j + 1, j + 1));
            result = new Single[] { u, b, v };
            return result;

        }
        /// <summary>
        /// See Algorithm 5.1.1 (Householder Vector).
        /// </summary>
        /// <returns></returns>
        Kean.Core.Basis.Tuple<Single, float> House()
        {
            Kean.Core.Basis.Tuple<Single, float> result;
            int n = this.Dimensions.Height;
            Single tail = this.Extract(0, 1, 1, n);
            float sigma = (tail.Transpose() * tail)[0, 0];
            Single nu = new Single(1, n);
            nu[0, 0] = 1;
            nu.Set(0, 1, tail);
            float beta = 0;
            if (sigma != 0)
            {
                float x00 = this[0, 0];
                float mu = Kean.Math.Single.SquareRoot(Kean.Math.Single.Squared(x00) + sigma);
                if (x00 <= 0)
                    nu[0, 0] = x00 - mu;
                else
                    nu[0, 0] = -sigma / (x00 + mu);
                float nu00Squared = Kean.Math.Single.Squared(nu[0, 0]);
                beta = 2 * nu00Squared / (sigma + nu00Squared);
                nu /= nu[0, 0];
            }
            result = Kean.Core.Basis.Tuple.Create<Single, float>(nu, beta);
            return result;
        }
        #endregion
        #region Svd Methods
        /// <summary>
        /// Svd least square solver A * x = y. See http://en.wikipedia.org/wiki/Linear_least_squares_(mathematics).
        /// The current matrix determines the matrix A above.
        /// </summary>
        /// <param name="y">The right hand column y vector of the equation system.</param>
        /// <returns>Return the least square solution to the system,</returns>
        public Single SolveSvd(Single b)
        {
            Single result;
            Single[] usv = this.Svd();
            Single u = usv[0];
            Single s = usv[1];
            Single v = usv[2];
            Single sPlus = new Single(s.Dimensions.Width, s.Dimensions.Height);
            int order = s.Order;
            for (int i = 0; i < order; i++)
            {
                float value = s[i, i];
                if (value != 0)
                    sPlus[i, i] = 1 / value;
            }
            // Least square solution and minimum norm solution
            if (this.Dimensions.Height >= this.Dimensions.Width)
                result = v * sPlus * u.Transpose() * b;
            else
            {
                Single d = new Single(1, this.Dimensions.Width).Paste(0, 0, u.Transpose() * b);

                for (int i = 0; i < order; i++)
                    d[0, i] *= sPlus[i, i];
                result = v * d;
            }
            return result;
        }
        /// <summary>
        /// Returns the Svd decomposition of the current matrix. Recall A = U * S * V'.
        /// </summary>
        /// <returns>Return Svd decomposition U = [0], S = [1], V = [2].</returns>
        public Single[] Svd()
        {
            return this.Svd(1e-10f);
        }
        /// <summary>
        /// Returns the Svd decomposition of the current matrix. Recall A = U * S * V'.
        /// </summary>
        /// <param name="tolerance">Tolerance used for accuracy of algorithm.</param>
        /// <returns>Return Svd decomposition U = [0], S = [1], V = [2].</returns>
        public Single[] Svd(float tolerance)
        {
            Single[] result;
            if (this.Dimensions.Height >= this.Dimensions.Width)
                result = this.SvdHelper(tolerance);
            else
            {
                result = this.Transpose().SvdHelper(tolerance);
                result = new Single[] { result[2], result[1].Transpose(), result[0] };
            }
            return result;
        }
        /// <summary>
        /// See Algorithm 8.6.2 (The SVD Algorithm).
        /// Returns the Svd decomposition of the current matrix. Recall A = U * S * V'.
        /// Height of matrix must be greater or equal to width.
        /// </summary>
        /// <param name="tolerance">Tolerance used for accuracy of algorithm.</param>
        /// <returns>Return Svd decomposition U = [0], S = [1], V = [2].</returns>
        Single[] SvdHelper(float tolerance)
        {
            Single[] result;
            int m = this.Dimensions.Height;
            int n = this.Dimensions.Width;
            if (m == 1 && n == 1)
                result = new Single[] { Single.Identity(1), this, Single.Identity(1) };
            else
            {
                Single[] ubv = this.BiDiagonalization();
                Single u = ubv[0];
                Single b = ubv[1]; //.Extract(0, n, 0, n);
                Single v = ubv[2];
                int q = 0;
                while (q < n)
                {
                    for (int i = 0; i < n - 1; i++)
                        if (Kean.Math.Single.Absolute(b[i + 1, i]) < tolerance * (Kean.Math.Single.Absolute(b[i, i]) + Kean.Math.Single.Absolute(b[i + 1, i + 1])))
                            b[i + 1, i] = 0;
                    
                    int p = 0;
                    q = 0;
                    int b22Order = 0;
                    int j = n - 1;
                    for (; j >= 1; j--)
                    {
                        if (Kean.Math.Single.Absolute(b[j, j - 1]) < tolerance)
                            q++;
                        else
                            break;
                    }
                    if (j == 0 && q > 0)
                        q++;
                    for (; j >= 1; j--)
                    {
                        if (Kean.Math.Single.Absolute(b[j, j - 1]) > tolerance)
                            b22Order++;
                        else
                            break;
                    }
                    if (j == 0 && b22Order > 0)
                        b22Order++;
                    p = n - b22Order - q;
                    if (b22Order == 0)
                        break;
                    if (q < n)
                    {
                        Single b22 = b.Extract(p, n - q, p, n - q);
                        bool zeros = false;
                        for (int i = 0; i < b22.Dimensions.Width; i++)
                            if (Kean.Math.Single.Absolute(b22[i, i]) < tolerance && Kean.Math.Single.Absolute(b22[i + 1, i]) > tolerance)
                            {
                                b22[i, i] = 0;
                                b22[i + 1, i] = 0;
                                zeros = true;
                            }
                        if (!zeros)
                        {
                            Single[] gkubv = b22.GolubKahanSvdStep();
                            Single uprime = Single.Diagonal(Single.Identity(p), gkubv[0], Single.Identity(q + m - n));
                            Single vprime = Single.Diagonal(Single.Identity(p), gkubv[2], Single.Identity(q));
                            u = u * uprime;
                            v = v * vprime;
                            b = uprime.Transpose() * b * vprime;
                        }
                    }


                }
                result = new Single[] { u, b, v };
            }
            return result;
        }
        /// <summary>
        /// See Algorithm 8.6.1 (Golub-Kahan SVD Step.)
        /// </summary>
        /// <returns></returns>
        Single[] GolubKahanSvdStep()
        {
            Single b = this.Copy();
            int m = b.Dimensions.Height;
            int n = b.Dimensions.Width;
            Single t = b.Transpose() * b;
            Single trail = t.Extract(n - 2, n, n - 2, n);
            float d = (trail[0, 0] - trail[1, 1]) / 2;
            float mu = trail[1, 1] + d - Kean.Math.Single.Sign(d) * Kean.Math.Single.SquareRoot(Kean.Math.Single.Squared(d) + Kean.Math.Single.Squared(trail[1, 0]));
            float y = t[0, 0] - mu;
            float z = t[1, 0];
            Single u = Single.Identity(m);
            Single v = Single.Identity(n);
            for (int k = 0; k < n - 1; k++)
            {
                float[] cs = Single.Givens(y, z);
                Single g = Single.GivensRotation(m, k, k + 1, cs[0], cs[1]);
                v = v * g;
                b = b * g;
                y = b[k, k];
                z = b[k, k + 1];
                cs = Single.Givens(y, z);
                g = Single.GivensRotation(n, k, k + 1, cs[0], cs[1]);
                b = g.Transpose() * b;
                u = u * g;
                if (k < n - 2)
                {
                    y = b[k + 1, k];
                    z = b[k + 2, k];
                }
            }
            return new Single[] { u, b, v };
        }
        /// <summary>
        /// Givens rotation. Creates an identity matrix of given order m and
        /// replace at positions (i,i) and (k,k) with c, 
        /// replace at positions (i,k)  -s and (k,i) with s,
        /// </summary>
        /// <param name="m">Order of matrix to be created.</param>
        /// <param name="i">First index.</param>
        /// <param name="k">Second index.</param>
        /// <param name="c">Value corresponding to a cosine value.</param>
        /// <param name="s">Value corresponding to a sine value.</param>
        /// <returns>Returns a Givens rotation.</returns>
        static Single GivensRotation(int m, int i, int k, float c, float s)
        {
            Single result = Single.Identity(m);
            result[i, i] = c;
            result[k, k] = c;
            result[k, i] = s;
            result[i, k] = -s;
            return result;
        }
        /// <summary>
        /// See Algorithm 5.1.3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static float[] Givens(float a, float b)
        {
            float[] result = new float[2];
            float c, s;
            if (b == 0)
            {
                c = 1;
                s = 0;
            }
            else
            {
                if (Kean.Math.Single.Absolute(b) > Kean.Math.Single.Absolute(a))
                {
                    float tau = -a / b;
                    s = 1 / Kean.Math.Single.SquareRoot(1 + Kean.Math.Single.Squared(tau));
                    c = s * tau;
                }
                else
                {
                    float tau = -b / a;
                    c = 1 / Kean.Math.Single.SquareRoot(1 + Kean.Math.Single.Squared(tau));
                    s = c * tau;
                }
            }
            result[0] = c;
            result[1] = s;
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
        public Single SolveLup(Single y)
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
                        Single[] lup = (this.Transpose() * this).LupDecomposition();
                        result = (lup[2] * this.Transpose() * y).ForwardSubstitution(lup[0]).BackwardSubstitution(lup[1]);
                    }
                }
                catch (Kean.Core.Error.Exception e)
                {
                }
            return result;
        }
        /// <summary>
        /// Best optimized least square solver A * x = y. The current matrix determines the matrix A above.
        /// </summary>
        /// <param name="y">The right hand column y vector of the equation system.</param>
        /// <returns>Return the least square solution to the system or null if no such is found.</returns>
        public Single Solve(Single y)
        {
            return this.SolveLup(y);
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
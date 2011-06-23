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
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Core.Basis.Extension;

namespace Kean.Math.Matrix
{
    public partial class Double
    {
        /// <summary>
        /// Return true if the current matrix is a diagonal matrix within the specified tolerance.
        /// </summary>
        /// <param name="tolerance">Tolerance used to check zero elements outside the diagonal.</param>
        /// <returns>True is the matrix is a diagonal matrix within the given tolerance.</returns>
        public bool IsDiagonal(double tolerance)
        {
            bool result = true;
            for (int x = 0; x < this.Dimensions.Width; x++)
                for (int y = 0; y < this.Dimensions.Height; y++)
                {
                    if (x != y && Kean.Math.Double.Absolute(this[x, y]) > tolerance)
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
        public double Trace()
        {
            double result = 0;
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
        public Double Cholesky()
        {
            if (!this.IsSquare)
                new Exception.InvalidDimensions();
            int order = this.Dimensions.Width;
            Double result = this.Copy();
            for (int j = 0; j < order; j++)
            {
                if (j > 0)
                    result.Set(j, j, result.Extract(j, j + 1, j, order) - result.Extract(0, j, j, order) * result.Extract(0, j, j, j + 1).Transpose());
                result.Set(j, j, result.Extract(j, j + 1, j, order) / Kean.Math.Double.SquareRoot(result[j, j]));
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
        public Double SolveCholesky(Double y)
        {
            Double result;
            if (this.Dimensions.Height < this.Dimensions.Width)
            {
                // Least norm
                Double transpose = this.Transpose();
                Double lower = (this * transpose).Cholesky();
                Double z = y.ForwardSubstitution(lower);
                z = z.BackwardSubstitution(lower.Transpose());
                result = transpose * z;
            }
            else
            {
                // Standard
                Double transpose = this.Transpose();
                Double lower = (transpose * this).Cholesky();
                Double z = (transpose * y).ForwardSubstitution(lower);
                result = z.BackwardSubstitution(lower.Transpose());
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
        public Double SolveQr(Double y)
        {
            Double result;
            if (this.Dimensions.Height < this.Dimensions.Width)
            {
                // Least norm
                Double transpose = this.Transpose();
                Double[] qr = transpose.QRFactorization();
                Double q = qr[0].Extract(0, transpose.Dimensions.Width, 0, transpose.Dimensions.Height);
                Double r = qr[1].Extract(0, transpose.Dimensions.Width, 0, transpose.Dimensions.Width);
                Double z = y.ForwardSubstitution(r.Transpose());
                result = q * z;
            }
            else
            {
                // Standard
                Double[] qr = this.QRFactorization();
                Double q = qr[0].Extract(0, this.Dimensions.Width, 0, this.Dimensions.Height);
                Double r = qr[1].Extract(0, this.Dimensions.Width, 0, this.Dimensions.Width);
                result = (q.Transpose() * y).BackwardSubstitution(r);
            }
            return result;
        }
        /// <summary>
        /// QR factorizion of the current matrix. See http://en.wikipedia.org/wiki/QR_decomposition
        /// Recall that A = QR.
        /// </summary>
        /// <returns>Return the QR-factorization array with Q = [0] and R = [1]. </returns>
        public Double[] QRFactorization()
        {
            int order = this.Dimensions.Height;
            int iterations = Kean.Math.Integer.Minimum(this.Dimensions.Height - 1, this.Dimensions.Width);
            Double r = this;
            Double q = Double.Identity(order);
            for (int i = 0; i < iterations; i++)
            {
                Double x = r.Extract(i, i + 1, i, r.Dimensions.Height);
                Double y = (Kean.Math.Double.Sign(x[0, 0]) * x.Norm) * Double.Basis(r.Dimensions.Height - i, 0);
                Double qi = Double.Identity(order).Paste(i, i, Double.HouseHolder(x, y));
                r = qi * r;
                q *= qi.Transpose();
            }
            return new Double[] { q, r };
        }
        // Contruct Householder transform from two  column vectors of same length and norm.
        public static Double HouseHolder(Double x, Double y)
        {
            if (x.Dimensions.Width != y.Dimensions.Width && x.Dimensions.Width != 1 && x.Dimensions.Height != y.Dimensions.Height)
                throw new Exception.InvalidDimensions();
            int length = x.Dimensions.Height;
            Double w = x - y;
            double norm = w.Norm;
            Double result = Double.Identity(length);
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
        public Double[] Eigenvalues()
        {
            if (!this.IsSquare)
                new Exception.InvalidDimensions();
            int order = this.Dimensions.Width;
            double tolerance = 1e-5;
            int iterations = 100;
            Double u = this;
            Double q = Double.Identity(order);
            int i = 0;
            double error = double.MaxValue;
            while (error > tolerance && i < iterations)
            {
                Double[] qr = u.QRFactorization();
                u = qr[0].Transpose() * u * qr[0];
                q *= qr[0];
                error = 0;
                for (int j = 0; j < order; j++)
                    for (int k = 0; k < order; k++)
                        error += j != k ? Kean.Math.Double.Absolute(u[j, k]) : 0;
                i++;
            }
            return new Double[] { q, u };
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
        public Double[] BiDiagonalization()
        {
            Double[] result;
            Double b = this.Copy();
            int n = b.Dimensions.Width;
            int m = b.Dimensions.Height;
            Double[] leftHouseholder = new Double[n];
            Double[] rightHouseholder = null;
            if (n - 2 >= 1)
                rightHouseholder = new Double[n - 2];

            for (int j = 0; j < n; j++)
            {
                Kean.Core.Basis.Tuple<Double, double> leftHousePair = b.Extract(j, j + 1, j, m).House();
                Double leftHouseMultiplier = Double.Identity(m - j) - leftHousePair.Item2 * leftHousePair.Item1 * leftHousePair.Item1.Transpose();
                leftHouseholder[j] = leftHouseMultiplier;
                b.Set(j, j, leftHouseMultiplier * b.Extract(j, j));
                if (j < n - 2)
                {
                    Kean.Core.Basis.Tuple<Double, double> rightHousePair = b.Extract(j + 1, n, j, j + 1).Transpose().House();
                    Double rightHouseMultiplier = Double.Identity(n - j - 1) - rightHousePair.Item2 * rightHousePair.Item1 * rightHousePair.Item1.Transpose();
                    rightHouseholder[j] = rightHouseMultiplier;
                    b.Set(j + 1, j, b.Extract(j + 1, j) * rightHouseMultiplier);
                }
            }
            Double u = Double.Identity(m);
            for (int j = n - 1; j >= 0; j--)
                u.Set(j, j, leftHouseholder[j] * u.Extract(j, j));
            Double v = Double.Identity(n);
            for (int j = n - 3; j >= 0; j--)
                v.Set(j + 1, j + 1, rightHouseholder[j] * v.Extract(j + 1, j + 1));
            result = new Double[] { u, b, v };
            return result;

        }
        /// <summary>
        /// See Algorithm 5.1.1 (Householder Vector).
        /// </summary>
        /// <returns></returns>
        Kean.Core.Basis.Tuple<Double, double> House()
        {
            Kean.Core.Basis.Tuple<Double, double> result;
            int n = this.Dimensions.Height;
            Double tail = this.Extract(0, 1, 1, n);
            double sigma = (tail.Transpose() * tail)[0, 0];
            Double nu = new Double(1, n);
            nu[0, 0] = 1;
            nu.Set(0, 1, tail);
            double beta = 0;
            if (sigma != 0)
            {
                double x00 = this[0, 0];
                double mu = Kean.Math.Double.SquareRoot(Kean.Math.Double.Squared(x00) + sigma);
                if (x00 <= 0)
                    nu[0, 0] = x00 - mu;
                else
                    nu[0, 0] = -sigma / (x00 + mu);
                double nu00Squared = Kean.Math.Double.Squared(nu[0, 0]);
                beta = 2 * nu00Squared / (sigma + nu00Squared);
                nu /= nu[0, 0];
            }
            result = Kean.Core.Basis.Tuple.Create<Double, double>(nu, beta);
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
        public Double SolveSvd(Double b)
        {
            Double result;
            Double[] usv = this.Svd();
            Double u = usv[0];
            Double s = usv[1];
            Double v = usv[2];
            Double sPlus = new Double(s.Dimensions.Width, s.Dimensions.Height);
            int order = s.Order;
            for (int i = 0; i < order; i++)
            {
                double value = s[i, i];
                if (value != 0)
                    sPlus[i, i] = 1 / value;
            }
            // Least square solution and minimum norm solution
            if (this.Dimensions.Height >= this.Dimensions.Width)
                result = v * sPlus * u.Transpose() * b;
            else
            {
                Double d = new Double(1, this.Dimensions.Width).Paste(0, 0, u.Transpose() * b);

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
        public Double[] Svd()
        {
            return this.Svd(1e-10);
        }
        /// <summary>
        /// Returns the Svd decomposition of the current matrix. Recall A = U * S * V'.
        /// </summary>
        /// <param name="tolerance">Tolerance used for accuracy of algorithm.</param>
        /// <returns>Return Svd decomposition U = [0], S = [1], V = [2].</returns>
        public Double[] Svd(double tolerance)
        {
            Double[] result;
            if (this.Dimensions.Height >= this.Dimensions.Width)
                result = this.SvdHelper(tolerance);
            else
            {
                result = this.Transpose().SvdHelper(tolerance);
                result = new Double[] { result[2], result[1].Transpose(), result[0] };
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
        Double[] SvdHelper(double tolerance)
        {
            Double[] result;
            int m = this.Dimensions.Height;
            int n = this.Dimensions.Width;
            if (m == 1 && n == 1)
                result = new Double[] { Double.Identity(1), this, Double.Identity(1) };
            else
            {
                Double[] ubv = this.BiDiagonalization();
                Double u = ubv[0];
                Double b = ubv[1]; //.Extract(0, n, 0, n);
                Double v = ubv[2];
                int q = 0;
                while (q < n)
                {
                    for (int i = 0; i < n - 1; i++)
                        if (Kean.Math.Double.Absolute(b[i + 1, i]) < tolerance * (Kean.Math.Double.Absolute(b[i, i]) + Kean.Math.Double.Absolute(b[i + 1, i + 1])))
                            b[i + 1, i] = 0;
                    Double b22;
                    int p = 0;
                    q = 0;
                    int b22Order = 0;
                    int j = n - 1;
                    for (; j >= 1; j--)
                    {
                        if (Kean.Math.Double.Absolute(b[j, j - 1]) < tolerance)
                            q++;
                        else
                            break;
                    }
                    if (j == 0 && q > 0)
                        q++;
                    for (; j >= 1; j--)
                    {
                        if (Kean.Math.Double.Absolute(b[j, j - 1]) > tolerance)
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
                        b22 = b.Extract(p, n - q, p, n - q);
                        bool zeros = false;
                        for (int i = 0; i < b22.Dimensions.Width; i++)
                            if (Kean.Math.Double.Absolute(b22[i, i]) < tolerance && Kean.Math.Double.Absolute(b22[i + 1, i]) > tolerance)
                            {
                                b22[i, i] = 0;
                                b22[i + 1, i] = 0;
                                zeros = true;
                            }
                        if (!zeros)
                        {
                            Double[] gkubv = b22.GolubKahanSvdStep();
                            Double uprime = Double.Diagonal(Double.Identity(p), gkubv[0], Double.Identity(q + m - n));
                            Double vprime = Double.Diagonal(Double.Identity(p), gkubv[2], Double.Identity(q));
                            u = u * uprime;
                            v = v * vprime;
                            b = uprime.Transpose() * b * vprime;
                        }
                    }


                }
                result = new Double[] { u, b, v };
            }
            return result;
        }
        /// <summary>
        /// See Algorithm 8.6.1 (Golub-Kahan SVD Step.)
        /// </summary>
        /// <returns></returns>
        Double[] GolubKahanSvdStep()
        {
            Double b = this.Copy();
            int m = b.Dimensions.Height;
            int n = b.Dimensions.Width;
            Double t = b.Transpose() * b;
            Double trail = t.Extract(n - 2, n, n - 2, n);
            double d = (trail[0, 0] - trail[1, 1]) / 2;
            double mu = trail[1, 1] + d - Kean.Math.Double.Sign(d) * Kean.Math.Double.SquareRoot(Kean.Math.Double.Squared(d) + Kean.Math.Double.Squared(trail[1, 0]));
            double y = t[0, 0] - mu;
            double z = t[1, 0];
            Double u = Double.Identity(m);
            Double v = Double.Identity(n);
            for (int k = 0; k < n - 1; k++)
            {
                double[] cs = Double.Givens(y, z);
                Double g = Double.GivensRotation(m, k, k + 1, cs[0], cs[1]);
                v = v * g;
                b = b * g;
                y = b[k, k];
                z = b[k, k + 1];
                cs = Double.Givens(y, z);
                g = Double.GivensRotation(n, k, k + 1, cs[0], cs[1]);
                b = g.Transpose() * b;
                u = u * g;
                if (k < n - 2)
                {
                    y = b[k + 1, k];
                    z = b[k + 2, k];
                }
            }
            return new Double[] { u, b, v };
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
        static Double GivensRotation(int m, int i, int k, double c, double s)
        {
            Double result = Double.Identity(m);
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
        static double[] Givens(double a, double b)
        {
            double[] result = new double[2];
            double c, s;
            if (b == 0)
            {
                c = 1;
                s = 0;
            }
            else
            {
                if (Kean.Math.Double.Absolute(b) > Kean.Math.Double.Absolute(a))
                {
                    double tau = -a / b;
                    s = 1 / Kean.Math.Double.SquareRoot(1 + Kean.Math.Double.Squared(tau));
                    c = s * tau;
                }
                else
                {
                    double tau = -b / a;
                    c = 1 / Kean.Math.Double.SquareRoot(1 + Kean.Math.Double.Squared(tau));
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
        Double[] LupDecomposition()
        {
            if (!this.IsSquare)
                throw new Exception.InvalidDimensions();
            int order = this.Order;
            Double l = Double.Identity(order);
            Double u = this.Copy();
            Double p = Double.Identity(order);


            int last = order - 1;
            for (int position = 0; position < last; position++)
            {
                int pivotRow = position;
                for (int y = position + 1; y < u.Dimensions.Height; y++)
                    if (Kean.Math.Double.Absolute(u[position, position]) < Kean.Math.Double.Absolute(u[position, y]))
                        pivotRow = y;
                p.SwapRows(position, pivotRow);
                u.SwapRows(position, pivotRow);
                if (u[position, position] != 0)
                {
                    for (int y = position + 1; y < order; y++)
                    {
                        double pivot = u[position, y] / u[position, position];
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
            return new Double[] { l, u, p };
        }
        void SwapRows(int row1, int row2)
        {
            int order = this.Order;
            if (row1 != row2)
            {
                for (int x = 0; x < order; x++)
                {
                    double buffer = this[x, row1];
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
        public Double ForwardSubstitution(Double lower)
        {
            Double result = new Double(this.Dimensions);
            for (int x = 0; x < this.Dimensions.Width; x++)
            {
                for (int y = 0; y < this.Dimensions.Height; y++)
                {
                    double accumulator = this[x, y];
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
        public Double BackwardSubstitution(Double upper)
        {
            Double result = new Double(this.Dimensions);
            for (int x = 0; x < this.Dimensions.Width; x++)
            {
                for (int y = this.Dimensions.Height - 1; y >= 0; y--)
                {
                    double accumulator = this[x, y];
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
        /// <summary>
        /// Lup least square solver A * x = y. See http://en.wikipedia.org/wiki/QR_decomposition
        /// The current matrix determines the matrix A above.
        /// </summary>
        /// <param name="y">The right hand column y vector of the equation system.</param>
        /// <returns>Return the least square solution to the system.</returns>
        public Double SolveLup(Double y)
        {
            Double result;
            if (this.IsSquare)
            {
                Double[] lup = this.LupDecomposition();
                result = (lup[2] * y).ForwardSubstitution(lup[0]).BackwardSubstitution(lup[1]);
            }
            else if (this.Dimensions.Width < this.Dimensions.Height)
            {
                Double[] lup = (this.Transpose() * this).LupDecomposition();
                result = (lup[2] * this.Transpose() * y).ForwardSubstitution(lup[0]).BackwardSubstitution(lup[1]);
            }
            else
                throw new Exception.InvalidDimensions();
            return result;
        }
        /// <summary>
        /// Computes the inverse of the current matrix using Lup-decomposition.
        /// </summary>
        /// <returns>Inverse of the current matrix.</returns>
        public Double Inverse()
        {
            if (!this.IsSquare)
                throw new Exception.InvalidDimensions();
            Double result;
            Double[] lup = this.LupDecomposition();
            result = (lup[2] * Double.Identity(this.Order)).ForwardSubstitution(lup[0]).BackwardSubstitution(lup[1]);
            return result;
        }
        /// <summary>
        /// Computes the determinant of the current matrix using the Lup-decomposition.
        /// </summary>
        /// <returns>Determinant of the current matrix.</returns>
        public double Determinant()
        {
            Double[] lup = this.LupDecomposition();
            double result = 1;
            for (int position = 0; position < lup[1].Dimensions.Height; position++)
                result *= lup[1][position, position];
            return result * lup[2].Sign();
        }
        /// <summary>
        /// Sign of a permutation matrix.
        /// </summary>
        /// <returns>Returns the sign of the permutation matrix.</returns>
        double Sign()
        {
            int[] permutation = new int[this.Dimensions.Height];
            for (int y = 0; y < this.Dimensions.Width; y++)
            {
                int x = 0;
                while (this[x, y] == 0 && x < this.Dimensions.Width)
                    x++;
                permutation[y] = x;
            }
            double accumulated = 1;
            for (int i = 0; i < permutation.Length; i++)
                for (int j = i + 1; j < permutation.Length; j++)
                    accumulated *= (double)(permutation[i] - permutation[j]) / (i - j);
            return Kean.Math.Double.Sign(accumulated);
        }
        #endregion
    }
}
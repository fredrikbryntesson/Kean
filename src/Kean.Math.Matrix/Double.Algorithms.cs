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
        public bool IsDiagonal
        {
            get
            {
                bool result = true;
                for (int x = 0; x < this.Dimensions.Width; x++)
                    for (int y = 0; y < this.Dimensions.Height; y++)
                    {
                        if (x != y && this[x, y] != 0)
                        {
                            result = false;
                            x = this.Dimensions.Width;
                            break;
                        }
                    }
                return result;
            }
        }
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
        /// Forward solver lower * x = y. Current object is y. 
        /// </summary>
        /// <param name="lower">Lower triangual matrix.</param>
        /// <returns>Solution x.</returns>
        public Double ForwardSubstituion(Double lower)
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
        public Double LeastSquareCholesky(Double y)
        {
            Double result;
            if (this.Dimensions.Height < this.Dimensions.Width)
            {
                // Least norm
                Double transpose = this.Transpose();
                Double lower = (this * transpose).Cholesky();
                Double z = y.ForwardSubstituion(lower);
                z = z.BackwardSubstitution(lower.Transpose());
                result = transpose * z;
            }
            else
            {
                // Standard
                Double transpose = this.Transpose();
                Double lower = (transpose * this).Cholesky();
                Double z = (transpose * y).ForwardSubstituion(lower);
                result = z.BackwardSubstitution(lower.Transpose());
            }
            return result;
        }
        public Double LeastSquareQr(Double y)
        {
            Double result;
            if (this.Dimensions.Height < this.Dimensions.Width)
            {
                // Least norm
                Double transpose = this.Transpose();
                Double[] qr = transpose.QRFactorization();
                Double q = qr[0].Extract(0, transpose.Dimensions.Width, 0, transpose.Dimensions.Height);
                Double r = qr[1].Extract(0, transpose.Dimensions.Width, 0, transpose.Dimensions.Width);
                Double z = y.ForwardSubstituion(r.Transpose());
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
        /// Computation of the Householder bidiagonalization of current matrix. 
        /// The method return {u, b, v}, where u,v are orthogonal matrices such that u' * current * v = b,
        /// where b is a bidiagonal matrix with a posssibly nonzero superdiagonal.
        /// </summary>
        /// <returns>Array of matrices {u,b,v}.</returns>
        public Double[] BiDiagonalization()
        {
            Double b = this.Copy();
            int n = b.Dimensions.Width;
            int m = b.Dimensions.Height;
            Double[] leftHouseholder = new Double[n];
            Double[] rightHouseholder = new Double[n - 2];

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

            return new Double[] { u, b, v };
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
        public Double[] Svd()
        {
            int m = this.Dimensions.Height;
            int n = this.Dimensions.Width;
            Double[] ubv = this.BiDiagonalization();
            Double u = ubv[0];
            Double b = ubv[1].Extract(0,n,0,n);
            Double v = ubv[2];
            double epsilon = 1e-5;
            int q = n-1;
            int p = 0;
            while (q < n)
            {
                for (int i = 0; i < n - 1; i++)
                    if (Kean.Math.Double.Absolute(b[i + 1, i]) < epsilon * (Kean.Math.Double.Absolute(b[i, i]) + Kean.Math.Double.Absolute(b[i + 1, i + 1])))
                        b[i + 1, i] = 0;
                Double b11, b22, b33;
                for (int j = n - 1; j >= 0; j--)
                {
                    Double lower = b.Extract(j, j);
                    if (lower.IsDiagonal)
                    {
                        b33 = lower;
                        q = j;
                    }
                    else
                        break;
                }
                for (int j = 1; j < q ; j++)
                {
                    Double upper = b.Extract(0, j, 0,j);
                    if (!upper.IsDiagonal)
                        break;
                    else
                    {
                        p = j;
                        b11 = upper;
                    }
                }
                b22 = b.Extract(p, q, p, q);
                if (q < n)
                {
                    bool zeros = false;
                    for (int i = 0; i < b22.Dimensions.Width; i++)
                        if (b22[i, i] == 0 && b[i + 1, i] != 0)
                        {
                            b22[i + 1, i] = 0;
                            zeros = true;
                        }
                    if (!zeros)
                    {
                        ; // todo
                    }

                }

            }
            return new Double[] { };
        }
        Double[] GolubKahanSvdStep()
        {
            Double b = this.Copy();
            int m = b.Dimensions.Height;
            int n = b.Dimensions.Width;
            Double t = b.Transpose() * b;
            Double trail = t.Extract(m, m + 1, m, m + 1);
            double trace = trail.Trace;
            double determinant = trail.Determinant;
            double mu1 = trace / 2 + Kean.Math.Double.SquareRoot(Kean.Math.Double.Squared(trace / 2) - determinant);
            double mu2 = trace / 2 - Kean.Math.Double.SquareRoot(Kean.Math.Double.Squared(trace / 2) - determinant);
            double tnn = trail[1,1];
            double mu = Kean.Math.Double.Absolute(tnn - mu1) < Kean.Math.Double.Absolute(tnn - mu2) ? mu1 : mu2;
            double y = t[0, 0];
            double z = t[1, 0];
            Double u = Double.Identity(m);
            Double v = Double.Identity(n);
            for (int k = 0; k < n; k++)
            {
                double[] cs = Double.Givens(y, z);
                Double g = Double.GivensRotation(m, k, k + 1, cs[0], cs[1]);
                v *= g;
                b *= g;
                y = b[k, k];
                z = b[k, k + 1];
                cs = Double.Givens(y, z);
                g = Double.GivensRotation(n, k, k + 1, cs[0], cs[1]);
                b = g.Transpose() * b;
                u = g.Transpose() * u;
                if (k < n - 1)
                {
                    y = b[k + 1, k];
                    z = b[k + 2, k];
                }
            }
            return new Double[] {u, b, v};
        }
        static Double GivensRotation(int m, int i, int k, double c, double s)
        {
            Double result = Double.Identity(m);
            result[i, i] = c;
            result[k, k] = c;
            result[k, i] = s;
            result[i, k] = -s;
            return result;
        }
        static double[] Givens(double a, double b)
        {
            double[] result = new double[2];
            double c,s;
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
    }
}
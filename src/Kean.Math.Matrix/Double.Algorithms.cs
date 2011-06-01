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
        /// Cholesky factorization of positive symmetric matrix. A = C * C'.  The matrix C is lower triangular.
        /// </summary>
        /// <returns> Cholesky factorization matix.</returns>
        public Double Cholesky()
        {
            if (!this.IsSquare)
                new Exception.InvalidDimensions();
            int order = this.Dimensions.Width;
            Double result = Double.Identity(order);
            Double[] l = new Double[order];
            Double working = this;
            for (int i = 0; i < order - 1; i++)
            {
                double a = working[0, 0];
                double aSquareRoot = Kean.Math.Double.SquareRoot(a);
                Double b = working.Extract(new Geometry2D.Integer.Box(0, 1, 1, working.Dimensions.Height - 1));
                l[i] = Double.Identity(order);
                l[i][i, i] = aSquareRoot;
                l[i] = l[i].Paste(new Geometry2D.Integer.Point(i, i + 1), (1 / aSquareRoot) * b);
                Double corner = working.Extract(new Geometry2D.Integer.Box(1, 1, working.Dimensions.Width - 1, working.Dimensions.Height - 1));
                working = corner - (1 / a) * b * b.Transpose();
            }
            l[order - 1] = Double.Identity(order);
            l[order - 1][order - 1, order - 1] = Kean.Math.Double.SquareRoot(working[0, 0]);
            for (int i = 0; i < l.Length; i++)
                result *= l[i];
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
        public Double LeastSquare(Double y)
        {
            if (this.Dimensions.Width > this.Dimensions.Height || this.Dimensions.Height != y.Dimensions.Height)
                throw new Exception.InvalidDimensions();
            Double transpose = this.Transpose();
            Double lower = (transpose * this).Cholesky();
            Double z = (transpose * y).ForwardSubstituion(lower);
            return z.BackwardSubstitution(lower.Transpose());
        }
        public Double LeastSquareQR(Double y)
        {
            if (this.Dimensions.Width > this.Dimensions.Height || this.Dimensions.Height != y.Dimensions.Height)
                throw new Exception.InvalidDimensions();
            Double[] qr  = this.Transpose().QRFactorization();
            Double z = y.ForwardSubstituion(qr[1].Transpose());
            return qr[0] * z;
        }
        
        public Double[] QRFactorization()
        {
            if (!this.IsSquare)
                throw new Exception.InvalidDimensions();
            int order = this.Dimensions.Width;
            int iterations = Kean.Math.Integer.Minimum(this.Dimensions.Height - 1, this.Dimensions.Width);
            Double[] q = new Double[iterations];
            Double worker = this;
            for (int i = 0; i < iterations; i++)
            {
                q[i] = Double.Identity(order).Paste(i, i, Double.HouseHolder(worker.Extract(new Kean.Math.Geometry2D.Integer.Box(i, i, 1, worker.Dimensions.Height - i))));
                worker = q[i] * worker;
            }
            Double qTotal = Double.Identity(order);
            for (int i = 0; i < iterations; i++)
                qTotal *= q[i].Transpose();
            return new Double[] { qTotal, worker };
        }
        public static Double HouseHolder(Double x)
        {
            int length = x.Dimensions.Height;
            Double e1 = new Double(1, length);
            e1[0, 0] = 1;
            double alpha = Kean.Math.Double.Power(-1, length) * Kean.Math.Double.Sign(x[0, 0]) * x.Norm;
            Double u = x + alpha * e1;
            Double v = u / u.Norm;
            return Double.Identity(length) - 2 * v * v.Transpose();
        }

    }
}

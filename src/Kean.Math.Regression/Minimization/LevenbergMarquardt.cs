using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.Math.Ransac.Minimization
{
    public class LevenbergMarquardt
    {
        Func<double[], double>[] function;
        Func<double[], double>[,] jacobian;
        int iterations;
        double error;
        public LevenbergMarquardt(Func<double[], double>[] function, Func<double[], double>[,] jacobian, int iterations, double error)
        {
            this.function = function;
            this.jacobian = jacobian;
            this.iterations = iterations;
            this.error = error;
        }
        public double[] Estimate(double[] guess)
        {
            double[] result = guess;
            int k  = 0;
            double ny = 2;
            do
            {
                Matrix.Double j = new Matrix.Double(this.jacobian.GetLength(1), this.jacobian.GetLength(0));
                for (int x = 0; x < j.Dimensions.Width; x++)
                    for (int y = 0; y < j.Dimensions.Height; y++)
                        j[x, y] = this.jacobian[y, x](result);
                Matrix.Double f = new Matrix.Double(1, this.function.Length);
                    for (int y = 0; y < f.Dimensions.Height; y++)
                        f[0, y] = this.function[y](result);
                    Matrix.Double a = j.Transpose() * j;
                    Matrix.Double g = j.Transpose() * f;
            }
            while (k < this.iterations);

            return result;

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.Math.Ransac.Minimization
{
    public class LevenbergMarquardt
    {
        Func<double[], double[]> function;
        Func<double[], double[,]> jacobian;
        int iterations;
        double error1;
        double error2;
        public LevenbergMarquardt(Func<double[], double[]> function, Func<double[], double[,]> jacobian, int iterations, double error1, double error2)
        {
            this.function = function;
            this.jacobian = jacobian;
            this.iterations = iterations;
            this.error1 = error1;
            this.error2 = error2;
        }
        public double[] Estimate(double[] x0)
        {
            double[] result = x0;
            int k = 0;
            double ny = 2;
            Matrix.Double x = (Matrix.Double)x0;
            Matrix.Double j = (Matrix.Double)this.jacobian(x0);
            Matrix.Double f = (Matrix.Double)this.function(x0);
            Matrix.Double a = j.Transpose() * j;
            Matrix.Double g = j.Transpose() * f;
            double tao = 1e-3;
            bool found = g.NormInfinity <= this.error1;
            double mu = 0;
            int order = a.Order;
            for (int i = 0; i < order; i++)
                mu = Kean.Math.Double.Maximum(mu, Kean.Math.Double.Absolute(a[i, i]));
            mu *= tao;
            while (!found && k < this.iterations)
            {
                k++;
                Matrix.Double hlm = (a + mu * Matrix.Double.Identity(order)).Solve(-g);
                if (hlm.Norm <= this.error2 * (x.Norm + this.error2))
                    found = true;
                else
                {
                    Matrix.Double xNew = x + hlm;
                    double rho = (Kean.Math.Double.Squared(((Matrix.Double)this.function((double[])x)).Norm) - Kean.Math.Double.Squared(((Matrix.Double)this.function((double[])xNew)).Norm)) / 2;
                    rho /= (hlm.Transpose() * (mu * hlm - g))[0, 0] / 2;
                    if (rho > 0)
                    {
                        x = xNew;
                        j = (Matrix.Double)this.jacobian(x0);
                        f = (Matrix.Double)this.function(x0);
                        a = j.Transpose() * j;
                        g = j.Transpose() * f;
                        found = g.NormInfinity <= this.error1;
                        mu = mu * Kean.Math.Double.Maximum(1.0 / 3.0, 1.0 - Kean.Math.Double.Power(2 * rho - 1, 3));
                        ny *= 2;
                    }
                }
            }
            return (double[])x;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.Math.Random.Generator
{
    public class Normal
    {
        Uniform[] generator;
        public Normal()
        {
            this.generator = new Uniform[2];
            this.generator[0] = new Uniform((uint)Environment.TickCount, 15, 4, 21, 36243606, 521288629, 88675123);
            this.generator[1] = new Uniform((uint)Environment.TickCount, 23, 24, 3, 842502087, 3579807591, 273326509);
        }
        public double[] NextDoublePoint()
        {
            double[] result = new double[2];
            double x1 = this.generator[0].NextDouble();
            while (x1 == 0)
                x1 = this.generator[0].NextDouble();
            double x2 = this.generator[1].NextDouble();
            while (x2 == 0)
                x2 = this.generator[1].NextDouble();
            result[0] = Kean.Math.Double.SquareRoot(-2 * Kean.Math.Double.Logarithm(x1)) * Kean.Math.Double.Cosinus(2 * Kean.Math.Double.Pi * x2);
            result[1] = Kean.Math.Double.SquareRoot(-2 * Kean.Math.Double.Logarithm(x1)) * Kean.Math.Double.Sinus(2 * Kean.Math.Double.Pi * x2);
            return result;
        }
        public double NextDouble(double mu, double sigma)
        {
            double result = this.NextDouble();
            result = result * sigma + mu;
            return result;
        }
        public double[] NextDoublePoint(double mu, double sigma)
        {
            double[] result = this.NextDoublePoint();
            result[0] = result[0] * sigma + mu;
            result[1] = result[1] * sigma + mu;
            return result;
        }
        public double NextDouble()
        {
            double result;
            double x1 = this.generator[0].NextDouble();
            while (x1 == 0)
                x1 = this.generator[0].NextDouble();
            double x2 = this.generator[1].NextDouble();
            while (x2 == 0)
                x2 = this.generator[1].NextDouble();
            result = Kean.Math.Double.SquareRoot(-2 * Kean.Math.Double.Logarithm(x1)) * Kean.Math.Double.Cosinus(2 * Kean.Math.Double.Pi * x2);
            return result;
        }


    }
}

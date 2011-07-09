using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kean.Math.Random.Generator
{
    public class Exponential
    {
        Uniform generator;
        public Exponential()
        {
            this.generator = new Uniform();
        }
        public double NextDouble()
        {
            return this.NextDouble(1);
        }
        public double NextDouble(double lambda)
        {
            double result = this.generator.NextDouble();
            while (result == 0)
                result = this.generator.NextDouble();
            result = -Kean.Math.Double.Logarithm(result) / lambda;
            return result;
        }
        public double[] NextDoubleArray(double lambda, int length)
        {
            double[] result = new double[length];
            for (int i = 0; i < length; i++)
                result[i] = this.NextDouble(lambda);
            return result;
        }
    }
}

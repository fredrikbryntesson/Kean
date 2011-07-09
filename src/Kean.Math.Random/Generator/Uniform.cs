using System;

namespace Kean.Math.Random.Generator
{
    public class Uniform
    {
        const double intFactor = 1.0 / ((double)int.MaxValue + 1);
        const double uintFactor = 1.0 / ((double)uint.MaxValue + 1);
        int a;
        int b;
        int c;
        uint x;
        uint y;
        uint z;
        uint w;
        public Uniform() : this((uint)Environment.TickCount) { }
        public Uniform(uint seed) : this(seed, 11, 8, 19, 36243606, 521288629, 88675123) { }
        public Uniform(uint seed, int a, int b, int c, uint y0, uint z0, uint w0)
        {
            this.x = seed;
            this.a = a;
            this.b = b;
            this.c = c;
            this.y = y0;
            this.z = z0;
            this.w = w0;
        }
        public uint NextUint()
        {
            uint t = (this.x ^ (this.x << this.a));
            this.x = this.y;
            this.y = this.z;
            this.z = this.w;
            return this.w = (this.w ^ (this.w >> this.c)) ^ (t ^ (t >> this.b));
        }
        public int NextInt()
        {
            int result;
            // Handle the special case where the value int.MaxValue is generated. This is outside of 
            // the range of permitted values, so we therefore call Next() to try again.
            uint chopped = this.NextUint() & 0x7FFFFFFF;
            if (chopped == 0x7FFFFFFF)
                result = this.NextInt();
            else
                result = (int)chopped;
            return result;

        }
        public int NextInt(int upperBound)
        {
            return (int)(upperBound * Uniform.intFactor * this.NextInt());
        }
        public int NextInt(int lowerBound, int upperBound)
        {
            return lowerBound + this.NextInt(upperBound - lowerBound);
        }
        public int[] NextIntArray(int length)
        {
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
                result[i] = this.NextInt();
            return result;
        }
        public int[] NextIntArray(int upperBound, int length)
        {
            return this.NextIntArray(0, upperBound, length);
        }
        public int[] NextIntArray(int lowerBound, int upperBound, int length)
        {
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
                result[i] = this.NextInt(lowerBound, upperBound);
            return result;
        }
        public int[] NextDifferentIntArray(int upperBound, int length)
        {
            return this.NextDifferentIntArray(0, upperBound, length);
        }
        public int[] NextDifferentIntArray(int lowerBound, int upperBound, int length)
        {
            int[] result = new int[length];
            bool different = false;
            if (length > upperBound)
                throw new Exception.InputData();
            while (!different)
            {
                result = this.NextIntArray(lowerBound, upperBound, length);
                different = true;
                for (int i = 0; i < length; i++)
                    for (int j = i + 1; j < length; j++)
                        different &= result[i] != result[j];
            }
            return result;
        }
        /// <summary>
        /// Returns a double in the interval [0,1). Uniform distribution.
        /// </summary>
        /// <returns></returns>
        public double NextDouble()
        {
            return this.NextDouble(0, 1);
        }
        public double NextDouble(double upperBound)
        {
            return this.NextDouble(0, upperBound);
        }
        public double NextDouble(double lowerBound, double upperBound)
        {
            return lowerBound + (upperBound - lowerBound) * Uniform.uintFactor * this.NextUint();
        }
        public double[] NextDoubleArray(int length)
        {
            return this.NextDoubleArray(1, length);    
        }
        public double[] NextDoubleArray(double upperBound, int length)
        {
            return this.NextDoubleArray(0, upperBound, length);
        }
        public double[] NextDoubleArray(double lowerBound, double upperBound, int length)
        {
            double[] result = new double[length];
            for (int i = 0; i < length; i++)
                result[i] = this.NextDouble(lowerBound, upperBound);
            return result;
        }
    }
}

using System;
using Kean.Core.Basis.Extension;

namespace Kean.Math.Random.Generator
{
    public class Uniform
    {
        struct Datum
        {
            public int a;
            public int b;
            public int c;
            public uint x;
            public uint y;
            public uint z;
            public uint w;
            public Datum(int a, int b, int c, uint x, uint y, uint z, uint w)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }
            public Datum(int a, int b, int c, uint y, uint z, uint w) : this(a,b,c,0,y,z,w) {} 
            public void SetSeed(uint seed)
            {
                this.x = seed;
            }
            public static implicit operator Datum(string value)
            {
                Datum result = new Datum();
                if (value.NotEmpty())
                {

                    try
                    {
                        string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length == 6)
                            result = new Datum(Kean.Math.Integer.Parse(values[0]),Kean.Math.Integer.Parse(values[1]), Kean.Math.Integer.Parse(values[2]), Kean.Math.UnsignedInteger.Parse(values[3]), Kean.Math.UnsignedInteger.Parse(values[4]), Kean.Math.UnsignedInteger.Parse(values[5]));
                    }
                    catch
                    {
                    }
                }
                return result;
            }
        }
        static Datum[] datums = new Datum[] 
        {
            "5, 14, 1, 36243606, 521288629, 88675123", 
            "15, 4, 21, 3063318884, 5413248387, 9218160800", 
            "23, 24, 3,  8149992844, 8536707798, 4050642383", 
            "5, 12, 29, 0459450119, 1354370154, 0032162936" 
        }; 
        static int counter = 0;

        const double intFactor = 1.0 / ((double)int.MaxValue + 1);
        const double uintFactor = 1.0 / ((double)uint.MaxValue + 1);
        Datum datum;
        public Uniform() : this((uint)DateTime.Now.Ticks) { }
        public Uniform(uint seed)
        {
            this.datum = Uniform.datums[Uniform.counter];
            Uniform.counter++;
            if (Uniform.counter == Uniform.datums.Length)
                Uniform.counter = 0;
        }
        public uint NextUint()
        {
            uint t = (this.datum.x ^ (this.datum.x << this.datum.a));
            this.datum.x = this.datum.y;
            this.datum.y = this.datum.z;
            this.datum.z = this.datum.w;
            return this.datum.w = (this.datum.w ^ (this.datum.w >> this.datum.c)) ^ (t ^ (t >> this.datum.b));
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

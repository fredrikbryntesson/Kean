using System;
using Kean.Core.Extension;

namespace Kean.Math
{
    public class Fraction
    {
        double value;
        public double Value 
        { 
            get {return this.value;}
            private set 
            {
                this.value = value;
                this.valueString = Kean.Math.Double.ToString(value);
            } 
        }
        string valueString;
        public string String
        {
            get { return this.valueString; }
            private set
            {
                this.valueString = value;
                this.value = this.Parser(value);
            }
        }
        public Fraction(string value)
        {
            this.String = value;
        }
        public Fraction(double value)
        {
            this.Value = value;
        }
        public Fraction(double value, int length, double epsilon)
        {
            this.String = this.EvaluateQuotients(this.ContinuousFraction(value, length, epsilon));
        }

        double Parser(string value)
        {
            double result = 0;
            if (value.NotEmpty())
            {
                string[] splitted = value.Split(new char[] { '[', ']', ';', ',', ':', '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (value.Contains('[', ']'))
                {
                    int[] quotients = splitted.Map<string, int>(s => Kean.Math.Integer.Parse(s));
                    splitted = this.EvaluateQuotients(quotients).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                }
                if (splitted.Length == 2)
                    result = Kean.Math.Double.Parse(splitted[0]) / Kean.Math.Double.Parse(splitted[1]);
                else
                    result = Kean.Math.Double.Parse(splitted[0]);
            }
            return result;
        }
        int[] ContinuousFraction(double value, int length, double epsilon)
        {
            int[] result;
            int[] quotients = new int[length];
            int integerPart = Kean.Math.Integer.Floor(value);
            int k = 0;
            while (k < length)
            {
                quotients[k++] = integerPart;
                double fraction = value - integerPart;
                if (fraction < epsilon)
                    break;
                else
                {
                    value = 1 / fraction;
                    integerPart = Kean.Math.Integer.Floor(value);
                }
            }
            result = new int[k];
            Array.Copy(quotients, 0, result, 0, k);
            return result;
        }
        string EvaluateQuotients(int[] quotients)
        {
            string result;
            int[] nominator = new int[quotients.Length + 2];
            int[] denominator = new int[quotients.Length + 2];
            nominator[1] = 1;
            denominator[0] = 1;
            for (int i = 2; i < quotients.Length + 2; i++)
            {
                nominator[i] = quotients[i - 2] * nominator[i - 1] + nominator[i - 2];
                denominator[i] = quotients[i - 2] * denominator[i - 1] + denominator[i - 2];
            }
            result = Kean.Math.Integer.ToString(nominator[nominator.Length - 1]) + "/" + Kean.Math.Integer.ToString(denominator[denominator.Length - 1]);
            return result;
        }

        #region Object Overrides
        public override string ToString()
        {
            return this;
        }
        #endregion
        #region Casts
        public static implicit operator string(Fraction value)
        {
            return value.NotNull() ? value.String : null;
        }
        public static implicit operator Fraction(string value)
        {
            return value.NotEmpty() ? new Fraction(value) : null;
        }
        public static implicit operator double(Fraction value)
        {
            return value.NotNull() ? value.Value : 0;
        }
        public static implicit operator Fraction(double value)
        {
            return new Fraction(value);
        }
        #endregion

    }
}

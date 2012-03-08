using System;
using Kean.Core;
using Kean.Core.Extension;

namespace Kean.Math
{
	public class Fraction : 
        IEquatable<Fraction>
	{
		public int Nominator { get; private set; }
		public int Denominator { get; private set; }
		Tuple<int[], int> quotientsAndGCD;
		public int GCD { get { return this.quotientsAndGCD.Item2; } }
		public string ContinousFraction
		{
			get
			{
				string result;
				int[] quotients = this.quotientsAndGCD.Item1;
				System.Text.StringBuilder builder = new System.Text.StringBuilder();
				builder.Append('[');
				builder.Append(Kean.Math.Integer.ToString(quotients[0]));
				if (quotients.Length > 1)
				{
					builder.Append(';');
					for (int i = 1; i < quotients.Length - 1; i++)
					{
						builder.Append(Kean.Math.Integer.ToString(quotients[i]));
						builder.Append(',');
					}
					builder.Append(Kean.Math.Integer.ToString(quotients[quotients.Length - 1]));
				}
				builder.Append(']');
				result = builder.ToString();
				return result;
			}
		}
		public Fraction(string value)
		{
			if (value.NotEmpty())
			{
				if (value.Contains('[', ']'))
				{
					string[] splitted = value.Split(new char[] { '[', ']', ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
					int[] nominatorDenominator = this.EvaluateQuotients(splitted.Map<string, int>(s => Kean.Math.Integer.Parse(s)));
					this.Nominator = nominatorDenominator[0];
					this.Denominator = nominatorDenominator[1];
				}
				else
				{
					if (value.Contains(':') || value.Contains('/'))
					{
						string[] splitted = value.Split(new char[] { ':', '/', ' ' }, StringSplitOptions.RemoveEmptyEntries);
						if (splitted.Length == 2)
						{
							this.Nominator = Kean.Math.Integer.Parse(splitted[0]);
							this.Denominator = Kean.Math.Integer.Parse(splitted[1]);
						}
					}
					else
					{
						value = value.Replace(',', '.');
						if (!value.Contains('.'))
						{
							this.Nominator = Kean.Math.Integer.Parse(value);
							this.Denominator = 1;
						}
						else
							this.DecimalToFraction(Kean.Math.Double.Parse(value));
					}
				}
			}
			else
			{
				this.Nominator = 0;
				this.Denominator = 1;
			}
			this.quotientsAndGCD = this.EuclidanAlgorithm();
		}
		public Fraction(double value)
		{
			this.DecimalToFraction(value);
		}
		void DecimalToFraction(double value)
		{
			int[] quotients = this.ContinuousFraction(value, 10, 1e-3);
			int[] nominator = new int[quotients.Length + 2];
			int[] denominator = new int[quotients.Length + 2];
			nominator[1] = 1;
			denominator[0] = 1;
			for (int i = 2; i < quotients.Length + 2; i++)
			{
				nominator[i] = quotients[i - 2] * nominator[i - 1] + nominator[i - 2];
				denominator[i] = quotients[i - 2] * denominator[i - 1] + denominator[i - 2];
			}
			this.Nominator = nominator[nominator.Length - 1];
			this.Denominator = denominator[denominator.Length - 1];
			this.quotientsAndGCD = this.EuclidanAlgorithm();
		}
		int[] EvaluateQuotients(int[] quotients)
		{
			int[] result;
			int[] nominator = new int[quotients.Length + 2];
			int[] denominator = new int[quotients.Length + 2];
			nominator[1] = 1;
			denominator[0] = 1;
			for (int i = 2; i < quotients.Length + 2; i++)
			{
				nominator[i] = quotients[i - 2] * nominator[i - 1] + nominator[i - 2];
				denominator[i] = quotients[i - 2] * denominator[i - 1] + denominator[i - 2];
			}
			result = new int[] { nominator[nominator.Length - 1], denominator[denominator.Length - 1] };
			return result;
		}
		Kean.Core.Tuple<int[], int> EuclidanAlgorithm()
		{
			Kean.Core.Tuple<int[], int> result = null;
			int nominator = this.Nominator;
			int denominator = this.Denominator;
			nominator = Kean.Math.Integer.Absolute(nominator);
			denominator = Kean.Math.Integer.Absolute(denominator);
			if (nominator == denominator)
				result = Kean.Core.Tuple.Create(new int[] { 1 }, nominator != 0 ? nominator : 1);
			else
			{
				int a, b;
				if (nominator > denominator)
				{
					a = nominator;
					b = denominator;
				}
				else
				{
					b = nominator;
					a = denominator;
				}
				System.Collections.Generic.List<int> quotients = new System.Collections.Generic.List<int>();
				int r = 0, q = 1;
				do
				{
					q = a / b;
					r = a - b * q;
					a = b;
					b = r;
					quotients.Add(q);
				} while (r != 0);
				int gcd = a;
				if (quotients.Count > 0)
					quotients[0] *= Kean.Math.Integer.Sign(this.Nominator) * Kean.Math.Integer.Sign(this.Denominator); 
				result = Kean.Core.Tuple.Create(quotients.ToArray(), gcd);
			
			}
			return result;
		}
		int[] ContinuousFraction(double value, int length, double epsilon)
		{
			int[] result;
			int[] quotients = new int[length];
			int integerPart = Kean.Math.Integer.Floor(value);
			int count = 0;
			while (count < length)
			{
				quotients[count++] = integerPart;
				double fraction = value - integerPart;
				if (fraction < epsilon)
					break;
				else
				{
					value = 1 / fraction;
					integerPart = Kean.Math.Integer.Floor(value);
				}
			}
			if (count < 2 || quotients[count - 1] != 1)
			{
				result = new int[count];
				Array.Copy(quotients, 0, result, 0, count);
			}
			else
			{
				result = new int[count - 1];
				Array.Copy(quotients, 0, result, 0, count - 1);
				result[count - 2] += 1;
			}
			return result;
		}
        #region Comparison Operators
        public static bool operator ==(Fraction left, Fraction right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) &&
                left.Nominator * right.Denominator - right.Nominator * left.Denominator == 0;
        }
        public static bool operator !=(Fraction left, Fraction right)
        {
            return !(left == right);
        }
        #endregion
		#region Object Overrides
        public override bool Equals(object other)
        {
            return (other is Fraction) && this.Equals(other as Fraction);
        }
        public bool Equals(Fraction other)
        {
            return this == other;
        }
     	public override string ToString()
		{
			return this;
		}
		#endregion
		#region Casts
		public static implicit operator string(Fraction value)
		{
			return value.NotNull() ? Kean.Math.Integer.ToString(value.Nominator) + ":" + Kean.Math.Integer.ToString(value.Denominator) : null;
		}
		public static implicit operator Fraction(string value)
		{
			return value.NotEmpty() ? new Fraction(value) : null;
		}
		public static explicit operator float(Fraction value)
		{
			return value.NotNull() ? value.Nominator / (float)value.Denominator : 0;
		}
		public static explicit operator Fraction(float value)
		{
			return new Fraction(value);
		}
		public static explicit operator double(Fraction value)
		{
			return value.NotNull() ? value.Nominator / (double)value.Denominator : 0;
		}
		public static explicit operator Fraction(double value)
		{
			return new Fraction(value);
		}
		#endregion

	}
}
